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
using System.Windows.Threading;

namespace Monitor
{
    /// <summary>
    /// Логика взаимодействия для SmallWindow.xaml
    /// </summary>
    public partial class SmallWindow : Window
    {
      

        /// <summary>
        /// Инициализация класса, малоко окна
        /// </summary>
        public SmallWindow()
        {
            InitializeComponent();



            DispatcherTimer _timer = new DispatcherTimer();
            _timer.Tick += OnTimerTick;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
        }

        /// <summary>
        /// Про закрытии окна, просто крываем его
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Запускаем обновление временни на малом табло
        /// </summary>
        private void OnTimerTick(object sender, EventArgs e)
        {
            ClockBox.Text = DateTime.Now.ToString("HH:mm");
        }

    }
}
