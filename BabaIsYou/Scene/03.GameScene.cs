using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class GameScene : Scene {
        #region private fields

        private string stageNumber; // 선택된 스테이지 번호
        private GameMap gameMap;

        #endregion // private fields





        #region public fields

        public GameScene(string stageNumber) {
            this.stageNumber = stageNumber;
            LoadMap();
        }

        public GameMap GameMap { get { return gameMap; } }

        #endregion // public fields





        #region public funcs

        public override void Render() {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"=== Stage {stageNumber} ===");
            gameMap.PrintMap();
            PrintRule();
        }

        public override void Input() {
            base.Input();
        }

        public override void Update() {
            switch (_keyInfo.Key) {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    gameMap.Move(Vector2.Up);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    gameMap.Move(Vector2.Down);
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    gameMap.Move(Vector2.Left);
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    gameMap.Move(Vector2.Right);
                    break;

                case ConsoleKey.Escape:
                    ConfirmExit();
                    break;
            }
        }

        #endregion // public funcs





        #region private funcs

        private void LoadMap() {
            if (stageNumber == "01") {
                gameMap = new GameMap(1, PrintClear, PrintGameOver);
            }
            else if (stageNumber == "02") {
                gameMap = new GameMap(2, PrintClear, PrintGameOver);
            }
            else if (stageNumber == "03") {
                gameMap = new GameMap(3, PrintClear, PrintGameOver);
            }
            else if (stageNumber == "04") {
                gameMap = new GameMap(4, PrintClear, PrintGameOver);
            }
            else if (stageNumber == "05") {
                gameMap = new GameMap(5, PrintClear, PrintGameOver);
            }
            else {
                throw new ArgumentException("Invalid stage number");
            }
        }

        private void ConfirmExit() {
            string[] options = { "예", "아니오" };
            int selectedIndex = 0;

            while (true) {
                Console.SetCursorPosition(0, gameMap.Map.GetLength(1) + 2);
                Console.Write("정말로 나가시겠습니까?");

                for (int i = 0; i < options.Length; i++) {
                    Console.SetCursorPosition(5, gameMap.Map.GetLength(1) + 4 + i);
                    if (i == selectedIndex)
                        Console.Write("▶ " + options[i]);
                    else
                        Console.Write("  " + options[i]);
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key) {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % options.Length;
                        break;

                    case ConsoleKey.Enter:
                        if (selectedIndex == 0) { // "예" 선택
                            GameManager.Instance.ChangeScene(new StageSelectScene());
                            return;
                        }
                        else { // "아니오" 선택
                            Console.SetCursorPosition(0, gameMap.Map.GetLength(1) + 2);
                            Console.WriteLine(new string(' ', Console.WindowWidth)); // 정말로 나가시겠습니까?
                            Console.WriteLine(new string(' ', Console.WindowWidth)); // 
                            Console.WriteLine(new string(' ', Console.WindowWidth)); // 예
                            Console.WriteLine(new string(' ', Console.WindowWidth)); // 아니오 // 해당 줄까지 지우기.
                            return;
                        }
                }
            }
        }

        private void PrintRule() {
            var objectDefinitions = new Dictionary<string, string>{
                { "BABA", "B" },
                { "#WALL", "#" },
                { "FLAG", "F" },
                { "ROCK", "R" },
                { "~WATER", "~" },
                { "SKULL", "S" },
                { "GRASS", "G" },
                { "IS", "I" },
                { "YOU", "Y" },
                { "WIN", "W" },
                { "STOP", "S" },
                { "PUSH", "P" },
                { "SINK", "S" },
                { "DEFEAT", "D" },};

            int startX = gameMap.Map.GetLength(0) * 2 + 2;
            int startY = 2; // 시작 위치

            int line = 0;
            foreach (var obj in objectDefinitions) {
                Console.SetCursorPosition(startX, startY + line);
                Util.SetConsoleColor(obj.Key);
                Console.Write(obj.Value + " ");
                Util.SetConsoleColor("");
                Console.Write($" = {obj.Key}");
                line++;
            }
        }

        private void PrintGameOver() {
            Render();
            Console.SetCursorPosition(0, gameMap.Map.GetLength(1) + 2);
            Console.WriteLine("게임 오버!");
            Console.WriteLine("다시 시작하려면 아무 키나 누르세요.");
            Console.ReadKey(true);
            Console.Clear();
            LoadMap();
        }

        private void PrintClear() {
            Render();
            Console.SetCursorPosition(0, gameMap.Map.GetLength(1) + 2);
            Console.WriteLine("스테이지 클리어!");
            Console.WriteLine("다음 스테이지로 이동하려면 아무 키나 누르세요.");
            Console.ReadKey(true);
            GameManager.Instance.ChangeScene(new StageSelectScene());
        }

        #endregion // private funcs
    }
}
