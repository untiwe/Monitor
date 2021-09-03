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
    public class CentralInfo : INotifyPropertyChanged
    {
        private string _TabloName;
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
        public TimeSpan GameTaimer
        {
            get => _GameTaimer;
            set
            {
                _GameTaimer = value;
                OnPropertyChanged("GameTaimerString");
            }
        }

        
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")//цель [CallerMemberName] ставить значение вызываемого set-era умолчанию, если его вызвали с пустой строкой, т.е. OnPropertyChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void SubtractTaimer(object sender, EventArgs e)
        {
            if (GameTaimer.Seconds > 0 || GameTaimer.Minutes > 0)
                GameTaimer = GameTaimer.Subtract(new TimeSpan(0, 0, 1));
        }


    }
}
