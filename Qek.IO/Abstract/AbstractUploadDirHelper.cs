using System;
using System.IO;

namespace Qek.IO
{
    public abstract class AbstractUploadDirHelper<T> : IUploadDirHelper<T> where T : struct, IConvertible
    {
        private string _myTaskDir;

        public AbstractUploadDirHelper(string myTaskDir)
        {
            if (string.IsNullOrEmpty(myTaskDir))
            {
                throw new Exception("myTaskDir is empty!");
            }
            if (!myTaskDir.EndsWith("\\"))
            {
                myTaskDir = myTaskDir + "\\";
            }
            this._myTaskDir = myTaskDir;
        }

        public string MyTaskDir
        {
            get { return _myTaskDir; }
        }
        /// <summary>
        /// 回傳存放檔案的路徑
        /// </summary>
        /// <param name="purpose">上傳檔案的分類</param>
        /// <returns></returns>
        public void ClearDir(T purpose, bool recursive)
        {
            string dirPath = string.Format(@"{0}{1}\", _myTaskDir, purpose.ToString());
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, recursive);
                ////foreach (var file in Directory.GetFiles(dirPath))
                ////{
                ////    try
                ////    {
                ////        //The reason is that although the process completes, it may take a few milliseconds for the OS to finish destroying it. 
                ////        //It's a multi-tasking OS after all, it's trying to service everyone at the same time. Unfortunetely, 
                ////        //there is no way to know when the file is no longer locked (presicely). 
                ////        //When you WaitForExit, you are creating a lock object on the process object. 
                ////        //The process object cannot be destroyed until the lock is released and you cannot wait on something that doesn't exist (chicken and egg kind of issue).

                ////        File.Delete(file);
                ////    }
                ////    catch (IOException)
                ////    {
                ////        System.Threading.Thread.Sleep(500);
                ////        this.ClearDir(purpose);
                ////    }
                ////}            
            }
        }

        /// <summary>
        /// 回傳存放檔案的路徑
        /// </summary>
        /// <param name="purpose">上傳檔案的分類</param>
        /// <returns></returns>
        public string ShowMeDir(T purpose)
        {
            return this.ShowMeDir(purpose, true);
        }

        /// <summary>
        /// 回傳存放檔案的路徑
        /// </summary>
        /// <param name="purpose">上傳檔案的分類</param>
        protected string ShowMeDir(string purpose)
        {
            return this.ShowMeDir(purpose, true);
        }

        /// <summary>
        /// 刪除資料匣下的檔案
        /// </summary>
        /// <param name="purpose">上傳檔案的分類</param>
        /// <param name="createFolderIfNotExist">若folder不存在的話，是否要建立</param>
        public string ShowMeDir(T purpose, bool createFolderIfNotExist)
        {
            return this.ShowMeDir(purpose.ToString(), createFolderIfNotExist);
        }

        /// <summary>
        /// 刪除資料匣下的檔案
        /// </summary>
        /// <param name="purpose">上傳檔案的分類</param>
        /// <param name="createFolderIfNotExist">若folder不存在的話，是否要建立</param>
        protected string ShowMeDir(string purpose, bool createFolderIfNotExist)
        {
            string dirPath = string.Format(@"{0}{1}\", _myTaskDir, purpose);
            if (createFolderIfNotExist && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }
    }
}
