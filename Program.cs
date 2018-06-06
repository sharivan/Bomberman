using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bomberman
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();

            if (FrmBomberman.SKIP_MENU)
                Application.Run(new FrmBomberman(null, new KeyBinding(), null));
            else
                Application.Run(new FrmMenu());
        }
    }
}
