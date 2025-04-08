using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class TitleScene : Scene {
        private int selectedIndex = 0;
        private string[] menuItems = { "게임 시작", "게임 종료" };

        public override void Render() {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("===== BABA IS YOU =====\n");

            for (int i = 0; i < menuItems.Length; i++) {
                if (i == selectedIndex) {
                    Console.WriteLine($"> {menuItems[i]} <");
                }
                else {
                    Console.WriteLine($"  {menuItems[i]}  ");
                }
            }
        }

        public override void Input() {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key) {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % menuItems.Length;
                    break;

                case ConsoleKey.Enter:
                    if (selectedIndex == 0) {
                        GameManager.Instance.ChangeScene(new StageSelectScene());
                    }
                    else if (selectedIndex == 1) {
                        GameManager.Instance.ExitGame();
                    }
                    break;
            }
        }

        public override void Update() { }
    }
}
