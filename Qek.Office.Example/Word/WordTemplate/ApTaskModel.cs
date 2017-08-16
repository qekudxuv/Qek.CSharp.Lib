using System.Collections.Generic;

namespace Qek.Office.Example.Word.WordTemplate
{
    internal class ApTaskModel
    {
        public string AppName { get; set; }
        public string RdManager { get; set; }
        public string SFM { get; set; }
        public string Symptom { get; set; }
        public string ReproSteps { get; set; }
        public string RootCause { get; set; }
        public string Solution { get; set; }
        public string ImpactedScope { get; set; }

        public virtual ISet<ApItsIssueModel> MyApItsIssues { get; set; }
        public virtual ISet<ApSubmitPreparationModel> MyApSubmitPreparations { get; set; }
    }

    internal class ApItsIssueModel
    {
        public virtual string ItsProjectName { get; set; }
        public virtual int ItsIssueNum { get; set; }
    }

    internal class ApSubmitPreparationModel
    {
        public virtual string AdsProjectName { get; set; }
        public virtual string SkuNames { get; set; }
    }
}
