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
    /// Класс, предоставляющий основную информацию о команде.
    /// </summary>
    public class TeamInfo : INotifyPropertyChanged
    {
        private string _TeamName;
        /// <summary>
        /// название команды
        /// </summary>
        /// <value>string</value>
        public string TeamName
        {
            get => _TeamName;
            set
            {
                _TeamName = value;
                OnPropertyChanged("TeamName");
            }
        }
        private string _TeamTitle;
        /// <summary>
        /// обозначение команды хозяева/готсти/друзья/племянники и т.д.
        /// </summary>
        /// <value>string</value>
        public string TeamTitle
        {
            get => _TeamTitle;
            set
            {
                _TeamTitle = value;
                OnPropertyChanged("TeamTitle");//или OnPropertyChanged();
            }
        }
        private short _TeamCounter = 0;//счетчик очков команды
        /// <summary>
        /// Счетчик отков команды
        /// </summary>
        /// <value>short</value>
        public short TeamCounter 
        { 
            get => _TeamCounter; 
            set
            {
                _TeamCounter = value;
                if (_TeamCounter < 0)
                    _TeamCounter = 0;
                OnPropertyChanged("TeamCounter");//или OnPropertyChanged();
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
