using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Monitor
{
    public partial class MainWindow : Window
    {


        private string[] com;//массив для разделения команды на ключ-значение
        private void NewCommand(string _stringComand)
        {
            com = _stringComand.Split("!:@:!");
            if (com.Length != 2)
                return;

            if (com[0] == "_view.TabloInfo.Period" && int.TryParse(com[1], out _))
                _view.TabloInfo.Period = Convert.ToInt32(com[1]);

            if (com[0] == "_view.LeftTeam.TeamCounter" && int.TryParse(com[1], out _))
                _view.LeftTeam.TeamCounter = Convert.ToInt32(com[1]);

            if (com[0] == "_view.RightTeam.TeamCounter" && int.TryParse(com[1], out _))
                _view.RightTeam.TeamCounter = Convert.ToInt32(com[1]);

            if (com[0] == "_view.LeftTeam.TeamName")
                _view.LeftTeam.TeamName = com[1];

            if (com[0] == "_view.LeftTeam.TeamTitle")
                _view.LeftTeam.TeamTitle = com[1];

            if (com[0] == "_view.RightTeam.TeamName")
                _view.RightTeam.TeamName = com[1];

            if (com[0] == "_view.RightTeam.TeamTitle")
                _view.RightTeam.TeamTitle = com[1];

            if (com[0] == "_view.TabloInfo.GameTaimerString")
                _view.TabloInfo.GameTaimerString = com[1];

            if (com[0] == "_view.LeftPlayer1.PlayerNomber" && int.TryParse(com[1], out _))
                _view.LeftPlayer1.PlayerNomber = Convert.ToInt32(com[1]);

            if (com[0] == "_view.LeftPlayer1.RemovingTimerString")
            {
                if (com[1] == "")
                    _view.LeftPlayer1.RemovingTimerString = "00:00";
                else
                    _view.LeftPlayer1.RemovingTimerString = com[1];
            }

            if (com[0] == "_view.LeftPlayer2.PlayerNomber" && int.TryParse(com[1], out _))
                _view.LeftPlayer2.PlayerNomber = Convert.ToInt32(com[1]);

            if (com[0] == "_view.LeftPlayer2.RemovingTimerString")
            {
                if (com[1] == "")
                    _view.LeftPlayer2.RemovingTimerString = "00:00";
                else
                    _view.LeftPlayer2.RemovingTimerString = com[1];
            }

            if (com[0] == "_view.LeftPlayer3.PlayerNomber" && int.TryParse(com[1], out _))
                _view.LeftPlayer3.PlayerNomber = Convert.ToInt32(com[1]);

            if (com[0] == "_view.LeftPlayer3.RemovingTimerString")
            {
                if (com[1] == "")
                    _view.LeftPlayer3.RemovingTimerString = "00:00";
                else
                    _view.LeftPlayer3.RemovingTimerString = com[1];
            }

            if (com[0] == "_view.RightPlayer1.PlayerNomber" && int.TryParse(com[1], out _))
                _view.RightPlayer1.PlayerNomber = Convert.ToInt32(com[1]);

            if (com[0] == "_view.RightPlayer1.RemovingTimerString")
            {
                if (com[1] == "")
                    _view.RightPlayer1.RemovingTimerString = "00:00";
                else
                    _view.RightPlayer1.RemovingTimerString = com[1];

            }

            if (com[0] == "_view.RightPlayer2.PlayerNomber" && int.TryParse(com[1], out _))
                _view.RightPlayer2.PlayerNomber = Convert.ToInt32(com[1]);

            if (com[0] == "_view.RightPlayer2.RemovingTimerString")
            {
                if (com[1] == "")
                    _view.RightPlayer2.RemovingTimerString = "00:00";
                else
                    _view.RightPlayer2.RemovingTimerString = com[1];
            }

            if (com[0] == "_view.RightPlayer3.PlayerNomber" && int.TryParse(com[1], out _))
                _view.RightPlayer3.PlayerNomber = Convert.ToInt32(com[1]);

            if (com[0] == "_view.RightPlayer3.RemovingTimerString")
            {
                if (com[1] == "")
                    _view.RightPlayer3.RemovingTimerString = "00:00";
                else
                    _view.RightPlayer3.RemovingTimerString = com[1];
            }

            if (com[0] == "StartStopTimer")
                Dispatcher.Invoke(() => StartStopMainTimer());//поток не переключается, не выполнив функцию, пришлось насильно вызывать через Dispatcher(но 100% есть способ лучше)

            if (com[0] == "_view.TabloInfo.TabloName")
                _view.TabloInfo.TabloName = com[1];

            if (com[0] == "_view.BigWindowState.IsShown")
                Dispatcher.Invoke(() => _OpenBigWindow());

            if (com[0] == "_view.BigWindowState.IsFullScreen")
                Dispatcher.Invoke(() => _StretchBigWindow());

            if (com[0] == "_view.SmallWindowState.IsShown")
                Dispatcher.Invoke(() => _OpenSmallWindow());

            if (com[0] == "_view.SmallWindowState.IsFullScreen")
                Dispatcher.Invoke(() => _StretchSmallWindow());

            if (com[0] == "_view.SmallWindowState.IsClockShow")
                Dispatcher.Invoke(() => _ChangeClock());

            if (com[0] == "_view.SmallWindowState.IsGameShow")
                Dispatcher.Invoke(() => _ChangeSwhowInfo());

            //работа с тайсером катания

            if (com[0] == "StartStopRelaxTimer")
                StartStopRelaxTimer();

            if (com[0] == "_view.RelaxTimer.IsRelaxTimer")
                ToggleRelaxTimer();


            if (com[0] == "_view.RelaxTimer.RelaxTaimerString")
                _view.RelaxTimer.RelaxTaimerString = com[1];
            
            
            







            Debug.WriteLine(com[0]);
            SendStatusToSockets();

        }

        private void SendStatusToSockets()
        {
            WebServer.WebServer.SendMessAllSockets(JsonSerializer.Serialize(new { mainTimerStatus = timer.IsEnabled, relaxTimerStatus = relaxTimer.IsEnabled, _view }));
        }
    }
}
