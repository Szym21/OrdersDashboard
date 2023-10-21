using OrdersDashboard.Context;
using OrdersDashboard.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OrdersDashboard.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        readonly ContractorsOrdersContext _context;
        public event PropertyChangedEventHandler? PropertyChanged;

        List<Contractor>? _contractors;
        public List<Contractor>? Contractors
        {
            get
            {
                return _contractors;
            }
            set
            {
                _contractors = value;
                NotifyPropertyChanged();
            }
        }

        List<Order>? _orders;
        public List<Order>? Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
                NotifyPropertyChanged();
            }
        }

        Contractor? _selectedContractor;
        public Contractor? SelectedContractor
        {
            get
            {
                return _selectedContractor;
            }
            set
            {
                _selectedContractor = value;
                NotifyPropertyChanged();
                FillOrders();
            }
        }

        Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get
            {
                return _selectedOrder;
            }
            set
            {
                _selectedOrder = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
           _context = new ContractorsOrdersContext();
            FillContractors();
        }
        void FillContractors()
        {
            this.Contractors = _context.Contractors
                .Select(c => c).ToList();
        }
        void FillOrders()
        {
            if (SelectedContractor is null) return;
            Contractor? contractor = this.SelectedContractor;

            this.Orders = _context.Orders
                .Where(order => order.IdKontrahenta == contractor.IdKontrahenta)
                .OrderBy(order => order.Numer)
                .ToList();
        }
        void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
