using Qek.Common;
using Qek.Common.Dto.Git;
using Qek.Err;
using System;
using System.Collections.Generic;

namespace Qek.Git
{
    /// <summary>
    /// Abstact class for git tag task
    /// </summary>
    public abstract class AbstractTagTask<T> where T : IGitReleaseModel
    {
        protected string _systemShortName = ConfigHelper.GetAppSetting("SystemShortName");
        protected string _gitAccount = ConfigHelper.GetAppSetting("GitAccount");
        protected string _gitServer = ConfigHelper.GetAppSetting("GitServer");
        protected string _gitAccountEmail = ConfigHelper.GetAppSetting("GitAccountEmail");

        protected MyGitBash _gitBash = new MyGitBash();
        protected IExceptionHandler _exceptionHandler = null;

        public AbstractTagTask()
        {
        }

        public AbstractTagTask(IExceptionHandler exceptionHandler)
        {
            this._exceptionHandler = exceptionHandler;
        }

        public void Execute()
        {
            try
            {
                IList<T> ReleaseDataList = this.GetReleaseRecordList();

                foreach (T model in ReleaseDataList)
                {
                    GitCommandLog log = Execute(model);
                    if (log != null)
                    {
                        this.UpdateReleaseRecord(model.ID, log.OverallResult);
                        this.RecordAndNotify(log, model.GitName, model.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_exceptionHandler != null)
                {
                    _exceptionHandler.Handle(ex);
                }
            }
        }

        private GitCommandLog Execute(T model)
        {
            GitCommandLog log = new GitCommandLog();
            try
            {
                string repoName = model.GitName;
                bool isRepoExist = _gitBash.RepoExist(repoName);
                if (string.IsNullOrEmpty(repoName))
                {
                    throw new Exception("[Warn] RepoName is empty.");
                }


                if (!isRepoExist)
                {
                    log.Merge(_gitBash.CloneRepo(_gitAccount, _gitServer, model.GitName));
                    if (!log.OverallResult)
                    {
                        throw new GenericModelException<GitCommandLog>(log, string.Format("[Warn] CloneRepo {0} failed", repoName));
                    }

                    log.Merge(_gitBash.SetRepoConfig(repoName, _gitAccount, _gitAccountEmail));
                    if (!log.OverallResult)
                    {
                        throw new GenericModelException<GitCommandLog>(log, string.Format("[Warn] SetRepoConfig {0} failed", repoName));
                    }
                }
                else
                {
                    log.Merge(_gitBash.FetchRepo(repoName));
                    if (!log.OverallResult)
                    {
                        throw new GenericModelException<GitCommandLog>(log, string.Format("[Warn] FetchRepo {0} failed", repoName));
                    }
                }

                ////string tagName = TagMaker(model);
                if (string.IsNullOrEmpty(model.TagName))
                {
                    throw new GenericModelException<GitCommandLog>(log, "[Warn] TagName is empty.");
                }
                else
                {
                    log.Merge(_gitBash.TagCommitID(repoName, model.TagName, model.ReleaseCommitID,
                        string.Format("{0} tool tagged", _systemShortName)));
                    if (!log.OverallResult)
                    {
                        throw new GenericModelException<GitCommandLog>(log, string.Format("[Fatal] {0} tagging failed", repoName));
                    }
                }

                //check local tag
                log.Merge(_gitBash.VerifyLocalTag(repoName, model.TagName, model.ReleaseCommitID));
                if (!log.OverallResult)
                {
                    throw new GenericModelException<GitCommandLog>(log, "[Fatal] VerifyLocalTag failed.");
                }

                //check remote tag
                log.Merge(_gitBash.VerifyRemoteTag(_gitAccount, _gitServer, model.GitName, model.TagName, model.ReleaseCommitID));
                if (!log.OverallResult)
                {
                    throw new GenericModelException<GitCommandLog>(log, "[Fatal] VerifyRemoteTag failed.");
                }
            }
            catch (Exception ex)
            {
                if (_exceptionHandler != null)
                {
                    _exceptionHandler.Handle(ex, model);
                }
            }
            return log;
        }

        /// <summary>
        /// 實作依據release data的某個欄位狀態條件
        /// 去取得 release data.
        /// </summary>
        /// <returns></returns>
        protected abstract IList<T> GetReleaseRecordList();

        /// <summary>
        /// 實作更新release data的某個欄位，讓下次抓release data的時候
        /// 不會再抓到此筆。
        /// </summary>
        /// <param name="UID">The PK.</param>
        /// <param name="isTagOk">Is Tag OK.</param>
        protected abstract void UpdateReleaseRecord(long UID, bool isTagOk);

        /// <summary>
        /// 實作依據傳入的model製作出來Tag.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        ////protected abstract string TagMaker(T model);

        /// <summary>
        /// 實作處理(紀錄 或 通知)
        /// 執行git command所產生的log
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="git">The git.</param>
        protected abstract void RecordAndNotify(GitCommandLog log, string git, long taskFK);
    }
}
