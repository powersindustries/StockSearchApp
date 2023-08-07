using System;
using System.Windows.Input;

namespace Stocks.ViewModel.Commands
{
    public class CompanySearchCommand : ICommand
    {


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public CompanySearchCommand(StockViewModel viewModel) 
        {
            ViewModel = viewModel;
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public StockViewModel ViewModel { get; set; }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public bool CanExecute(object? parameter)
        {
            return ViewModel.Query != "";
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public void Execute(object? parameter)
        {
            ViewModel.SearchCompany();
        }
    }
}
