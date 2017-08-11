using Qek.Common;

namespace Qek.Mail
{
    /// <summary>
    /// DateTime:   2013/11/25
    /// Author:     Sam.SH_Chang#21978
    /// Use For Sending Mail
    /// Reference From CommonLib/SMTP/SMTPMail
    /// </summary>
    public sealed class MyHtmlSmtpMail : BaseSmtpMail
    {
        private static bool _isTest = ConfigHelper.GetAppSetting<bool>("IsTest");
        private static ENV _env = ConfigHelper.GetAppSetting<ENV>("ENV");
        private static string _testMailReceivers = ConfigHelper.GetAppSetting("TestMailReceivers", _env);

        private MyHtmlSmtpMail() :
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
        public static MyHtmlSmtpMail GenMailObjByAppConfig()
        {
            return new MyHtmlSmtpMail();
        }
        #endregion

        #region Override Methods
        public override void SendMail(string content)
        {
            this.SendMail(base.Subject, content);
        }

        public override void SendMail(string subject, string content)
        {
            if (_isTest)
            {
                content = string.Format(@"<table border='1' style='font-size:12px; font-family: Verdana;'>
                                        <tr><td colspan='2' bgcolor='#72C530'>Original Receiver</td></tr>
                                        <tr><td>MailTO:</td><td>{0}</td></tr>
                                        <tr><td>MailCC:</td><td>{1}</td></tr>
                                        <tr><td>MailBCC:</td><td>{2}</td></tr> 
                                    </table>
                                    <br />
                                    {3}", base.GetMailTo(";"), base.GetCcTo(";"), base.GetBccTo(";"), content);
                base.ClearMailTo();
                base.ClearCcTo();
                base.ClearBccTo();
                base.SetMailTo(_testMailReceivers);
                subject = "[Test]" + subject;
            }
            base.SendMail(subject, content);
        }
        #endregion Override Methods
    }
}

