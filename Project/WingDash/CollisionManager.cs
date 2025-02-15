using System;
using System.Collections.Generic;

namespace WingDash
{
    public class CollisionManager
    {
        public bool CheckCollision(Player player, List<Obstacle> obstacles)
        {
            foreach (var obstacle in obstacles)
            {
                // Eğer kuş engelin x pozisyonundaysa ve engelin y pozisyonuna çarpıyorsa
                if (player.PositionX >= obstacle.PositionX && player.PositionX < obstacle.PositionX + Obstacle.Width)
                {
                    if (obstacle.IsUpper && player.PositionY <= obstacle.PositionY + obstacle.Height)
                    {
                        return true;
                    }
                    else if (!obstacle.IsUpper && player.PositionY + player.Height >= obstacle.PositionY - obstacle.Height)
                    {
                        return true;
                    }
                }
            }

            // Eğer kuş ekranın altına veya üstüne çarptıysa oyun biter
            if (player.PositionY >= Console.WindowHeight || player.PositionY < 0)
            {
                return true;
            }

            return false;
        }
    }
}
