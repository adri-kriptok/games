using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.JAEditor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MdiForm(args));

            // 20720
            // 20721
            // 20722

            // 461
            // 973

            //var f1 = File.ReadAllBytes("E:\\DOS\\Juegos\\_strateg\\jaggedA\\GAME1.SAV");
            //var f2 = File.ReadAllBytes("E:\\DOS\\Juegos\\_strateg\\jaggedA\\GAME2.SAV");
            
            //for (int i = 0; i < f1.Length; i++)
            //{
            //    if (f1[i] != f2[i])
            //    {
            //        Trace.WriteLine(i);
            
            //    }
            //}
        }
    }
}
