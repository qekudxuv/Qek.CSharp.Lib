using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace Qek.Mail
{
    public abstract class AbstractExceptionMail : AbstractMailTemplate
    {
        private enum MailType
        {
            Web,
            Tool
        }

        private static readonly string _webMailTemplate;
        private static readonly string _toolMailTemplate;
        private MailType _mailType;
        protected Exception _ex;
        protected string _account;//end user account
        protected HttpContext _ctx;
        protected string _input;

        static AbstractExceptionMail()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyFilename = assembly.GetName().Name;

            using (StreamReader sr1 = new StreamReader(assembly.GetManifestResourceStream(assemblyFilename + ".Mail.EmailTemplate.WebExceptionMail.htm")))
            {
                _webMailTemplate = sr1.ReadToEnd();
            }
            using (StreamReader sr2 = new StreamReader(assembly.GetManifestResourceStream(assemblyFilename + ".Mail.EmailTemplate.ToolExceptionMail.htm")))
            {
                _toolMailTemplate = sr2.ReadToEnd();
            }
        }

        /// <summary>
        /// 表示是TOOL Exception
        /// Initializes a new instance of the <see cref="ExceptionMail"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="input">The input.</param>
        public AbstractExceptionMail(Exception ex, string input = "")
            : base(BaseHtmlSmtpMail.GenMailObjByAppConfig())
        {
            this._mailType = MailType.Tool;
            this._ex = ex;
            this._input = input;
        }

        /// <summary>
        /// 表示是WEB Exception
        /// Initializes a new instance of the <see cref="ExceptionMail"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="input">The input.</param>
        public AbstractExceptionMail(Exception ex, string account, HttpContext ctx, string input = "")
            : base(BaseHtmlSmtpMail.GenMailObjByAppConfig())
        {
            this._mailType = MailType.Web;
            this._ex = ex;
            this._account = account;
            this._ctx = ctx;
            this._input = input;
        }

        protected override bool Verify()
        {
            bool flag = false;
            if (this._mailType == MailType.Web)
            {
                flag = (this._ex != null && this._ctx != null);
            }
            else if (this._mailType == MailType.Tool)
            {
                flag = this._ex != null;
            }
            return flag;
        }

        protected override string GetMailContent()
        {
            StringBuilder mailContent = null;
            if (this._mailType == MailType.Web)
            {
                mailContent = new StringBuilder(_webMailTemplate);
                mailContent = mailContent.Replace("${Time}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                mailContent = mailContent.Replace("${Account}", _account);
                mailContent = mailContent.Replace("${IPAddress}", _ctx.Request.UserHostAddress);
                mailContent = mailContent.Replace("${URL}", _ctx.Request.Url.AbsoluteUri);

                mailContent = mailContent.Replace("${Input}", _input);
                mailContent = mailContent.Replace("${Source}", _ex.Source);
                mailContent = mailContent.Replace("${ExceptionType}", _ex.GetType().ToString());
                mailContent = mailContent.Replace("${ExceptionMsg}", _ex.Message);
                mailContent = mailContent.Replace("${StackTrace}", _ex.StackTrace);

                mailContent = mailContent.Replace("${InnerExceptionSource}", _ex.InnerException == null ? "" : _ex.InnerException.Source);
                mailContent = mailContent.Replace("${InnerExceptionType}", _ex.InnerException == null ? "" : _ex.InnerException.GetType().ToString());
                mailContent = mailContent.Replace("${InnerExceptionMsg}", _ex.InnerException == null ? "" : _ex.InnerException.Message);
                mailContent = mailContent.Replace("${InnerExceptionStackTrace}", _ex.InnerException == null ? "" : _ex.InnerException.StackTrace);

                mailContent = mailContent.Replace("${ClientInfo}", _ctx.Request.Browser.Browser + "(" + _ctx.Request.Browser.Version + ")");

            }
            else if (this._mailType == MailType.Tool)
            {
                mailContent = new StringBuilder(_toolMailTemplate);

                mailContent = mailContent.Replace("${Time}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                mailContent = mailContent.Replace("${MachineName}", Environment.MachineName);
                mailContent = mailContent.Replace("${ProcessName}", Process.GetCurrentProcess().ProcessName);
                mailContent = mailContent.Replace("${OS}", string.Format("{0} ({1})", Environment.OSVersion.Platform, Environment.OSVersion.Version));

                mailContent = mailContent.Replace("${Input}", _input);
                mailContent = mailContent.Replace("${Source}", _ex.Source);
                mailContent = mailContent.Replace("${ExceptionType}", _ex.GetType().ToString());
                mailContent = mailContent.Replace("${ExceptionMsg}", _ex.Message);
                mailContent = mailContent.Replace("${StackTrace}", _ex.StackTrace);

                mailContent = mailContent.Replace("${InnerExceptionSource}", _ex.InnerException == null ? "" : _ex.InnerException.Source);
                mailContent = mailContent.Replace("${InnerExceptionType}", _ex.InnerException == null ? "" : _ex.InnerException.GetType().ToString());
                mailContent = mailContent.Replace("${InnerExceptionMsg}", _ex.InnerException == null ? "" : _ex.InnerException.Message);
                mailContent = mailContent.Replace("${InnerExceptionStackTrace}", _ex.InnerException == null ? "" : _ex.InnerException.StackTrace);
            }

            if (_ex is System.Net.Mail.SmtpFailedRecipientException)
            {
                mailContent = mailContent.Replace("${AddInfo}",
                        "Invalid Receiver<br/>" +
                        ((System.Net.Mail.SmtpFailedRecipientException)_ex).FailedRecipient.Replace("<", "").Replace(">", ""));
            }
            else
            {
                mailContent = mailContent.Replace("${AddInfo}", string.Empty);
            }

            return mailContent.ToString();
        }
    }
}

