using Qek.Office.Example.Word.WordTemplate;
using System.Collections.Generic;
using System.IO;

namespace Qek.Office.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //prepare fake data
            ApTaskModel model = new ApTaskModel
            {
                AppName = "AppName",
                RdManager = "RdManager",
                SFM = "SFM",
                Symptom = "Symptom",
                ReproSteps = "ReproSteps",
                RootCause = "RootCause",
                Solution = "Solution",
                ImpactedScope = "ImpactedScope",

                MyApItsIssues = new HashSet<ApItsIssueModel>()
                {
                    new ApItsIssueModel { ItsProjectName = "ItsProjectName 1", ItsIssueNum = 1},
                    new ApItsIssueModel { ItsProjectName = "ItsProjectName 2", ItsIssueNum = 2}
                },

                MyApSubmitPreparations = new HashSet<ApSubmitPreparationModel>()
                {
                    new ApSubmitPreparationModel { AdsProjectName = "AdsProjectName 1", SkuNames = "SkuNames 1" },
                    new ApSubmitPreparationModel { AdsProjectName = "AdsProjectName 2", SkuNames = "SkuNames 2" }
                }
            };

            //specify the path of word template
            var s1 = Path.Combine(Directory.GetCurrentDirectory(), "Word", "WordTemplate", "ApTaskWordTemplate.dotx");
            //specify the path of output file
            var s2 = Path.Combine(Directory.GetCurrentDirectory(), "Word", "WordTemplate", "ApTaskForm.docx");
            ExportWordService svc = new ExportWordService();
            svc.ExportWord(model, s1, s2);
        }
    }
}
