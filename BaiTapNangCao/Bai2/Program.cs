
using Bai2;
using System;
using System.Windows.Forms;

namespace BaiTapXuLyChuoi
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Diem());
        }
    }
}
