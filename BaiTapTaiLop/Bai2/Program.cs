using Bai2;
using System;
using System.Windows.Forms;

namespace BaiTapQuanLyDiemCombo
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmDiem_combo());
        }
    }
}