using System;
using System.Windows.Input;
using DeveloperTimer.Models;

namespace DeveloperTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private TimerViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(int minutes, int seconds)
            : this()
        {
            vm = new TimerViewModel();
            DataContext = vm;
            vm.Minutes = minutes;
            vm.Seconds = seconds;
        }

        public DateTime Time { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action action;

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }

        public event EventHandler CanExecuteChanged;
    }
}
