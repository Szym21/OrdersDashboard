using OrdersDashboard.Commands;
using OrdersDashboard.Context;
using OrdersDashboard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OrdersDashboard.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        readonly ContractorsOrdersContext _context;

        string? _orderFilter;
        public string? OrderFilter
        {
            get { return _orderFilter; }
            set { _orderFilter = value; NotifyPropertyChanged(); }
        }

        Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get { return _selectedOrder; }
            set { _selectedOrder = value; NotifyPropertyChanged(); }
        }
        List<Order>? _orders;
        public List<Order>? Orders
        {
            get { return _orders; }
            set { _orders = value; NotifyPropertyChanged(); }
        }
        public ICommand? FilterOrderCmd { get; set; }
        public ICommand? AddNewOrderCmd { get; set; }
        public ICommand? SaveNewOrderCmd { get; set; }
        public ICommand? DeleteOrderCmd { get; set; }

        public OrdersViewModel()
        {
            _context = new ContractorsOrdersContext();
            FillOrders();
            AddCommands();
        }

        void AddCommands()
        {
            FilterOrderCmd = new RelayCommand(FilterOrders, CanFilterOrders);
            AddNewOrderCmd = new RelayCommand(AddOrder, CanAddOrder);
            SaveNewOrderCmd = new RelayCommand(SaveOrder, CanSaveOrder);
            DeleteOrderCmd = new RelayCommand(DeleteOrder, CanDeleteOrder);
        }

        void FillOrders()
        {
            this.Orders = _context.Orders
                .Select(order => order)
                .ToList();
        }
        void FilterOrders(object value)
        {
            this.Orders = _context.Orders.Where(c => c.Numer.Contains(OrderFilter)).ToList();
        }
        bool CanFilterOrders(object value) => OrderFilter is not null;
        void AddOrder(object value)
        {
            SelectedOrder = new();
            SelectedOrder.IdZamowienia = null;
        }
        bool CanAddOrder(object value) => true;
        void SaveOrder(object value)
        {
            if (SelectedOrder?.IdZamowienia is null)
            {
                SelectedOrder.DataDodania = System.DateTime.Now;
                _context.Orders.Add(SelectedOrder);
                _context.SaveChanges();
                MessageBox.Show($"Dodano nowe zamówienie: {SelectedOrder.Numer}");
            }
            else
            {
                if (_context.Contractors.Find(SelectedOrder.IdKontrahenta) is not null)
                {
                    _context.SaveChanges();
                    MessageBox.Show($"Zakończono edycje zamówienia: {SelectedOrder.Numer}");
                }
                else
                {
                    MessageBox.Show($"Nie ma kontrahenta o takim identyfikatorze: {SelectedOrder.IdKontrahenta}");
                }
            }
            SelectedOrder = null;
            FillOrders();
        }
        bool CanSaveOrder(object value) => SelectedOrder is not null;
        void DeleteOrder(object value)
        {
            _context.Orders.Remove(SelectedOrder);
            _context.SaveChanges();
            SelectedOrder = null;
            FillOrders();
        }
        bool CanDeleteOrder(object value) => SelectedOrder is not null;
    }
}
