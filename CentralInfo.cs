using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// Основаная информация, выводима на табло.
    /// </summary>
    public class CentralInfo : INotifyPropertyChanged
    {
        private string _TabloName;
        /// <summary>
        /// Название табло/места/стадиона по желанию оператора.
        /// </summary>
        /// <value>string</value>
        public string TabloName
        {
            get => _TabloName;
            set
            {
                _TabloName = value;
                OnPropertyChanged("TabloName");
            }
        }
        private short _Period;
        /// <summary>
        /// Номер текущего периода
        /// </summary>
        /// <value>short</value>
        public short Period
        {
            get => _Period;
            set
            {
                _Period = value;
                if (_Period < 0)
                    _Period = 0;
                OnPropertyChanged("Period");
            }
        }
       
        private TimeSpan _GameTaimer;
        /// <summary>
        /// Время оставшееся до конца периода.
        /// </summary>
        /// <value>TimeSpan</value>
        public TimeSpan GameTaimer
        {
            get => _GameTaimer;
            set
            {
                _GameTaimer = value;
                OnPropertyChanged("GameTaimerString");
            }
        }

        /// <summary>
        /// Время оставшееся до конца периода, в виде строки. Сделано специально для привзяки к нему TextBox без дополнительной филтраци
        /// </summary>
        /// <value>string</value>
        public string GameTaimerString
        {
            get 
            {
                TimeSpan ts = GameTaimer;
                return string.Format("{0:00}:{1:ss}", Math.Floor(ts.TotalMinutes), ts);
            }
            set
            {
                try
                {
                List<int> MinutesAndSeconds = new List<int>(value.Split(":").Select(Int32.Parse).ToArray());
                GameTaimer = new TimeSpan (0, MinutesAndSeconds[0], MinutesAndSeconds[1]);
                OnPropertyChanged("GameTaimerString");
                }
                catch (System.FormatException) { return; }
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

        /// <summary>
        /// Делегат, вызываемый если время периода подошло к концу
        /// </summary>
        public delegate void TimerEnded();

        /// <summary>
        /// Собитые, которое возникает, когда заканчивается время таймера
        /// </summary>
        public event TimerEnded TimeEndEvent;

        /// <summary>
        /// Метод уменьшения временни на 1 секунду, сделалн для привязки тику таймера DispatcherTimer
        /// </summary>
        public void SubtractTaimer(object sender, EventArgs e)
        {

            if (GameTaimer.Seconds == 1 && GameTaimer.Minutes == 0)
                TimeEndEvent?.Invoke();

            if (GameTaimer.Seconds > 0 || GameTaimer.Minutes > 0)
                GameTaimer = GameTaimer.Subtract(new TimeSpan(0, 0, 1));
            else
                TimeEndEvent?.Invoke();


        }


    }
}
