namespace Lab2.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order? GetById(int id);
        int Add(Order order);
        void Update(Order order);
        void Delete(int id);
        IEnumerable<OrderItem> GetOrderItemsById(int orderId);

        IEnumerable<Order> Search(string? orderNumber, DateTime? orderDate, OrderStatus? status);
    }
}