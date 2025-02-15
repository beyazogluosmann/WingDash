using System;
using System.Collections.Generic;
using WingDash;

namespace WingDash
{
    public class ObstacleManager
    {
        private List<Obstacle> obstacles = new List<Obstacle>();
        private int spawnInterval = 20; // Engel oluşturma sıklığı
        private int timer = 0;
        private int gapSize = 10; // Engel aralığı
        private int minDistanceBetweenObstacles = 20; // Engeller arasındaki minimum mesafe

        public List<Obstacle> Obstacles => obstacles;

        public ObstacleManager()
        {
            // Başlangıçta birkaç engel ekle
            SpawnInitialObstacles();
        }

        private void SpawnInitialObstacles()
        {
            int initialDistance = 30; // İlk engeller arasındaki mesafe
            for (int i = 0; i < 3; i++)
            {
                int gapPosition = new Random().Next(5, Console.WindowHeight - gapSize - 5);
                int positionX = Console.WindowWidth + i * initialDistance; // Başlangıç engelleri ileri taşındı
                obstacles.Add(new Obstacle(positionX, 0, gapPosition, true));
                obstacles.Add(new Obstacle(positionX, Console.WindowHeight, Console.WindowHeight - gapPosition - gapSize, false));
            }
        }

        public void Update()
        {
            timer++;
            if (timer >= spawnInterval)
            {
                timer = 0;
                if (CanSpawnObstacle())
                {
                    SpawnObstacle();
                }
            }

            foreach (var obstacle in obstacles)
            {
                obstacle.Move();
            }

            obstacles.RemoveAll(o => o.PositionX < 0);
        }

        private bool CanSpawnObstacle()
        {
            if (obstacles.Count == 0) return true;
            int lastObstacleX = obstacles[obstacles.Count - 1].PositionX;
            return (Console.WindowWidth - lastObstacleX) >= minDistanceBetweenObstacles;
        }

        private void SpawnObstacle()
        {
            Random rand = new Random();
            int gapPosition = rand.Next(5, Console.WindowHeight - gapSize - 5);

            obstacles.Add(new Obstacle(Console.WindowWidth - 1, 0, gapPosition, true));
            obstacles.Add(new Obstacle(Console.WindowWidth - 1, Console.WindowHeight, Console.WindowHeight - gapPosition - gapSize, false));
        }

        public void Draw()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.Draw();
            }
        }

        public void Clear()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.Clear();
            }
        }
    }
}
