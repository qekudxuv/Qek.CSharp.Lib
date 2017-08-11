using System;
using System.Collections.Generic;
using System.Linq;

namespace Qek.Common.Dto.Paginate
{
    [Serializable]
    public class PaginateResult<T>
    {
        //當前分頁的結果資料集
        public IEnumerable<T> PaginateResultList { get; set; }

        //當前分頁的筆數
        public int PaginateResultCount
        {
            get
            {
                int count = 0;
                if (this.PaginateResultList != null)
                {
                    count = this.PaginateResultList.Count();
                }
                return count;
            }
        }

        //查詢結果總筆數
        public int TotalCount { get; set; }

        //private constructor
        private PaginateResult(IEnumerable<T> paginateResultList, int totalCount)
        {
            this.PaginateResultList = paginateResultList;
            this.TotalCount = totalCount;
        }

        public static PaginateResult<T> Wrap(IEnumerable<T> paginateResultList, int totalCount)
        {
            return new PaginateResult<T>(paginateResultList, totalCount);
        }
    }
}
