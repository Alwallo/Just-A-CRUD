using DataAccess.Entities;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        IEnumerable<Order> GetOrderDateByMonth(int month);

        IEnumerable<Order> GetOrderDateByMonthAndLetter(int month, string letter);
    }
}
