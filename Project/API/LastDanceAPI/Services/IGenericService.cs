namespace LastDanceAPI.Services
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetByID(int id);
    }
}
