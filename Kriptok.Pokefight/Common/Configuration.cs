using Kriptok.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Pokefight.Common
{
    public class Configuration : BaseConfiguration
    {
        /// <summary>
        /// En el constructor le pongo la configuración por default.
        /// </summary>
        public Configuration()
        {
            Player1 = new ControlConfig()
            {
                Up = Keys.U,
                Down = Keys.J,
                Left = Keys.H,
                Right = Keys.K,
                Punch = Keys.Q
            };

            Player2 = new ControlConfig()
            {
                Up = Keys.Up,
                Down = Keys.Down,
                Left = Keys.Left,
                Right = Keys.Right,
                Punch = Keys.Enter
            };
        }

        public ControlConfig Player1;
        public ControlConfig Player2;
    }
}
