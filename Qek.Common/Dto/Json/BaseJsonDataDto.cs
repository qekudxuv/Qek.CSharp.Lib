using System;

namespace Qek.Common.Dto.Json
{
    [Serializable]
    public class BaseJsonDataDto<T> : BaseJsonDto
    {
        private T _data;

        public T Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public BaseJsonDataDto()
        {

        }

        public BaseJsonDataDto(bool isSuccess, string errorMessage, T data)
            : base(isSuccess, errorMessage)
        {
            _data = data;
        }
    }
}

