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
    public partial class FrmMenu : Form
    {
        private Bitmap playNorm;
        private Bitmap playFocus;

        private Bitmap rankNorm;
        private Bitmap rankFocus;

        private Bitmap optionsNorm;
        private Bitmap optionsFocus;

        private KeyBinding keyBinding;
        private List<RankEntry> rank;
        private SoundCollection sounds;
        private bool gameStarted;

        public FrmMenu() => InitializeComponent();

        private void LoadRankFromDisk()
        {
            using (var stream = new FileStream(@"rank.bin", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            {
                if (stream.Length == 0)
                    return;

                int count = stream.ReadByte();
                for (int i = 0; i < count; i++)
                {
                    var entry = new RankEntry(stream);
                    rank.Add(entry);
                }
            }
        }

        private void LoadKeyBindingFromDisk()
        {
            using (var stream = new FileStream(@"keys.bin", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            {
                if (stream.Length > 0)
                    keyBinding.ReadFromStream(stream);
            }
        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {
            gameStarted = false;

            sounds = new SoundCollection();
            sounds.Add("reset");
            sounds.Add("select");
            sounds.Add("confirm");
            sounds.Add("PAS_OK2");
            sounds.Add("BOM_11_L");
            sounds.Add("BOM_11_M");
            sounds.Add("BOM_11_S");
            sounds.Add("BOM_BOUND");
            sounds.Add("BOM_KICK");
            sounds.Add("BOM_SET");
            sounds.Add("GOAL");
            sounds.Add("HURRYUP");
            sounds.Add("ITEM_GET");
            sounds.Add("P1UP");
            sounds.Add("TIME_UP");
            sounds.Add("pause");
            sounds.Add("intro", "Music.TheStringALongs-MyBlueHeaven.mp3");
            sounds.Add("stage1", "Music.BookerTTheMgs-TimeIsTight.mp3");
            sounds.Add("stage2", "Music.VillageStompers-WashingtonSquare.mp3");
            sounds.Add("stage3", "Music.TheVentures-WalkDontRun64.mp3");
            sounds.Add("stage4", "Music.RayConniff-BesameMucho.mp3");
            sounds.Add("stage5", "Music.TheChamps-Tequila.mp3");
            sounds.Add("stage6", "Music.HotButter-PopCorn.mp3");
            sounds.Add("stage7", "Music.ChuckMangione-ChildrenOfSanchez.mp3");
            sounds.Add("gameover", "Music.JohnnyTheHurricanes-BeatnikFly.mp3");

            playNorm = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Menu.jogarnorm.png"));
            playFocus = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Menu.jogarfocus.png"));

            rankNorm = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Menu.ranknorm.png"));
            rankFocus = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Menu.rankfocus.png"));

            optionsNorm = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Menu.opcnorm.png"));
            optionsFocus = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Menu.opcfocus.png"));

            keyBinding = new KeyBinding();
            rank = new List<RankEntry>();

            LoadKeyBindingFromDisk();
            LoadRankFromDisk();
        }

        private void FrmMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (playNorm != null)
            {
                playNorm.Dispose();
                playNorm = null;
            }

            if (playFocus != null)
            {
                playFocus.Dispose();
                playFocus = null;
            }

            if (rankNorm != null)
            {
                rankNorm.Dispose();
                rankNorm = null;
            }

            if (rankFocus != null)
            {
                rankFocus.Dispose();
                rankFocus = null;
            }

            if (optionsNorm != null)
            {
                optionsNorm.Dispose();
                optionsNorm = null;
            }

            if (optionsFocus != null)
            {
                optionsFocus.Dispose();
                optionsFocus = null;
            }

            sounds.Close();
        }

        private void FrmMenu_Shown(object sender, EventArgs e) => sounds.Play("intro", true);

        private void LoadGame()
        {
            gameStarted = true;

            sounds.Stop("intro");
            sounds.Play("PAS_OK2");

            Hide();

            var frmBomberman = new FrmBomberman(this, keyBinding, sounds)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            frmBomberman.Show();
        }

        private void picsinglenorm_MouseEnter(object sender, EventArgs e)
        {
            sounds.Play("select");
            picsingle.Image = playFocus;
        }

        private void picsingle_MouseLeave(object sender, EventArgs e) => picsingle.Image = playNorm;

        private void picrank_MouseEnter(object sender, EventArgs e)
        {
            sounds.Play("select");
            picrank.Image = rankFocus;
        }

        private void picrank_MouseLeave(object sender, EventArgs e) => picrank.Image = rankNorm;

        private void picoptions_MouseEnter(object sender, EventArgs e)
        {
            sounds.Play("select");
            picoptions.Image = optionsFocus;
        }

        private void picoptions_MouseLeave(object sender, EventArgs e) => picoptions.Image = optionsNorm;

        private void picsingle_Click(object sender, EventArgs e) => LoadGame();

        private void picrank_Click(object sender, EventArgs e)
        {
            sounds.Play("confirm");
            OpenRank();
        }

        private void picoptions_Click(object sender, EventArgs e)
        {
            sounds.Play("confirm");

            Hide();

            var frmop = new FrmOptions(this, keyBinding, sounds)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            frmop.Show();
        }

        private void OpenRank() => OpenRank(-1, -1);

        private void OpenRank(int level, int score)
        {
            Hide();

            var frmrank = new FrmRanking(this, rank, sounds, level, score)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            frmrank.Show();
        }

        public void ReportScore(int level, int score) => OpenRank(level, score);

        private void FrmMenu_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && gameStarted)
            {
                gameStarted = false;
                sounds.Play("intro", true);
            }
        }
    }
}
