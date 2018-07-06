using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace HookDemo
{
    public partial class Form1 : Form
    {

        private Hocy_Hook hook_Main ;

        public Form1()
        {
            InitializeComponent();

            hook_Main = new Hocy_Hook();
            this.hook_Main.OnMouseActivity += new MouseEventHandler(Hook_MainMouseMove);
            this.hook_Main.OnKeyDown += new KeyEventHandler(Hook_MainKeyDown);
            this.hook_Main.OnKeyPress += new KeyPressEventHandler(Hook_MainKeyPress);
            this.hook_Main.OnKeyUp += new KeyEventHandler(Hook_MainKeyUp);
        }

        #region 窗体事件

        private void Hk_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.hook_Main.UnInstallHook();
        }
         
        private void Start_Click(object sender, EventArgs e)
        {
            hook_Main.InstallHook( HookType.All);
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            this.hook_Main.UnInstallHook();
        }

        private void Stopkeyboard_Click(object sender, EventArgs e)
        {
            hook_Main.InstallHook(HookType.Mouse);
        }

        private void Btn_HookKeyboard_Click(object sender, EventArgs e)
        {
            hook_Main.InstallHook(HookType.Keyboard);
        }

        #endregion


        #region 钩子事件

        private void Hook_MainKeyUp(object sender, KeyEventArgs e)
        {
            
            LogWrite("KeyUp 		- " + e.KeyData.ToString());
        }

        private void Hook_MainMouseMove(object sender, MouseEventArgs e)
        {
            labelMousePosition.Text = String.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
            if (e.Clicks > 0) LogWrite("MouseButton 	- " + e.Button.ToString());
        }
         
        private void Hook_MainKeyDown(object sender, KeyEventArgs e)
        {
            LogWrite("KeyDown 	- " + e.KeyData.ToString());
        }

        private void Hook_MainKeyPress(object sender, KeyPressEventArgs e)
        {
            LogWrite("KeyPress 	- " + e.KeyChar);
        }

        #endregion

        #region 私有函数

        private void LogWrite(string txt)
        {

            this.resultinfo.AppendText(txt + Environment.NewLine);
            this.resultinfo.SelectionStart = this.resultinfo.Text.Length;
        }



        #endregion

     
    }
}