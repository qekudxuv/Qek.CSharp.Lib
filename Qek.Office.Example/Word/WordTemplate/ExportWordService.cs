using Qek.Office.Word.WordTemplate;
using System;
using System.Collections;
using System.Data;
using System.Linq;

namespace Qek.Office.Example.Word.WordTemplate
{
    internal class ExportWordService
    {
        /// <summary>
        /// Exports the word.
        /// </summary>
        /// <param name="apTask">The ap task.</param>
        /// <param name="newFilePath">The new file path.</param>
        public void ExportWord(ApTaskModel apTask, string wordTemplateFilePath, string newFilePath)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AppName", apTask.AppName);
            ht.Add("RdManager", apTask.RdManager);
            ht.Add("SFM", apTask.SFM);
            ht.Add("PQCS#", apTask.MyApItsIssues == null || apTask.MyApItsIssues.Count == 0 ?
                "N/A" : apTask.MyApItsIssues.Select(c => new { c.ItsProjectName, c.ItsIssueNum })
                                .Aggregate(string.Empty, (current, its) => current + (its.ItsProjectName + " (" + its.ItsIssueNum + ")" + Environment.NewLine)));

            ht.Add("Symptom", apTask.Symptom);
            ht.Add("ReproSteps", apTask.ReproSteps);
            ht.Add("RootCause", apTask.RootCause);
            ht.Add("Solution", apTask.Solution);
            ht.Add("ImpactedScope", apTask.ImpactedScope);

            ITableRenderable dt = new ProjectSkusDataTable(apTask);
            ht.Add("ImpactedProjectsAndSkus", dt);
            ExportWordTool taskExportTool = new ExportWordTool(wordTemplateFilePath, ht);

            taskExportTool.Export(newFilePath);
        }
    }
}
