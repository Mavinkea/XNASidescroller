using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class ScrollingBackground
    {
        Texture2D texture;
        Vector2 vector1;
        Vector2 vector2;

        public ScrollingBackground(Texture2D txt, Vector2 inVect)
        {
            texture = txt;
            vector1 = inVect;
            vector2 = inVect;
            vector2.X -= texture.Width;
        }

        public void DrawLoop(SpriteBatch sb)
        {
            sb.Draw(texture, vector1, Color.White);
            sb.Draw(texture, vector2, Color.White);
        }

        public void Update(int x, int y)
        {
             if (vector1.X + texture.Width <= 0)
                        vector1.X = vector2.X + texture.Width;
                if (vector2.X + texture.Width <= 0)
                        vector2.X = vector1.X + texture.Width;

                if (x >= 0)
                {
                        vector1.X += x;
                        vector2.X += x;
                }
                if (x < 0)
                {
                        vector1.X -= x - (x * 2);
                        vector2.X -= x - (x * 2);
                }

                if (vector1.X >= 800)
                {
                    vector1.X = 0;
                }
                if (vector2.X >= 0)
                {
                    vector2.X = -800;
                }
        }
    }
}
