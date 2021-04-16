using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace Hugo_LAND.Client
{
    public abstract class GameObject
    {
        public abstract void Update(double gameTime, double elapsedTime);
        public abstract void Draw(Graphics graphics);
    }
}
