using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class CreateInfo
    {
        #region Field Members    
        protected string _creatorID;
        protected string _creatorName;
        protected DateTime _createTime = DateTime.Now;
        #endregion Field Members

        #region Property Members        
        [XmlElement(ElementName = "CreatorID")]
        public virtual string CreatorID
        {
            get { return _creatorID; }
            set { _creatorID = value; }
        }
        [XmlElement(ElementName = "CreatorName")]
        public virtual string CreatorName
        {
            get { return _creatorName; }
            set { _creatorName = value; }
        }
        [XmlElement(ElementName = "CreateTime")]
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        #endregion Property Members

        public CreateInfo()
        { }

        public CreateInfo(string creatorID, string creatorName, DateTime createTime)
        {
            this._creatorID = creatorID;
            this._creatorName = creatorName;
            this._createTime = createTime;
        }
    }
}
