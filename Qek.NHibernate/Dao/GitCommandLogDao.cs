using Qek.Common.Dto.Git;
using Qek.NHibernate.Model;
using System;

namespace Qek.NHibernate.Dao
{
    /// <summary>
    /// GitCommandLog Dao
    /// </summary>
    public class GitCommandLogDao : CRUDDao<GitCommandLogModel>
    {
        public GitCommandLogModel Save(GitCommandLog log, string systemShortName, string taskTableName = "", long? taskFK = null)
        {
            GitCommandLogModel logModel = new GitCommandLogModel();
            logModel.Task = systemShortName;
            logModel.TaskTableName = taskTableName;
            logModel.TaskFK = taskFK;
            logModel.InputCommand = log.InputCommand.ToString();
            logModel.DataMessage = log.DataMessage.ToString();
            logModel.ErrMessage = log.ErrMessage.ToString();
            logModel.TotalInputCommand = log.TotalInputCommand;
            logModel.TotalSucceedCommand = log.TotalSucceedCommand;
            logModel.OverallResult = log.OverallResult;
            logModel.CreateTime = DateTime.Now;

            return base.Save(logModel);
        }
    }
}

