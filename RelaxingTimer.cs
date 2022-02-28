using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
   public class RelaxingTimer : INotifyPropertyChanged
    {

        private TimeSpan _RelaxTaimer;
        /// <summary>
        /// Время оставшееся до катания.
        /// </summary>
        public TimeSpan RelaxTaimer
        {
            get => _RelaxTaimer;
            set
            {
                _RelaxTaimer = value;
                OnPropertyChanged("RelaxTaimerString");
            }
        }

        /// <summary>
        /// Время оставшееся до конца катания, в виде строки. Сделано специально для привзяки к нему TextBox без дополнительной фильтраци
        /// </summary>
        /// <value>string</value>
        public string RelaxTaimerString
        {
            get
            {
                TimeSpan ts = RelaxTaimer;
                return string.Format("{0:00}:{1:ss}", Math.Floor(ts.TotalMinutes), ts);
            }
            set
            {
                try
                {
                    List<int> MinutesAndSeconds = new List<int>(value.Split(":").Select(Int32.Parse).ToArray());
                    RelaxTaimer = new TimeSpan(0, MinutesAndSeconds[0], MinutesAndSeconds[1]);
                    OnPropertyChanged("RelaxTaimerString");
                }
                catch (System.FormatException) { return; }
            }
        }


        private bool _IsRelaxTimer = true;
        /// <summary>
        /// Переключатель, указвающий, вермя катания или игры.
        /// </summary>
        public bool IsRelaxTimer
        {
            get => _IsRelaxTimer;
            set
            {
                _IsRelaxTimer = value;
                OnPropertyChanged("IsRelaxTimer");
            }
        }

        /// <summary>
        /// После с датой для показа текущей даты и временни
        /// </summary>
        public string ViewDate { get => DateTime.Now.ToString("MM.dd.yy H:mm"); set { } }

        /// <summary>
        /// Метод, вызываемый для обновлдения текущего временни
        /// </summary>
        public void UpdateViewDate() => OnPropertyChanged("ViewDate");


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
        /// Делегат, вызываемый если время катния подошло к концу
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
            UpdateViewDate();
            if (RelaxTaimer.Hours > 1 && RelaxTaimer.Seconds == 1 && RelaxTaimer.Minutes == 0)
                TimeEndEvent?.Invoke();

            if (RelaxTaimer.Hours > 0 || RelaxTaimer.Seconds > 0 || RelaxTaimer.Minutes > 0)
                RelaxTaimer = RelaxTaimer.Subtract(new TimeSpan(0, 0, 1));
            else
                TimeEndEvent?.Invoke();
        }
    }
}
