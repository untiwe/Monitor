using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class TeamInfo
    {
        private string _TeamName;//название команды
        public string TeamName { get; set; }
        private string _TeamTitle;//обозначение команды хозяева/готсти/друзья/племянники и т.д.
        public string TeamTitle { get; set; }
        private short _TeamCounter;//счетчик очков команды
        public short TeamCounter { get; set; }

        

    }
}
