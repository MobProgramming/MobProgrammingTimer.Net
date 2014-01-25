using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ApprovalUtilities.Utilities;
using DeveloperTimer.Annotations;

namespace DeveloperTimer.Models
{
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

        public ICommand StartTimer
        {
            get { return new RelayCommand(() => MessageBox.Show("HHELWODFFODS")); }

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