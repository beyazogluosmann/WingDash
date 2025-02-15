using System;

namespace WingDash
{
    public class Obstacle
    {
        private static string pipeArt = "####";
        public static int Width => pipeArt.Length;

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public int Height { get; private set; }
        public bool IsUpper { get; private set; }
        public bool Passed { get; set; } // Engel geçildi mi kontrolü

        public Obstacle(int startX, int startY, int height, bool isUpper)
        {
            PositionX = startX;
            PositionY = startY;
            Height = height;
            IsUpper = isUpper;
            Passed = false;
        }

        public void Move()
        {
            PositionX -= 2; // Hareket hızını artırdık
        }

        public void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Green; // Rengi yeşil olarak ayarla
            if (IsUpper)
            {
                for (int i = 0; i < Height; i++)
                {
                    int drawY = PositionY + i;
                    if (PositionX >= 0 && PositionX < Console.WindowWidth && drawY >= 0 && drawY < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(PositionX, drawY);
                        Console.Write(pipeArt);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Height; i++)
                {
                    int drawY = PositionY - i;
                    if (PositionX >= 0 && PositionX < Console.WindowWidth && drawY >= 0 && drawY < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(PositionX, drawY);
                        Console.Write(pipeArt);
                    }
                }
            }
            Console.ResetColor(); // Rengi varsayılan hale getir
        }

        public void Clear()
        {
            if (IsUpper)
            {
                for (int i = 0; i < Height; i++)
                {
                    int clearY = PositionY + i;
                    if (PositionX >= 0 && PositionX < Console.WindowWidth && clearY >= 0 && clearY < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(PositionX, clearY);
                        Console.Write(new string(' ', Width)); // Engelin kapladığı alanı temizleyin
                    }
                }
            }
            else
            {
                for (int i = 0; i < Height; i++)
                {
                    int clearY = PositionY - i;
                    if (PositionX >= 0 && PositionX < Console.WindowWidth && clearY >= 0 && clearY < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(PositionX, clearY);
                        Console.Write(new string(' ', Width)); // Engelin kapladığı alanı temizleyin
                    }
                }
            }
        }
    }
}
