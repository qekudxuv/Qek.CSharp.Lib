using Qek.Common.Dto.Git;

namespace Qek.Mail
{
    /// <summary>
    /// GitCommand Log Mail
    /// </summary>
    public class GitCommandLogMail : AbstractGitCommandLogMail
    {
        private string _mailReceivers;
        private string _mailSubject;

        public GitCommandLogMail(GitCommandLog log, string mailSubject, string mailReceivers)
            : base(log)
        {
            this._mailSubject = mailSubject;
            this._mailReceivers = mailReceivers;
        }

        protected override void SetMailReceiver()
        {
            base._mySmtpMail.SetMailTo(_mailReceivers);
        }

        protected override void SetMailAttachment()
        {
            return;
        }

        protected override string GetMailSubject()
        {
            return _mailSubject;
        }
    }
}

