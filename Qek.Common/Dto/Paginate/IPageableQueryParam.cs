namespace Qek.Common.Dto.Paginate
{
    public interface IPageableQueryParam
    {
        //分頁索引
        int PageIndex { get; set; }

        //分頁大小
        int PageSize { get; set; }
    }
}
