using System.Collections.Generic;

namespace StudentsManager.DataAccess.Interface
{
    public interface IDataProvider<T>
    {
        T GetById(int id);

        void Change(T newEntity);

        void Add(T entity);

        void Remove(int id);

        IEnumerable<T> GetAll();

        void SubmitChanges();
    }
}
