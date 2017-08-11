using System;

namespace Qek.Common.Dto.Json
{
    [Serializable]
    public class BaseJsonDto
    {
        private bool _isSuccess = true;
        private string _errorMessage = string.Empty;

        public BaseJsonDto()
        {

        }
        public BaseJsonDto(bool isSuccess, string errorMessage)
        {
            this._isSuccess = isSuccess;
            this._errorMessage = errorMessage;
        }

        public bool IsSuccess
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}

