using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{   /// <summary>
    /// Класс стилей для табло, что бы пользователь мог его менять.
    /// </summary>
    public class WindowsStyles
    {

        /// <summary>
        /// Цвет цифр большого экрана
        /// </summary>
        public string BigWindowColorNumbers { get { return ConfigurationManager.AppSettings.Get("BigWindowColorNumbers"); } }
        /// <summary>
        /// Цвет букв большого экрана
        /// </summary>
        public string BigWindowColorText { get { return ConfigurationManager.AppSettings.Get("BigWindowColorText"); } }
        /// <summary>
        /// Цвет таймера периода малого экрана
        /// </summary>
        public string SmallWindowColorPeriod { get { return ConfigurationManager.AppSettings.Get("SmallWindowColorPeriod"); } }
        /// <summary>
        /// Цвет общей информации малого экрана
        /// </summary>
        public string SmallWindowColorInfo { get { return ConfigurationManager.AppSettings.Get("SmallWindowColorInfo"); } }
        /// <summary>
        /// Цвет часов малого экрана
        /// </summary>
        public string SmallWindowColorClock { get { return ConfigurationManager.AppSettings.Get("SmallWindowColorClock"); } }

        private bool _SmallWindowVisibleInfo = false;
        /// <summary>
        /// Видимость всей информации на малом окне (кроче часов)
        /// </summary>
        public bool SmallWindowVisibleInfo {get => _SmallWindowVisibleInfo; set => _SmallWindowVisibleInfo = value; }
    }
}
