namespace TestLayoutEngine
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlModuleSelector = new System.Windows.Forms.Panel();
            pnlMenu = new System.Windows.Forms.Panel();
            pnlDashboard = new System.Windows.Forms.Panel();
            pnlLogo = new System.Windows.Forms.Panel();
            pnlDialogView = new System.Windows.Forms.Panel();
            btnDump = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // pnlModuleSelector
            // 
            pnlModuleSelector.BackColor = System.Drawing.Color.Gray;
            pnlModuleSelector.Location = new System.Drawing.Point(161, 15);
            pnlModuleSelector.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlModuleSelector.Name = "pnlModuleSelector";
            pnlModuleSelector.Size = new System.Drawing.Size(599, 162);
            pnlModuleSelector.TabIndex = 0;
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = System.Drawing.Color.Blue;
            pnlMenu.Location = new System.Drawing.Point(13, 185);
            pnlMenu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new System.Drawing.Size(225, 576);
            pnlMenu.TabIndex = 1;
            // 
            // pnlDashboard
            // 
            pnlDashboard.BackColor = System.Drawing.Color.Green;
            pnlDashboard.Location = new System.Drawing.Point(767, 15);
            pnlDashboard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlDashboard.Name = "pnlDashboard";
            pnlDashboard.Size = new System.Drawing.Size(321, 659);
            pnlDashboard.TabIndex = 2;
            // 
            // pnlLogo
            // 
            pnlLogo.BackColor = System.Drawing.Color.Red;
            pnlLogo.Location = new System.Drawing.Point(13, 15);
            pnlLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new System.Drawing.Size(142, 162);
            pnlLogo.TabIndex = 3;
            // 
            // pnlDialogView
            // 
            pnlDialogView.BackColor = System.Drawing.Color.Goldenrod;
            pnlDialogView.Location = new System.Drawing.Point(244, 185);
            pnlDialogView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlDialogView.Name = "pnlDialogView";
            pnlDialogView.Size = new System.Drawing.Size(516, 576);
            pnlDialogView.TabIndex = 4;
            // 
            // btnDump
            // 
            btnDump.Location = new System.Drawing.Point(0, 0);
            btnDump.Name = "btnDump";
            btnDump.Size = new System.Drawing.Size(131, 40);
            btnDump.TabIndex = 5;
            btnDump.Text = "Dump";
            btnDump.UseVisualStyleBackColor = true;
            btnDump.Click += btnDump_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new System.Drawing.Size(1124, 924);
            Controls.Add(btnDump);
            Controls.Add(pnlDialogView);
            Controls.Add(pnlLogo);
            Controls.Add(pnlDashboard);
            Controls.Add(pnlMenu);
            Controls.Add(pnlModuleSelector);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            VisibleChanged += Form1_VisibleChanged;
            Layout += Form1_Layout;
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlLogo;

        private System.Windows.Forms.Panel pnlDashboard;

        private System.Windows.Forms.Panel pnlMenu;

        private System.Windows.Forms.Panel pnlModuleSelector;

        #endregion

        private System.Windows.Forms.Panel pnlDialogView;
        private System.Windows.Forms.Button btnDump;
    }
}