using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class RemovingPlayer : INotifyPropertyChanged
    {
        private short _PlayerNomber;
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
        public TimeSpan RemovingTimer
        {
            get => _RemovingTimer;
            set
            {
                _RemovingTimer = value;
                OnPropertyChanged("RemovingTimerString");
            }
        }
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        { 
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void SubtractTaimer(object sender, EventArgs e)
        {
            if (RemovingTimer.Seconds == 0 && RemovingTimer.Minutes == 0)
                PlayerNomber = 0;
            if (RemovingTimer.Seconds > 0 || RemovingTimer.Minutes > 0)
                RemovingTimer = RemovingTimer.Subtract(new TimeSpan(0, 0, 1));
        }
    }
}
