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

namespace Monitor
{
    /// <summary>
    /// Логика взаимодействия для SmallWindow.xaml
    /// </summary>
    public partial class SmallWindow : Window
    {
        private bool _TimeVisibility = false;
        public bool TimeVisibility
        {
            get
            {
                _TimeVisibility = !_TimeVisibility;
                if (_TimeVisibility)
                {
                    Style style = this.FindResource("TimeShow") as Style;
                    Clock.Style = style;
                }
                else
                {
                    Style style = this.FindResource("TimeHidden") as Style;
                    Clock.Style = style;
                }

                return _TimeVisibility;
            }
        }

        public SmallWindow()
        {
            InitializeComponent();
            //Style style = this.FindResource("TimeShow") as Style;
            //Clock.Style = style;
            //style = this.FindResource("TimeHidden") as Style;
            //Clock.Style = style;
            StartRefrashDateTime();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void StartRefrashDateTime()
        {//обновляем вермя дня на малом табло 
            Task.Factory.StartNew(() => { 
                while (true){

                    this.Dispatcher.Invoke(() => {
                        ClockBox.Text = DateTime.Now.ToString("HH:mm:ss");
                    });

                    Task.Delay(1000);


                }
            });
            
        } 

    }
}
