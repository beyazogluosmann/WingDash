using System;

namespace WingDash
{
    public class Player
    {
        private string[] birdArt = new string[]
        {
            " \\",
            "(o>",
            " / )",
            "  |"
        };

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public int Height => birdArt.Length;
        private int gravity = 1;
        private int jumpStrength = -3;

        public Player(int startX, int startY)
        {
            PositionX = startX;
            PositionY = startY;
        }

        public void Jump()
        {
            PositionY += jumpStrength;
        }

        public void Fall()
        {
            PositionY += gravity;
        }

        public void Draw()
        {
            for (int i = 0; i < birdArt.Length; i++)
            {
                int drawY = PositionY + i;
                if (drawY >= 0 && drawY < Console.WindowHeight)
                {
                    Console.SetCursorPosition(PositionX, drawY);
                    Console.Write(birdArt[i]);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < birdArt.Length; i++)
            {
                int clearY = PositionY + i;
                if (clearY >= 0 && clearY < Console.WindowHeight)
                {
                    Console.SetCursorPosition(PositionX, clearY);
                    Console.Write("    "); // Kuşun kapladığı alanı temizleyin
                }
            }
        }
    }
}
