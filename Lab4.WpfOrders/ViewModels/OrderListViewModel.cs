using CommunityToolkit.Mvvm.Input;
using Lab2.DataAccess;
using Lab2.DataAccess.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Lab4.WpfOrders.ViewModels
{
    public class OrderListViewModel : BaseViewModel
    {
        private readonly IOrderRepository _repository;

        public OrderListViewModel(IOrderRepository repository)
        {
            _repository = repository;
            LoadOrdersCommand = new RelayCommand(LoadOrders);
            SelectOrderCommand = new RelayCommand(LoadOrderItems);
            SearchOrderCommand = new RelayCommand(SearchOrder);
            ClearSearchOrderCommand = new RelayCommand(ClearSearchOrder);
        }

        public ICommand LoadOrdersCommand { get; }

        public ICommand SelectOrderCommand { get; }

        public ICommand SearchOrderCommand { get; }

        public ICommand ClearSearchOrderCommand { get; }

        public ObservableCollection<Order> Orders { get; } = new();

        public ObservableCollection<OrderItem> OrderItems { get; } = new();

        public IEnumerable<OrderStatus> StatusOptions { get; } = Enum.GetValues<OrderStatus>();

        private Order? _selectedOrder;

        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        private bool _isCollectionEmpty;

        public bool IsCollectionEmpty
        {
            get => _isCollectionEmpty;
            set => SetProperty(ref _isCollectionEmpty, value);
        }

        private string? _searchOrderNumber;

        public string? SearchOrderNumber
        {
            get => _searchOrderNumber;
            set => SetProperty(ref _searchOrderNumber, value);
        }

        private DateTime? _searchOrderDate;
        public DateTime? SearchOrderDate
        {
            get => _searchOrderDate;
            set => SetProperty(ref _searchOrderDate, value);
        }

        private OrderStatus? _searchStatus;
        public OrderStatus? SearchStatus
        {
            get => _searchStatus;
            set => SetProperty(ref _searchStatus, value);
        }

        public void LoadOrders()
        {
            FillOrders(_repository.GetAll());
        }

        private void SearchOrder()
        {
            FillOrders(_repository.Search(SearchOrderNumber, SearchOrderDate, SearchStatus));
        }

        private void ClearSearchOrder()
        {
            SearchOrderNumber = null;
            SearchOrderDate = null;
            SearchStatus = null;
            LoadOrders();
        }

        private void FillOrders(IEnumerable<Order> orders)
        {
            Orders.Clear();
            OrderItems.Clear();
            SelectedOrder = null;

            foreach (var order in orders)
            {
                Orders.Add(order);
            }

            IsCollectionEmpty = Orders.Count == 0;
        }

        private void LoadOrderItems()
        {
            OrderItems.Clear();
            if (SelectedOrder == null)
            {
                return;
            }

            foreach (var orderItem in _repository.GetOrderItemsById(SelectedOrder.Id))
            {
                OrderItems.Add(orderItem);
            }
        }
    }
}