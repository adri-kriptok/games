using Kriptok.Asteridian.Scenes;
using Kriptok.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Asteridian
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new Level00(), s =>
            {
                s.FullScreen();
                s.Title = "Asteridian";
                s.Mode = WindowSizeEnum.M320x200;
                s.Mode = WindowSizeEnum.M320x200To240;
                s.CaptureMouse();
                s.OpenMidiNotePlayer();                
            });
        }
    }
}
