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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit; //добавляем маску для ввода
//тригеры для таймера 00:00 https://metanit.com/sharp/wpf/14.3.php
namespace Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BigWindow bigWindow = new BigWindow();
        public View _view = new View();

        public MainWindow()
        {
            InitializeComponent();
            _view.LeftTeam.TeamTitle = "Россия";
            _view.LeftTeam.TeamName = "Зенит";
            _view.RightTeam.TeamTitle = "Англия";
            _view.RightTeam.TeamName = "Манчестер";
            _view.TabloInfo.TabloName = "Табло для хоккея";
            DataContext = _view;
            bigWindow.DataContext = DataContext;
        }

        private void OpenBigWindow(object sender, MouseButtonEventArgs e)
        {
            bigWindow.Show();
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

    }
}
