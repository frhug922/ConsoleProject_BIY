using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class TitleScene : Scene {
        private int _selectedIndex = 0;
        private string[] _menuItems = { "게임 시작", "게임 종료" };

        public override void Render() {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("===== BABA IS YOU =====\n");

            for (int i = 0; i < _menuItems.Length; i++) {
                if (i == _selectedIndex) {
                    Console.WriteLine($"> {_menuItems[i]} <");
                }
                else {
                    Console.WriteLine($"  {_menuItems[i]}  ");
                }
            }
        }

        public override void Input() {
            base.Input();
        }

        public override void Update() {
            switch (_keyInfo.Key) {
                case ConsoleKey.UpArrow:
                    _selectedIndex = (_selectedIndex - 1 + _menuItems.Length) % _menuItems.Length;
                    break;

                case ConsoleKey.DownArrow:
                    _selectedIndex = (_selectedIndex + 1) % _menuItems.Length;
                    break;

                case ConsoleKey.Enter:
                    if (_selectedIndex == 0) {
                        GameManager.Instance.ChangeScene(new StageSelectScene());
                    }
                    else if (_selectedIndex == 1) {
                        GameManager.Instance.ExitGame();
                    }
                    break;
            }
        }
    }
}
