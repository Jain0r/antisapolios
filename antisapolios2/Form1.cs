using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace antisapolios2
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hwnd, int id);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT lpwndpl);

        MenuItem configMenuItem;
        MenuItem exitMenuItem;

        NotifyIcon notifyIcon;
        Process objectiveProcess = null;
        const int mActionHotKeyID = 1;
        public Form1()
        {
            InitializeComponent();

            configMenuItem = new MenuItem("Configuration", new EventHandler(ShowConfig));
            exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon("Icono.ico");
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { configMenuItem, exitMenuItem });
            notifyIcon.Visible = true;

            Process[] processList = Process.GetProcesses();


            foreach (Process process in processList)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    if (process.ProcessName.Contains("opera"))
                        this.objectiveProcess = process;
                }
            }

        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();

            if (m.Msg == 0x0312 && m.WParam.ToInt32() == mActionHotKeyID)
            {
                GetWindowPlacement(objectiveProcess.MainWindowHandle, ref placement);
                if (placement.showCmd != 2)
                    ShowWindow(objectiveProcess.MainWindowHandle, Actions.SW_SHOWMINIMIZED);
                else
                {
                    ShowWindow(objectiveProcess.MainWindowHandle, Actions.SW_SHOWNORMAL);
                }
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ShowConfig(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        internal class Actions
        {
            public static int SW_HIDE = 0;
            public static int SW_SHOWNORMAL = 1;
            public static int SW_SHOWMINIMIZED = 2;
            public static int SW_MAXIMIZE = 3;
            public static int SW_SHOWNOACTIVATE = 4;
            public static int SW_SHOW = 5;
            public static int SW_MINIMIZE = 6;
            public static int SW_SHOWMINNOACTIVE = 7;
            public static int SW_SHOWNA = 8;
            public static int SW_RESTORE = 9;
            public static int SW_SHOWDEFAULT = 10;
            public static int SW_FORCEMINIMIZE = 11;
        }

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey(this.Handle, mActionHotKeyID, 0, (int)Keys.NumPad3);
            this.Hide();
        }
    }

}
