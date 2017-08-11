using Qek.Common.Dto.Tool;
using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class SignApkKeyLogModel : DomainObject<int>
    {
        #region Field Members
        private string _signPurpose;
        private string _taskTableName;
        private long? _taskFK;
        private bool _overallResult;
        private string _inputCommand;
        private string _dataMessage;
        private string _errMessage;
        private DateTime _createTime = DateTime.Now;
        #endregion Field Members

        #region Constructors
        public SignApkKeyLogModel()
        {

        }

        public SignApkKeyLogModel(string signPurpose, SignApkKeyLog log)
        {
            this._signPurpose = signPurpose;
            this._overallResult = log.OverallResult;
            this._inputCommand = log.InputCommand.ToString();
            this._dataMessage = log.DataMessage.ToString();
            this._errMessage = log.ErrMessage.ToString();
        }
        public SignApkKeyLogModel(string signPurpose, string taskTableName, SignApkKeyLog log)
            : this(signPurpose, log)
        {
            this._taskTableName = taskTableName;
        }
        public SignApkKeyLogModel(string signPurpose, string taskTableName, long taskFK, SignApkKeyLog log)
            : this(signPurpose, log)
        {
            this._taskTableName = taskTableName;
            this._taskFK = taskFK;
        }
        public SignApkKeyLogModel(string signPurpose, string taskTableName, int taskFK, SignApkKeyLog log)
            : this(signPurpose, log)
        {
            this._taskTableName = taskTableName;
            this._taskFK = (long)taskFK;
        }
        #endregion Field Constructors

        #region Property Members
        [XmlElement(ElementName = "SignPurpose")]
        public virtual string SignPurpose
        {
            get { return _signPurpose; }
            set { _signPurpose = value; }
        }
        [XmlElement(ElementName = "TaskTableName")]
        public virtual string TaskTableName
        {
            get { return _taskTableName; }
            set { _taskTableName = value; }
        }
        [XmlElement(ElementName = "TaskFK")]
        public virtual long? TaskFK
        {
            get { return _taskFK; }
            set { _taskFK = value; }
        }
        [XmlElement(ElementName = "OverallResult")]
        public virtual bool OverallResult
        {
            get { return _overallResult; }
            set { _overallResult = value; }
        }
        [XmlElement(ElementName = "InputCommand")]
        public virtual string InputCommand
        {
            get { return _inputCommand; }
            set { _inputCommand = value; }
        }
        [XmlElement(ElementName = "DataMessage")]
        public virtual string DataMessage
        {
            get { return _dataMessage; }
            set { _dataMessage = value; }
        }
        [XmlElement(ElementName = "ErrMessage")]
        public virtual string ErrMessage
        {
            get { return _errMessage; }
            set { _errMessage = value; }
        }
        [XmlElement(ElementName = "CreateTime")]
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        #endregion Property Members

        #region Override Object's method
        /// <summary>
        /// Hash code should ONLY contain the "business value signature" of the object and not the ID
        /// </summary>
        public override int GetHashCode()
        {
            return (this.CreateTime).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(
                "SignPurpose=[{0}]" + Environment.NewLine +
                "OverallResult=[{1}]" + Environment.NewLine +
                "InputCommand=[{2}]" + Environment.NewLine +
                "DataMessage=[{3}]" + Environment.NewLine +
                "ErrMessage=[{4}]",
                SignPurpose, OverallResult, InputCommand, DataMessage, ErrMessage);
        }
        #endregion override Object's method
    }
}
