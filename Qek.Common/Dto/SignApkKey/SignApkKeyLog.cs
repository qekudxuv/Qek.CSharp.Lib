using System;
using System.Text;

namespace Qek.Common.Dto.Tool
{
    public class SignApkKeyLog
    {
        private bool _overallResult;
        private StringBuilder _inputCommand = new StringBuilder();
        private StringBuilder _dataMessage = new StringBuilder();
        private StringBuilder _errMessage = new StringBuilder();

        public virtual bool OverallResult
        {
            get { return _overallResult; }
            set { _overallResult = value; }
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

        public override string ToString()
        {
            return string.Format(
                "OverallResult=[{0}]" + Environment.NewLine +
                "InputCommand=[{1}]" + Environment.NewLine +
                "DataMessage=[{2}]" + Environment.NewLine +
                "ErrMessage=[{3}]",
                OverallResult, InputCommand, DataMessage, ErrMessage);
        }
    }
}

