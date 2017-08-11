using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class ReviewInfo<T>
    {
        #region Field Members
        protected T _reviewStatus;
        protected string _reviewerID;
        protected string _reviewerName;
        protected DateTime _reviewTime = DateTime.Now;
        #endregion Field Members

        #region Property Members
        [XmlElement(ElementName = "ReviewStatus")]
        public virtual T ReviewStatus
        {
            get { return _reviewStatus; }
            set { _reviewStatus = value; }
        }
        [XmlElement(ElementName = "ReviewerID")]
        public virtual string ReviewerID
        {
            get { return _reviewerID; }
            set { _reviewerID = value; }
        }
        [XmlElement(ElementName = "ReviewerName")]
        public virtual string ReviewerName
        {
            get { return _reviewerName; }
            set { _reviewerName = value; }
        }
        [XmlElement(ElementName = "ReviewTime")]
        public virtual DateTime ReviewTime
        {
            get { return _reviewTime; }
            set { _reviewTime = value; }
        }
        #endregion Property Members

        public ReviewInfo()
        { }

        public ReviewInfo(T reviewStatus, string reviewerID, string reviewerName, DateTime reviewTime)
        {
            this._reviewStatus = reviewStatus;
            this._reviewerID = reviewerID;
            this._reviewerName = reviewerName;
            this._reviewTime = reviewTime;
        }
    }
}
