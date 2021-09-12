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
    /// Класс для вывода онформации об удаленом игроке
    /// </summary>
    public class RemovingPlayer : INotifyPropertyChanged
    {
        private short _PlayerNomber;
        /// <summary>
        /// Номер удаленного игрока
        /// </summary>
        /// <value>short</value>
        public short PlayerNomber
        {
            get => _PlayerNomber;
            set
            {
                _PlayerNomber = value;
                OnPropertyChanged("PlayerNomber");
            }
        }
        private TimeSpan _RemovingTimer;
        /// <summary>
        /// Время оставшееся до возврата игрока
        /// </summary>
        /// <value>TimeSpan</value>
        public TimeSpan RemovingTimer
        {
            get => _RemovingTimer;
            set
            {
                _RemovingTimer = value;
                OnPropertyChanged("RemovingTimerString");
            }
        }

        /// <summary>
        /// Время оставшееся до возврата игрока в виде строки. Сделано специально для привзяки к нему TextBox без дополнительной филтрации.
        /// </summary>
        /// <value>String</value>
        public string RemovingTimerString
        {
            get
            {
                TimeSpan ts = RemovingTimer;
                return string.Format("{0:00}:{1:ss}", Math.Floor(ts.TotalMinutes), ts);
            }
            set
            {
                try
                {
                    List<int> MinutesAndSeconds = new List<int>(value.Split(":").Select(Int32.Parse).ToArray());
                    RemovingTimer = new TimeSpan(0, MinutesAndSeconds[0], MinutesAndSeconds[1]);
                    OnPropertyChanged("RemovingTimerString");
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
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        { 
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Метод уменьшения временни на 1 секунду, сделалн для привязки тику таймера DispatcherTimer
        /// </summary>
        public void SubtractTaimer(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {

                if (RemovingTimer.Seconds == 0 && RemovingTimer.Minutes == 0)
                    PlayerNomber = 0;
                if (RemovingTimer.Seconds > 0 || RemovingTimer.Minutes > 0)
                    RemovingTimer = RemovingTimer.Subtract(new TimeSpan(0, 0, 1));
            });
        }
    }
}
