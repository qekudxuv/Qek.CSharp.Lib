using System;
using System.Web;

namespace Qek.Err
{
    public interface IExceptionHandler
    {
        /// <summary>
        /// 表示是TOOL Exception
        /// 讓各系統實作，要如何對exception做處理
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="input">The input.</param>
        void Handle(Exception ex, string input = "");

        /// <summary>
        /// 表示是TOOL Exception
        /// 讓各系統實作，要如何對exception做處理
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="input">The input.</param>
        void Handle(Exception ex, object input);

        /// <summary>
        /// 表示是WEB Exception
        /// 讓各系統實作，要如何對exception做處理
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="input">The input.</param>
        void Handle(Exception ex, string account, HttpContext ctx, string input = "");

        /// <summary>
        /// 表示是WEB Exception
        /// 讓各系統實作，要如何對exception做處理
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="account">The account.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="input">The input.</param>
        void Handle(Exception ex, string account, HttpContext ctx, object input);
    }
}

