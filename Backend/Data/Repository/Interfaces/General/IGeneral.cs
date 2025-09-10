namespace Data.Repository.Interfaces.General
{
    public interface IGeneral<T> : IGetTotalGeneral<T>
        where T : class
    {
    }
}