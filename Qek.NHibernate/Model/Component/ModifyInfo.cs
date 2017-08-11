using System;
using System.Xml.Serialization;

namespace Qek.NHibernate.Model
{
    [Serializable]
    public class ModifyInfo
    {
        #region Field Members    
        protected string _modifierID;
        protected string _modifierName;
        protected DateTime _modifyTime = DateTime.Now;
        #endregion Field Members

        #region Property Members        
        [XmlElement(ElementName = "ModifierID")]
        public virtual string ModifierID
        {
            get { return _modifierID; }
            set { _modifierID = value; }
        }
        [XmlElement(ElementName = "ModifierName")]
        public virtual string ModifierName
        {
            get { return _modifierName; }
            set { _modifierName = value; }
        }
        [XmlElement(ElementName = "ModifyTime")]
        public virtual DateTime ModifyTime
        {
            get { return _modifyTime; }
            set { _modifyTime = value; }
        }
        //be used in memory
        public virtual bool IsAnyModified
        {
            get;
            set;
        }
        #endregion Property Members

        public ModifyInfo()
        { }

        public ModifyInfo(string modifierID, string modifierName, DateTime modifyTime)
        {
            this._modifierID = modifierID;
            this._modifierName = modifierName;
            this._modifyTime = modifyTime;
        }
    }
}
