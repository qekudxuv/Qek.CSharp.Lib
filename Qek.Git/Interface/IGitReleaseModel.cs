namespace Qek.Git
{
    /// <summary>
    /// Interface for those release records need to be tagged.
    /// </summary>
    public interface IGitReleaseModel
    {
        long ID { get; set; }
        string GitName { get; set; }
        string ReleaseCommitID { get; set; }
        string TagName { get; }
    }
}

