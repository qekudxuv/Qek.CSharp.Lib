using Newtonsoft.Json.Linq;
using Qek.Mail;
using Qek.NHibernate.Dao;
using Qek.NHibernate.Model;
using System;
using System.Web;

namespace Qek.Err
{
    public abstract class AbstractExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// 表示是TOOL Exception
        /// 讓各系統實作，要如何對exception做處理
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="input">The input.</param>
        public abstract void Handle(Exception ex, string input = "");

        /// <summary>
        /// 表示是TOOL Exception
        /// 讓各系統實作，要如何對exception做處理
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="input">The input.</param>
        public void Handle(Exception ex, object input)
        {
            this.Handle(ex, input == null ?
                string.Format("{0} is null", input.GetType().ToString()) :
                JObject.FromObject(input).ToString());
        }

        /// <summary>
        /// 表示是WEB Exception
        /// 讓各系統實作，要如何對exception做處理
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="input">The input.</param>
        public abstract void Handle(Exception ex, string account, HttpContext ctx, string input = "");

        /// <summary>
        /// 表示是WEB Exception
        /// 讓各系統實作，要如何對exception做處理
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="input">The input.</param>
        public void Handle(Exception ex, string account, HttpContext ctx, object input)
        {
            this.Handle(ex, account, ctx, input == null ?
                string.Format("{0} is null", input.GetType().ToString()) :
                JObject.FromObject(input).ToString());
        }

        /// <summary>
        /// 紀錄exception至資料庫
        /// (使用此方法，系統需支援Nhibernate，反之則可進行覆寫)
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected void RecordException(Exception ex, string input = "")
        {
            ToolExceptionLogModel log = new ToolExceptionLogModel();

            log.MachineName = Environment.MachineName;
            log.ProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            log.OS = string.Format("{0} ({1})", Environment.OSVersion.Platform, Environment.OSVersion.Version);

            log.Input = input;
            log.Source = ex.Source ?? "";
            log.ExceptionMessage = ex.Message;
            log.ExceptionType = ex.GetType().ToString();
            log.StackTrace = ex.StackTrace;
            if (ex.InnerException != null)
            {
                log.InnerExceptionSource = ex.InnerException.Source ?? "";
                log.InnerExceptionMessage = ex.InnerException.Message;
                log.InnerExceptionType = ex.InnerException.GetType().ToString();
                log.InnerExceptionStackTrace = ex.InnerException.StackTrace;
            }
            log.CreateTime = DateTime.Now;

            ToolExceptionLogDao toolExceptionLogDao = new ToolExceptionLogDao();
            toolExceptionLogDao.Save(log);
        }

        /// <summary>
        /// 紀錄exception至資料庫
        /// (使用此方法，系統需支援Nhibernate，反之則可進行覆寫)
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="ctx">The CTX.</param>
        protected void RecordException(Exception ex, string account, HttpContext ctx, string input = "")
        {
            WebExceptionLogModel log = new WebExceptionLogModel();

            log.RemoteAddress = ctx.Request.UserHostAddress;
            log.RequestUrl = ctx.Request.Url.AbsoluteUri;
            log.UserName = account;
            log.ClientInfo = ctx.Request.Browser.Browser + "(" + ctx.Request.Browser.Version + ")";

            log.Input = input;
            log.Source = ex.Source ?? "";
            log.ExceptionMessage = ex.Message;
            log.ExceptionType = ex.GetType().ToString();
            log.StackTrace = ex.StackTrace;
            if (ex.InnerException != null)
            {
                log.InnerExceptionSource = ex.InnerException.Source ?? "";
                log.InnerExceptionMessage = ex.InnerException.Message;
                log.InnerExceptionType = ex.InnerException.GetType().ToString();
                log.InnerExceptionStackTrace = ex.InnerException.StackTrace;
            }
            log.CreateTime = DateTime.Now;

            WebExceptionLogDao webExceptionLogDao = new WebExceptionLogDao();
            webExceptionLogDao.Save(log);
        }

        /// <summary>
        /// 發exception信件
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected void NotifyException(Exception ex, string input = "")
        {
            IMail exMail = new ExceptionMail(ex, input);
            exMail.SendMail();
        }

        /// <summary>
        /// 發exception信件
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="ctx">The CTX.</param>
        protected void NotifyException(Exception ex, string account, HttpContext ctx, string input = "")
        {
            IMail exMail = new ExceptionMail(ex, account, ctx, input);
            exMail.SendMail();
        }
    }
}

