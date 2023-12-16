namespace OldSchool.Infrastructure;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll(); // получение всех объектов
    ValueTask<T?> GetByIdAsync(int id); // получение одного объекта по id
    void Create(T item); // создание объекта
    void Update(T item); // обновление объекта
    Task DeleteAsync(int id); // удаление объекта по id
    Task SaveAsync();  // сохранение изменений
}