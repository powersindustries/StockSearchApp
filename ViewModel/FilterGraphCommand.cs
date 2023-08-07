using System;
using System.Windows.Input;

namespace Stocks.ViewModel.Commands
{
    public class FilterGraphCommand : ICommand
    {


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public FilterGraphCommand(StockViewModel viewModel)
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
            return ViewModel.StockData.Count > 0;
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public void Execute(object? parameter)
        {
            if (parameter != null)
            {
                ViewModel.FilterGraph((GraphFormats)parameter);
            }
        }
    }
}
