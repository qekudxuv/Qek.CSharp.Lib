using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class WebExceptionLogModel : BaseExceptionLogModel
    {
        #region Field Members
        private string _userName;
        private string _remoteAddress;
        private string _requestUrl;
        private string _clientInfo;
        #endregion Field Members

        #region Property Members
        [XmlElement(ElementName = "UserName")]
        public virtual string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        [XmlElement(ElementName = "RemoteAddress")]
        public virtual string RemoteAddress
        {
            get { return _remoteAddress; }
            set { _remoteAddress = value; }
        }
        [XmlElement(ElementName = "RequestUrl")]
        public virtual string RequestUrl
        {
            get { return _requestUrl; }
            set { _requestUrl = value; }
        }
        [XmlElement(ElementName = "ClientInfo")]
        public virtual string ClientInfo
        {
            get { return _clientInfo; }
            set { _clientInfo = value; }
        }
        #endregion Property Members
    }
}

