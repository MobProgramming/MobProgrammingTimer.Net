using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ApprovalUtilities.Utilities;
using DeveloperTimer.Annotations;

namespace DeveloperTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

    public class TimerViewModel : INotifyPropertyChanged
    {
        private int seconds;
        private int minutes;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Minutes
        {
            get { return minutes; }
            set
            {
                if (value == minutes) return;
                minutes = value;
                OnPropertyChanged();
            }
        }

        public string DisplayMinutes
        {
            get { return "{0:00}".FormatWith(minutes); }
        }

        public string DisplaySeconds
        {
            get { return "{0:00}".FormatWith(seconds); }
        }


        public int Seconds
        {
            get { return seconds; }
            set
            {
                if (value == seconds) return;
                seconds = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
