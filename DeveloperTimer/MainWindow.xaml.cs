using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Timers;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Brush = System.Windows.Media.Brush;

namespace DeveloperTimer
{
    public partial class MainWindow
    {
        public delegate void UpdateUxDelegate(Label lblTime);

        DateTime endTime;
        private readonly Func<string, MessageBoxResult> queryUserBeforeResetTimer;
        private readonly Func<bool> isTopMost;
        private readonly ITimeController timeController;
        private readonly ItemRing<string> nameRing;

        public MainWindow()
            : this(new TimeController(500), msg => MessageBox.Show(msg, "Diligence", MessageBoxButton.YesNo), () => true, new ItemRing<string>())
        {
        }

        public MainWindow(ITimeController timeController, Func<string, MessageBoxResult> queryUserBeforeResetTimer, Func<bool> isTopMost, ItemRing<string> itemRing)
        {
            this.queryUserBeforeResetTimer = queryUserBeforeResetTimer;
            this.isTopMost = isTopMost;
            this.timeController = timeController;

            nameRing = itemRing;
            // After all initializations
            InitializeComponent();
            sOpacitySlider.Value = 1;
            InitializeTimer();

            endTime = this.timeController.Now;

            txtMinutes.Text = timeController.DelaySpan.Minutes.ToString();
            txtSeconds.Text = timeController.DelaySpan.Seconds.ToString();

            MinimizeWindow();

            nameRing.ItemsChanged += (o, e) => UpdateUxNames();
        }

        private IEnumerable<string> LoadDefaultNames()
        {
            var names = new List<string>();
            using (var stream = new StreamReader(GetLocalPath() + "Names.txt"))
            {
                while (!stream.EndOfStream)
                {
                    var readLine = stream.ReadLine();
                    if (string.IsNullOrWhiteSpace(readLine))
                    {
                        continue;
                    }

                    names.Add(readLine.Trim());
                }
            }

            var enumerableRandomizer = new EnumerableRandomizer();

            return enumerableRandomizer.Randomize(names);
        }

        private void InitializeTimer()
        {
            endTime = timeController.Now + timeController.DelaySpan;
            timeController.Listen(OnTimedEvent);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ExecuteUpdateOnUiThread();
        }

        private void ExecuteUpdateOnUiThread()
        {
            if (lblTime.Dispatcher.CheckAccess())
            {
                UpdateUI(lblTime);
            }
            else
            {
                lblTime.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new UpdateUxDelegate(UpdateUI), lblTime);
            }
        }

        private void UpdateUI(Label lblTime)
        {
            var span = endTime - timeController.Now;
            lblTime.Content = String.Format("{0}:{1:00}", span.Minutes, span.Seconds);

            if (span > new TimeSpan(0, 0, 0))
            {
                return;
            }

            gLayoutGrid.Opacity = 1;
            ShowWindow();
            if (gUserQueue.Visibility != System.Windows.Visibility.Visible)
            {
                MaximizeWindow();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
        }

        public void ResetTimer()
        {
            int minutes;
            int seconds;
            if (Int32.TryParse(txtMinutes.Text, out minutes) &&
                Int32.TryParse(txtSeconds.Text, out seconds))
            {
                timeController.DelaySpan = new TimeSpan(0, minutes, seconds);
            }

            endTime = timeController.Now + timeController.DelaySpan;
            MinimizeWindow();
        }

        private void MinimizeWindow()
        {
            gLayoutGrid.Opacity = 0.35;
            gUserQueue.IsEnabled = false;

            SwitchToHeadsUp();

            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Normal;
            Topmost = isTopMost();
            Width = 500;
            Height = 250;
            Top = SystemParameters.VirtualScreenHeight - 250;
            Left = SystemParameters.VirtualScreenLeft;

            HideWindow();
        }

        private void SwitchToHeadsUp()
        {
            gUserQueue.Visibility = Visibility.Collapsed;
            lblNextDeveloper.Visibility = Visibility.Collapsed;
            gridNextButtons.Visibility = Visibility.Collapsed;
        }

        private void HideWindow()
        {
            var value = 255 * sOpacitySlider.Value;
            var textValue = String.Format("#{0:x2}FFFFFF", (int)value);
            var bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom(textValue);
            this.Activate();
        }

        private void MaximizeWindow()
        {
            gLayoutGrid.Opacity = 1;
            gUserQueue.IsEnabled = true;

            SwitchToFullView();

            Topmost = isTopMost();
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Normal;
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;

            Top = SystemParameters.VirtualScreenTop;
            Left = SystemParameters.VirtualScreenLeft;
            ShowWindow();

            IncrementDeveloperIndex();
            SetNextDeveloperMessage();
        }

        private void SwitchToFullView()
        {
            gUserQueue.Visibility = Visibility.Visible;
            lblNextDeveloper.Visibility = Visibility.Visible;
            gridNextButtons.Visibility = Visibility.Visible;
        }

        private void ShowWindow()
        {
            var bc = new BrushConverter();
            Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Activate();
        }

        private void IncrementDeveloperIndex()
        {
            nameRing.Increment();
        }

        private void SetNextDeveloperMessage()
        {
            lblNextDeveloper.Content = String.Format("{0} please sit at the keyboard.", nameRing.Current);
            lblCurrentDeveloper.Content = String.Format("Current Dev: {0}", nameRing.Current);
            lblNextDeveloperName.Content = String.Format("Next Dev: {0}", nameRing.Next);
        }

        private void sOpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var value = 255 * e.NewValue;
            var textValue = String.Format("#{0:x2}FFFFFF", (int)value);
            BrushConverter bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom(textValue);
        }

