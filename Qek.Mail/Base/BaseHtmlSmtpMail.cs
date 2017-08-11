using Qek.Common;

namespace Qek.Mail
{
    public class BaseHtmlSmtpMail : BaseSmtpMail
    {
        private static ENV _env = ConfigHelper.GetAppSetting<ENV>("ENV");

        private BaseHtmlSmtpMail() :
            base
            (
                true,//IsHTML
                ConfigHelper.GetAppSetting("MailServer", _env),
                ConfigHelper.GetAppSetting("FromName", _env),
                ConfigHelper.GetAppSetting("MailFrom", _env)
            )
        {
        }

        #region Static Methods
        /// <summary>
        /// Generate the SmtpMail instance by using value that set within app config.
        /// </summary>
        /// <returns>SmtpMail instance</returns>
        public static BaseHtmlSmtpMail GenMailObjByAppConfig()
        {
            return new BaseHtmlSmtpMail();
        }
        #endregion
    }
}
