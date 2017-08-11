using Qek.Common;
using System;
using System.Web;

namespace Qek.Mail
{
    /// <summary>
    /// Exception Mail
    /// </summary>
    public class ExceptionMail : AbstractExceptionMail
    {
        private static ENV _env = ConfigHelper.GetAppSetting<ENV>("ENV");
        private static string _systemShortName = ConfigHelper.GetAppSetting("SystemShortName");
        private static string _exceptionMailReceivers = ConfigHelper.GetAppSetting("ExceptionMailReceivers", _env);

        /// <summary>
        /// 表示是TOOL Exception
        /// Initializes a new instance of the <see cref="ExceptionMail"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ExceptionMail(Exception ex, string input = "")
            : base(ex, input)
        {
        }

        /// <summary>
        /// 表示是WEB Exception
        /// Initializes a new instance of the <see cref="ExceptionMail"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="ctx">The CTX.</param>
        public ExceptionMail(Exception ex, string account, HttpContext ctx, string input = "")
            : base(ex, account, ctx, input)
        {
        }

        protected override void SetMailReceiver()
        {
            base._mySmtpMail.SetMailTo(_exceptionMailReceivers);
        }

        protected override void SetMailAttachment()
        {
            return;
        }

        protected override string GetMailSubject()
        {
            return string.Format("[{0}][{1}]",
            _systemShortName,
            base._ex.GetType());
        }
    }
}

