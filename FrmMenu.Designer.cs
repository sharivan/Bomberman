namespace Bomberman
{
    partial class FrmMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMenu));
            this.picsingle = new System.Windows.Forms.PictureBox();
            this.picoptions = new System.Windows.Forms.PictureBox();
            this.picrank = new System.Windows.Forms.PictureBox();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picsingle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picoptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picrank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // picsingle
            // 
            this.picsingle.BackColor = System.Drawing.Color.Transparent;
            this.picsingle.Image = ((System.Drawing.Image)(resources.GetObject("picsingle.Image")));
            this.picsingle.Location = new System.Drawing.Point(78, 310);
            this.picsingle.Name = "picsingle";
            this.picsingle.Size = new System.Drawing.Size(258, 53);
            this.picsingle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picsingle.TabIndex = 0;
            this.picsingle.TabStop = false;
            this.picsingle.Click += new System.EventHandler(this.picsingle_Click);
            this.picsingle.MouseEnter += new System.EventHandler(this.picsinglenorm_MouseEnter);
            this.picsingle.MouseLeave += new System.EventHandler(this.picsingle_MouseLeave);
            // 
            // picoptions
            // 
            this.picoptions.BackColor = System.Drawing.Color.Transparent;
            this.picoptions.Image = ((System.Drawing.Image)(resources.GetObject("picoptions.Image")));
            this.picoptions.Location = new System.Drawing.Point(78, 429);
            this.picoptions.Name = "picoptions";
            this.picoptions.Size = new System.Drawing.Size(258, 53);
            this.picoptions.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picoptions.TabIndex = 2;
            this.picoptions.TabStop = false;
            this.picoptions.Click += new System.EventHandler(this.picoptions_Click);
            this.picoptions.MouseEnter += new System.EventHandler(this.picoptions_MouseEnter);
            this.picoptions.MouseLeave += new System.EventHandler(this.picoptions_MouseLeave);
            // 
            // picrank
            // 
            this.picrank.BackColor = System.Drawing.Color.Transparent;
            this.picrank.Image = ((System.Drawing.Image)(resources.GetObject("picrank.Image")));
            this.picrank.Location = new System.Drawing.Point(78, 370);
            this.picrank.Name = "picrank";
            this.picrank.Size = new System.Drawing.Size(258, 53);
            this.picrank.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picrank.TabIndex = 4;
            this.picrank.TabStop = false;
            this.picrank.Click += new System.EventHandler(this.picrank_Click);
            this.picrank.MouseEnter += new System.EventHandler(this.picrank_MouseEnter);
            this.picrank.MouseLeave += new System.EventHandler(this.picrank_MouseLeave);
            // 
            // imgLogo
            // 
            this.imgLogo.BackColor = System.Drawing.Color.Transparent;
            this.imgLogo.Image = ((System.Drawing.Image)(resources.GetObject("imgLogo.Image")));
            this.imgLogo.Location = new System.Drawing.Point(12, 12);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(760, 119);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgLogo.TabIndex = 3;
            this.imgLogo.TabStop = false;
            // 
            // FrmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.picrank);
            this.Controls.Add(this.imgLogo);
            this.Controls.Add(this.picoptions);
            this.Controls.Add(this.picsingle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BOMBERMAN";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMenu_FormClosed);
            this.Load += new System.EventHandler(this.FrmMenu_Load);
            this.Shown += new System.EventHandler(this.FrmMenu_Shown);
            this.VisibleChanged += new System.EventHandler(this.FrmMenu_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.picsingle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picoptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picrank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picsingle;
        private System.Windows.Forms.PictureBox picoptions;
        private System.Windows.Forms.PictureBox picrank;
        private System.Windows.Forms.PictureBox imgLogo;
    }
}

