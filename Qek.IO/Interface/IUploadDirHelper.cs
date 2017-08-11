namespace Qek.IO
{
    public interface IUploadDirHelper<T>
    {
        string MyTaskDir { get; }

        /// <summary>
        /// 回傳存放檔案的路徑
        /// </summary>
        /// <param name="purpose">上傳檔案的分類</param>
        /// <returns></returns>
        string ShowMeDir(T purpose);

        /// <summary>
        /// 回傳存放檔案的路徑
        /// </summary>
        /// <param name="purpose">上傳檔案的分類</param>
        /// <param name="createFolderIfNotExist">若folder不存在的話，是否要建立</param>
        /// <returns></returns>
        string ShowMeDir(T purpose, bool createFolderIfNotExist);

        /// <summary>
        /// 刪除資料匣下的檔案
        /// </summary>
        /// <param name="purpose">上傳檔案的分類</param>
        void ClearDir(T purpose, bool recursive);
    }
}

