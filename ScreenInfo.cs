using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// Для предоставления информации о состоянии экранов
    /// </summary>
    public class ScreenState : INotifyPropertyChanged
    {
        private bool _IsShown;
        /// <summary>
        /// Открыто ли окно
        /// </summary>
        public bool IsShown {
            get => _IsShown; 
            set
            {
                _IsShown = value;
                OnPropertyChanged();
            }
        }

        private bool _IsFullScreen;
        /// <summary>
        /// Рястянуто ли окно на весь экран
        /// </summary>
        public bool IsFullScreen
        {
            get => _IsFullScreen;
            set
            {
                _IsFullScreen = value;
                OnPropertyChanged();
            }
        }

        private bool _IsGameShow = true;
        /// <summary>
        /// Показывается ли информация по текузей игре (для малого экрана)
        /// </summary>
        public bool IsGameShow
        {
            get => _IsGameShow;
            set
            {
                _IsGameShow = value;
                OnPropertyChanged();
            }
        }

        private bool _IsClockShow;
        /// <summary>
        /// Показываются ли часы (для малого экрана)
        /// </summary>
        public bool IsClockShow
        {
            get => _IsClockShow;
            set
            {
                _IsClockShow = value;
                OnPropertyChanged();
            }
        }

        private bool _IsRelaxTime;
        /// <summary>
        /// Установлен ли режим катания(переключение с режимом игры)
        /// </summary>
        public bool IsRelaxTime
        {
            get => _IsRelaxTime;
            set
            {
                _IsRelaxTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Событие для обновления привязаных данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод, вызываемый set-ром при обновлении данных
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string prop = "")//цель [CallerMemberName] ставить значение вызываемого set-era умолчанию, если его вызвали с пустой строкой, т.е. OnPropertyChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
