using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class View

    {
        public TeamInfo LeftTeam { get; set; } = new TeamInfo();
        public TeamInfo RightTeam { get; set; } = new TeamInfo();
        public CentralInfo TabloInfo { get; set; } = new CentralInfo();
        public RemovingPlayer LeftPlayer1 { get; set; } = new RemovingPlayer();
        public RemovingPlayer LeftPlayer2 { get; set; } = new RemovingPlayer();
        public RemovingPlayer LeftPlayer3 { get; set; } = new RemovingPlayer();
        public RemovingPlayer RightPlayer1 { get; set; } = new RemovingPlayer();
        public RemovingPlayer RightPlayer2 { get; set; } = new RemovingPlayer();
        public RemovingPlayer RightPlayer3 { get; set; } = new RemovingPlayer();
    }
}
