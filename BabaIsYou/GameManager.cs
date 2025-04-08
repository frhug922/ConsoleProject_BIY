using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class GameManager {
        #region private fields

        private static GameManager instance = null;
        private bool isRunning = true;
        private Scene currScene; // 현재 씬을 저장

        #endregion // private fields





        #region public fields

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

        #endregion // public fields





        #region public funcs

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

        #endregion // public funcs
    }
}
