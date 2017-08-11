using Qek.Common.Dto.Git;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Qek.Mail
{
    public abstract class AbstractGitCommandLogMail : AbstractMailTemplate
    {
        private static readonly string _mailTemplate;
        protected GitCommandLog _gitCommandLog;

        static AbstractGitCommandLogMail()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyFilename = assembly.GetName().Name;

            using
            (
                StreamReader sr = new StreamReader(
                    assembly.GetManifestResourceStream(assemblyFilename + ".Mail.EmailTemplate.GitCommandLogMail.htm"))
            )
            {
                _mailTemplate = sr.ReadToEnd();
            }
        }

        public AbstractGitCommandLogMail(GitCommandLog gitCommandLog)
            : base(BaseHtmlSmtpMail.GenMailObjByAppConfig())
        {
            this._gitCommandLog = gitCommandLog;
        }

        protected override bool Verify()
        {
            return this._gitCommandLog != null;
        }

        protected override string GetMailContent()
        {
            StringBuilder mailContent = new StringBuilder(_mailTemplate);

            mailContent = mailContent.Replace("${IsSucceed}", _gitCommandLog.OverallResult.ToString());
            mailContent = mailContent.Replace("${TotalInputCommand}", _gitCommandLog.TotalInputCommand.ToString());
            mailContent = mailContent.Replace("${TotalSucceedCommand}", _gitCommandLog.TotalSucceedCommand.ToString());
            mailContent = mailContent.Replace("${InputCommand}", _gitCommandLog.InputCommand.ToString().Replace(Environment.NewLine, "<br />"));
            mailContent = mailContent.Replace("${DataMessage}", _gitCommandLog.DataMessage.ToString().Replace(Environment.NewLine, "<br />"));
            mailContent = mailContent.Replace("${ErrMessage}", _gitCommandLog.ErrMessage.ToString().Replace(Environment.NewLine, "<br />"));

            return mailContent.ToString();
        }
    }
}

