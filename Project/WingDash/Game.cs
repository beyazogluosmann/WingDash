using System;
using System.IO;
using System.Media;
using System.Threading;

namespace WingDash
{
    public class Game
    {
        private Player player;
        private ObstacleManager obstacleManager;
        private CollisionManager collisionManager;
        private bool isRunning;
        private int score;
        private int highScore;
        private string highScoreFile = "highscore.txt";
        private SoundPlayer jumpPlayer;
        private SoundPlayer scorePlayer;
        private SoundPlayer collisionPlayer;

        public Game()
        {
            player = new Player(10, 10);
            obstacleManager = new ObstacleManager();
            collisionManager = new CollisionManager();
            isRunning = true;
            score = 0;
            highScore = LoadHighScore();

            try
            {
                jumpPlayer = new SoundPlayer(Resource1.kanatsesi); // Kanat sesi
                scorePlayer = new SoundPlayer(Resource1.skorsesi); // Puan sesi
                collisionPlayer = new SoundPlayer(Resource1.carpti); // Çarpma sesi
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading sound files: " + ex.Message);
            }
        }

        public void Run()
        {
            ShowMenu();

            Console.CursorVisible = false;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"High Score: {highScore}"); // En yüksek skoru göster

            while (isRunning)
            {
                Update();
                Draw();
                Thread.Sleep(100); // Uyku süresi ayarlandı
            }

            // Oyun bittiğinde en yüksek skoru güncelle
            if (score > highScore)
            {
                highScore = score;
                SaveHighScore(highScore);
            }

            ShowGameOverScreen();
        }

        private void ShowMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            string[] menuText = new string[]
            {
                "**************************",
                "*      WINGDASH          *",
                "**************************",
                "* 1. Start Game          *",
                "* 2. Quit                *",
                "**************************",
                "Select an option: "
            };

            int centerX, centerY = Console.WindowHeight / 2 - menuText.Length / 2;

            for (int i = 0; i < menuText.Length; i++)
            {
                centerX = (Console.WindowWidth - menuText[i].Length) / 2;
                if (centerX < 0) centerX = 0; // Değerin negatif olmamasını sağla
                Console.SetCursorPosition(centerX, centerY + i);
                Console.WriteLine(menuText[i]);
            }
            Console.ResetColor();

            var choice = Console.ReadKey(true).Key;

            switch (choice)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Environment.Exit(0);
                    break;
                default:
                    ShowMenu();
                    break;
            }
        }

        private void PlayJumpSound()
        {
            try
            {
                jumpPlayer?.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing jump sound: " + ex.Message);
            }
        }

        private void PlayScoreSound()
        {
            try
            {
                scorePlayer?.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing score sound: " + ex.Message);
            }
        }

        private void PlayCollisionSound()
        {
            try
            {
                collisionPlayer?.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing collision sound: " + ex.Message);
            }
        }

        private void ShowGameOverScreen()
        {
            PlayCollisionSound();

            // "Game Over" yazısını ortada göstermek için
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            string[] gameOverText = new string[]
            {
                "  ____                        ___                   ",
                " / ___| __ _ _ __ ___   ___  / _ \\__   _____ _ __   ",
                "| |  _ / _` | '_ ` _ \\ / _ \\| | | \\ \\ / / _ \\ '__|  ",
                "| |_| | (_| | | | | | |  __/| |_| |\\ V /  __/ |     ",
                " \\____|\\__,_|_| |_| |_|\\___| \\___/  \\_/ \\___|_|     ",
                "                                                   "
            };

            int centerX, centerY = Console.WindowHeight / 2 - gameOverText.Length / 2;

            for (int i = 0; i < gameOverText.Length; i++)
            {
                centerX = (Console.WindowWidth - gameOverText[i].Length) / 2;
                if (centerX < 0) centerX = 0; // Değerin negatif olmamasını sağla
                Console.SetCursorPosition(centerX, centerY + i);
                Console.WriteLine(gameOverText[i]);
            }
            Console.ResetColor();
            Console.SetCursorPosition((Console.WindowWidth - 10) / 2, centerY + gameOverText.Length); // Yazının altına puan yazmak için
            Console.WriteLine($"Score: {score}");
            Console.SetCursorPosition((Console.WindowWidth - 20) / 2, centerY + gameOverText.Length + 1); // En yüksek skoru yazmak için
            Console.WriteLine($"High Score: {highScore}");

            // Tekrar oyna seçeneği
            Console.SetCursorPosition((Console.WindowWidth - 20) / 2, centerY + gameOverText.Length + 3);
            Console.WriteLine("Press R to Restart or Q to Quit");

            // Kullanıcıdan giriş al
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
            } while (keyInfo.Key != ConsoleKey.R && keyInfo.Key != ConsoleKey.Q);

            if (keyInfo.Key == ConsoleKey.R)
            {
                Restart();
            }
            else if (keyInfo.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            }
        }

        private void Restart()
        {
            // Oyunu yeniden başlat
            Game newGame = new Game();
            newGame.Run();
        }

        private void Update()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Spacebar)
                {
                    player.Jump();
                    PlayJumpSound();
                }
            }

            player.Fall();
            obstacleManager.Update();
            isRunning = !collisionManager.CheckCollision(player, obstacleManager.Obstacles);
            UpdateScore();
        }

        private void UpdateScore()
        {
            foreach (var obstacle in obstacleManager.Obstacles)
            {
                if (obstacle.PositionX < player.PositionX && !obstacle.Passed && obstacle.IsUpper)
                {
                    score++;
                    obstacle.Passed = true;
                    PlayScoreSound();
                }
            }
        }

        private void Draw()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write($"Score: {score}");
            Console.SetCursorPosition(0, 1);
            Console.Write($"High Score: {highScore}");
            player.Draw();
            obstacleManager.Draw();
        }

        private int LoadHighScore()
        {
            if (File.Exists(highScoreFile))
            {
                string scoreStr = File.ReadAllText(highScoreFile);
                if (int.TryParse(scoreStr, out int highScore))
                {
                    return highScore;
                }
            }
            return 0; // Varsayılan en yüksek skor
        }

        private void SaveHighScore(int highScore)
        {
            File.WriteAllText(highScoreFile, highScore.ToString());
        }
    }
}
