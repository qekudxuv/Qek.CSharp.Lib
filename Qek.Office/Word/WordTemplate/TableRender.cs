using System.Data;

namespace Qek.Office.Word.WordTemplate
{
    /// <summary>
    /// DateTime:   2013/07/24
    /// Author:     Sam.SH_Chang#21978
    /// 若來源資料為DataTable，則Qek.Office.Word.WordTemplate.ExportWordTool
    /// 會以此做為預設表格格式來將資料輸出
    /// </summary>
    public class TableRender : ITableRenderable
    {
        public int ColCount { get; set; }
        public int RowCount { get; set; }
        public DataTable MyDataTable { get; set; }

        public TableRender()
        {
        }
        public TableRender(DataTable dataTable)
        {
            this.SetDataSource(dataTable);
        }

        public void SetDataSource(DataTable dataTable)
        {
            this.MyDataTable = dataTable;
            this.ColCount = dataTable.Columns.Count;
            this.RowCount = dataTable.Rows.Count + 1;
        }

        public void Render(Microsoft.Office.Interop.Word.Table table)
        {
            table.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            table.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            table.Range.Font.Name = "Calibri";

            // 填入表格欄位(Column)名稱
            for (int i = 0; i < table.Columns.Count; i++)
            {
                table.Cell(1, i + 1).Range.Text = MyDataTable.Columns[i].ColumnName;
            }

            // 填入表格資料
            int rowIndex = 2;
            foreach (DataRow row in MyDataTable.Rows)
            {
                int colIndex = 1;
                foreach (DataColumn col in MyDataTable.Columns)
                {
                    table.Cell(rowIndex, colIndex).Range.Text = row[col.ColumnName].ToString();
                    colIndex++;
                }
                rowIndex++;
            }
        }
    }
}
