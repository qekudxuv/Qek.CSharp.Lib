using System.Collections.Generic;

namespace Qek.Mail
{
    public interface ISmtpService
    {
        string Subject { get; set; }
        bool IsBodyHtml { get; set; }
        bool HasAnyReceiver { get; }
        global::System.Net.Mail.MailPriority MailPriority { get; set; }

        void AddAttachment(string attachmentPath);
        void SetAttachment(string attachmentPath);
        void ClearAttachment();

        void AddMailTo(string emails, string sep = ";");
        void AddCcTo(string emails, string sep = ";");
        void AddBccTo(string emails, string sep = ";");
        void AddReceiverBlackList(string emails, string sep = ";");

        void RemoveMailTo(string emails, string sep = ";");
        void RemoveMailTo(IEnumerable<string> emails);
        void RemoveCcTo(string emails, string sep = ";");
        void RemoveCcTo(IEnumerable<string> emails);
        void RemoveBccTo(string emails, string sep = ";");
        void RemoveBccTo(IEnumerable<string> emails);
        void RemoveReceiverBlackList(string emails, string sep = ";");
        void RemoveReceiverBlackList(IEnumerable<string> emails);

        void ClearMailTo();
        void ClearCcTo();
        void ClearBccTo();
        void ClearReceiverBlackList();

        void SetMailTo(string emails, string sep = ";");
        void SetCcTo(string emails, string sep = ";");
        void SetBccTo(string emails, string sep = ";");
        void SetReceiverBlackList(string emails, string sep = ";");

        string GetMailTo(string sep = ";");
        string GetCcTo(string sep = ";");
        string GetBccTo(string sep = ";");
        string GetReceiverBlackList(string sep = ";");

        void SendMail(string content);
        void SendMail(string subject, string content);
        void SendMail(string subject, global::System.Text.StringBuilder content);
        void SendMail(global::System.Text.StringBuilder content);

        void SetMailFrom(string fromName, string mailFrom);
        void SetMailFrom(string mailFrom);
    }
}
