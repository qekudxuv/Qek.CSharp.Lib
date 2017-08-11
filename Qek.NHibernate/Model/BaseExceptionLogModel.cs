using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class BaseExceptionLogModel : DomainObject<int>
    {
        #region Field Members
        private string _input;
        private string _source;
        private string _exceptionType;
        private string _exceptionMessage;
        private string _stackTrace;

        private string _innerExceptionSource;
        private string _innerExceptionType;
        private string _innerExceptionMessage;
        private string _innerExceptionStackTrace;
        private DateTime _createTime = DateTime.Now;
        #endregion Field Members

        #region Property Members
        [XmlElement(ElementName = "Input")]
        public virtual string Input
        {
            get { return _input; }
            set { _input = value; }
        }
        [XmlElement(ElementName = "Source")]
        public virtual string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        [XmlElement(ElementName = "ExceptionType")]
        public virtual string ExceptionType
        {
            get { return _exceptionType; }
            set { _exceptionType = value; }
        }
        [XmlElement(ElementName = "ExceptionMessage")]
        public virtual string ExceptionMessage
        {
            get { return _exceptionMessage; }
            set { _exceptionMessage = value; }
        }
        [XmlElement(ElementName = "StackTrace")]
        public virtual string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }
        [XmlElement(ElementName = "InnerExceptionSource")]
        public virtual string InnerExceptionSource
        {
            get { return _innerExceptionSource; }
            set { _innerExceptionSource = value; }
        }
        [XmlElement(ElementName = "InnerExceptionType")]
        public virtual string InnerExceptionType
        {
            get { return _innerExceptionType; }
            set { _innerExceptionType = value; }
        }
        [XmlElement(ElementName = "InnerExceptionMessage")]
        public virtual string InnerExceptionMessage
        {
            get { return _innerExceptionMessage; }
            set { _innerExceptionMessage = value; }
        }
        [XmlElement(ElementName = "InnerExceptionStackTrace")]
        public virtual string InnerExceptionStackTrace
        {
            get { return _innerExceptionStackTrace; }
            set { _innerExceptionStackTrace = value; }
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

