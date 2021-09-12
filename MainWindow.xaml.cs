using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfScreenHelper;
using Xceed.Wpf.Toolkit; //добавляем маску для ввода
using System.Configuration;
using System.Collections.Specialized;
//тригеры для таймера 00:00 https://metanit.com/sharp/wpf/14.3.php
namespace Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BigWindow bigWindow = new BigWindow();
        SmallWindow smallWindow = new SmallWindow();
        /// <summary>
        /// Класс контекста для привязки данных
        /// </summary>
        public View _view = new View();
        DispatcherTimer timer;

        /// <summary>
        /// Конструктор главно окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _view.LeftTeam.TeamTitle = "";
            _view.RightTeam.TeamTitle = "";


            DataContext = _view;
            bigWindow.DataContext = DataContext;
            bigWindow.IsVisibleChanged += SetImgBigMonitor;//подписка на изменение видимости для смены иконки на кнопке
            smallWindow.DataContext = DataContext;
            smallWindow.IsVisibleChanged += SetImgSmallMonitor;


            InitDispatcherTimer();
            _view.TabloInfo.TimeEndEvent += EndMainTimer;

            //строим кнопки с выбором монитора
            ConstructBibMonitorButtons(SelectBigMonitorWrapper, "SelectBigMonitorButtons");
            ConstructBibMonitorButtons(SelectSmallMonitorWrapper, "SelectSmallMonitorButtons");

            SetSettings();
        }
        /// <summary>
        /// Вводим название табло и имена команд из файла настроек
        /// </summary>
        private void SetSettings()
        {
            _view.TabloInfo.TabloName = ConfigurationManager.AppSettings.Get("TabloName");
            _view.LeftTeam.TeamName =  ConfigurationManager.AppSettings.Get("LeftTeamName");
            _view.RightTeam.TeamName = ConfigurationManager.AppSettings.Get("RightTeamName");
        }

        //Открытие/закрытие окон экранов
        private void OpenBigWindow(object sender, RoutedEventArgs e) => OpenWindow(bigWindow);
        private void OpenSmallWindow(object sender, RoutedEventArgs e) => OpenWindow(smallWindow);
       

        private void OpenWindow(Window window)
        {
            if (!window.IsVisible)
                window.Show();
            else
                window.Close();
        }

        private void StretchBigWindow(object sender, RoutedEventArgs e) => _StretchBigWindow();
       
        private void _StretchBigWindow()
        {//развернуть или уменьшить окно для экрана стадиона
            ToggleStretch_IbgButton(bigWindow);
            if (bigWindow.WindowState == System.Windows.WindowState.Maximized)
                StrechSmallWindiwBTN.Source = new BitmapImage(new Uri("pack://application:,,,/Monitor;component/Resources/smallest.png"));
            else
                StrechSmallWindiwBTN.Source = new BitmapImage(new Uri("pack://application:,,,/Monitor;component/Resources/bigest.png"));
        }
        private void StretchSmallWindow(object sender, RoutedEventArgs e) => _StretchSmallWindow();

        private void _StretchSmallWindow()
        {//развернуть или уменьшить окно для экрана тренеров
            ToggleStretch_IbgButton(smallWindow);
            if (smallWindow.WindowState == System.Windows.WindowState.Maximized)
                StrechSmallWindiwBTN.Source = new BitmapImage(new Uri("pack://application:,,,/Monitor;component/Resources/smallest.png"));
            else
                StrechSmallWindiwBTN.Source = new BitmapImage(new Uri("pack://application:,,,/Monitor;component/Resources/bigest.png"));

        }

        private void ToggleStretch_IbgButton(Window window)
        {
            if (smallWindow.WindowState == System.Windows.WindowState.Maximized)
            {
                smallWindow.WindowState = System.Windows.WindowState.Normal;
                smallWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                
            }
            else
            {
                smallWindow.WindowState = System.Windows.WindowState.Maximized;
                smallWindow.WindowStyle = WindowStyle.None;
                
            }
        }

        private void SetImgBigMonitor(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (bigWindow.Visibility == Visibility)
                OpenBigWindiwBTN.Source = new BitmapImage(new Uri("pack://application:,,,/Monitor;component/Resources/crossOrange1.png"));
            else
                OpenBigWindiwBTN.Source = new BitmapImage(new Uri("pack://application:,,,/Monitor;component/Resources/monitorOrange1.png"));
        }

        private void SetImgSmallMonitor(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (smallWindow.Visibility == Visibility)
                OpenSmallWindiwBTN.Source = new BitmapImage(new Uri("pack://application:,,,/Monitor;component/Resources/crossOrange1.png"));
            else
                OpenSmallWindiwBTN.Source = new BitmapImage(new Uri("pack://application:,,,/Monitor;component/Resources/monitorOrange1.png"));
        }

        


        //копки счетчика левой команды
        private void LeftTeamMinusRefreshCount(object sender, RoutedEventArgs e) => _view.LeftTeam.TeamCounter = 0;
        private void LeftTeamPlusOneCount(object sender, RoutedEventArgs e) => _view.LeftTeam.TeamCounter++;
        private void LeftTeamMinusOneCount(object sender, RoutedEventArgs e) => _view.LeftTeam.TeamCounter--;

        //копки счетчика правой команды
        private void RightTeamRefreshCount(object sender, RoutedEventArgs e) => _view.RightTeam.TeamCounter = 0;
        private void RightTeamPlusOneCount(object sender, RoutedEventArgs e) => _view.RightTeam.TeamCounter++;
        private void RightTeamMinusOneCount(object sender, RoutedEventArgs e) => _view.RightTeam.TeamCounter--;

        //кнопки периода игры
        private void PeriodPlusOneCount(object sender, RoutedEventArgs e) => _view.TabloInfo.Period++;
        private void PeriodMinusOneCount(object sender, RoutedEventArgs e) => _view.TabloInfo.Period--;

        /// <summary>
        /// Запускаем таймер, инициализаруем подписки на его tick.
        /// </summary>
        public void InitDispatcherTimer()
        { 
            timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += _view.TabloInfo.SubtractTaimer;
            timer.Tick += _view.LeftPlayer1.SubtractTaimer;
            timer.Tick += _view.LeftPlayer2.SubtractTaimer;
            timer.Tick += _view.LeftPlayer3.SubtractTaimer;
            timer.Tick += _view.RightPlayer1.SubtractTaimer;
            timer.Tick += _view.RightPlayer2.SubtractTaimer;
            timer.Tick += _view.RightPlayer3.SubtractTaimer;
        }

        void StartTimer() => timer.Start();
        void StopTimer() => timer.Stop();

        //управление центральным таймером
        private void StopMainTimer() => _StopMainTimer();

        private void _StopMainTimer()
        {
            MainTimer.IsReadOnly = false;
            MainTimer.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            HostTimer1.IsReadOnly = false;
            HostTimer1.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            HostTimer2.IsReadOnly = false;
            HostTimer2.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            HostTimer3.IsReadOnly = false;
            HostTimer3.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            GuestsTimer1.IsReadOnly = false;
            GuestsTimer1.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            GuestsTimer2.IsReadOnly = false;
            GuestsTimer2.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            GuestsTimer3.IsReadOnly = false;
            GuestsTimer3.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            StopTimer();
        }
        //private void StartMainTimer() => _StartMainTimer();

        private void _StartMainTimer()
        {
            //Setter setter = new System.Windows.Setter(Background, Color.FromArgb(0, 0, 0, 0));
            //Style = Resources["DeleteTimers"] as Style;
            //var x = Style.Setters;

            MainTimer.IsReadOnly = true;
            MainTimer.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));

            HostTimer1.IsReadOnly = true;
            HostTimer1.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            HostTimer2.IsReadOnly = true;
            HostTimer2.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            HostTimer3.IsReadOnly = true;
            HostTimer3.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            GuestsTimer1.IsReadOnly = true;
            GuestsTimer1.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            GuestsTimer2.IsReadOnly = true;
            GuestsTimer2.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            GuestsTimer3.IsReadOnly = true;
            GuestsTimer3.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));

            StartTimer();
        }

        private void BlockUnblockTimers(bool IsBlock)
        {

        }

        private void StartStopMainTimerBtn(object sender, RoutedEventArgs e) {
            StartStopMainTimer();
        }
        
        private void RefrashMainTimer(object sender, RoutedEventArgs e)
        {
            _view.TabloInfo.GameTaimer = new TimeSpan(0, 0, 0);
            StopMainTimer();
        }

        private void EndMainTimer() => _StopMainTimer();

        private void AddMainTimer5Minutes(object sender, RoutedEventArgs e)
        {
            _view.TabloInfo.GameTaimer = new TimeSpan(0, 5, 0);
        }
        private void AddMainTimer10Minutes(object sender, RoutedEventArgs e)
        {
            _view.TabloInfo.GameTaimer = new TimeSpan(0, 10, 0);
        }
        private void AddMainTimer20Minutes(object sender, RoutedEventArgs e)
        {
            _view.TabloInfo.GameTaimer = new TimeSpan(0, 20, 0);
        }

        //Кнопки упарвления, временнем удаления игроков

        private void PlayerDeletTimerPlus(RemovingPlayer Player, int minutes)
        {
            Player.RemovingTimer = Player.RemovingTimer.Add(new TimeSpan(0, minutes, 0));
        }
        
        private void PlayerDeletTimerReset(RemovingPlayer Player) {
            Player.RemovingTimer = new TimeSpan(0, 0, 0);
            Player.PlayerNomber = 0;
        }

        private void LeftPlayer1TimerPlus2(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer1, 2);
        private void LeftPlayer1TimerPlus5(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer1, 5);
        private void LeftPlayer1TimerPlus10(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer1, 10);
        private void LeftPlayer1TimerReset(object sender, RoutedEventArgs e) => PlayerDeletTimerReset(_view.LeftPlayer1);

        private void LeftPlayer2TimerPlus2(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer2, 2);
        private void LeftPlayer2TimerPlus5(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer2, 5);
        private void LeftPlayer2TimerPlus10(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer2, 10);
        private void LeftPlayer2TimerReset(object sender, RoutedEventArgs e) => PlayerDeletTimerReset(_view.LeftPlayer2);

        private void LeftPlayer3TimerPlus2(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer3, 2);
        private void LeftPlayer3TimerPlus5(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer3, 5);
        private void LeftPlayer3TimerPlus10(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.LeftPlayer3, 10);
        private void LeftPlayer3TimerReset(object sender, RoutedEventArgs e) => PlayerDeletTimerReset(_view.LeftPlayer3);

        private void RightPlayer1TimerPlus2(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer1, 2);
        private void RightPlayer1TimerPlus5(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer1, 5);
        private void RightPlayer1TimerPlus10(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer1, 10);
        private void RightPlayer1TimerReset(object sender, RoutedEventArgs e) => PlayerDeletTimerReset(_view.RightPlayer1);

        private void RightPlayer2TimerPlus2(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer2, 2);
        private void RightPlayer2TimerPlus5(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer2, 5);
        private void RightPlayer2TimerPlus10(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer2, 10);
        private void RightPlayer2TimerReset(object sender, RoutedEventArgs e) => PlayerDeletTimerReset(_view.RightPlayer2);

        private void RightPlayer3TimerPlus2(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer3, 2);
        private void RightPlayer3TimerPlus5(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer3, 5);
        private void RightPlayer3TimerPlus10(object sender, RoutedEventArgs e) => PlayerDeletTimerPlus(_view.RightPlayer3, 10);
        private void RightPlayer3TimerReset(object sender, RoutedEventArgs e) => PlayerDeletTimerReset(_view.RightPlayer3);

        private void ChangeClock(object sender, RoutedEventArgs e)
        //влючение/выключение часов на малом экране
        {
            if (smallWindow.TimeVisibility)
                TimeBtn.Content = "Убрать";
            else
                TimeBtn.Content = "Время";

        }


        //выбор мониторов и открытие двух окон
        private void OpenWindows(object sender, RoutedEventArgs e)
        {
           
            foreach (RadioButton i in SelectBigMonitorWrapper.Children)
            {if ((bool)i.IsChecked)
                {

                    int screenNumber;
                    int.TryParse(i.Content.ToString(), out screenNumber);
                    var screen = WpfScreenHelper.Screen.AllScreens.ToArray()[screenNumber-1];
                    bigWindow.Left = screen.Bounds.Left;
                    bigWindow.Top = screen.Bounds.Top;
                }
            }

            foreach (RadioButton i in SelectSmallMonitorWrapper.Children)
            {
                if ((bool)i.IsChecked)
                {

                    int screenNumber;
                    int.TryParse(i.Content.ToString(), out screenNumber);
                    var screen = WpfScreenHelper.Screen.AllScreens.ToArray()[screenNumber-1];
                    smallWindow.Left = screen.Bounds.Left;
                    smallWindow.Top = screen.Bounds.Top;
                }
            }

            OpenWindow(bigWindow);
            OpenWindow(smallWindow);
           
            _StretchBigWindow();
            _StretchSmallWindow();



        }
        private void ConstructBibMonitorButtons(StackPanel Panel, string groupName)
        {
            //конструктор радиокнопок при запуске главного окна
            int ScreensCount = WpfScreenHelper.Screen.AllScreens.ToArray().Length;

            //SelectBigMonitorWrapper
            for (int i = 0; i < ScreensCount; i++)
            {
                RadioButton radioButton = new RadioButton { Content = (i+1).ToString(), GroupName = groupName, HorizontalAlignment = HorizontalAlignment.Center};
                Panel.Children.Add(radioButton);
            }

        }


        private void AddUpdateAppSettings(string key, string value)
        {
           var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
           var settings = configFile.AppSettings.Settings;
           if (settings[key] == null)
           {
               settings.Add(key, value);
           }
           else
           {
               settings[key].Value = value;
           }
           configFile.Save(ConfigurationSaveMode.Modified);
           ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
           
        }

        private void NameTablo_KeyUp(object sender, KeyEventArgs e) => AddUpdateAppSettings("TabloName", _view.TabloInfo.TabloName);
        private void HostName_KeyUp(object sender, KeyEventArgs e) => AddUpdateAppSettings("LeftTeamName", _view.LeftTeam.TeamName);
        private void GuestsName_KeyUp(object sender, KeyEventArgs e) => AddUpdateAppSettings("RightTeamName", _view.RightTeam.TeamName);

        
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                StartStopMainTimer();
        }

        private void StartStopMainTimer()
        {
            if (timer.IsEnabled)
            {
                _StopMainTimer();
            }
            else
                _StartMainTimer();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {//Делаем выход из приложения при закрытии главного окна. Для закрытя скрытых окон
            Application.Current.Shutdown();
        }

    }
}
