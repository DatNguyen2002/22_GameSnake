

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Program
    {
        private static int width = 20;
        private static int height = 10;
        private static int score = 0;
        private static int delay = 200;
        private static bool gameOver = false;
        private static Random random = new Random();

        private static List<int> snakeX = new List<int>();
        private static List<int> snakeY = new List<int>();
        private static int foodX;
        private static int foodY;
        private static Direction direction = Direction.Right;

        static void Main()
        {
            Console.Title = "Rắn Săn Mồi";
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            InitializeGame();
            DrawBorder();
            GenerateFood();
            DrawFood();
            DrawSnake();

            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    HandleKey(Console.ReadKey(true).Key);
                }

                MoveSnake();

                if (IsCollision())
                {
                    gameOver = true;
                    Console.SetCursorPosition(width / 2 - 4, height / 2);
                    Console.Write("GAME OVER");
                }

                if (snakeX[0] == foodX && snakeY[0] == foodY)
                {
                    EatFood();
                    GenerateFood();
                    DrawFood();
                }

                Thread.Sleep(delay);
            }

            Console.ReadKey();
        }

        private static void InitializeGame()
        {
            snakeX.Clear();
            snakeY.Clear();
            snakeX.Add(width / 2);
            snakeY.Add(height / 2);
            score = 0;
            delay = 200;
            gameOver = false;
            direction = Direction.Right;
        }

        private static void DrawBorder()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            // Vẽ viền trên
            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            Console.Write(new string('═', width - 2));
            Console.Write("╗");

            // Vẽ viền dưới
            Console.SetCursorPosition(0, height - 1);
            Console.Write("╚");
            Console.Write(new string('═', width - 2));
            Console.Write("╝");

            // Vẽ viền trái và phải
            for (int y = 1; y < height - 1; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("║");
                Console.SetCursorPosition(width - 1, y);
                Console.Write("║");
            }
        }

        private static void GenerateFood()
        {
            foodX = random.Next(1, width - 1);
            foodY = random.Next(1, height - 1);
        }

        private static void DrawFood()
        {
            Console.SetCursorPosition(foodX, foodY);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("F");
        }

        private static void DrawSnake()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < snakeX.Count; i++)
            {
                Console.SetCursorPosition(snakeX[i], snakeY[i]);
                Console.Write(i == 0 ? "@" : "O");
            }
        }

        private static void HandleKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (direction != Direction.Down)
                        direction = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    if (direction != Direction.Up)
                        direction = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    if (direction != Direction.Right)
                        direction = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    if (direction != Direction.Left)
                        direction = Direction.Right;
                    break;
            }
        }

        private static void MoveSnake()
        {
            int headX = snakeX[0];
            int headY = snakeY[0];

            switch (direction)
            {
                case Direction.Up:
                    headY--;
                    break;
                case Direction.Down:
                    headY++;
                    break;
                case Direction.Left:
                    headX--;
                    break;
                case Direction.Right:
                    headX++;
                    break;
            }

            snakeX.Insert(0, headX);
            snakeY.Insert(0, headY);

            if (snakeX.Count > score + 1)
            {
                snakeX.RemoveAt(snakeX.Count - 1);
                snakeY.RemoveAt(snakeY.Count - 1);
            }

            DrawSnake();
        }

        private static bool IsCollision()
        {
            int headX = snakeX[0];
            int headY = snakeY[0];

            if (headX == 0 || headX == width - 1 || headY == 0 || headY == height - 1)
            {
                return true;
            }

            for (int i = 1; i < snakeX.Count; i++)
            {
                if (snakeX[i] == headX && snakeY[i] == headY)
                {
                    return true;
                }
            }

            return false;
        }

        private static void EatFood()
        {
            score++;
            if (score % 5 == 0 && delay > 50)
            {
                delay -= 10;
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
