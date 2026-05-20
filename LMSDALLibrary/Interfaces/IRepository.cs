namespace LMSDALLibrary.Interfaces;

public interface IRepository<T> where T : class
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T item);
    void Delete(int id);
    void Update(T item);

}