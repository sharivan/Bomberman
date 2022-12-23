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
    public partial class FrmRanking : Form
    {
        private readonly Form caller;
        private readonly List<RankEntry> entries;
        private readonly SoundCollection sounds;
        private int futureEntryIndex;
        private int newLevel;
        private int newScore;

        private Bitmap backNorm;
        private Bitmap backFocus;

        private Label[] lblRankName;
        private Label[] lblRankLevel;
        private Label[] lblRankScore;

        public FrmRanking(Form caller, List<RankEntry> entries, SoundCollection sounds)
            : this(caller, entries, sounds , - 1, -1)
        {
        }

        public FrmRanking(Form caller, List<RankEntry> entries, SoundCollection sounds, int newLevel, int newScore)
        {
            this.caller = caller;
            this.entries = entries;
            this.sounds = sounds;
            this.newLevel = newLevel;
            this.newScore = newScore;

            InitializeComponent();
        }

        public int GetFutureEntryIndex(int score)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                RankEntry entry = entries[i];
                if (entry.Score < score)
                    return i;
            }

            return entries.Count < 9 ? entries.Count : -1;
        }

        public void AddRank(int index, string name, int level, int score) => AddRank(index, new RankEntry(name, level, score));

        public void AddRank(int index, RankEntry entry)
        {
            entries.Insert(index, entry);
            if (entries.Count > 9)
                entries.RemoveRange(9, entries.Count - 9);
            RefreshLabels();
            SaveToDisk();

            lblRankName[index].ForeColor = Color.YellowGreen;
            lblRankName[index].Font = new Font(lblRankName[index].Font, FontStyle.Bold);

            lblRankLevel[index].ForeColor = Color.YellowGreen;
            lblRankLevel[index].Font = new Font(lblRankName[index].Font, FontStyle.Bold);

            lblRankScore[index].ForeColor = Color.YellowGreen;
            lblRankScore[index].Font = new Font(lblRankName[index].Font, FontStyle.Bold);
        }

        private void RefreshLabels()
        {
            int count = Math.Min(9, entries.Count);
            for (int i = 0; i < count; i++)
            {
                RankEntry entry = entries[i];
                lblRankName[i].Text = entry.Name;
                lblRankLevel[i].Text = "Lv " + (entry.Level + 1);
                lblRankScore[i].Text = entry.Score.ToString();
            }
        }

        private void SaveToDisk()
        {
            using (var stream = new FileStream(@"rank.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                stream.WriteByte((byte)entries.Count);
                for (int i = 0; i < entries.Count; i++)
                {
                    RankEntry entry = entries[i];
                    entry.WriteToStream(stream);
                }
            }
        }

        private void FrmRanking_Load(object sender, EventArgs e)
        {
            lblRankName = new Label[9];
            lblRankLevel = new Label[9];
            lblRankScore = new Label[9];

            lblRankName[0] = lblRank1Name;
            lblRankName[1] = lblRank2Name;
            lblRankName[2] = lblRank3Name;
            lblRankName[3] = lblRank4Name;
            lblRankName[4] = lblRank5Name;
            lblRankName[5] = lblRank6Name;
            lblRankName[6] = lblRank7Name;
            lblRankName[7] = lblRank8Name;
            lblRankName[8] = lblRank9Name;

            lblRankLevel[0] = lblRank1Level;
            lblRankLevel[1] = lblRank2Level;
            lblRankLevel[2] = lblRank3Level;
            lblRankLevel[3] = lblRank4Level;
            lblRankLevel[4] = lblRank5Level;
            lblRankLevel[5] = lblRank6Level;
            lblRankLevel[6] = lblRank7Level;
            lblRankLevel[7] = lblRank8Level;
            lblRankLevel[8] = lblRank9Level;

            lblRankScore[0] = lblRank1Score;
            lblRankScore[1] = lblRank2Score;
            lblRankScore[2] = lblRank3Score;
            lblRankScore[3] = lblRank4Score;
            lblRankScore[4] = lblRank5Score;
            lblRankScore[5] = lblRank6Score;
            lblRankScore[6] = lblRank7Score;
            lblRankScore[7] = lblRank8Score;
            lblRankScore[8] = lblRank9Score;

            futureEntryIndex = -1;

            backNorm = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Rank.voltarnorm.png"));
            backFocus = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.Rank.voltarfocus.png"));

            RefreshLabels();
        }

        private void FrmRanking_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (backNorm != null)
            {
                backNorm.Dispose();
                backNorm = null;
            }

            if (backFocus != null)
            {
                backFocus.Dispose();
                backFocus = null;
            }

            caller?.Show();
        }

        private void FrmRanking_Shown(object sender, EventArgs e)
        {
            if (newLevel >= 0)
            {
                futureEntryIndex = GetFutureEntryIndex(newScore);
                if (futureEntryIndex != -1)
                {
                    AddRank(futureEntryIndex, "Bomberman", newLevel, newScore);
                    lblRankName[futureEntryIndex].Visible = false;
                    int x = lblRankName[futureEntryIndex].Location.X + (lblRankName[futureEntryIndex].Width - txtNewName.Width) / 2;
                    int y = lblRankName[futureEntryIndex].Location.Y + (lblRankName[futureEntryIndex].Height - txtNewName.Height) / 2;
                    txtNewName.Location = new Point(x, y);
                    txtNewName.Visible = true;
                    txtNewName.Focus();
                }
            }
        }

        private void txtNewName_KeyDown(object sender, KeyEventArgs e)
        {
            if (futureEntryIndex != -1 && e.KeyCode == Keys.Return)
            {
                txtNewName.Visible = false;
                string newName = txtNewName.Text.Trim();
                if (newName.Length > FrmBomberman.MAX_PLAYER_NAME)
                    newName = newName.Substring(0, FrmBomberman.MAX_PLAYER_NAME);
                lblRankName[futureEntryIndex].Text = newName;
                lblRankName[futureEntryIndex].Visible = true;

                entries[futureEntryIndex].Name = newName;
                SaveToDisk();

                futureEntryIndex = -1;
                newLevel = -1;
                newScore = -1;
            }
        }

        private void picBack_Click(object sender, EventArgs e)
        {
            sounds?.Play("confirm");
            Close();
        }

        private void picBack_MouseEnter(object sender, EventArgs e)
        {
            sounds?.Play("select");
            picBack.Image = backFocus;
        }

        private void picBack_MouseLeave(object sender, EventArgs e) => picBack.Image = backNorm;
    }
}
