using Qek.Office.Word.WordTemplate;
using System;
using System.Data;

namespace Qek.Office.Example.Word.WordTemplate
{
    internal class ProjectSkusDataTable : ITableRenderable
    {
        public int ColCount { get; set; }
        public int RowCount { get; set; }
        public DataTable MyDataTable { get; set; }

        public ProjectSkusDataTable(ApTaskModel apTask)
        {
            this.SetDataSource(this.ConvertToDataTable(apTask));
        }

        private DataTable ConvertToDataTable(ApTaskModel apTask)
        {
            DataTable dt = new DataTable();
            DataRow row = null;
            dt.Columns.Add(new DataColumn("Project"));
            dt.Columns.Add(new DataColumn("Skus"));

            foreach (var item in apTask.MyApSubmitPreparations)
            {
                row = dt.NewRow();
                row["Project"] = item.AdsProjectName;
                row["Skus"] = item.SkuNames;
                dt.Rows.Add(row);
            }

            return dt;
        }
        public void SetDataSource(DataTable dataTable)
        {
            MyDataTable = dataTable;
            ColCount = 1;
            RowCount = MyDataTable.Rows.Count;
        }
        public void Render(Microsoft.Office.Interop.Word.Table table)
        {
            table.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            table.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            table.Range.Font.Name = "Calibri";

            // 填入表格資料
            int rowIndex = 1;
            foreach (DataRow row in MyDataTable.Rows)
            {
                ////table.Cell(rowIndex, 1).Range.Font.Bold = 1;
                ////table.Cell(rowIndex, 1).Range.Text = string.Format("{0}{1}({2})",
                ////    row["Project"].ToString(),
                ////    Environment.NewLine,
                ////    row["Skus"].ToString());

                Microsoft.Office.Interop.Word.Range range = table.Cell(rowIndex, 1).Range;

                Microsoft.Office.Interop.Word.Paragraph p1 = range.Paragraphs.Add(range);
                p1.Range.Font.Bold = 0;
                p1.Range.Font.Size = 10;
                p1.Range.Text = string.Format("({0})", row["Skus"].ToString());

                Microsoft.Office.Interop.Word.Paragraph p2 = range.Paragraphs.Add(range);
                p2.Range.Font.Bold = 1;
                p2.Range.Font.Size = 12;
                p2.Range.Text = row["Project"].ToString() + Environment.NewLine;

                rowIndex++;
            }
        }
    }
}
