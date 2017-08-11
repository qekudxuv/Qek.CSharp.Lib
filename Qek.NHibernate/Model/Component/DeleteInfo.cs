using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class DeleteInfo
    {
        #region Field Members    
        protected string _deletorID;
        protected string _deletorName;
        protected DateTime? _deleteTime;
        protected bool _isDeleted = false;
        #endregion Field Members

        #region Property Members        
        [XmlElement(ElementName = "DeletorID")]
        public virtual string DeletorID
        {
            get { return _deletorID; }
            set { _deletorID = value; }
        }
        [XmlElement(ElementName = "DeletorName")]
        public virtual string DeletorName
        {
            get { return _deletorName; }
            set { _deletorName = value; }
        }
        [XmlElement(ElementName = "DeleteTime")]
        public virtual DateTime? DeleteTime
        {
            get { return _deleteTime; }
            set { _deleteTime = value; }
        }
        [XmlElement(ElementName = "IsDeleted")]
        public virtual bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }
        #endregion Property Members

        public DeleteInfo()
        { }

        public DeleteInfo(string deletorID, string deletorName, DateTime deleteTime, bool isDeleted)
        {
            this._deletorID = deletorID;
            this._deletorName = deletorName;
            this._deleteTime = deleteTime;
            this._isDeleted = isDeleted;
        }
    }
}
