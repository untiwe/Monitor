using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
    { /// <summary>
      /// Класс контекста для привязки данных к окнам.
      /// </summary>
    public class View

    {   /// <summary>
        /// Команда с левой половины табло. Обычно хозяева
        /// </summary>
        public TeamInfo LeftTeam { get; set; } = new TeamInfo();
        /// <summary>
        /// Команда с правой половины табло. Обычно гости
        /// </summary>
        public TeamInfo RightTeam { get; set; } = new TeamInfo();
        /// <summary>
        /// Основаная информация для табло. Период,время периода, название. 
        /// </summary>
        public CentralInfo TabloInfo { get; set; } = new CentralInfo();
        /// <summary>
        /// Игрок 1 удаления, левой команды
        /// </summary>
        public RemovingPlayer LeftPlayer1 { get; set; } = new RemovingPlayer();
        /// <summary>
        /// Игрок 3 удаления, левой команды
        /// </summary>
        public RemovingPlayer LeftPlayer2 { get; set; } = new RemovingPlayer();
        /// <summary>
        /// Игрок 2 удаления, левой команды
        /// </summary>
        public RemovingPlayer LeftPlayer3 { get; set; } = new RemovingPlayer();
        /// <summary>
        /// Игрок 1 удаления, правой команды
        /// </summary>
        public RemovingPlayer RightPlayer1 { get; set; } = new RemovingPlayer();
        /// <summary>
        /// Игрок 2 удаления, правой команды
        /// </summary>
        public RemovingPlayer RightPlayer2 { get; set; } = new RemovingPlayer();
        /// <summary>
        /// Игрок 3 удаления, правой команды
        /// </summary>
        public RemovingPlayer RightPlayer3 { get; set; } = new RemovingPlayer();
        /// <summary>
        /// Цвета для текста табло
        /// </summary>
        public WindowsStyles BigWindowColors { get; set; } = new WindowsStyles();
        /// <summary>
        /// Состояние большого окна
        /// </summary>
        public ScreenState BigWindowState { get; set; } = new ScreenState();
        /// <summary>
        /// Состояние малого окна
        /// </summary>
        public ScreenState SmallWindowState { get; set; } = new ScreenState();

        /// <summary>
        /// Таймер для времени катания
        /// </summary>
        public RelaxingTimer RelaxTimer { get; set; } = new RelaxingTimer();
    }
}
