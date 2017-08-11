using System.Data;

namespace Qek.Office.Word.WordTemplate
{
    /// <summary>
    /// DateTime:   2013/07/24
    /// Author:     Sam.SH_Chang#21978
    /// 若有DataTable資料需要輸出成表格格式，可實作此介面來制定表格格式。
    /// 實作範例請參考 
    /// 1) //MASDSystem/_Library/MASDKernel/Word/WordTemplate/TableRender
    //  2) //MASDSystem/_WebSystem/SUMS/SUMS.BLL/Tool/ExportWord/ProjectSkusDataTable.cs 
    /// </summary>
    public interface ITableRenderable
    {
        /// <summary>
        /// Gets or sets the col count.
        /// </summary>
        /// <value>The col count.</value>
        int ColCount { get; set; }

        /// <summary>
        /// Gets or sets the row count.
        /// </summary>
        /// <value>The row count.</value>
        int RowCount { get; set; }

        /// <summary>
        /// 資料來源
        /// </summary>
        /// <value>My data table.</value>
        DataTable MyDataTable { get; set; }

        /// <summary>
        /// 設定資料來源
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        void SetDataSource(DataTable dataTable);

        /// <summary>
        /// 將資料來源繪製出欲呈現的格式
        /// </summary>
        /// <param name="table">The table.</param>
        void Render(global::Microsoft.Office.Interop.Word.Table table);
    }
}
