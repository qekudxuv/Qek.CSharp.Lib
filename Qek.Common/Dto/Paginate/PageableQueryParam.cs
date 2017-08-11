using System;

namespace Qek.Common.Dto.Paginate
{
    [Serializable]
    public class PageableQueryParam : IPageableQueryParam
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
