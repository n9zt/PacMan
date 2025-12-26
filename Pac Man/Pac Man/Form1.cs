using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Imaging;
using System.Media;

namespace Pac_Man
{
    public partial class Form1 : Form
    {
        bool goup, godown, goright, goleft;
        bool noup, nodown, noright, noleft;
        List<PictureBox> walls = new List<PictureBox>();
        List<PictureBox> coins = new List<PictureBox>();
        int speed = 12;
        int score = 0;

        Ghost red, pink, green, blue;
        List<Ghost> ghosts = new List<Ghost>();

        public Form1()
        {
            InitializeComponent();
            SetUp();

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && !noleft)
            {
                goright = godown = goup = false;
                noright = nodown = noup = false;
                goleft = true;
                // add directions to images
                pacman.Image = Properties.Resources.pacman;
            }
            if (e.KeyCode == Keys.Right && !noright)
            {
                goleft = godown = goup = false;
                noleft = nodown = noup = false;
                goright = true;
                // add directions to images
                pacman.Image = Properties.Resources.pacman;
            }
            if (e.KeyCode == Keys.Up && !noup)
            {
                goright = godown = goleft = false;
                noright = nodown = noleft = false;
                goup = true;
                // add directions to images
                pacman.Image = Properties.Resources.pacman;
            }
            if (e.KeyCode == Keys.Down && !nodown)
            {
                goright = goleft = goup = false;
                noright = noleft = noup = false;
                godown = true;
                // add directions to images
                pacman.Image = Properties.Resources.pacman;
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            PlayerMovements();

            foreach (PictureBox wall in walls)
            {
                CheckBoundaries(pacman, wall);
            }

            foreach (PictureBox coin in coins)
            {
                CollectingCoins(pacman, coin);
            }

            if (score == coins.Count())
            {
                ShowCoins();
                score = 0;
            }

            red.GhostMovement(pacman);
            blue.GhostMovement(pacman);
            pink.GhostMovement(pacman);
            green.GhostMovement(pacman);

            foreach (Ghost ghost in ghosts)
            {
                GhostCollision(ghost, pacman, ghost.image);
            }
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            panelMenu.Enabled = false;
            panelMenu.Visible = false;

            goup = godown = goright = goleft = false;
            noup = nodown = noright = noleft = false;
            score = 0;

            red.image.Location = new Point(100, 100);
            pink.image.Location = new Point(877, 130);
            green.image.Location = new Point(132, 584);
            blue.image.Location = new Point(848, 597);

            foreach (Ghost ghost in ghosts)
            {
                GhostCollision(ghost, pacman, ghost.image);
            }

            GameTimer.Start();

        }
        private void SetUp()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "wall")
                {
                    walls.Add((PictureBox)x);
                }
                if (x is PictureBox && x.Tag == "coin")
                {
                    coins.Add((PictureBox)x);
                }
            }

            red = new Ghost(this, Properties.Resources.red, 100, 100);
            ghosts.Add(red);
            blue = new Ghost(this, Properties.Resources.blue, 848, 597);
            ghosts.Add(blue);
            green = new Ghost(this, Properties.Resources.green, 132, 584);
            ghosts.Add(green);
            pink = new Ghost(this, Properties.Resources.pink, 877, 130);
            ghosts.Add(pink);

        }
        private void PlayerMovements()
        {
            if (goleft) { pacman.Left -= speed; }
            if (goright) { pacman.Left += speed; }
            if (goup) { pacman.Top -= speed; }
            if (godown) { pacman.Top += speed; }

            if (pacman.Left < -30)
            {
                pacman.Left = this.ClientSize.Width - pacman.Width;
            }
            if (pacman.Left + pacman.Width > this.ClientSize.Width)
            {
                pacman.Left = -10;
            }
            if(pacman.Top < -30)
            {
                pacman.Top = this.ClientSize.Height - pacman.Height;
            }
            if (pacman.Top + pacman.Height > this.ClientSize.Height)
            {
                pacman.Top = -10;
            }
        }
        private void ShowCoins()
        {
            foreach (PictureBox coin in coins)
            {
                coin.Visible = true;
            }
        }
        private void CheckBoundaries(PictureBox pacaman, PictureBox wall)
        {
            if (pacman.Bounds.IntersectsWith(wall.Bounds))
            {
                if (goleft)
                {
                    noleft = true;
                    goleft = false;
                    pacman.Left = wall.Right + 2;
                }
                if (goright)
                {
                    noright = true;
                    goright = false;
                    pacman.Left = wall.Left - pacman.Width - 2;
                }
                if (goup)
                {
                    noup = true;
                    goup = false;
                    pacman.Top = wall.Bottom + 2;
                }
                if (godown)
                {
                    nodown = true;
                    godown = false;
                    pacman.Top = wall.Top - pacman.Height - 2;
                }
            }
        }
        private void CollectingCoins(PictureBox pacaman, PictureBox Coin)
        {
            if (pacaman.Bounds.IntersectsWith(Coin.Bounds))
            {
                if (Coin.Visible)
                {
                    Coin.Visible = false;
                    score += 1;
                }
            }
        }
        private void GhostCollision(Ghost g, PictureBox pacman, PictureBox ghost)
        {
            if (pacman.Bounds.IntersectsWith(ghost.Bounds))
            {
                GameOver("You died, Score:: " + score);
                g.ChangeDirection();
            }
        }
        private void GameOver(string message)
        {
            panelMenu.Visible = true;
            panelMenu.Enabled = true;
            GameTimer.Stop();
            ShowCoins();
            pacman.Location = new Point(478, 449);
            lblInfo.Text = message;


        }

        private void pacman_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox59_Click(object sender, EventArgs e)
        {

        }
    }
}