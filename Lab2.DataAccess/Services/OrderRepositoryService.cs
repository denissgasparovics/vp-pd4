using Lab2.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab2.DataAccess.Services
{
    public class OrderRepositoryService : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepositoryService(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Order> GetAll()
        {
            return _dbContext.Orders.ToList();
        }

        public Order? GetById(int id)
        {
            return _dbContext.Orders
                .Include(s => s.OrderItems)
                .FirstOrDefault(s => s.Id == id);
        }

        public int Add(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
            return order.Id;
        }

        public void Update(Order order)
        {
            var existing = _dbContext.Orders.FirstOrDefault(s => s.Id == order.Id);
            if (existing == null)
            {
                return;
            }

            existing.OrderNumber = order.OrderNumber;
            existing.OrderDate = order.OrderDate;
            existing.Status = order.Status;
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = GetById(id);
            if (order == null)
            {
                return;
            }

            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
        }

        public IEnumerable<OrderItem> GetOrderItemsById(int orderId)
        {
            var order = GetById(orderId);
            return order?.OrderItems.ToList() ?? new List<OrderItem>();
        }

        public IEnumerable<Order> Search(string? orderNumber, DateTime? orderDate, OrderStatus? status)
        {
            var query = _dbContext.Orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                query = query.Where(s => s.OrderNumber.Contains(orderNumber));
            }

            if (orderDate.HasValue)
            {
                var from = orderDate.Value.Date;
                var to = from.AddDays(1);
                query = query.Where(o => o.OrderDate >= from && o.OrderDate < to);
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            return query.ToList();
        }
    }
}