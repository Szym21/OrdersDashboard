using OrdersDashboard.Commands;
using OrdersDashboard.Context;
using OrdersDashboard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OrdersDashboard.ViewModels
{
    public class ContractorViewModel :BaseViewModel
    {
        readonly ContractorsOrdersContext _context;

        bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { _editMode = value; NotifyPropertyChanged(); }
        }

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

        public ICommand? NewContractorCmd { get; set; }
        public ICommand? EditContractorCmd { get; set; }
        public ICommand? DeleteContractorCmd { get; set; }
        public ICommand? SaveChangesCmd { get; set; }
        public ICommand? DiscardChangesCmd { get; set; }

        public ContractorViewModel()
        {
            _context = new ContractorsOrdersContext();
            FillContractors();
            AddCommands();
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

        void AddCommands()
        {
            NewContractorCmd = new RelayCommand(AddNewContractor, CanAddNewContractor);
            EditContractorCmd = new RelayCommand(EditContractor, CanEditContractor);
            DeleteContractorCmd = new RelayCommand(DeleteContractor, CanDeleteContractor);
            SaveChangesCmd = new RelayCommand(SaveChanges, CanSaveChanges);
            DiscardChangesCmd = new RelayCommand(DiscardChanges, CanDiscardChanges);
        }
        void AddNewContractor(object value)
        {
            SelectedContractor = new();
            EditMode = true;
            SelectedContractor.IdKontrahenta = null;
        }
        bool CanAddNewContractor(object value) => !EditMode;
        void EditContractor(object value) => EditMode = true;
        bool CanEditContractor(object value) => SelectedContractor is not null;
        void DeleteContractor(object value)
        {
            EditMode = false;
            _context.Contractors.Remove(SelectedContractor);
            _context.SaveChanges();
            SelectedContractor = null;
            FillContractors();
        }
        bool CanDeleteContractor(object value) => SelectedContractor?.Orders.Count == 0;
        void SaveChanges(object value)
        {
            if (SelectedContractor?.IdKontrahenta is null)
            {
                SelectedContractor.DataDodania = System.DateTime.Now;
                _context.Contractors.Add(SelectedContractor);
                _context.SaveChanges();
                MessageBox.Show($"Dodano nowego kontrahenta: {SelectedContractor.Nazwa}");
            }
            else
            {
                _context.SaveChanges();
                MessageBox.Show($"Zakończono edycje kontrahenta: {SelectedContractor.Nazwa}");
            }
            SelectedContractor = null;
            EditMode = false;
            FillContractors();
        }
        bool CanSaveChanges(object value) => SelectedContractor is not null && EditMode;
        void DiscardChanges(object value)
        { 
            SelectedContractor = null;
            EditMode = false;
        }
        bool CanDiscardChanges(object value) => SelectedContractor is not null && EditMode;
    }
}
