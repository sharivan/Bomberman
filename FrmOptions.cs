using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bomberman
{
    public partial class FrmOptions : Form
    {
        private Form caller;
        private KeyBinding keyBinding;

        private Bitmap saveNorm;
        private Bitmap saveFocus;

        private Bitmap discardNorm;
        private Bitmap discardFocus;

        private Bitmap restoreDefaultsNorm;
        private Bitmap restoreDefaultsFocus;

        private KeyBinding tempKeyBinding;
        private int selectedKeyOption;
        private Label[] lblKeyLabel;
        private Label[] lblKey;
        private SoundCollection sounds;

        public FrmOptions(Form caller, KeyBinding keyBinding, SoundCollection sounds)
        {
            this.caller = caller;
            this.keyBinding = keyBinding;
            this.sounds = sounds;

            InitializeComponent();
        }

        private static string KeyToString(Keys key)
        {
            switch (key)
            {
                case Keys.Add: return "+";
                case Keys.Alt: return "Alt";
                case Keys.Back: return "Backspace";
                case Keys.ControlKey: return "Control";
                case Keys.Divide: return "/";
                case Keys.Down: return "Seta para baixo";
                case Keys.Left: return "Seta para esquerda";
                case Keys.Multiply: return "*";
                case Keys.NumPad0: return "NP 0";
                case Keys.NumPad1: return "NP 1";
                case Keys.NumPad2: return "NP 2";
                case Keys.NumPad3: return "NP 3";
                case Keys.NumPad4: return "NP 4";
                case Keys.NumPad5: return "NP 5";
                case Keys.NumPad6: return "NP 6";
                case Keys.NumPad7: return "NP 7";
                case Keys.NumPad8: return "NP 8";
                case Keys.NumPad9: return "NP 9";
                case Keys.PageDown: return "Page Down";
                case Keys.PageUp: return "Page Up";
                case Keys.Return: return "Enter";
                case Keys.Right: return "Seta para direita";
                case Keys.ShiftKey: return "Shift";
                case Keys.Space: return "Espaço";
                case Keys.Subtract: return "-";
                case Keys.Up: return "Seta para cima";
            }

            return key.ToString();
        }

        private static string KeyOptionToString(KeyBinding keyBinding, int optionIndex)
        {
            switch (optionIndex)
            {
                case 0: return KeyToString(keyBinding.Left);
                case 1: return KeyToString(keyBinding.Up);
                case 2: return KeyToString(keyBinding.Right);
                case 3: return KeyToString(keyBinding.Down);
                case 4: return KeyToString(keyBinding.DropBomb);
                case 5: return KeyToString(keyBinding.Kick);
                case 6: return KeyToString(keyBinding.Detonate);
                case 7: return KeyToString(keyBinding.Pause);
            }

            return "Unknow";
        }

        private static void SetKey(KeyBinding keyBinding, int optionIndex, Keys key)
        {
            switch (optionIndex)
            {
                case 0:
                    keyBinding.Left = key;
                    break;
                case 1:
                    keyBinding.Up = key;
                    break;
                case 2:
                    keyBinding.Right = key;
                    break;
                case 3:
                    keyBinding.Down = key;
                    break;
                case 4:
                    keyBinding.DropBomb = key;
                    break;
                case 5:
                    keyBinding.Kick = key;
                    break;
                case 6:
                    keyBinding.Detonate = key;
                    break;
                case 7:
                    keyBinding.Pause = key;
                    break;
            }
        }

        private void FrmOptions_Load(object sender, EventArgs e)
        {
            tempKeyBinding = new KeyBinding(keyBinding);
            selectedKeyOption = -1;

            lblKeyLabel = new Label[8];
            lblKey = new Label[8];

            lblKeyLabel[0] = lblLeftLabel;
            lblKeyLabel[1] = lblUpLabel;
            lblKeyLabel[2] = lblRightLabel;
            lblKeyLabel[3] = lblDownLabel;
            lblKeyLabel[4] = lblDropBombLabel;
            lblKeyLabel[5] = lblKickLabel;
            lblKeyLabel[6] = lblDetonateLabel;
            lblKeyLabel[7] = lblPauseLabel;

            lblKey[0] = lblLeftKey;
            lblKey[1] = lblUpKey;
            lblKey[2] = lblRightKey;
            lblKey[3] = lblDownKey;
            lblKey[4] = lblDropBombKey;
            lblKey[5] = lblKickKey;
            lblKey[6] = lblDetonateKey;
            lblKey[7] = lblPauseKey;

            saveNorm = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Options.salvarnorm.png"));
            saveFocus = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Options.salvarfocus.png"));

            discardNorm = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Options.descartnorm.png"));
            discardFocus = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Options.descartfocus.png"));

            restoreDefaultsNorm = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Options.restnorm.png"));
            restoreDefaultsFocus = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Options.restfocus.png"));
        }

        private void FrmOptions_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (saveNorm != null)
            {
                saveNorm.Dispose();
                saveNorm = null;
            }
            if (saveFocus != null)
            {
                saveFocus.Dispose();
                saveFocus = null;
            }

            if (discardNorm != null)
            {
                discardNorm.Dispose();
                discardNorm = null;
            }
            if (discardFocus != null)
            {
                discardFocus.Dispose();
                discardFocus = null;
            }

            if (restoreDefaultsNorm != null)
            {
                restoreDefaultsNorm.Dispose();
                restoreDefaultsNorm = null;
            }
            if (restoreDefaultsFocus != null)
            {
                restoreDefaultsFocus.Dispose();
                restoreDefaultsFocus = null;
            }

            if (caller != null)
                caller.Show();
        }

        private void UpdateLabels()
        {
            lblLeftKey.Text = KeyToString(tempKeyBinding.Left);
            lblUpKey.Text = KeyToString(tempKeyBinding.Up);
            lblRightKey.Text = KeyToString(tempKeyBinding.Right);
            lblDownKey.Text = KeyToString(tempKeyBinding.Down);
            lblDropBombKey.Text = KeyToString(tempKeyBinding.DropBomb);
            lblKickKey.Text = KeyToString(tempKeyBinding.Kick);
            lblDetonateKey.Text = KeyToString(tempKeyBinding.Detonate);
            lblPauseKey.Text = KeyToString(tempKeyBinding.Pause);
        }

        private void SaveToDisk()
        {
            using (FileStream stream = new FileStream(@"keys.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                keyBinding.WriteToStream(stream);
            }
        }

        private void FrmOptions_Shown(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        private void EditKey(int optionIndex)
        {
            if (selectedKeyOption != -1)
                CancelEditKey();

            selectedKeyOption = optionIndex;
            lblKeyLabel[optionIndex].ForeColor = Color.Turquoise;
            lblKeyLabel[optionIndex].Font = new Font(lblKeyLabel[optionIndex].Font, FontStyle.Bold);
            lblKey[optionIndex].Text = "";

            if (sounds != null)
                sounds.Play("confirm");
        }

        private void CancelEditKey()
        {
            if (selectedKeyOption == -1)
                return;

            lblKeyLabel[selectedKeyOption].ForeColor = Color.White;
            lblKeyLabel[selectedKeyOption].Font = new Font(lblKeyLabel[selectedKeyOption].Font, FontStyle.Regular);
            lblKey[selectedKeyOption].Text = KeyOptionToString(tempKeyBinding, selectedKeyOption);

            selectedKeyOption = -1;

            if (sounds != null)
                sounds.Play("reset");
        }

        private void UpdateKey(Keys key)
        {
            if (selectedKeyOption == -1)
                return;

            lblKeyLabel[selectedKeyOption].ForeColor = Color.White;
            lblKeyLabel[selectedKeyOption].Font = new Font(lblKeyLabel[selectedKeyOption].Font, FontStyle.Regular);
            SetKey(tempKeyBinding, selectedKeyOption, key);
            lblKey[selectedKeyOption].Text = KeyOptionToString(tempKeyBinding, selectedKeyOption);

            selectedKeyOption = -1;

            if (sounds != null)
                sounds.Play("confirm");
        }

        private void lblLeftKey_DoubleClick(object sender, EventArgs e)
        {
            EditKey(0);
        }

        private void lblUpKey_DoubleClick(object sender, EventArgs e)
        {
            EditKey(1);
        }

        private void lblRightKey_DoubleClick(object sender, EventArgs e)
        {
            EditKey(2);
        }

        private void lblDownKey_DoubleClick(object sender, EventArgs e)
        {
            EditKey(3);
        }

        private void lblDropBombKey_DoubleClick(object sender, EventArgs e)
        {
            EditKey(4);
        }

        private void lblKickKey_DoubleClick(object sender, EventArgs e)
        {
            EditKey(5);
        }

        private void lblDetonateKey_DoubleClick(object sender, EventArgs e)
        {
            EditKey(6);
        }

        private void lblPauseKey_DoubleClick(object sender, EventArgs e)
        {
            EditKey(7);
        }

        private void FrmOptions_KeyDown(object sender, KeyEventArgs e)
        {
            if (selectedKeyOption == -1)
                return;

            Keys key = e.KeyCode;
            if (key == Keys.Escape)
                CancelEditKey();
            else
                UpdateKey(key);
        }

        private bool IsEditing(Label label)
        {
            if (selectedKeyOption == -1)
                return false;

            return label == lblKeyLabel[selectedKeyOption] || label == lblKey[selectedKeyOption];
        }

        private void Action_MouseEnter(object sender, EventArgs e)
        {
            if (sounds != null)
                sounds.Play("select");
            Label label = (Label)sender;
            if (!IsEditing(label))
                label.ForeColor = Color.YellowGreen;
            label.Font = new Font(label.Font, FontStyle.Bold);
        }

        private void Action_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (!IsEditing(label))
                label.ForeColor = Color.White;
            label.Font = new Font(label.Font, FontStyle.Regular);
        }

        private void picsingle_MouseEnter(object sender, EventArgs e)
        {
            if (sounds != null)
                sounds.Play("select");
            picSave.Image = saveFocus;
        }

        private void picSave_MouseLeave(object sender, EventArgs e)
        {
            picSave.Image = saveNorm;
        }

        private void picDiscard_MouseEnter(object sender, EventArgs e)
        {
            if (sounds != null)
                sounds.Play("select");
            picDiscard.Image = discardFocus;
        }

        private void picDiscard_MouseLeave(object sender, EventArgs e)
        {
            picDiscard.Image = discardNorm;
        }

        private void picDiscard_Click(object sender, EventArgs e)
        {
            if (sounds != null)
                sounds.Play("confirm");
            Close();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            if (sounds != null)
                sounds.Play("confirm");
            keyBinding.UpdateFrom(tempKeyBinding);
            SaveToDisk();
            Close();
        }

        private void picRestoreDefaults_Click(object sender, EventArgs e)
        {
            if (sounds != null)
                sounds.Play("reset");
            tempKeyBinding = new KeyBinding();
            UpdateLabels();
        }

        private void picRestoreDefaults_MouseEnter(object sender, EventArgs e)
        {
            if (sounds != null)
                sounds.Play("select");
            picRestoreDefaults.Image = restoreDefaultsFocus;
        }

        private void picRestoreDefaults_MouseLeave(object sender, EventArgs e)
        {
            picRestoreDefaults.Image = restoreDefaultsNorm;
        }
    }
}
