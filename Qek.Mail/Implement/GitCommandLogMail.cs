using Qek.Common;
using Qek.Common.Dto.Git;

namespace Qek.Mail
{
    /// <summary>
    /// GitCommand Log Mail
    /// </summary>
    public class GitCommandLogMail : AbstractGitCommandLogMail
    {
        private string _adminMailReceiver = ConfigHelper.GetAppSetting("GitCommandLogMailReceiver");
        private string _systemShortName = ConfigHelper.GetAppSetting("SystemShortName");
        private string _mailSubject;

        public GitCommandLogMail(GitCommandLog log, string mailSubject)
            : base(log)
        {
            this._mailSubject = mailSubject;
        }

        protected override void SetMailReceiver()
        {
            base._mySmtpMail.SetMailTo(_adminMailReceiver);
        }

        protected override void SetMailAttachment()
        {
            return;
        }

        protected override string GetMailSubject()
        {
            return string.Format("[{0}][{1}]",
                _systemShortName,
                _mailSubject);
        }
    }
}

