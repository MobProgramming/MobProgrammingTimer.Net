using System.ComponentModel;
using System.Runtime.CompilerServices;
using ApprovalUtilities.Utilities;
using DeveloperTimer.Annotations;

namespace DeveloperTimer
{
    public class CountDownViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string TimeRemaining
        {
            get { return "m:{0:00} s:{1:00}".FormatWith(6 ,28); }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}