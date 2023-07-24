using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Domain.States;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Domain.Models
{
    public class OrderModel
    {

        private int id;
        private string productName;
        private decimal unitPrice;
        private int quantity;
        private DateTime orderDate;
        private decimal total;

        private IOrderRepository OrderRepository;
        public EntityState state { private get; set; }

        public int Id { get => id; set => id = value; }

        [Required]
        public string ProductName { get => productName; set => productName = value; }
        public decimal UnitPrice { get => unitPrice; set => unitPrice = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
        public decimal Total { get => total; set => total = value; }

        public OrderModel()
        {
            OrderRepository = new OrderRepository();
        }

        public string SaveChanges()
        {
            string message = null;
            try
            {
                var orderDataModel = new Order();
                orderDataModel.ID = id;
                orderDataModel.ProductName = productName;
                orderDataModel.UnitPrice = unitPrice;
                orderDataModel.Quantity = quantity;
                orderDataModel.OrderDate = orderDate;
                switch (state)
                {
                    case EntityState.Inserted:
                        OrderRepository.Insert(orderDataModel);
                        message = "Registrado Exitosamente";
                        break;
                    case EntityState.Updated:
                        OrderRepository.Update(orderDataModel);
                        message = "Actualizado Exitosamente";
                        break;
                    case EntityState.Deleted:
                        OrderRepository.Delete(orderDataModel.ID);
                        message = "Eliminado Exitosamente";
                        break;
                }
            }
            catch (Exception ex)
            {
                SqlException sqlException = ex as SqlException;
                if (sqlException != null)
                {
                    message = ex.ToString();
                }
            }
            return message;
        }

        public List<OrderModel> GetAll()
        {
            var orderDataModel = OrderRepository.GetAll();
            var listOrders = new List<OrderModel>();
            foreach (var item in orderDataModel)
            {
                listOrders.Add(new OrderModel
                {
                    id = item.ID,
                    productName = item.ProductName,
                    unitPrice = item.UnitPrice,
                    quantity = item.Quantity,
                    orderDate = item.OrderDate,
                    total = CalculateTotal(item.UnitPrice, item.Quantity)
                });
            }
            return listOrders;
        }

        private decimal CalculateTotal(decimal UnitPrice, int Quantity)
        {
            return UnitPrice * Quantity;
        }

        public IEnumerable<OrderModel> FilterByNameOrId(string filter)
        {
            return GetAll().FindAll(e => e.Id.ToString().Equals(filter) || e.ProductName.ToLower().Contains(filter.ToLower()));
        }

        public IEnumerable<OrderModel> FilterByMonth(int month)
        {
            var orderDataModel = OrderRepository.GetOrderDateByMonth(month);
            var listOrders = new List<OrderModel>();
            foreach (var item in orderDataModel)
            {
                listOrders.Add(new OrderModel
                {
                    Id = item.ID,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    OrderDate = item.OrderDate,
                    Total = CalculateTotal(item.UnitPrice, item.Quantity)
                });
            }
            return listOrders;
        }

        public IEnumerable<OrderModel> FilterByMonthAndDate(int month, string letter)
        {
            var orderDataModel = OrderRepository.GetOrderDateByMonthAndLetter(month, letter);
            var listOrders = new List<OrderModel>();
            foreach (var item in orderDataModel)
            {
                listOrders.Add(new OrderModel
                {
                    Id = item.ID,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    OrderDate = item.OrderDate,
                    Total = CalculateTotal(item.UnitPrice, item.Quantity)
                });
            }
            return listOrders;
        }

    }
}
