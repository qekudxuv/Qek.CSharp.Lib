namespace Qek.Common.Dto.Git
{
    public class GitCommandDto
    {
        #region Field Members
        private string _gitCommand = string.Empty;
        private int _sleepMilliSecond = 0;
        private bool _isReturnZeroAfterExecute = true;
        #endregion Field Members

        public string GitCommand
        {
            get { return _gitCommand; }
            set { _gitCommand = value; }
        }
        public int SleepMilliSecond
        {
            get { return _sleepMilliSecond; }
            set { _sleepMilliSecond = value; }
        }
        public bool IsReturnZeroAfterExecute
        {
            get { return _isReturnZeroAfterExecute; }
            set { _isReturnZeroAfterExecute = value; }
        }

        public GitCommandDto()
        {

        }
        public GitCommandDto(string gitCommand, int sleepMilliSecond, bool isReturnZeroAfterExecute)
        {
            this.GitCommand = gitCommand;
            this.SleepMilliSecond = sleepMilliSecond;
            this.IsReturnZeroAfterExecute = isReturnZeroAfterExecute;
        }
    }
}

