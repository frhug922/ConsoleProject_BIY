using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class GameManager {
        private static GameManager instance = null;
        private bool isRunning = true;
        private Scene currScene; // 현재 씬을 저장

        public static GameManager Instance {
            get {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        public GameManager() {
            instance = this;
        }

        public void Play() {
            while (isRunning) {
                currScene.Render();
                currScene.Input();
                currScene.Update();
            }
        }

        public void ChangeScene(Scene newScene) {
            currScene = newScene; // 씬 변경
            Console.Clear();
        }

        public void ExitGame() {
            isRunning = false; // 게임 종료
        }
    }
}
