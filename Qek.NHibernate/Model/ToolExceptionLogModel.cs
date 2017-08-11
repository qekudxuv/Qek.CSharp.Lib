using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class ToolExceptionLogModel : BaseExceptionLogModel
    {
        #region Field Members
        private string _machineName;
        private string _processName;
        private string _os;
        #endregion Field Members

        #region Property Members
        [XmlElement(ElementName = "MachineName")]
        public virtual string MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }
        [XmlElement(ElementName = "ProcessName")]
        public virtual string ProcessName
        {
            get { return _processName; }
            set { _processName = value; }
        }
        [XmlElement(ElementName = "OS")]
        public virtual string OS
        {
            get { return _os; }
            set { _os = value; }
        }
        #endregion Property Members
    }
}

