namespace Bomberman
{
    partial class FrmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOptions));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLeftLabel = new System.Windows.Forms.Label();
            this.lblUpLabel = new System.Windows.Forms.Label();
            this.lblRightLabel = new System.Windows.Forms.Label();
            this.lblDownLabel = new System.Windows.Forms.Label();
            this.lblDropBombLabel = new System.Windows.Forms.Label();
            this.lblKickLabel = new System.Windows.Forms.Label();
            this.lblDetonateLabel = new System.Windows.Forms.Label();
            this.lblPauseLabel = new System.Windows.Forms.Label();
            this.lblLeftKey = new System.Windows.Forms.Label();
            this.lblUpKey = new System.Windows.Forms.Label();
            this.lblRightKey = new System.Windows.Forms.Label();
            this.lblDownKey = new System.Windows.Forms.Label();
            this.lblDropBombKey = new System.Windows.Forms.Label();
            this.lblKickKey = new System.Windows.Forms.Label();
            this.lblDetonateKey = new System.Windows.Forms.Label();
            this.lblPauseKey = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.picDiscard = new System.Windows.Forms.PictureBox();
            this.picRestoreDefaults = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiscard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRestoreDefaults)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.lblLeftLabel);
            this.flowLayoutPanel1.Controls.Add(this.lblUpLabel);
            this.flowLayoutPanel1.Controls.Add(this.lblRightLabel);
            this.flowLayoutPanel1.Controls.Add(this.lblDownLabel);
            this.flowLayoutPanel1.Controls.Add(this.lblDropBombLabel);
            this.flowLayoutPanel1.Controls.Add(this.lblKickLabel);
            this.flowLayoutPanel1.Controls.Add(this.lblDetonateLabel);
            this.flowLayoutPanel1.Controls.Add(this.lblPauseLabel);
            this.flowLayoutPanel1.Controls.Add(this.lblLeftKey);
            this.flowLayoutPanel1.Controls.Add(this.lblUpKey);
            this.flowLayoutPanel1.Controls.Add(this.lblRightKey);
            this.flowLayoutPanel1.Controls.Add(this.lblDownKey);
            this.flowLayoutPanel1.Controls.Add(this.lblDropBombKey);
            this.flowLayoutPanel1.Controls.Add(this.lblKickKey);
            this.flowLayoutPanel1.Controls.Add(this.lblDetonateKey);
            this.flowLayoutPanel1.Controls.Add(this.lblPauseKey);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(152, 151);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(498, 248);
            this.flowLayoutPanel1.TabIndex = 16;
            // 
            // lblLeftLabel
            // 
            this.lblLeftLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeftLabel.ForeColor = System.Drawing.Color.White;
            this.lblLeftLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLeftLabel.Location = new System.Drawing.Point(3, 0);
            this.lblLeftLabel.Name = "lblLeftLabel";
            this.lblLeftLabel.Size = new System.Drawing.Size(160, 30);
            this.lblLeftLabel.TabIndex = 8;
            this.lblLeftLabel.Text = "Esquerda";
            this.lblLeftLabel.DoubleClick += new System.EventHandler(this.lblLeftKey_DoubleClick);
            this.lblLeftLabel.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblLeftLabel.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblUpLabel
            // 
            this.lblUpLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblUpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpLabel.ForeColor = System.Drawing.Color.White;
            this.lblUpLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblUpLabel.Location = new System.Drawing.Point(3, 30);
            this.lblUpLabel.Name = "lblUpLabel";
            this.lblUpLabel.Size = new System.Drawing.Size(160, 30);
            this.lblUpLabel.TabIndex = 4;
            this.lblUpLabel.Text = "Cima";
            this.lblUpLabel.DoubleClick += new System.EventHandler(this.lblUpKey_DoubleClick);
            this.lblUpLabel.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblUpLabel.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblRightLabel
            // 
            this.lblRightLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblRightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRightLabel.ForeColor = System.Drawing.Color.White;
            this.lblRightLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblRightLabel.Location = new System.Drawing.Point(3, 60);
            this.lblRightLabel.Name = "lblRightLabel";
            this.lblRightLabel.Size = new System.Drawing.Size(160, 30);
            this.lblRightLabel.TabIndex = 2;
            this.lblRightLabel.Text = "Direita";
            this.lblRightLabel.DoubleClick += new System.EventHandler(this.lblRightKey_DoubleClick);
            this.lblRightLabel.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblRightLabel.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblDownLabel
            // 
            this.lblDownLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblDownLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownLabel.ForeColor = System.Drawing.Color.White;
            this.lblDownLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDownLabel.Location = new System.Drawing.Point(3, 90);
            this.lblDownLabel.Name = "lblDownLabel";
            this.lblDownLabel.Size = new System.Drawing.Size(160, 30);
            this.lblDownLabel.TabIndex = 10;
            this.lblDownLabel.Text = "Baixo";
            this.lblDownLabel.DoubleClick += new System.EventHandler(this.lblDownKey_DoubleClick);
            this.lblDownLabel.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblDownLabel.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblDropBombLabel
            // 
            this.lblDropBombLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblDropBombLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDropBombLabel.ForeColor = System.Drawing.Color.White;
            this.lblDropBombLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDropBombLabel.Location = new System.Drawing.Point(3, 120);
            this.lblDropBombLabel.Name = "lblDropBombLabel";
            this.lblDropBombLabel.Size = new System.Drawing.Size(160, 30);
            this.lblDropBombLabel.TabIndex = 16;
            this.lblDropBombLabel.Text = "Plantar bomba";
            this.lblDropBombLabel.DoubleClick += new System.EventHandler(this.lblDropBombKey_DoubleClick);
            this.lblDropBombLabel.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblDropBombLabel.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblKickLabel
            // 
            this.lblKickLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblKickLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKickLabel.ForeColor = System.Drawing.Color.White;
            this.lblKickLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblKickLabel.Location = new System.Drawing.Point(3, 150);
            this.lblKickLabel.Name = "lblKickLabel";
            this.lblKickLabel.Size = new System.Drawing.Size(160, 30);
            this.lblKickLabel.TabIndex = 18;
            this.lblKickLabel.Text = "Chutar";
            this.lblKickLabel.DoubleClick += new System.EventHandler(this.lblKickKey_DoubleClick);
            this.lblKickLabel.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblKickLabel.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblDetonateLabel
            // 
            this.lblDetonateLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblDetonateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetonateLabel.ForeColor = System.Drawing.Color.White;
            this.lblDetonateLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetonateLabel.Location = new System.Drawing.Point(3, 180);
            this.lblDetonateLabel.Name = "lblDetonateLabel";
            this.lblDetonateLabel.Size = new System.Drawing.Size(160, 30);
            this.lblDetonateLabel.TabIndex = 20;
            this.lblDetonateLabel.Text = "Detonar bomba";
            this.lblDetonateLabel.DoubleClick += new System.EventHandler(this.lblDetonateKey_DoubleClick);
            this.lblDetonateLabel.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblDetonateLabel.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblPauseLabel
            // 
            this.lblPauseLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblPauseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPauseLabel.ForeColor = System.Drawing.Color.White;
            this.lblPauseLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblPauseLabel.Location = new System.Drawing.Point(3, 210);
            this.lblPauseLabel.Name = "lblPauseLabel";
            this.lblPauseLabel.Size = new System.Drawing.Size(160, 30);
            this.lblPauseLabel.TabIndex = 22;
            this.lblPauseLabel.Text = "Pausar jogo";
            this.lblPauseLabel.DoubleClick += new System.EventHandler(this.lblPauseKey_DoubleClick);
            this.lblPauseLabel.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblPauseLabel.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblLeftKey
            // 
            this.lblLeftKey.BackColor = System.Drawing.Color.Transparent;
            this.lblLeftKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeftKey.ForeColor = System.Drawing.Color.White;
            this.lblLeftKey.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLeftKey.Location = new System.Drawing.Point(169, 0);
            this.lblLeftKey.Name = "lblLeftKey";
            this.lblLeftKey.Size = new System.Drawing.Size(329, 30);
            this.lblLeftKey.TabIndex = 3;
            this.lblLeftKey.Text = "Seta para esquerda";
            this.lblLeftKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblLeftKey.DoubleClick += new System.EventHandler(this.lblLeftKey_DoubleClick);
            this.lblLeftKey.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblLeftKey.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblUpKey
            // 
            this.lblUpKey.BackColor = System.Drawing.Color.Transparent;
            this.lblUpKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpKey.ForeColor = System.Drawing.Color.White;
            this.lblUpKey.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblUpKey.Location = new System.Drawing.Point(169, 30);
            this.lblUpKey.Name = "lblUpKey";
            this.lblUpKey.Size = new System.Drawing.Size(329, 30);
            this.lblUpKey.TabIndex = 5;
            this.lblUpKey.Text = "Seta para cima";
            this.lblUpKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblUpKey.DoubleClick += new System.EventHandler(this.lblUpKey_DoubleClick);
            this.lblUpKey.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblUpKey.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblRightKey
            // 
            this.lblRightKey.BackColor = System.Drawing.Color.Transparent;
            this.lblRightKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRightKey.ForeColor = System.Drawing.Color.White;
            this.lblRightKey.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblRightKey.Location = new System.Drawing.Point(169, 60);
            this.lblRightKey.Name = "lblRightKey";
            this.lblRightKey.Size = new System.Drawing.Size(329, 30);
            this.lblRightKey.TabIndex = 9;
            this.lblRightKey.Text = "Seta para direita";
            this.lblRightKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblRightKey.DoubleClick += new System.EventHandler(this.lblRightKey_DoubleClick);
            this.lblRightKey.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblRightKey.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblDownKey
            // 
            this.lblDownKey.BackColor = System.Drawing.Color.Transparent;
            this.lblDownKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownKey.ForeColor = System.Drawing.Color.White;
            this.lblDownKey.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDownKey.Location = new System.Drawing.Point(169, 90);
            this.lblDownKey.Name = "lblDownKey";
            this.lblDownKey.Size = new System.Drawing.Size(329, 30);
            this.lblDownKey.TabIndex = 11;
            this.lblDownKey.Text = "Seta para baixo";
            this.lblDownKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblDownKey.DoubleClick += new System.EventHandler(this.lblDownKey_DoubleClick);
            this.lblDownKey.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblDownKey.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblDropBombKey
            // 
            this.lblDropBombKey.BackColor = System.Drawing.Color.Transparent;
            this.lblDropBombKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDropBombKey.ForeColor = System.Drawing.Color.White;
            this.lblDropBombKey.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDropBombKey.Location = new System.Drawing.Point(169, 120);
            this.lblDropBombKey.Name = "lblDropBombKey";
            this.lblDropBombKey.Size = new System.Drawing.Size(329, 30);
            this.lblDropBombKey.TabIndex = 17;
            this.lblDropBombKey.Text = "Espaço";
            this.lblDropBombKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblDropBombKey.DoubleClick += new System.EventHandler(this.lblDropBombKey_DoubleClick);
            this.lblDropBombKey.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblDropBombKey.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblKickKey
            // 
            this.lblKickKey.BackColor = System.Drawing.Color.Transparent;
            this.lblKickKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKickKey.ForeColor = System.Drawing.Color.White;
            this.lblKickKey.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblKickKey.Location = new System.Drawing.Point(169, 150);
            this.lblKickKey.Name = "lblKickKey";
            this.lblKickKey.Size = new System.Drawing.Size(329, 30);
            this.lblKickKey.TabIndex = 19;
            this.lblKickKey.Text = "K";
            this.lblKickKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblKickKey.DoubleClick += new System.EventHandler(this.lblKickKey_DoubleClick);
            this.lblKickKey.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblKickKey.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblDetonateKey
            // 
            this.lblDetonateKey.BackColor = System.Drawing.Color.Transparent;
            this.lblDetonateKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetonateKey.ForeColor = System.Drawing.Color.White;
            this.lblDetonateKey.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetonateKey.Location = new System.Drawing.Point(169, 180);
            this.lblDetonateKey.Name = "lblDetonateKey";
            this.lblDetonateKey.Size = new System.Drawing.Size(329, 30);
            this.lblDetonateKey.TabIndex = 21;
            this.lblDetonateKey.Text = "Control";
            this.lblDetonateKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblDetonateKey.DoubleClick += new System.EventHandler(this.lblDetonateKey_DoubleClick);
            this.lblDetonateKey.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblDetonateKey.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // lblPauseKey
            // 
            this.lblPauseKey.BackColor = System.Drawing.Color.Transparent;
            this.lblPauseKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPauseKey.ForeColor = System.Drawing.Color.White;
            this.lblPauseKey.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblPauseKey.Location = new System.Drawing.Point(169, 210);
            this.lblPauseKey.Name = "lblPauseKey";
            this.lblPauseKey.Size = new System.Drawing.Size(329, 30);
            this.lblPauseKey.TabIndex = 23;
            this.lblPauseKey.Text = "Enter";
            this.lblPauseKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPauseKey.DoubleClick += new System.EventHandler(this.lblPauseKey_DoubleClick);
            this.lblPauseKey.MouseEnter += new System.EventHandler(this.Action_MouseEnter);
            this.lblPauseKey.MouseLeave += new System.EventHandler(this.Action_MouseLeave);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel2.Controls.Add(this.picSave);
            this.flowLayoutPanel2.Controls.Add(this.picDiscard);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(135, 464);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(528, 59);
            this.flowLayoutPanel2.TabIndex = 20;
            // 
            // picSave
            // 
            this.picSave.BackColor = System.Drawing.Color.Transparent;
            this.picSave.Image = ((System.Drawing.Image)(resources.GetObject("picSave.Image")));
            this.picSave.Location = new System.Drawing.Point(3, 3);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(258, 53);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picSave.TabIndex = 21;
            this.picSave.TabStop = false;
            this.picSave.Click += new System.EventHandler(this.picSave_Click);
            this.picSave.MouseEnter += new System.EventHandler(this.picsingle_MouseEnter);
            this.picSave.MouseLeave += new System.EventHandler(this.picSave_MouseLeave);
            // 
            // picDiscard
            // 
            this.picDiscard.BackColor = System.Drawing.Color.Transparent;
            this.picDiscard.Image = ((System.Drawing.Image)(resources.GetObject("picDiscard.Image")));
            this.picDiscard.Location = new System.Drawing.Point(267, 3);
            this.picDiscard.Name = "picDiscard";
            this.picDiscard.Size = new System.Drawing.Size(258, 53);
            this.picDiscard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picDiscard.TabIndex = 22;
            this.picDiscard.TabStop = false;
            this.picDiscard.Click += new System.EventHandler(this.picDiscard_Click);
            this.picDiscard.MouseEnter += new System.EventHandler(this.picDiscard_MouseEnter);
            this.picDiscard.MouseLeave += new System.EventHandler(this.picDiscard_MouseLeave);
            // 
            // picRestoreDefaults
            // 
            this.picRestoreDefaults.BackColor = System.Drawing.Color.Transparent;
            this.picRestoreDefaults.Image = ((System.Drawing.Image)(resources.GetObject("picRestoreDefaults.Image")));
            this.picRestoreDefaults.Location = new System.Drawing.Point(274, 526);
            this.picRestoreDefaults.Name = "picRestoreDefaults";
            this.picRestoreDefaults.Size = new System.Drawing.Size(258, 53);
            this.picRestoreDefaults.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picRestoreDefaults.TabIndex = 24;
            this.picRestoreDefaults.TabStop = false;
            this.picRestoreDefaults.Click += new System.EventHandler(this.picRestoreDefaults_Click);
            this.picRestoreDefaults.MouseEnter += new System.EventHandler(this.picRestoreDefaults_MouseEnter);
            this.picRestoreDefaults.MouseLeave += new System.EventHandler(this.picRestoreDefaults_MouseLeave);
            // 
            // FrmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(798, 601);
            this.Controls.Add(this.picRestoreDefaults);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmOptions";
            this.Text = "Opções";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmOptions_FormClosed);
            this.Load += new System.EventHandler(this.FrmOptions_Load);
            this.Shown += new System.EventHandler(this.FrmOptions_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmOptions_KeyDown);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiscard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRestoreDefaults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblLeftKey;
        private System.Windows.Forms.Label lblRightLabel;
        private System.Windows.Forms.Label lblUpKey;
        private System.Windows.Forms.Label lblUpLabel;
        private System.Windows.Forms.Label lblDownKey;
        private System.Windows.Forms.Label lblDownLabel;
        private System.Windows.Forms.Label lblRightKey;
        private System.Windows.Forms.Label lblLeftLabel;
        private System.Windows.Forms.Label lblPauseKey;
        private System.Windows.Forms.Label lblPauseLabel;
        private System.Windows.Forms.Label lblDetonateKey;
        private System.Windows.Forms.Label lblDetonateLabel;
        private System.Windows.Forms.Label lblKickKey;
        private System.Windows.Forms.Label lblKickLabel;
        private System.Windows.Forms.Label lblDropBombKey;
        private System.Windows.Forms.Label lblDropBombLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.PictureBox picSave;
        private System.Windows.Forms.PictureBox picDiscard;
        private System.Windows.Forms.PictureBox picRestoreDefaults;

    }
}