using OrdersDashboard.Commands;
using OrdersDashboard.Context;
using OrdersDashboard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OrdersDashboard.ViewModels
{
    public class ContractorViewModel : BaseViewModel
    {
        readonly ContractorsOrdersContext _context;

        string? _contractorFilter;
        public string? ContractorFilter
        {
            get { return _contractorFilter; }
            set { _contractorFilter = value; NotifyPropertyChanged(); }
        }

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

        public ICommand? NewContractorCmd { get; set; }
        public ICommand? EditContractorCmd { get; set; }
        public ICommand? DeleteContractorCmd { get; set; }
        public ICommand? SaveChangesCmd { get; set; }
        public ICommand? DiscardChangesCmd { get; set; }
        public ICommand? FilterContractorCmd { get; set; }

        public ContractorViewModel()
        {
            _context = new ContractorsOrdersContext();
            FillContractors();
            AddCommands();
        }
        void FillContractors()
        {
            this.Contractors = _context.Contractors
                .Select(contractor => contractor).ToList();
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
            SaveChangesCmd = new RelayCommand(SaveContractorChanges, CanSaveChanges);
            DiscardChangesCmd = new RelayCommand(DiscardChanges, CanDiscardChanges);
            FilterContractorCmd = new RelayCommand(FilterContractors, CanFilterContractors);
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
        void SaveContractorChanges(object value)
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
        void FilterContractors(object value)
        {
            this.Contractors = _context.Contractors
                .Where(c =>c.Nazwa != null && c.Nazwa.Contains(ContractorFilter)).ToList();
        }
        bool CanFilterContractors(object value) => !EditMode && ContractorFilter is not null;
    }
}
