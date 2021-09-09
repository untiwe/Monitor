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
using System.Windows.Shapes;

namespace Monitor
{
    /// <summary>
    /// Логика взаимодействия для BigWindow.xaml
    /// </summary>
    /// 
    public partial class BigWindow : Window
    {

        public BigWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        public void SetStyles()
        {
            Setter setter = new Setter(Control.ForegroundProperty, Color.FromArgb(0, 0, 0, 0));
            Style = Resources["NumbersStyle"] as Style;
            Debug.WriteLine(Style);
            Style.Setters.Add(setter);
        }

        public void CreateVisibility() => this.Visibility = Visibility.Visible;
    }
}
