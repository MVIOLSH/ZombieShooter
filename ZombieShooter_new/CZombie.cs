using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieShooter_new.Properties;

namespace ZombieShooter_new
{
    class CZombie : CImageBase
    {
        private Rectangle _zombieHotSpot = new Rectangle();
        public CZombie()
            :base (Resources.ZOMBIE)
        {
            _zombieHotSpot.X = Left + 20;
            _zombieHotSpot.Y = Top - 1;
            _zombieHotSpot.Width = 56;
            _zombieHotSpot.Height = 75;

        }
       

        public void Update(int X, int Y)
        {
            Left = X;
            Top = Y;
            _zombieHotSpot.X = Left +20;
            _zombieHotSpot.Y = Top - 1;
        }

        public bool Hit(int X, int Y)
        {
            Rectangle c = new Rectangle(X, Y, 1, 1); //Create a cursor rectangle 
            if(_zombieHotSpot.Contains(c))
            {
                return true;
            }
            return false;
        }


    }
}
