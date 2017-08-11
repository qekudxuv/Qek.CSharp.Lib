using Qek.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Qek.Mail
{
    /// <summary>
    /// DateTime:   2013/03/04
    /// Author:     Sam.SH_Chang#21978
    /// Use For Sending Mail
    /// Reference From CommonLib/SMTP/SMTPMail
    /// </summary>
    public class BaseSmtpMail : ISmtpService
    {
        #region Instance Fields
        private MailMessage _mailMsg = new MailMessage();
        private SmtpClient _smtpClient = new SmtpClient();
        private ICollection<string> _receiverBlackList = new HashSet<string>();
        #endregion

        #region Constructor
        public BaseSmtpMail(bool isHTML, string mailServer, string fromName, string mailFrom,
            string mailToList, string mailCCList, string mailBCCList)
        {
            _mailMsg.IsBodyHtml = isHTML;
            _mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
            _mailMsg.From = (new MailAddress(mailFrom, fromName));
            _smtpClient.Host = mailServer;

            this.AddMailTo(mailToList);
            this.AddCcTo(mailCCList);
            this.AddBccTo(mailBCCList);
        }

        public BaseSmtpMail(bool isHTML, string mailServer, string fromName, string mailFrom)
        {
            _mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
            _mailMsg.From = new MailAddress(mailFrom, fromName);
            _mailMsg.IsBodyHtml = isHTML;
            _smtpClient.Host = mailServer;
        }

        public BaseSmtpMail(bool isHTML, string mailServer, string mailFrom)
        {
            _mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
            _mailMsg.From = new MailAddress(mailFrom);
            _mailMsg.IsBodyHtml = isHTML;
            _smtpClient.Host = mailServer;
        }

        public BaseSmtpMail(bool isHTML, string mailServer)
        {
            _mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
            _mailMsg.IsBodyHtml = isHTML;
            _smtpClient.Host = mailServer;
        }

        public BaseSmtpMail(string mailServer)
        {
            _mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
            _mailMsg.IsBodyHtml = false;
            _smtpClient.Host = mailServer;
        }
        #endregion

        #region Properties
        public string Subject
        {
            get { return _mailMsg.Subject; }
            set { _mailMsg.Subject = value; }
        }
        public bool IsBodyHtml
        {
            get { return _mailMsg.IsBodyHtml; }
            set { _mailMsg.IsBodyHtml = value; }
        }
        public bool HasAnyReceiver
        {
            get
            {
                //設定 mail 不寄給黑名單
                if (this._receiverBlackList.Any())
                {
                    this.RemoveMailTo(this._receiverBlackList);
                    this.RemoveCcTo(this._receiverBlackList);
                    this.RemoveBccTo(this._receiverBlackList);
                }
                return _mailMsg.To.Any() || _mailMsg.CC.Any() || _mailMsg.Bcc.Any();
            }
        }
        public MailPriority MailPriority
        {
            get { return _mailMsg.Priority; }
            set { _mailMsg.Priority = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the mail from.
        /// </summary>
        /// <param name="fromName">From name.</param>
        /// <param name="mailFrom">The mail from.</param>
        public void SetMailFrom(string fromName, string mailFrom)
        {
            _mailMsg.From = new MailAddress(mailFrom, fromName);
        }

        /// <summary>
        /// Sets the mail from.
        /// </summary>
        /// <param name="mailFrom">The mail from.</param>
        public void SetMailFrom(string mailFrom)
        {
            _mailMsg.From = new MailAddress(mailFrom);
        }

        /// <summary>
        /// Clears the mail to.
        /// </summary>
        public void ClearMailTo()
        {
            this._mailMsg.To.Clear();
        }

        /// <summary>
        /// Firstly Clear All mail to Then Set the mail to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void SetMailTo(string emails, string sep = ";")
        {
            this._mailMsg.To.Clear();
            this.AddMailTo(emails, sep);
        }

        /// <summary>
        /// Adds the mail to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void AddMailTo(string emails, string sep = ";")
        {
            string[] emailAry = emails.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAry)
            {
                if (!string.IsNullOrEmpty(email) && ValidationHelper.IsValidEmail(email) && !_mailMsg.To.Contains(new MailAddress(email)))
                {
                    _mailMsg.To.Add(new MailAddress(email));
                }
            }
        }

        /// <summary>
        /// Removes the mail to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void RemoveMailTo(string emails, string sep = ";")
        {
            string[] emailAry = emails.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAry)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    _mailMsg.To.Remove(new MailAddress(email));
                }
            }
        }

        /// <summary>
        /// Removes the mail to.
        /// </summary>
        /// <param name="emailSet">The email set.</param>
        public void RemoveMailTo(IEnumerable<string> emails)
        {
            if (emails != null && emails.Any())
            {
                foreach (string email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        _mailMsg.To.Remove(new MailAddress(email));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the mail to.
        /// </summary>
        /// <param name="sep">The sep.</param>
        /// <returns></returns>
        public string GetMailTo(string sep = ";")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _mailMsg.To)
            {
                sb.Append(item.Address).Append(sep);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Clears the cc to.
        /// </summary>
        public void ClearCcTo()
        {
            this._mailMsg.CC.Clear();
        }

        /// <summary>
        /// Firstly Clear All CC to Then Set the CC to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void SetCcTo(string emails, string sep = ";")
        {
            this.ClearCcTo();
            this.AddCcTo(emails, sep);
        }

        /// <summary>
        /// Adds the cc to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void AddCcTo(string emails, string sep = ";")
        {
            string[] emailAry = emails.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAry)
            {
                if (!string.IsNullOrEmpty(email) && ValidationHelper.IsValidEmail(email) && !_mailMsg.CC.Contains(new MailAddress(email)))
                {
                    _mailMsg.CC.Add(new MailAddress(email));
                }
            }
        }

        /// <summary>
        /// Removes the cc to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void RemoveCcTo(string emails, string sep = ";")
        {
            string[] emailAry = emails.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAry)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    _mailMsg.CC.Remove(new MailAddress(email));
                }
            }
        }

        /// <summary>
        /// Removes the cc to.
        /// </summary>
        /// <param name="emailSet">The email set.</param>
        public void RemoveCcTo(IEnumerable<string> emails)
        {
            if (emails != null && emails.Any())
            {
                foreach (string email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        _mailMsg.CC.Remove(new MailAddress(email));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the cc to.
        /// </summary>
        /// <param name="sep">The sep.</param>
        /// <returns></returns>
        public string GetCcTo(string sep = ";")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _mailMsg.CC)
            {
                sb.Append(item.Address).Append(sep);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Clears the BCC to.
        /// </summary>
        public void ClearBccTo()
        {
            this._mailMsg.Bcc.Clear();
        }

        /// <summary>
        /// Firstly Clear All BCC to Then Set the BCC to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void SetBccTo(string emails, string sep = ";")
        {
            this._mailMsg.Bcc.Clear();
            this.AddBccTo(emails, sep);
        }

        /// <summary>
        /// Adds the BCC to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void AddBccTo(string emails, string sep = ";")
        {
            string[] emailAry = emails.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAry)
            {
                if (!string.IsNullOrEmpty(email) && ValidationHelper.IsValidEmail(email) && !_mailMsg.Bcc.Contains(new MailAddress(email)))
                {
                    _mailMsg.Bcc.Add(new MailAddress(email));
                }
            }
        }

        /// <summary>
        /// Removes the BCC to.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void RemoveBccTo(string emails, string sep = ";")
        {
            string[] emailAry = emails.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAry)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    _mailMsg.Bcc.Remove(new MailAddress(email));
                }
            }
        }

        /// <summary>
        /// Removes the BCC to.
        /// </summary>
        /// <param name="emailSet">The email set.</param>
        public void RemoveBccTo(IEnumerable<string> emails)
        {
            if (emails != null && emails.Any())
            {
                foreach (string email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        _mailMsg.Bcc.Remove(new MailAddress(email));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the BCC to.
        /// </summary>
        /// <param name="sep">The sep.</param>
        /// <returns></returns>
        public string GetBccTo(string sep = ";")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _mailMsg.Bcc)
            {
                sb.Append(item.Address).Append(sep);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Clears the receiver black list.
        /// </summary>
        public void ClearReceiverBlackList()
        {
            this._receiverBlackList.Clear();
        }

        /// <summary>
        /// Sets the receiver black list.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void SetReceiverBlackList(string emails, string sep = ";")
        {
            this._receiverBlackList.Clear();
            this.AddReceiverBlackList(emails, sep);
        }

        /// <summary>
        /// Adds the receiver black list.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void AddReceiverBlackList(string emails, string sep = ";")
        {
            string[] emailAry = emails.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAry)
            {
                if (!string.IsNullOrEmpty(email) && ValidationHelper.IsValidEmail(email))
                {
                    this._receiverBlackList.Add(email);
                }
            }
        }

        /// <summary>
        /// Removes the receiver black list.
        /// </summary>
        /// <param name="emails">The mail address.</param>
        public void RemoveReceiverBlackList(string emails, string sep = ";")
        {
            string[] emailAry = emails.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAry)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    this._receiverBlackList.Remove(email);
                }
            }
        }

        /// <summary>
        /// Removes the receiver black list.
        /// </summary>
        /// <param name="emails">The emails.</param>
        public void RemoveReceiverBlackList(IEnumerable<string> emails)
        {
            if (emails != null && emails.Any())
            {
                foreach (string email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        this._receiverBlackList.Remove(email);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the receiver black list.
        /// </summary>
        /// <param name="sep">The sep.</param>
        /// <returns></returns>
        public string GetReceiverBlackList(string sep = ";")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this._receiverBlackList)
            {
                sb.Append(item).Append(sep);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Clears the attachment.
        /// </summary>
        public void ClearAttachment()
        {
            this._mailMsg.Attachments.Clear();
        }

        /// <summary>
        /// Sets the attachment.
        /// </summary>
        /// <param name="attachmentPath">The attachment path.</param>
        public void SetAttachment(string attachmentPath)
        {
            this._mailMsg.Attachments.Clear();
            this.AddAttachment(attachmentPath);
        }

        /// <summary>
        /// Adds the attachment.
        /// </summary>
        /// <param name="attachmentPath">The attachment path.</param>
        public void AddAttachment(string attachmentPath)
        {
            if (attachmentPath.Trim().Length > 0 && File.Exists(attachmentPath))
            {
                Attachment atAttachCount = new Attachment(attachmentPath);
                atAttachCount.Name = Path.GetFileName(attachmentPath);
                atAttachCount.NameEncoding = Encoding.GetEncoding("utf-8");
                atAttachCount.TransferEncoding = TransferEncoding.Base64;
                atAttachCount.ContentDisposition.Inline = true;
                atAttachCount.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                _mailMsg.Attachments.Add(atAttachCount);
            }
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        public void SendMail(string subject, StringBuilder content)
        {
            this.SendMail(subject, content.ToString());
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        public virtual void SendMail(string subject, string content)
        {
            if (this.HasAnyReceiver)
            {
                _mailMsg.Subject = subject;
                _mailMsg.Body = content;
                _smtpClient.Send(_mailMsg);
            }
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="content">The content.</param>
        public void SendMail(StringBuilder content)
        {
            this.SendMail(content.ToString());
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="content">The content.</param>
        public virtual void SendMail(string content)
        {
            if (this.HasAnyReceiver)
            {
                _mailMsg.Body = content;
                _smtpClient.Send(_mailMsg);
            }
        }
        #endregion
    }
}
