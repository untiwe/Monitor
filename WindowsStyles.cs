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
        private string _ColorNumbers;
        /// <summary>
        /// Цвет цифр большого экрана
        /// </summary>
        public string ColorNumbers { get { return ConfigurationManager.AppSettings.Get("ColorNumbers"); } }
    }
}
