using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        int Insert (Entity entity);
        int Update (Entity entity);
        int Delete (int ID);
        IEnumerable<Entity> GetAll ();
    }
}
