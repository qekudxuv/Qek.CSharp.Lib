using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class RequestInfo
    {
        #region Field Members    
        protected string _requestorID;
        protected string _requestorName;
        protected DateTime _requestTime = DateTime.Now;
        #endregion Field Members

        #region Property Members        
        [XmlElement(ElementName = "RequestorID")]
        public virtual string RequestorID
        {
            get { return _requestorID; }
            set { _requestorID = value; }
        }
        [XmlElement(ElementName = "RequestorName")]
        public virtual string RequestorName
        {
            get { return _requestorName; }
            set { _requestorName = value; }
        }
        [XmlElement(ElementName = "RequestTime")]
        public virtual DateTime RequestTime
        {
            get { return _requestTime; }
            set { _requestTime = value; }
        }
        #endregion Property Members

        public RequestInfo()
        { }

        public RequestInfo(string requestorID, string requestorName, DateTime requestTime)
        {
            this._requestorID = requestorID;
            this._requestorName = requestorName;
            this._requestTime = requestTime;
        }
    }
}
