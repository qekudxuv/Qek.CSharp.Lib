using System.Text;

namespace Qek.Common.Dto.Git
{
    public class GitCommandLog
    {
        private StringBuilder _inputCommand = new StringBuilder();
        private StringBuilder _dataMessage = new StringBuilder();
        private StringBuilder _errMessage = new StringBuilder();
        private int _totalInputCommand = 0;
        private int _totalSucceedCommand = 0;
        private bool? _overallResult;
        private int _mergeCount = 0;

        public GitCommandLog()
        {

        }

        public GitCommandLog(GitCommandLog log)
        {
            this._inputCommand = log.InputCommand;
            this._dataMessage = log.DataMessage;
            this._errMessage = log.ErrMessage;
            this._totalInputCommand = log.TotalInputCommand;
            this._totalSucceedCommand = log.TotalSucceedCommand;
        }

        public StringBuilder InputCommand
        {
            get { return _inputCommand; }
            set { _inputCommand = value; }
        }
        public StringBuilder DataMessage
        {
            get { return _dataMessage; }
            set { _dataMessage = value; }
        }
        public StringBuilder ErrMessage
        {
            get { return _errMessage; }
            set { _errMessage = value; }
        }
        public int TotalInputCommand
        {
            get { return _totalInputCommand; }
            set { _totalInputCommand = value; }
        }
        public int TotalSucceedCommand
        {
            get { return _totalSucceedCommand; }
            set { _totalSucceedCommand = value; }
        }

        /// <summary>
        /// Gets a value indicating whether [overall result].
        /// 若Overall Result需要更細膩的判斷可以自行設值至CustomerOverallResult
        /// </summary>
        /// <value>
        ///   <c>true</c> if [overall result]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool OverallResult
        {
            get
            {
                if (this._overallResult.HasValue)
                {
                    return (bool)this._overallResult;
                }
                else
                {
                    return (TotalInputCommand == TotalSucceedCommand);
                }
            }
            set
            {
                this._overallResult = value;
            }
        }

        public void Merge(GitCommandLog log)
        {
            _mergeCount++;

            this.InputCommand.AppendLine("======================== Merge InputCommand " + _mergeCount + " ========================")
                .AppendLine(log.InputCommand.ToString());
            this.DataMessage.AppendLine("======================== Merge DataMessage " + _mergeCount + " ========================")
                .AppendLine(log.DataMessage.ToString());
            this.ErrMessage.AppendLine("======================== Merge ErrMessage " + _mergeCount + " ========================")
                .AppendLine(log.ErrMessage.ToString());
            this.TotalInputCommand += log.TotalInputCommand;
            this.TotalSucceedCommand += log.TotalSucceedCommand;
            bool overallResult1 = this.OverallResult;
            bool overallResult2 = log.OverallResult;
            this.OverallResult = overallResult1 && overallResult2;
            //this.OverallResult = this.OverallResult && log.OverallResult;
        }
    }
}