using OrdersDashboard.Context;
using OrdersDashboard.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrdersDashboard.ViewModels
{
    public class ContractorViewModel :BaseViewModel
    {
        readonly ContractorsOrdersContext _context;

        List<Contractor>? _contractors;
        public List<Contractor>? Contractors
        {
            get { return _contractors; }
            set { _contractors = value; NotifyPropertyChanged(); }
        }

        List<Order>? _orders;
        public List<Order>? Orders
        {
            get { return _orders; }
            set { _orders = value; NotifyPropertyChanged(); }
        }

        Contractor? _selectedContractor;
        public Contractor? SelectedContractor
        {
            get { return _selectedContractor; }
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
            get { return _selectedOrder; }
            set { _selectedOrder = value; NotifyPropertyChanged(); }
        }

        public ContractorViewModel()
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
    }
}
