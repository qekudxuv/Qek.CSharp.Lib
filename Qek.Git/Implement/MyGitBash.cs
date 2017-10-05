using Qek.Common.Dto.Git;
using System.Collections.Generic;
using System.IO;

namespace Qek.Git
{
    /// <summary>
    /// 此類別的存在
    /// 1) 主要是讓BaseGitBash不被App.config綁定
    /// 2) 各Method依據需求，將 git command 組成一段 script
    /// </summary>
    public class MyGitBash : BaseGitBash
    {
        public MyGitBash(string gitBashFile, string workingDirectory)
            : base(gitBashFile, workingDirectory)
        {
        }

        public GitCommandLog GC(string repoName)
        {
            GitCommandLog log = null;
            this.SwitchRepo(repoName);

            IList<string> script = new List<string>();
            script.Add("git gc");
            script.Add("git prune");
            script.Add("git fetch -p --all");
            log = base.RunScript(script);

            return log;
        }

        public GitCommandLog TagCommitID(string repoName, string tagName, string commitID, string comment)
        {
            GitCommandLog log = null;
            this.SwitchRepo(repoName);

            string tagPrefix = "refs/tags/";
            if (tagName.StartsWith(tagPrefix))
            {
                tagName = tagName.Replace(tagPrefix, "");
            }

            IList<GitCommandDto> script = new List<GitCommandDto>();
            //Remove local tag
            script.Add(new GitCommandDto()
            {
                GitCommand = string.Format("git tag -d {0}", tagName)
            });
            //Add local tag
            script.Add(new GitCommandDto()
            {
                GitCommand = string.Format("git tag -a {0} {1} -m \"{2}\"", tagName, commitID, comment)
            });
            //Push specific tag to remote site
            script.Add(new GitCommandDto()
            {
                GitCommand = string.Format("git push origin refs/tags/{0} -f", tagName),
                SleepMilliSecond = 2000//暫停2秒
            });
            //Push all tag to remote site
            //script.Add(new GitCommandDto()
            //    {
            //        GitCommand = "git push origin --tag",
            //        SleepMilliSecond = 2000//暫停2秒
            //    });
            log = base.RunScript(script);
            return log;
        }

        public GitCommandLog VerifyLocalTag(string repoName, string tagName, string commitID)
        {
            this.SwitchToRoot();
            string tagPrefix = "refs/tags/";
            if (tagName.StartsWith(tagPrefix))
            {
                tagName = tagName.Replace(tagPrefix, "");
            }
            GitCommandLog log = base.RunScript(string.Format("git ls-remote {0} refs/tags/{1}^{{}}", repoName, tagName));
            log.OverallResult = log.DataMessage.ToString().Contains(commitID);
            return log;
        }

        public GitCommandLog VerifyRemoteTag(string gitAccount, string gitServer, string gitName, string tagName, string commitID)
        {
            this.SwitchToRoot();
            string tagPrefix = "refs/tags/";
            if (tagName.StartsWith(tagPrefix))
            {
                tagName = tagName.Replace(tagPrefix, "");
            }
            GitCommandLog log = base.RunScript(string.Format("git ls-remote ssh://{0}@{1}/{2} refs/tags/{3}^{{}}", gitAccount, gitServer, gitName, tagName));
            log.OverallResult = log.DataMessage.ToString().Contains(commitID);
            return log;
        }

        public GitCommandLog CloneRepo(string gitAccount, string gitServer, string gitName, string branchName = "")
        {
            GitCommandLog log = null;
            this.SwitchToRoot();
            if (!string.IsNullOrEmpty(branchName))
            {
                log = base.RunScript(
                    string.Format("git clone ssh://{0}@{1}/{2} -b {3} {2}",
                        gitAccount,
                        gitServer,
                        gitName,
                        branchName));
            }
            else
            {
                log = base.RunScript(
                        string.Format("git clone ssh://{0}@{1}/{2} {2}",
                            gitAccount,
                            gitServer,
                            gitName));
            }
            return log;
        }

        public GitCommandLog PullRepo(string repoName)
        {
            GitCommandLog log = null;
            this.SwitchRepo(repoName);
            log = base.RunScript("git pull");
            return log;
        }

        public GitCommandLog FetchRepo(string repoName)
        {
            GitCommandLog log = null;
            this.SwitchRepo(repoName);
            log = base.RunScript("git fetch --all");
            return log;
        }

        public GitCommandLog SetRepoConfig(string repoName, string gitAccountName, string gitAccountEmail)
        {
            GitCommandLog log = null;
            this.SwitchRepo(repoName);
            IList<string> script = new List<string>();
            script.Add(string.Format("git config user.name {0}", gitAccountName));
            script.Add(string.Format("git config user.email {0}", gitAccountEmail));
            script.Add("git config core.autocrlf input");
            script.Add("git config core.safecrlf true");
            log = base.RunScript(script);
            return log;
        }

        //public bool RepoExist(string remoteGitName, out string localRepoName)
        //{
        //    bool isRepoExist = false;
        //    localRepoName = string.Empty;
        //    string[] spRepo = remoteGitName.Trim().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (spRepo.Length > 0)
        //    {
        //        isRepoExist = Directory.Exists(base.RootWorkingDirectory + spRepo[spRepo.Length - 1]);
        //        localRepoName = spRepo[spRepo.Length - 1];
        //    }
        //    return isRepoExist;
        //}

        public bool RepoExist(string remoteGitName)
        {
            return Directory.Exists(base.RootWorkingDirectory + remoteGitName);
        }
    }
}

