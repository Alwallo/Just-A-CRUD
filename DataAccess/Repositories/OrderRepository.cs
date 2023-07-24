using DataAccess.Entities;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Repositories
{
    public class OrderRepository : MasterRepository, IOrderRepository
    {
        private string delete, getAll, insert, update, sp, sp2;
        private List<String> attributes;
        List<dynamic> dynamics;


        public OrderRepository()
        {
            getAll = "SELECT * FROM [Order Example]";
            insert = "INSERT INTO [Order Example] (ProductName, UnitPrice, Quantity) VALUES (@ProductName, @UnitPrice, @Quantity)";
            update = "UPDATE [Order Example] SET ProductName = @ProductName, UnitPrice = @UnitPrice, Quantity = @Quantity, OrderDate = GETDATE() WHERE ID = @ID";
            delete = "DELETE FROM [Order Example] WHERE ID = @ID";
            sp = "OrderByMonthAndLetter";
            sp2 = "OrderDateByMonth";
        }

        public int Delete(int ID)
        {
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ID", ID));
            return ExecuteNonQuery(delete);
        }

        public IEnumerable<Order> GetAll()
        {
            var tableResult = ExecuteReader(getAll);
            var ListOrder = new List<Order>();
            foreach (DataRow item in tableResult.Rows)
            {
                ListOrder.Add(new Order()
                {
                    ID = Convert.ToInt32(item[0]),
                    ProductName = item[1].ToString(),
                    UnitPrice = Convert.ToDecimal(item[2]),
                    Quantity = Convert.ToInt32(item[3]),
                    OrderDate = Convert.ToDateTime(item[4])
                });
            }
            return ListOrder;
        }

        public int Insert(Order entity)
        {
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ProductName", entity.ProductName));
            parameters.Add(new SqlParameter("@UnitPrice", entity.UnitPrice));
            parameters.Add(new SqlParameter("@Quantity", entity.Quantity));
            return ExecuteNonQuery(insert);
        }

        public int Update(Order entity)
        {
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ID", entity.ID));
            parameters.Add(new SqlParameter("@ProductName", entity.ProductName));
            parameters.Add(new SqlParameter("@UnitPrice", entity.UnitPrice));
            parameters.Add(new SqlParameter("@Quantity", entity.Quantity));
            return ExecuteNonQuery(update);
        }

        public IEnumerable<Order> GetOrderDateByMonth(int month)
        {
            attributes = new List<String>();
            attributes.Add("@Month");
            dynamics = new List<dynamic>();
            dynamics.Add(month);
            var tableResult = ExecuteReader(sp2, attributes, dynamics);
            var ListOrder = new List<Order>();
            parameters = new List<SqlParameter>();
            foreach (DataRow item in tableResult.Rows)
            {
                ListOrder.Add(new Order()
                {
                    ID = Convert.ToInt32(item[0]),
                    ProductName = item[1].ToString(),
                    UnitPrice = Convert.ToDecimal(item[2]),
                    Quantity = Convert.ToInt32(item[3]),
                    OrderDate = Convert.ToDateTime(item[4]),
                });
            }
            return ListOrder;
        }

        public IEnumerable<Order> GetOrderDateByMonthAndLetter(int month, string letter)
        {
            attributes = new List<String>();
            attributes.Add("@Month");
            attributes.Add("@Letter");
            dynamics = new List<dynamic>();
            dynamics.Add(month);
            dynamics.Add(letter);
            var tableResult = ExecuteReader(sp, attributes, dynamics);
            var ListOrder = new List<Order>();
            parameters = new List<SqlParameter>();
            foreach (DataRow item in tableResult.Rows)
            {
                ListOrder.Add(new Order()
                {
                    ID = Convert.ToInt32(item[0]),
                    ProductName = item[1].ToString(),
                    UnitPrice = Convert.ToDecimal(item[2]),
                    Quantity = Convert.ToInt32(item[3]),
                    OrderDate = Convert.ToDateTime(item[4]),
                });
            }
            return ListOrder;
        }
    }
}
