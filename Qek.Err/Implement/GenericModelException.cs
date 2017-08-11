using Newtonsoft.Json.Linq;
using System;

namespace Qek.Err
{
    [Serializable]
    public class GenericModelException<T> : Exception where T : class
    {
        private T _model = null;

        public GenericModelException(T model, string message)
            : base(message)
        {
            this._model = model;
        }

        public GenericModelException(T model, string message, Exception innerException)
            : base(message, innerException)
        {
            this._model = model;
        }

        public override string Message
        {
            get
            {
                return string.Format(
                    Environment.NewLine + "Class :【{0}】" +
                    Environment.NewLine + "Message :【{1}】" +
                    Environment.NewLine + "Data :【{2}】",
                    _model == null ? "" : _model.GetType().ToString(),
                    base.Message,
                    _model == null ? "" : JObject.FromObject(_model).ToString());
            }
        }
    }
}

