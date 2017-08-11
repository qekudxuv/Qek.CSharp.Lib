namespace Qek.NHibernate.Model
{
    public interface IReviewable<T>
    {
        ReviewInfo<T> ReviewInfo { get; set; }
    }
}
