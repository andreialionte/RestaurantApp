using RestaurantApp.Data;
using RestaurantApp.Repository.IRepository;

namespace RestaurantApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContextEf _ef;

        public Repository(DataContextEf ef)
        {
            _ef = ef;
        }

        public T Get(int Id)
        {
            return _ef.Set<T>().Find(Id);
        }

        public IEnumerable<T> GetAll()
        {
            return _ef.Set<T>().ToList();
        }

        public void Add(T entity)
        {
            _ef.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            _ef.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _ef.Set<T>().Update(entity);
        }

        public void Save()
        {
            _ef.SaveChanges();
        }

    }

}

