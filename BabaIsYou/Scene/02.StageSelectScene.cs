﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class StageSelectScene : Scene {
        #region private fields

        private int selectedIndex = 0;
        private string[] stages = { "01", "02", "03", "04", "05", "06", "07" };

        #endregion // private fields





        #region public funcs

        public override void Render() {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("===== Stage Select =====\n");

            for (int i = 0; i < stages.Length; i++) {
                if (i == selectedIndex) {
                    Console.WriteLine($"> Stage {stages[i]} <");
                }
                else {
                    Console.WriteLine($"  Stage {stages[i]}  ");
                }
            }
        }

        public override void Input() {
            base.Input();
        }

        public override void Update() {
            switch (_keyInfo.Key) {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    selectedIndex = (selectedIndex - 1 + stages.Length) % stages.Length;
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    selectedIndex = (selectedIndex + 1) % stages.Length;
                    break;

                case ConsoleKey.Enter:
                    GameManager.Instance.ChangeScene(new GameScene(stages[selectedIndex]));
                    break;

                case ConsoleKey.Escape:
                    ConfirmExit();
                    break;
            }
        }

        #endregion // public funcs





        #region private funcs

        private void ConfirmExit() {
            string[] options = { "예", "아니오" };
            int selectedIndex = 0;

            while (true) {
                Console.SetCursorPosition(0, stages.Length + 2);
                Console.Write("정말로 나가시겠습니까?");

                // 메뉴 출력
                for (int i = 0; i < options.Length; i++) {
                    Console.SetCursorPosition(5, stages.Length + 4 + i);
                    if (i == selectedIndex) {
                        Console.Write("▶ " + options[i]);
                    }
                    else {
                        Console.Write("  " + options[i]);
                    }
                }

                // 입력 처리
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
                            GameManager.Instance.ChangeScene(new TitleScene());
                            return;
                        }
                        else { // "아니오" 선택
                            Console.SetCursorPosition(0, stages.Length + 2);
                            Console.WriteLine(new string(' ', Console.WindowWidth)); // 정말로 나가시겠습니까?
                            Console.WriteLine(new string(' ', Console.WindowWidth)); // 
                            Console.WriteLine(new string(' ', Console.WindowWidth)); // 예
                            Console.WriteLine(new string(' ', Console.WindowWidth)); // 아니오 // 해당 줄까지 지우기.
                            return;
                        }
                }
            }
        }

        #endregion // private funcs
    }
}
