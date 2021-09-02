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
    }
}
