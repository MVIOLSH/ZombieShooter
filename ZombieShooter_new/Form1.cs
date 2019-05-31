//#define MyDebug

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZombieShooter_new.Properties;

namespace ZombieShooter_new
{
    public partial class ZombieShooter : Form
    {
        #region Variables
        const int FrameNum = 8;
        const int SplatNum = 3;
        bool splat = false;

        int _gameFrame = 0;
        int _splatTime = 0;

        int _hits = 0;
        int _misses = 0;
        int _totalShots = 0;
        double _averageShots = 0;
        #endregion
#if MyDebug
        int _cursX = 0;
        int _cursY = 0;
#endif
        CZombie _zombie;
        CSplat _splat;
        CSign _sign;
        CScoreFrame _scoreFrame;
        Random rnd = new Random();

        public ZombieShooter()
        {
            InitializeComponent();

            //Create Scope 
            Bitmap b = new Bitmap(Resources.gun_sight);
            this.Cursor = CustomCursor.CreateCursor(b, b.Height / 2, b.Width / 2);

            _zombie = new CZombie() { Left = 20, Top = 278 };
            _scoreFrame = new CScoreFrame() { Left = 155, Top = 85 };
            _sign = new CSign() { Left = 0, Top = 0 };
            _splat = new CSplat();

        }

        private void timerGameLoop_Tick(object sender, EventArgs e)
        {
            if (_gameFrame >= FrameNum)
            {
                UpdateZombie();
                _gameFrame = 0;
            }
            
            if(splat)
            { 
                if (_splatTime >= SplatNum)
                {
                    splat = false;
                    _splatTime = 0;
                    UpdateZombie();
                }
                _splatTime++;
            }
            _gameFrame++;

             this.Refresh();

        }

        private void UpdateZombie()
        {
            _zombie.Update(
            rnd.Next(Resources.ZOMBIE.Width, this.Width - Resources.ZOMBIE.Width),
            rnd.Next(this.Height / 2, this.Height - Resources.ZOMBIE.Height*2));

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            _sign.DrawImage(dc);
            _scoreFrame.DrawImage(dc);

            if (splat == true)
            {
                _splat.DrawImage(dc);
            }
            else
            {
                _zombie.DrawImage(dc);

            } 

            
#if MyDebug
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Stencil", 12, FontStyle.Regular);
            TextRenderer.DrawText(dc, "X=" + _cursX.ToString() + ":" + "Y=" + _cursY.ToString(), _font,
                new Rectangle(30, 28, 120, 20), SystemColors.ControlLight, flags);
#endif

            //On Screen score
            TextFormatFlags flags = TextFormatFlags.Left;
            Font _font = new System.Drawing.Font("Stencil", 12, FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, "SHOTS:  " + _totalShots.ToString(), _font, new Rectangle(180, 160, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "HITS:  " + _hits.ToString(), _font, new Rectangle(180, 180, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "MISSES:  " + _misses.ToString(), _font, new Rectangle(180, 200, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "AVG:  " + _averageShots.ToString("F0") + "%", _font, new Rectangle(180, 220, 120, 20), SystemColors.ControlText, flags);
           





            base.OnPaint(e);
        }

        private void ZombieShooter_MouseMove(object sender, MouseEventArgs e)
        {
#if MyDebug
            _cursX = e.X;
            _cursY = e.Y;
#endif

            this.Refresh();
        }

        private void ZombieShooter_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 690 && e.X < 756 && e.Y > 120 && e.Y < 154)  // Start buton region definition
            {
                timerGameLoop.Start();
            }
            else if (e.X > 690 && e.X < 756 && e.Y > 176 && e.Y < 204)  // Stop buton region definition)
            {
                timerGameLoop.Stop();
            }
            else if (e.X > 693 && e.X < 753 && e.Y > 224 && e.Y < 251)  // Restart buton region definition
            {
                timerGameLoop.Stop();
                _averageShots = 0;
                _misses = 0;
                _totalShots = 0;
                _hits = 0;

            }
            else if (e.X > 696 && e.X < 757 && e.Y > 274 && e.Y < 298)  // Quit buton region definition
            {
                timerGameLoop.Stop();
                this.Close();
            }
            else
            {
                if (_zombie.Hit(e.X, e.Y))
                {
                    splat = true;
                    _splat.Left = _zombie.Left - Resources.Splat.Width / 3;
                    _splat.Top = _zombie.Top - Resources.Splat.Height / 3;
                    _hits++;
                }
                else
                {
                    _misses++;
                }
                _totalShots = _hits + _misses;
                _averageShots = (double)_hits / (double)_totalShots * 100.0;
                
            }
            FireGun();

        }
        private void FireGun()
        {
            SoundPlayer simpleSound = new SoundPlayer(Resources.gun);
            simpleSound.Play();
        }
    }

}
