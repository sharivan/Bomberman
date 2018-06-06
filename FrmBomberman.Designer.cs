using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bomberman
{
    partial class FrmBomberman
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBomberman));
            this.ilBombermanWalkDown = new System.Windows.Forms.ImageList(this.components);
            this.ilBombermanWalkLeft = new System.Windows.Forms.ImageList(this.components);
            this.ilBombermanWalkUp = new System.Windows.Forms.ImageList(this.components);
            this.ilBombermanWalkRight = new System.Windows.Forms.ImageList(this.components);
            this.ilBlocks = new System.Windows.Forms.ImageList(this.components);
            this.ilBomb = new System.Windows.Forms.ImageList(this.components);
            this.ilFlame = new System.Windows.Forms.ImageList(this.components);
            this.ilSoftBlock00 = new System.Windows.Forms.ImageList(this.components);
            this.ilPowerUp = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep00Up = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep00Right = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep00Down = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep00Left = new System.Windows.Forms.ImageList(this.components);
            this.ilBombermanDeath = new System.Windows.Forms.ImageList(this.components);
            this.ilPortal = new System.Windows.Forms.ImageList(this.components);
            this.ilRedBomb = new System.Windows.Forms.ImageList(this.components);
            this.ilBackground = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep01Up = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep01Right = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep01Down = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep01Left = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep02Up = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep02Right = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep02Down = new System.Windows.Forms.ImageList(this.components);
            this.ilCreep02Left = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus00Up = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus00Right = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus00Down = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus00Left = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus01Up = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus01Right = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus01Down = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus01Left = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus02Up = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus02Right = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus02Down = new System.Windows.Forms.ImageList(this.components);
            this.ilCactus02Left = new System.Windows.Forms.ImageList(this.components);
            this.ilSoftBlock01 = new System.Windows.Forms.ImageList(this.components);
            this.ilSoftBlock02 = new System.Windows.Forms.ImageList(this.components);
            this.ilSoftBlock03 = new System.Windows.Forms.ImageList(this.components);
            this.ilSoftBlock04 = new System.Windows.Forms.ImageList(this.components);
            this.ilSoftBlock05 = new System.Windows.Forms.ImageList(this.components);
            this.ilSoftBlock06 = new System.Windows.Forms.ImageList(this.components);
            this.ilBombermanKickLeft = new System.Windows.Forms.ImageList(this.components);
            this.ilBombermanKickUp = new System.Windows.Forms.ImageList(this.components);
            this.ilBombermanKickRight = new System.Windows.Forms.ImageList(this.components);
            this.ilBombermanKickDown = new System.Windows.Forms.ImageList(this.components);
            this.ilRemoteControlBomb = new System.Windows.Forms.ImageList(this.components);
            this.ilRemoteControlRedBomb = new System.Windows.Forms.ImageList(this.components);
            this.ilTopPanel = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // ilBombermanWalkDown
            // 
            this.ilBombermanWalkDown.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanWalkDown.ImageStream")));
            this.ilBombermanWalkDown.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanWalkDown.Images.SetKeyName(0, "00.png");
            this.ilBombermanWalkDown.Images.SetKeyName(1, "01.png");
            this.ilBombermanWalkDown.Images.SetKeyName(2, "02.png");
            this.ilBombermanWalkDown.Images.SetKeyName(3, "03.png");
            this.ilBombermanWalkDown.Images.SetKeyName(4, "04.png");
            this.ilBombermanWalkDown.Images.SetKeyName(5, "05.png");
            this.ilBombermanWalkDown.Images.SetKeyName(6, "06.png");
            this.ilBombermanWalkDown.Images.SetKeyName(7, "07.png");
            // 
            // ilBombermanWalkLeft
            // 
            this.ilBombermanWalkLeft.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanWalkLeft.ImageStream")));
            this.ilBombermanWalkLeft.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanWalkLeft.Images.SetKeyName(0, "00.png");
            this.ilBombermanWalkLeft.Images.SetKeyName(1, "01.png");
            this.ilBombermanWalkLeft.Images.SetKeyName(2, "02.png");
            this.ilBombermanWalkLeft.Images.SetKeyName(3, "03.png");
            this.ilBombermanWalkLeft.Images.SetKeyName(4, "04.png");
            this.ilBombermanWalkLeft.Images.SetKeyName(5, "05.png");
            this.ilBombermanWalkLeft.Images.SetKeyName(6, "06.png");
            this.ilBombermanWalkLeft.Images.SetKeyName(7, "07.png");
            // 
            // ilBombermanWalkUp
            // 
            this.ilBombermanWalkUp.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanWalkUp.ImageStream")));
            this.ilBombermanWalkUp.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanWalkUp.Images.SetKeyName(0, "00.png");
            this.ilBombermanWalkUp.Images.SetKeyName(1, "01.png");
            this.ilBombermanWalkUp.Images.SetKeyName(2, "02.png");
            this.ilBombermanWalkUp.Images.SetKeyName(3, "03.png");
            this.ilBombermanWalkUp.Images.SetKeyName(4, "04.png");
            this.ilBombermanWalkUp.Images.SetKeyName(5, "05.png");
            this.ilBombermanWalkUp.Images.SetKeyName(6, "06.png");
            this.ilBombermanWalkUp.Images.SetKeyName(7, "07.png");
            // 
            // ilBombermanWalkRight
            // 
            this.ilBombermanWalkRight.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanWalkRight.ImageStream")));
            this.ilBombermanWalkRight.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanWalkRight.Images.SetKeyName(0, "00.png");
            this.ilBombermanWalkRight.Images.SetKeyName(1, "01.png");
            this.ilBombermanWalkRight.Images.SetKeyName(2, "02.png");
            this.ilBombermanWalkRight.Images.SetKeyName(3, "03.png");
            this.ilBombermanWalkRight.Images.SetKeyName(4, "04.png");
            this.ilBombermanWalkRight.Images.SetKeyName(5, "05.png");
            this.ilBombermanWalkRight.Images.SetKeyName(6, "06.png");
            this.ilBombermanWalkRight.Images.SetKeyName(7, "07.png");
            // 
            // ilBlocks
            // 
            this.ilBlocks.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBlocks.ImageStream")));
            this.ilBlocks.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBlocks.Images.SetKeyName(0, "000.png");
            this.ilBlocks.Images.SetKeyName(1, "001.png");
            this.ilBlocks.Images.SetKeyName(2, "002.png");
            this.ilBlocks.Images.SetKeyName(3, "003.png");
            this.ilBlocks.Images.SetKeyName(4, "004.png");
            this.ilBlocks.Images.SetKeyName(5, "005.png");
            this.ilBlocks.Images.SetKeyName(6, "06.png");
            // 
            // ilBomb
            // 
            this.ilBomb.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBomb.ImageStream")));
            this.ilBomb.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBomb.Images.SetKeyName(0, "000.png");
            this.ilBomb.Images.SetKeyName(1, "001.png");
            this.ilBomb.Images.SetKeyName(2, "002.png");
            // 
            // ilFlame
            // 
            this.ilFlame.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFlame.ImageStream")));
            this.ilFlame.TransparentColor = System.Drawing.Color.Transparent;
            this.ilFlame.Images.SetKeyName(0, "000.png");
            this.ilFlame.Images.SetKeyName(1, "001.png");
            this.ilFlame.Images.SetKeyName(2, "002.png");
            this.ilFlame.Images.SetKeyName(3, "003.png");
            this.ilFlame.Images.SetKeyName(4, "004.png");
            // 
            // ilSoftBlock00
            // 
            this.ilSoftBlock00.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSoftBlock00.ImageStream")));
            this.ilSoftBlock00.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSoftBlock00.Images.SetKeyName(0, "00.png");
            this.ilSoftBlock00.Images.SetKeyName(1, "01.png");
            this.ilSoftBlock00.Images.SetKeyName(2, "02.png");
            this.ilSoftBlock00.Images.SetKeyName(3, "03.png");
            this.ilSoftBlock00.Images.SetKeyName(4, "04.png");
            this.ilSoftBlock00.Images.SetKeyName(5, "05.png");
            this.ilSoftBlock00.Images.SetKeyName(6, "06.png");
            // 
            // ilPowerUp
            // 
            this.ilPowerUp.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilPowerUp.ImageStream")));
            this.ilPowerUp.TransparentColor = System.Drawing.Color.Transparent;
            this.ilPowerUp.Images.SetKeyName(0, "bomb_pass.png");
            this.ilPowerUp.Images.SetKeyName(1, "bomb_up.png");
            this.ilPowerUp.Images.SetKeyName(2, "clock.png");
            this.ilPowerUp.Images.SetKeyName(3, "fire_up.png");
            this.ilPowerUp.Images.SetKeyName(4, "heart.png");
            this.ilPowerUp.Images.SetKeyName(5, "kick.png");
            this.ilPowerUp.Images.SetKeyName(6, "life.png");
            this.ilPowerUp.Images.SetKeyName(7, "red_bomb.png");
            this.ilPowerUp.Images.SetKeyName(8, "remote_control.png");
            this.ilPowerUp.Images.SetKeyName(9, "roller.png");
            this.ilPowerUp.Images.SetKeyName(10, "soft_block_pass.png");
            this.ilPowerUp.Images.SetKeyName(11, "vest.png");
            // 
            // ilCreep00Up
            // 
            this.ilCreep00Up.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep00Up.ImageStream")));
            this.ilCreep00Up.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep00Up.Images.SetKeyName(0, "00.png");
            this.ilCreep00Up.Images.SetKeyName(1, "01.png");
            this.ilCreep00Up.Images.SetKeyName(2, "02.png");
            this.ilCreep00Up.Images.SetKeyName(3, "03.png");
            this.ilCreep00Up.Images.SetKeyName(4, "04.png");
            this.ilCreep00Up.Images.SetKeyName(5, "05.png");
            // 
            // ilCreep00Right
            // 
            this.ilCreep00Right.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep00Right.ImageStream")));
            this.ilCreep00Right.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep00Right.Images.SetKeyName(0, "00.png");
            this.ilCreep00Right.Images.SetKeyName(1, "01.png");
            this.ilCreep00Right.Images.SetKeyName(2, "02.png");
            this.ilCreep00Right.Images.SetKeyName(3, "03.png");
            this.ilCreep00Right.Images.SetKeyName(4, "04.png");
            this.ilCreep00Right.Images.SetKeyName(5, "05.png");
            this.ilCreep00Right.Images.SetKeyName(6, "06.png");
            // 
            // ilCreep00Down
            // 
            this.ilCreep00Down.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep00Down.ImageStream")));
            this.ilCreep00Down.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep00Down.Images.SetKeyName(0, "00.png");
            this.ilCreep00Down.Images.SetKeyName(1, "01.png");
            this.ilCreep00Down.Images.SetKeyName(2, "02.png");
            this.ilCreep00Down.Images.SetKeyName(3, "03.png");
            this.ilCreep00Down.Images.SetKeyName(4, "04.png");
            this.ilCreep00Down.Images.SetKeyName(5, "05.png");
            // 
            // ilCreep00Left
            // 
            this.ilCreep00Left.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep00Left.ImageStream")));
            this.ilCreep00Left.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep00Left.Images.SetKeyName(0, "00.png");
            this.ilCreep00Left.Images.SetKeyName(1, "01.png");
            this.ilCreep00Left.Images.SetKeyName(2, "02.png");
            this.ilCreep00Left.Images.SetKeyName(3, "03.png");
            this.ilCreep00Left.Images.SetKeyName(4, "04.png");
            this.ilCreep00Left.Images.SetKeyName(5, "05.png");
            this.ilCreep00Left.Images.SetKeyName(6, "06.png");
            // 
            // ilBombermanDeath
            // 
            this.ilBombermanDeath.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanDeath.ImageStream")));
            this.ilBombermanDeath.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanDeath.Images.SetKeyName(0, "01.png");
            this.ilBombermanDeath.Images.SetKeyName(1, "02.png");
            this.ilBombermanDeath.Images.SetKeyName(2, "03.png");
            this.ilBombermanDeath.Images.SetKeyName(3, "04.png");
            // 
            // ilPortal
            // 
            this.ilPortal.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilPortal.ImageStream")));
            this.ilPortal.TransparentColor = System.Drawing.Color.Transparent;
            this.ilPortal.Images.SetKeyName(0, "00.png");
            this.ilPortal.Images.SetKeyName(1, "01.png");
            this.ilPortal.Images.SetKeyName(2, "02.png");
            this.ilPortal.Images.SetKeyName(3, "03.png");
            this.ilPortal.Images.SetKeyName(4, "04.png");
            this.ilPortal.Images.SetKeyName(5, "05.png");
            this.ilPortal.Images.SetKeyName(6, "06.png");
            this.ilPortal.Images.SetKeyName(7, "07.png");
            this.ilPortal.Images.SetKeyName(8, "08.png");
            this.ilPortal.Images.SetKeyName(9, "09.png");
            this.ilPortal.Images.SetKeyName(10, "10.png");
            this.ilPortal.Images.SetKeyName(11, "11.png");
            this.ilPortal.Images.SetKeyName(12, "12.png");
            this.ilPortal.Images.SetKeyName(13, "13.png");
            this.ilPortal.Images.SetKeyName(14, "14.png");
            this.ilPortal.Images.SetKeyName(15, "15.png");
            this.ilPortal.Images.SetKeyName(16, "16.png");
            this.ilPortal.Images.SetKeyName(17, "17.png");
            this.ilPortal.Images.SetKeyName(18, "18.png");
            this.ilPortal.Images.SetKeyName(19, "19.png");
            this.ilPortal.Images.SetKeyName(20, "20.png");
            this.ilPortal.Images.SetKeyName(21, "21.png");
            this.ilPortal.Images.SetKeyName(22, "22.png");
            this.ilPortal.Images.SetKeyName(23, "26.png");
            this.ilPortal.Images.SetKeyName(24, "27.png");
            this.ilPortal.Images.SetKeyName(25, "28.png");
            this.ilPortal.Images.SetKeyName(26, "29.png");
            this.ilPortal.Images.SetKeyName(27, "30.png");
            this.ilPortal.Images.SetKeyName(28, "31.png");
            this.ilPortal.Images.SetKeyName(29, "32.png");
            this.ilPortal.Images.SetKeyName(30, "33.png");
            this.ilPortal.Images.SetKeyName(31, "34.png");
            this.ilPortal.Images.SetKeyName(32, "35.png");
            // 
            // ilRedBomb
            // 
            this.ilRedBomb.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilRedBomb.ImageStream")));
            this.ilRedBomb.TransparentColor = System.Drawing.Color.Transparent;
            this.ilRedBomb.Images.SetKeyName(0, "01.png");
            this.ilRedBomb.Images.SetKeyName(1, "02.png");
            this.ilRedBomb.Images.SetKeyName(2, "03.png");
            // 
            // ilBackground
            // 
            this.ilBackground.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBackground.ImageStream")));
            this.ilBackground.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBackground.Images.SetKeyName(0, "00.png");
            this.ilBackground.Images.SetKeyName(1, "01.png");
            this.ilBackground.Images.SetKeyName(2, "02.png");
            this.ilBackground.Images.SetKeyName(3, "03.png");
            this.ilBackground.Images.SetKeyName(4, "04.png");
            this.ilBackground.Images.SetKeyName(5, "05.png");
            this.ilBackground.Images.SetKeyName(6, "06.png");
            // 
            // ilCreep01Up
            // 
            this.ilCreep01Up.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep01Up.ImageStream")));
            this.ilCreep01Up.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep01Up.Images.SetKeyName(0, "00.png");
            this.ilCreep01Up.Images.SetKeyName(1, "01.png");
            this.ilCreep01Up.Images.SetKeyName(2, "02.png");
            this.ilCreep01Up.Images.SetKeyName(3, "03.png");
            this.ilCreep01Up.Images.SetKeyName(4, "04.png");
            this.ilCreep01Up.Images.SetKeyName(5, "05.png");
            // 
            // ilCreep01Right
            // 
            this.ilCreep01Right.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep01Right.ImageStream")));
            this.ilCreep01Right.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep01Right.Images.SetKeyName(0, "00.png");
            this.ilCreep01Right.Images.SetKeyName(1, "01.png");
            this.ilCreep01Right.Images.SetKeyName(2, "02.png");
            this.ilCreep01Right.Images.SetKeyName(3, "03.png");
            this.ilCreep01Right.Images.SetKeyName(4, "04.png");
            this.ilCreep01Right.Images.SetKeyName(5, "05.png");
            this.ilCreep01Right.Images.SetKeyName(6, "06.png");
            // 
            // ilCreep01Down
            // 
            this.ilCreep01Down.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep01Down.ImageStream")));
            this.ilCreep01Down.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep01Down.Images.SetKeyName(0, "00.png");
            this.ilCreep01Down.Images.SetKeyName(1, "01.png");
            this.ilCreep01Down.Images.SetKeyName(2, "02.png");
            this.ilCreep01Down.Images.SetKeyName(3, "03.png");
            this.ilCreep01Down.Images.SetKeyName(4, "04.png");
            this.ilCreep01Down.Images.SetKeyName(5, "05.png");
            // 
            // ilCreep01Left
            // 
            this.ilCreep01Left.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep01Left.ImageStream")));
            this.ilCreep01Left.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep01Left.Images.SetKeyName(0, "00.png");
            this.ilCreep01Left.Images.SetKeyName(1, "01.png");
            this.ilCreep01Left.Images.SetKeyName(2, "02.png");
            this.ilCreep01Left.Images.SetKeyName(3, "03.png");
            this.ilCreep01Left.Images.SetKeyName(4, "04.png");
            this.ilCreep01Left.Images.SetKeyName(5, "05.png");
            this.ilCreep01Left.Images.SetKeyName(6, "06.png");
            // 
            // ilCreep02Up
            // 
            this.ilCreep02Up.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep02Up.ImageStream")));
            this.ilCreep02Up.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep02Up.Images.SetKeyName(0, "00.png");
            this.ilCreep02Up.Images.SetKeyName(1, "01.png");
            this.ilCreep02Up.Images.SetKeyName(2, "02.png");
            this.ilCreep02Up.Images.SetKeyName(3, "03.png");
            this.ilCreep02Up.Images.SetKeyName(4, "04.png");
            this.ilCreep02Up.Images.SetKeyName(5, "05.png");
            // 
            // ilCreep02Right
            // 
            this.ilCreep02Right.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep02Right.ImageStream")));
            this.ilCreep02Right.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep02Right.Images.SetKeyName(0, "00.png");
            this.ilCreep02Right.Images.SetKeyName(1, "01.png");
            this.ilCreep02Right.Images.SetKeyName(2, "02.png");
            this.ilCreep02Right.Images.SetKeyName(3, "03.png");
            this.ilCreep02Right.Images.SetKeyName(4, "04.png");
            this.ilCreep02Right.Images.SetKeyName(5, "05.png");
            this.ilCreep02Right.Images.SetKeyName(6, "06.png");
            // 
            // ilCreep02Down
            // 
            this.ilCreep02Down.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep02Down.ImageStream")));
            this.ilCreep02Down.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep02Down.Images.SetKeyName(0, "00.png");
            this.ilCreep02Down.Images.SetKeyName(1, "01.png");
            this.ilCreep02Down.Images.SetKeyName(2, "02.png");
            this.ilCreep02Down.Images.SetKeyName(3, "03.png");
            this.ilCreep02Down.Images.SetKeyName(4, "04.png");
            this.ilCreep02Down.Images.SetKeyName(5, "05.png");
            // 
            // ilCreep02Left
            // 
            this.ilCreep02Left.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCreep02Left.ImageStream")));
            this.ilCreep02Left.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCreep02Left.Images.SetKeyName(0, "00.png");
            this.ilCreep02Left.Images.SetKeyName(1, "01.png");
            this.ilCreep02Left.Images.SetKeyName(2, "02.png");
            this.ilCreep02Left.Images.SetKeyName(3, "03.png");
            this.ilCreep02Left.Images.SetKeyName(4, "04.png");
            this.ilCreep02Left.Images.SetKeyName(5, "05.png");
            this.ilCreep02Left.Images.SetKeyName(6, "06.png");
            // 
            // ilCactus00Up
            // 
            this.ilCactus00Up.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus00Up.ImageStream")));
            this.ilCactus00Up.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus00Up.Images.SetKeyName(0, "00.png");
            this.ilCactus00Up.Images.SetKeyName(1, "01.png");
            this.ilCactus00Up.Images.SetKeyName(2, "02.png");
            this.ilCactus00Up.Images.SetKeyName(3, "03.png");
            this.ilCactus00Up.Images.SetKeyName(4, "04.png");
            this.ilCactus00Up.Images.SetKeyName(5, "05.png");
            // 
            // ilCactus00Right
            // 
            this.ilCactus00Right.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus00Right.ImageStream")));
            this.ilCactus00Right.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus00Right.Images.SetKeyName(0, "00.png");
            this.ilCactus00Right.Images.SetKeyName(1, "01.png");
            this.ilCactus00Right.Images.SetKeyName(2, "02.png");
            this.ilCactus00Right.Images.SetKeyName(3, "03.png");
            this.ilCactus00Right.Images.SetKeyName(4, "04.png");
            this.ilCactus00Right.Images.SetKeyName(5, "05.png");
            this.ilCactus00Right.Images.SetKeyName(6, "06.png");
            // 
            // ilCactus00Down
            // 
            this.ilCactus00Down.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus00Down.ImageStream")));
            this.ilCactus00Down.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus00Down.Images.SetKeyName(0, "00.png");
            this.ilCactus00Down.Images.SetKeyName(1, "01.png");
            this.ilCactus00Down.Images.SetKeyName(2, "02.png");
            this.ilCactus00Down.Images.SetKeyName(3, "03.png");
            this.ilCactus00Down.Images.SetKeyName(4, "04.png");
            this.ilCactus00Down.Images.SetKeyName(5, "05.png");
            // 
            // ilCactus00Left
            // 
            this.ilCactus00Left.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus00Left.ImageStream")));
            this.ilCactus00Left.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus00Left.Images.SetKeyName(0, "00.png");
            this.ilCactus00Left.Images.SetKeyName(1, "01.png");
            this.ilCactus00Left.Images.SetKeyName(2, "02.png");
            this.ilCactus00Left.Images.SetKeyName(3, "03.png");
            this.ilCactus00Left.Images.SetKeyName(4, "04.png");
            this.ilCactus00Left.Images.SetKeyName(5, "05.png");
            this.ilCactus00Left.Images.SetKeyName(6, "06.png");
            // 
            // ilCactus01Up
            // 
            this.ilCactus01Up.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus01Up.ImageStream")));
            this.ilCactus01Up.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus01Up.Images.SetKeyName(0, "00.png");
            this.ilCactus01Up.Images.SetKeyName(1, "01.png");
            this.ilCactus01Up.Images.SetKeyName(2, "02.png");
            this.ilCactus01Up.Images.SetKeyName(3, "03.png");
            this.ilCactus01Up.Images.SetKeyName(4, "04.png");
            this.ilCactus01Up.Images.SetKeyName(5, "05.png");
            // 
            // ilCactus01Right
            // 
            this.ilCactus01Right.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus01Right.ImageStream")));
            this.ilCactus01Right.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus01Right.Images.SetKeyName(0, "00.png");
            this.ilCactus01Right.Images.SetKeyName(1, "01.png");
            this.ilCactus01Right.Images.SetKeyName(2, "02.png");
            this.ilCactus01Right.Images.SetKeyName(3, "03.png");
            this.ilCactus01Right.Images.SetKeyName(4, "04.png");
            this.ilCactus01Right.Images.SetKeyName(5, "05.png");
            this.ilCactus01Right.Images.SetKeyName(6, "06.png");
            // 
            // ilCactus01Down
            // 
            this.ilCactus01Down.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus01Down.ImageStream")));
            this.ilCactus01Down.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus01Down.Images.SetKeyName(0, "00.png");
            this.ilCactus01Down.Images.SetKeyName(1, "01.png");
            this.ilCactus01Down.Images.SetKeyName(2, "02.png");
            this.ilCactus01Down.Images.SetKeyName(3, "03.png");
            this.ilCactus01Down.Images.SetKeyName(4, "04.png");
            this.ilCactus01Down.Images.SetKeyName(5, "05.png");
            // 
            // ilCactus01Left
            // 
            this.ilCactus01Left.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus01Left.ImageStream")));
            this.ilCactus01Left.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus01Left.Images.SetKeyName(0, "00.png");
            this.ilCactus01Left.Images.SetKeyName(1, "01.png");
            this.ilCactus01Left.Images.SetKeyName(2, "02.png");
            this.ilCactus01Left.Images.SetKeyName(3, "03.png");
            this.ilCactus01Left.Images.SetKeyName(4, "04.png");
            this.ilCactus01Left.Images.SetKeyName(5, "05.png");
            this.ilCactus01Left.Images.SetKeyName(6, "06.png");
            // 
            // ilCactus02Up
            // 
            this.ilCactus02Up.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus02Up.ImageStream")));
            this.ilCactus02Up.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus02Up.Images.SetKeyName(0, "00.png");
            this.ilCactus02Up.Images.SetKeyName(1, "01.png");
            this.ilCactus02Up.Images.SetKeyName(2, "02.png");
            this.ilCactus02Up.Images.SetKeyName(3, "03.png");
            this.ilCactus02Up.Images.SetKeyName(4, "04.png");
            this.ilCactus02Up.Images.SetKeyName(5, "05.png");
            // 
            // ilCactus02Right
            // 
            this.ilCactus02Right.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus02Right.ImageStream")));
            this.ilCactus02Right.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus02Right.Images.SetKeyName(0, "00.png");
            this.ilCactus02Right.Images.SetKeyName(1, "01.png");
            this.ilCactus02Right.Images.SetKeyName(2, "02.png");
            this.ilCactus02Right.Images.SetKeyName(3, "03.png");
            this.ilCactus02Right.Images.SetKeyName(4, "04.png");
            this.ilCactus02Right.Images.SetKeyName(5, "05.png");
            this.ilCactus02Right.Images.SetKeyName(6, "06.png");
            // 
            // ilCactus02Down
            // 
            this.ilCactus02Down.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus02Down.ImageStream")));
            this.ilCactus02Down.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus02Down.Images.SetKeyName(0, "00.png");
            this.ilCactus02Down.Images.SetKeyName(1, "01.png");
            this.ilCactus02Down.Images.SetKeyName(2, "02.png");
            this.ilCactus02Down.Images.SetKeyName(3, "03.png");
            this.ilCactus02Down.Images.SetKeyName(4, "04.png");
            this.ilCactus02Down.Images.SetKeyName(5, "05.png");
            // 
            // ilCactus02Left
            // 
            this.ilCactus02Left.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCactus02Left.ImageStream")));
            this.ilCactus02Left.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCactus02Left.Images.SetKeyName(0, "00.png");
            this.ilCactus02Left.Images.SetKeyName(1, "01.png");
            this.ilCactus02Left.Images.SetKeyName(2, "02.png");
            this.ilCactus02Left.Images.SetKeyName(3, "03.png");
            this.ilCactus02Left.Images.SetKeyName(4, "04.png");
            this.ilCactus02Left.Images.SetKeyName(5, "05.png");
            this.ilCactus02Left.Images.SetKeyName(6, "06.png");
            // 
            // ilSoftBlock01
            // 
            this.ilSoftBlock01.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSoftBlock01.ImageStream")));
            this.ilSoftBlock01.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSoftBlock01.Images.SetKeyName(0, "00.png");
            this.ilSoftBlock01.Images.SetKeyName(1, "01.png");
            this.ilSoftBlock01.Images.SetKeyName(2, "02.png");
            this.ilSoftBlock01.Images.SetKeyName(3, "03.png");
            this.ilSoftBlock01.Images.SetKeyName(4, "04.png");
            this.ilSoftBlock01.Images.SetKeyName(5, "05.png");
            this.ilSoftBlock01.Images.SetKeyName(6, "06.png");
            // 
            // ilSoftBlock02
            // 
            this.ilSoftBlock02.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSoftBlock02.ImageStream")));
            this.ilSoftBlock02.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSoftBlock02.Images.SetKeyName(0, "00.png");
            this.ilSoftBlock02.Images.SetKeyName(1, "01.png");
            this.ilSoftBlock02.Images.SetKeyName(2, "02.png");
            this.ilSoftBlock02.Images.SetKeyName(3, "03.png");
            this.ilSoftBlock02.Images.SetKeyName(4, "04.png");
            this.ilSoftBlock02.Images.SetKeyName(5, "05.png");
            this.ilSoftBlock02.Images.SetKeyName(6, "06.png");
            // 
            // ilSoftBlock03
            // 
            this.ilSoftBlock03.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSoftBlock03.ImageStream")));
            this.ilSoftBlock03.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSoftBlock03.Images.SetKeyName(0, "00.png");
            this.ilSoftBlock03.Images.SetKeyName(1, "01.png");
            this.ilSoftBlock03.Images.SetKeyName(2, "02.png");
            this.ilSoftBlock03.Images.SetKeyName(3, "03.png");
            this.ilSoftBlock03.Images.SetKeyName(4, "04.png");
            this.ilSoftBlock03.Images.SetKeyName(5, "05.png");
            this.ilSoftBlock03.Images.SetKeyName(6, "06.png");
            // 
            // ilSoftBlock04
            // 
            this.ilSoftBlock04.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSoftBlock04.ImageStream")));
            this.ilSoftBlock04.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSoftBlock04.Images.SetKeyName(0, "00.png");
            this.ilSoftBlock04.Images.SetKeyName(1, "01.png");
            this.ilSoftBlock04.Images.SetKeyName(2, "02.png");
            this.ilSoftBlock04.Images.SetKeyName(3, "03.png");
            this.ilSoftBlock04.Images.SetKeyName(4, "04.png");
            this.ilSoftBlock04.Images.SetKeyName(5, "05.png");
            this.ilSoftBlock04.Images.SetKeyName(6, "06.png");
            // 
            // ilSoftBlock05
            // 
            this.ilSoftBlock05.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSoftBlock05.ImageStream")));
            this.ilSoftBlock05.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSoftBlock05.Images.SetKeyName(0, "00.png");
            this.ilSoftBlock05.Images.SetKeyName(1, "01.png");
            this.ilSoftBlock05.Images.SetKeyName(2, "02.png");
            this.ilSoftBlock05.Images.SetKeyName(3, "03.png");
            this.ilSoftBlock05.Images.SetKeyName(4, "04.png");
            this.ilSoftBlock05.Images.SetKeyName(5, "05.png");
            this.ilSoftBlock05.Images.SetKeyName(6, "06.png");
            // 
            // ilSoftBlock06
            // 
            this.ilSoftBlock06.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSoftBlock06.ImageStream")));
            this.ilSoftBlock06.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSoftBlock06.Images.SetKeyName(0, "00.png");
            this.ilSoftBlock06.Images.SetKeyName(1, "01.png");
            this.ilSoftBlock06.Images.SetKeyName(2, "02.png");
            this.ilSoftBlock06.Images.SetKeyName(3, "03.png");
            this.ilSoftBlock06.Images.SetKeyName(4, "04.png");
            this.ilSoftBlock06.Images.SetKeyName(5, "05.png");
            this.ilSoftBlock06.Images.SetKeyName(6, "06.png");
            // 
            // ilBombermanKickLeft
            // 
            this.ilBombermanKickLeft.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanKickLeft.ImageStream")));
            this.ilBombermanKickLeft.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanKickLeft.Images.SetKeyName(0, "00.png");
            this.ilBombermanKickLeft.Images.SetKeyName(1, "01.png");
            this.ilBombermanKickLeft.Images.SetKeyName(2, "02.png");
            this.ilBombermanKickLeft.Images.SetKeyName(3, "03.png");
            // 
            // ilBombermanKickUp
            // 
            this.ilBombermanKickUp.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanKickUp.ImageStream")));
            this.ilBombermanKickUp.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanKickUp.Images.SetKeyName(0, "00.png");
            this.ilBombermanKickUp.Images.SetKeyName(1, "01.png");
            this.ilBombermanKickUp.Images.SetKeyName(2, "02.png");
            this.ilBombermanKickUp.Images.SetKeyName(3, "03.png");
            // 
            // ilBombermanKickRight
            // 
            this.ilBombermanKickRight.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanKickRight.ImageStream")));
            this.ilBombermanKickRight.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanKickRight.Images.SetKeyName(0, "00.png");
            this.ilBombermanKickRight.Images.SetKeyName(1, "01.png");
            this.ilBombermanKickRight.Images.SetKeyName(2, "02.png");
            this.ilBombermanKickRight.Images.SetKeyName(3, "03.png");
            // 
            // ilBombermanKickDown
            // 
            this.ilBombermanKickDown.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilBombermanKickDown.ImageStream")));
            this.ilBombermanKickDown.TransparentColor = System.Drawing.Color.Transparent;
            this.ilBombermanKickDown.Images.SetKeyName(0, "00.png");
            this.ilBombermanKickDown.Images.SetKeyName(1, "01.png");
            this.ilBombermanKickDown.Images.SetKeyName(2, "02.png");
            this.ilBombermanKickDown.Images.SetKeyName(3, "03.png");
            // 
            // ilRemoteControlBomb
            // 
            this.ilRemoteControlBomb.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilRemoteControlBomb.ImageStream")));
            this.ilRemoteControlBomb.TransparentColor = System.Drawing.Color.Transparent;
            this.ilRemoteControlBomb.Images.SetKeyName(0, "00.png");
            this.ilRemoteControlBomb.Images.SetKeyName(1, "01.png");
            this.ilRemoteControlBomb.Images.SetKeyName(2, "02.png");
            this.ilRemoteControlBomb.Images.SetKeyName(3, "03.png");
            this.ilRemoteControlBomb.Images.SetKeyName(4, "04.png");
            this.ilRemoteControlBomb.Images.SetKeyName(5, "05.png");
            this.ilRemoteControlBomb.Images.SetKeyName(6, "06.png");
            this.ilRemoteControlBomb.Images.SetKeyName(7, "07.png");
            this.ilRemoteControlBomb.Images.SetKeyName(8, "08.png");
            this.ilRemoteControlBomb.Images.SetKeyName(9, "09.png");
            this.ilRemoteControlBomb.Images.SetKeyName(10, "10.png");
            this.ilRemoteControlBomb.Images.SetKeyName(11, "11.png");
            this.ilRemoteControlBomb.Images.SetKeyName(12, "12.png");
            this.ilRemoteControlBomb.Images.SetKeyName(13, "13.png");
            this.ilRemoteControlBomb.Images.SetKeyName(14, "14.png");
            this.ilRemoteControlBomb.Images.SetKeyName(15, "15.png");
            this.ilRemoteControlBomb.Images.SetKeyName(16, "16.png");
            this.ilRemoteControlBomb.Images.SetKeyName(17, "17.png");
            this.ilRemoteControlBomb.Images.SetKeyName(18, "18.png");
            this.ilRemoteControlBomb.Images.SetKeyName(19, "19.png");
            this.ilRemoteControlBomb.Images.SetKeyName(20, "20.png");
            this.ilRemoteControlBomb.Images.SetKeyName(21, "21.png");
            this.ilRemoteControlBomb.Images.SetKeyName(22, "22.png");
            this.ilRemoteControlBomb.Images.SetKeyName(23, "23.png");
            // 
            // ilRemoteControlRedBomb
            // 
            this.ilRemoteControlRedBomb.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilRemoteControlRedBomb.ImageStream")));
            this.ilRemoteControlRedBomb.TransparentColor = System.Drawing.Color.Transparent;
            this.ilRemoteControlRedBomb.Images.SetKeyName(0, "00.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(1, "01.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(2, "02.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(3, "03.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(4, "04.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(5, "05.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(6, "06.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(7, "07.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(8, "08.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(9, "09.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(10, "10.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(11, "11.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(12, "12.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(13, "13.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(14, "14.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(15, "15.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(16, "16.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(17, "17.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(18, "18.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(19, "19.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(20, "20.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(21, "21.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(22, "22.png");
            this.ilRemoteControlRedBomb.Images.SetKeyName(23, "23.png");
            // 
            // ilTopPanel
            // 
            this.ilTopPanel.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTopPanel.ImageStream")));
            this.ilTopPanel.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTopPanel.Images.SetKeyName(0, "bombs.png");
            this.ilTopPanel.Images.SetKeyName(1, "fires.png");
            this.ilTopPanel.Images.SetKeyName(2, "hearts.png");
            this.ilTopPanel.Images.SetKeyName(3, "lives.png");
            // 
            // FrmBomberman
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(960, 896);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FrmBomberman";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bomberman";
            this.Load += new System.EventHandler(this.frmBomberman_Load);
            this.SizeChanged += new System.EventHandler(this.frmBomberman_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmBomberman_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBomberman_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmBomberman_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageList ilBombermanWalkDown;
        private ImageList ilBombermanWalkLeft;
        private ImageList ilBombermanWalkUp;
        private ImageList ilBombermanWalkRight;
        private ImageList ilBlocks;
        private ImageList ilBomb;
        private ImageList ilFlame;
        private ImageList ilSoftBlock00;
        private ImageList ilPowerUp;
        private ImageList ilCreep00Up;
        private ImageList ilCreep00Right;
        private ImageList ilCreep00Down;
        private ImageList ilCreep00Left;
        private ImageList ilBombermanDeath;
        private ImageList ilPortal;
        private ImageList ilRedBomb;
        private ImageList ilBackground;
        private ImageList ilCreep01Up;
        private ImageList ilCreep01Right;
        private ImageList ilCreep01Down;
        private ImageList ilCreep01Left;
        private ImageList ilCreep02Up;
        private ImageList ilCreep02Right;
        private ImageList ilCreep02Down;
        private ImageList ilCreep02Left;
        private ImageList ilCactus00Up;
        private ImageList ilCactus00Right;
        private ImageList ilCactus00Down;
        private ImageList ilCactus00Left;
        private ImageList ilCactus01Up;
        private ImageList ilCactus01Right;
        private ImageList ilCactus01Down;
        private ImageList ilCactus01Left;
        private ImageList ilCactus02Up;
        private ImageList ilCactus02Right;
        private ImageList ilCactus02Down;
        private ImageList ilCactus02Left;
        private ImageList ilSoftBlock01;
        private ImageList ilSoftBlock02;
        private ImageList ilSoftBlock03;
        private ImageList ilSoftBlock04;
        private ImageList ilSoftBlock05;
        private ImageList ilSoftBlock06;
        private ImageList ilBombermanKickLeft;
        private ImageList ilBombermanKickUp;
        private ImageList ilBombermanKickRight;
        private ImageList ilBombermanKickDown;
        private ImageList ilRemoteControlBomb;
        private ImageList ilRemoteControlRedBomb;
        private ImageList ilTopPanel;
    }
}

