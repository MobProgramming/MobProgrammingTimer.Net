using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ApprovalUtilities.Persistence;

namespace DeveloperTimer
{
    /// <summary>
    /// Interaction logic for CountDownWindow.xaml
    /// </summary>
    public partial class CountDownWindow : Window
    {
        private CountDownViewModel vm;

        public CountDownWindow()
        {
            InitializeComponent();
        }

        public CountDownWindow(int minutes, int seconds, ILoader<DateTime> loaderTime):this()
        {
            vm = new CountDownViewModel(minutes, seconds, loaderTime);
            this.DataContext = vm;
        }
    }
}