        private void btnAddName_Click(object sender, RoutedEventArgs e)
        {
            AddNameToQueue(txtAddName.Text);
        }

        private void txtAddName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddNameToQueue(txtAddName.Text);
            }
        }

        private void AddNameToQueue(string name)
        {
            txtAddName.Text = "";
            if (String.IsNullOrWhiteSpace(name)) return;
            nameRing.Add(name);
        }

        private void UpdateUxNames()
        {
            var current = lbNames.Items.Count > 0 ? lbNames.Items[lbNames.SelectedIndex].ToString() : string.Empty;
            lbNames.Items.Clear();

            foreach (var name in nameRing)
            {
                lbNames.Items.Add(name);
            }

            lbNames.SelectedIndex = string.IsNullOrWhiteSpace(current)
                ? 0
                : nameRing.IndexOf(current);
        }

        private void btnMoveNameUp_Click(object sender, RoutedEventArgs e)
        {
            var current = nameRing.Current;
            nameRing.MoveUp(nameRing[lbNames.SelectedIndex]);
            SetNextDeveloperMessage();
        }

        private void btnMoveNameDown_Click(object sender, RoutedEventArgs e)
        {
            var current = nameRing.Current;
            nameRing.MoveDown(nameRing[lbNames.SelectedIndex]);
            SetNextDeveloperMessage();
        }

        private void btnRemoveName_Click(object sender, RoutedEventArgs e)
        {
            if (lbNames.Items.Count <= 0) return;
            var priorSelectedIndex = lbNames.SelectedIndex;

            nameRing.RemoveAt(lbNames.SelectedIndex);
            if (nameRing.Any())
            {
                lbNames.SelectedIndex = priorSelectedIndex % lbNames.Items.Count;
            }
            SetNextDeveloperMessage();
        }

        private void btnAddAllDev_Click(object sender, RoutedEventArgs e)
        {
            lbNames.Items.Clear();

            var defaultNames = LoadDefaultNames();

            foreach (var defaultName in defaultNames)
            {
                nameRing.Add(defaultName);
            }

            SetNextDeveloperMessage();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            var span = endTime - timeController.Now;
            if (span < new TimeSpan(0, 0, 0))
                return;
            Topmost = isTopMost();
            if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                var bc = new BrushConverter();
                Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
            else if (!gUserQueue.IsEnabled)
            {
                if (Left == SystemParameters.VirtualScreenLeft)
                    Left = SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - this.Width;
                else
                    Left = SystemParameters.VirtualScreenLeft;
            }

        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            var span = endTime - timeController.Now;
            if (span < new TimeSpan(0, 0, 0))
                return;
            Topmost = isTopMost();
            var value = 255 * sOpacitySlider.Value;
            var textValue = String.Format("#{0:x2}FFFFFF", (int)value);
            BrushConverter bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom(textValue);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var iconPath = GetLocalPath() + @"Resources\appointment-new-3.ico";
            Icon = new BitmapImage(new Uri(iconPath));
        }

        private string GetLocalPath()
        {
            return new DirectoryInfo(Path.GetDirectoryName(GetType().Assembly.Location)).FullName + "\\";
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            IncrementDeveloperIndex();
            SetNextDeveloperMessage();
            ResetTimer();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            EndSession();
        }

        private void EndSession()
        {
            endTime = DateTime.Now;
            UpdateUI(lblTime);
        }
    }
}
