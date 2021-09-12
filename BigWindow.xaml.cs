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
        /// <summary>
        /// Конструктор большого окна
        /// </summary>
        /// 
        public BigWindow()
        {
            InitializeComponent();
            SetStyles();//пробуем поменать стили
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Устанавливаем цвет текста
        /// </summary>
        public void SetStyles()
        {
            Setter setter = new Setter(ForegroundProperty, Color.FromArgb(128, 128, 128, 0));
            Style _Style = Resources["NumbersStyle"] as Style;
            _Style.Setters.Add(setter);


            //Setter setter = new Setter(ForegroundProperty, Color.FromArgb(0, 0, 0, 0));
            //Style = Resources["NumbersStyle"] as Style;
            //Style.Setters.Add(setter);
        }
        //Debug.WriteLine(Style);

        /// <summary>
        /// Делаем окно снова видимым
        /// </summary>
        public void CreateVisibility() => this.Visibility = Visibility.Visible;
    }
}
