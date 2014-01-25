using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ApprovalUtilities.Persistence;
using ApprovalUtilities.Utilities;
using DeveloperTimer.Annotations;

namespace DeveloperTimer
{
    public class CountDownViewModel : INotifyPropertyChanged
    {
        private readonly int minutes;
        private readonly int seconds;
        private readonly ILoader<DateTime> loaderTime;
        private DateTime end;

        public CountDownViewModel(int minutes, int seconds, ILoader<DateTime> loaderTime)
        {
            this.minutes = minutes;
            this.seconds = seconds;
            this.loaderTime = loaderTime;
            end = loaderTime.Load().AddMinutes(minutes).AddSeconds(seconds);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string TimeRemaining
        {
            get
            {
                var diff = end - loaderTime.Load();
                return "m:{0:00} s:{1:00}".FormatWith(diff.Minutes ,diff.Seconds);
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