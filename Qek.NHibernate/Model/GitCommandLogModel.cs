using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class GitCommandLogModel : DomainObject<int>
    {
        #region Field Members
        private string _task;
        private string _taskTableName;
        private long? _taskFK;
        private string _inputCommand;
        private string _dataMessage;
        private string _errMessage;
        private int _totalInputCommand;
        private int _totalSucceedCommand;
        private bool _overallResult;
        private DateTime _createTime = DateTime.Now;
        #endregion Field Members

        #region Property Members
        [XmlElement(ElementName = "Task")]
        public virtual string Task
        {
            get { return _task; }
            set { _task = value; }
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
        [XmlElement(ElementName = "TotalInputCommand")]
        public virtual int TotalInputCommand
        {
            get { return _totalInputCommand; }
            set { _totalInputCommand = value; }
        }
        [XmlElement(ElementName = "TotalSucceedCommand")]
        public virtual int TotalSucceedCommand
        {
            get { return _totalSucceedCommand; }
            set { _totalSucceedCommand = value; }
        }
        [XmlElement(ElementName = "OverallResult")]
        public virtual bool OverallResult
        {
            get { return _overallResult; }
            set { _overallResult = value; }
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
            return (this._createTime).GetHashCode();
        }
        #endregion override Object's method
    }
}

