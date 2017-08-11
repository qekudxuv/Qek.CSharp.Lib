using System;
using System.Collections.Generic;
using System.Linq;

namespace Qek.Common.Dto.Json
{
    [Serializable]
    public class BaseJsonDataListDto<T> : BaseJsonDto
    {
        private IEnumerable<T> _dataList;

        public IEnumerable<T> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }

        public int DataCount
        {
            get
            {
                return _dataList == null ? 0 : _dataList.Count();
            }
        }

        public BaseJsonDataListDto()
        {

        }

        public BaseJsonDataListDto(bool isSuccess, string errorMessage, IEnumerable<T> dataList)
            : base(isSuccess, errorMessage)
        {
            _dataList = dataList;
        }
    }
}

