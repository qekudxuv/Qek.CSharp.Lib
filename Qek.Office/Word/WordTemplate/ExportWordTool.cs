using Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.IO;
using DataTable = System.Data.DataTable;

namespace Qek.Office.Word.WordTemplate
{
    /// <summary>
    /// DateTime:   2013/07/24
    /// Author:     Sam.SH_Chang#21978
    /// 主要負責以Work Template製作出Word
    /// 使用範例請參考 //MASDSystem/_WebSystem/SUMS/SUMS.BLL/ApTaskBlo.cs 方法: ExportWord
    /// </summary>
    public class ExportWordTool : IExportable
    {
        private readonly string _templateFilePath = string.Empty;
        private Hashtable _dataHashtable = null;
        private ITableRenderable _defaultTableRender = new TableRender();

        public ExportWordTool(string templateFilePath, Hashtable dataHashtable)
        {
            this._templateFilePath = templateFilePath;
            this._dataHashtable = dataHashtable;
        }

        public void SetDefaultTableRender(ITableRenderable defaultTableRender)
        {
            this._defaultTableRender = defaultTableRender;
        }

        public void Export(string newFilePath)
        {
            Object oMissing = System.Reflection.Missing.Value;
            Object oTemplatePath = _templateFilePath;
            Application wordApp = null;
            Document wordDoc = null;

            try
            {
                wordApp = new Application();
                wordApp.Visible = false;
                wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
                if (wordDoc == null)
                {
                    throw new Exception(
                        string.Format("wordDoc is null, oTemplatePath={0}, CurrentUser={1}",
                        oTemplatePath.ToString(),
                        System.Security.Principal.WindowsIdentity.GetCurrent().Name)
                    );
                }
                foreach (Field myMergeField in wordDoc.Fields)
                {
                    Range rngFieldCode = myMergeField.Code;

                    String fieldText = rngFieldCode.Text;

                    // ONLY GETTING THE MAILMERGE FIELDS
                    if (fieldText.StartsWith(" MERGEFIELD"))
                    {
                        // THE TEXT COMES IN THE FORMAT OF
                        // MERGEFIELD  MyFieldName  \\* MERGEFORMAT
                        // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"

                        Int32 endMerge = fieldText.IndexOf("\\");
                        Int32 fieldNameLength = fieldText.Length - endMerge;
                        String fieldName = fieldText.Substring(11, endMerge - 11);

                        // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .dot FILE
                        fieldName = fieldName.Trim();

                        if (_dataHashtable.ContainsKey(fieldName))
                        {
                            Object value = _dataHashtable[fieldName];
                            if (value is string || value.GetType().IsPrimitive)
                            {
                                myMergeField.Select();
                                wordApp.Selection.Range.Font.Name = "Calibri";
                                wordApp.Selection.TypeText(value.ToString());
                            }
                            else if (value is DataTable)//會使用預設方法將資料呈現出來
                            {
                                myMergeField.Select();
                                this._defaultTableRender.SetDataSource((DataTable)value);
                                Table table = wordApp.Selection.Tables.Add(myMergeField.Result, _defaultTableRender.RowCount, _defaultTableRender.ColCount, ref oMissing, ref oMissing);
                                this._defaultTableRender.Render(table);
                            }
                            else if (value is ITableRenderable)//要自已實作介面,將欲呈現的format實作出來
                            {
                                myMergeField.Select();
                                var tableRender = (ITableRenderable)value;
                                Table table = wordApp.Selection.Tables.Add(myMergeField.Result, tableRender.RowCount, tableRender.ColCount, ref oMissing, ref oMissing);
                                tableRender.Render(table);
                            }
                            else
                            {
                                throw new ArgumentException(string.Format("{0} is not an acceptable type for rendering", value.GetType().Name));
                            }
                        }
                    }
                }

                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }

                //wordDoc.SaveAs(newFilePath);
                //wordApp.Documents.Open(newFilePath);               

                object objFileName = newFilePath;
                //object FileFormat = WdSaveFormat.wdFormatRTF;
                //object LockComments = false;
                //object AddToRecentFiles = true;
                //object ReadOnlyRecommended = false;
                //object EmbedTrueTypeFonts = false;
                //object SaveNativePictureFormat = true;
                //object SaveFormsData = true;
                //object SaveAsAOCELetter = false;
                //object Encoding = Microsoft.Office.Interop.Word.msoEncodingUSASCII;
                //object InsertLineBreaks = false;
                //object AllowSubstitutions = false;
                //object LineEnding = WdLineEndingType.wdCRLF;
                //object AddBiDiMarks = false;

                wordDoc.SaveAs(ref objFileName,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            }
            finally
            {
                if (wordApp != null)
                {
                    ((_Application)wordApp).Quit(ref oMissing, ref oMissing, ref oMissing);
                }

                if (wordDoc != null)
                {
                    wordDoc = null;
                }

                wordApp = null;
            }
        }
    }
}
