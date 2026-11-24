using System.Windows;
using Lab2.DataAccess;
using Lab2.DataAccess.Interfaces;
using Lab2.DataAccess.Services;
using Lab4.WpfOrders.ViewModels;

namespace Lab4.WpfOrders
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dbContext = new OrderDbContext();
            IOrderRepository repository = new OrderRepositoryService(dbContext);
            var viewModel = new OrderListViewModel(repository);

            var window = new MainWindow { DataContext = viewModel };
            window.Show();

            viewModel.LoadOrders();
        }

    }

}
