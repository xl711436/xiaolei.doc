namespace HookDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelMousePosition = new System.Windows.Forms.Label();
            this.Btn_HookMouse = new System.Windows.Forms.Button();
            this.resultinfo = new System.Windows.Forms.TextBox();
            this.Btn_StopHook = new System.Windows.Forms.Button();
            this.Btn_HookAll = new System.Windows.Forms.Button();
            this.Btn_HookKeyboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelMousePosition
            // 
            this.labelMousePosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMousePosition.AutoSize = true;
            this.labelMousePosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMousePosition.Location = new System.Drawing.Point(12, 338);
            this.labelMousePosition.Name = "labelMousePosition";
            this.labelMousePosition.Size = new System.Drawing.Size(43, 14);
            this.labelMousePosition.TabIndex = 9;
            this.labelMousePosition.Text = "label1";
            // 
            // Btn_HookMouse
            // 
            this.Btn_HookMouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_HookMouse.Location = new System.Drawing.Point(125, 9);
            this.Btn_HookMouse.Name = "Btn_HookMouse";
            this.Btn_HookMouse.Size = new System.Drawing.Size(75, 23);
            this.Btn_HookMouse.TabIndex = 8;
            this.Btn_HookMouse.Text = "监控鼠标";
            this.Btn_HookMouse.UseVisualStyleBackColor = true;
            this.Btn_HookMouse.Click += new System.EventHandler(this.Stopkeyboard_Click);
            // 
            // resultinfo
            // 
            this.resultinfo.Location = new System.Drawing.Point(12, 38);
            this.resultinfo.Multiline = true;
            this.resultinfo.Name = "resultinfo";
            this.resultinfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.resultinfo.Size = new System.Drawing.Size(363, 291);
            this.resultinfo.TabIndex = 7;
            this.resultinfo.Text = "监控键盘和鼠标的操作记录：";
            // 
            // Btn_StopHook
            // 
            this.Btn_StopHook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_StopHook.Location = new System.Drawing.Point(292, 9);
            this.Btn_StopHook.Name = "Btn_StopHook";
            this.Btn_StopHook.Size = new System.Drawing.Size(75, 23);
            this.Btn_StopHook.TabIndex = 6;
            this.Btn_StopHook.Text = "停止监控";
            this.Btn_StopHook.UseVisualStyleBackColor = true;
            this.Btn_StopHook.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Btn_HookAll
            // 
            this.Btn_HookAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_HookAll.Location = new System.Drawing.Point(12, 9);
            this.Btn_HookAll.Name = "Btn_HookAll";
            this.Btn_HookAll.Size = new System.Drawing.Size(102, 23);
            this.Btn_HookAll.TabIndex = 5;
            this.Btn_HookAll.Text = "监控鼠标键盘";
            this.Btn_HookAll.UseVisualStyleBackColor = true;
            this.Btn_HookAll.Click += new System.EventHandler(this.Start_Click);
            // 
            // Btn_HookKeyboard
            // 
            this.Btn_HookKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_HookKeyboard.Location = new System.Drawing.Point(203, 9);
            this.Btn_HookKeyboard.Name = "Btn_HookKeyboard";
            this.Btn_HookKeyboard.Size = new System.Drawing.Size(75, 23);
            this.Btn_HookKeyboard.TabIndex = 10;
            this.Btn_HookKeyboard.Text = "监控键盘";
            this.Btn_HookKeyboard.UseVisualStyleBackColor = true;
            this.Btn_HookKeyboard.Click += new System.EventHandler(this.Btn_HookKeyboard_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 368);
            this.Controls.Add(this.Btn_HookKeyboard);
            this.Controls.Add(this.labelMousePosition);
            this.Controls.Add(this.Btn_HookMouse);
            this.Controls.Add(this.resultinfo);
            this.Controls.Add(this.Btn_StopHook);
            this.Controls.Add(this.Btn_HookAll);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Hk_Form_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMousePosition;
        private System.Windows.Forms.Button Btn_HookMouse;
        private System.Windows.Forms.TextBox resultinfo;
        private System.Windows.Forms.Button Btn_StopHook;
        private System.Windows.Forms.Button Btn_HookAll;
        private System.Windows.Forms.Button Btn_HookKeyboard;
    }
}

