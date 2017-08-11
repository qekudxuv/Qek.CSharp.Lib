namespace Qek.Mail
{
    public abstract class AbstractMailTemplate : IMail
    {
        protected ISmtpService _mySmtpMail = MyHtmlSmtpMail.GenMailObjByAppConfig();//default

        public AbstractMailTemplate() { }

        public AbstractMailTemplate(ISmtpService smtpMail)
        {
            this._mySmtpMail = smtpMail;
        }

        /// <summary>
        /// 設定收件人
        /// </summary>
        protected abstract void SetMailReceiver();

        /// <summary>
        /// 設定附件
        /// </summary>
        protected abstract void SetMailAttachment();

        /// <summary>
        /// 取得主旨
        /// </summary>
        /// <returns></returns>
        protected abstract string GetMailSubject();

        /// <summary>
        /// 若子類別有需要先行驗證，就覆寫此方法，去決定是否發mail
        /// </summary>
        /// <returns></returns>
        protected virtual bool Verify()
        {
            return true;
        }

        /// <summary>
        /// 取得信件內容
        /// </summary>
        /// <returns></returns>
        protected abstract string GetMailContent();


        public void SendMail()
        {
            if (Verify())
            {
                //設定收件人
                this.SetMailReceiver();
                //設定附件
                this.SetMailAttachment();
                //設定主旨、內容並寄出電子郵件
                this._mySmtpMail.SendMail(
                            this.GetMailSubject(),
                            this.GetMailContent());
            }
        }
    }
}

