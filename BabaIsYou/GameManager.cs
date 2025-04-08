using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class GameManager {
        #region static fields

        private static GameManager _instance = null;

        public static GameManager Instance {
            get {
                if (_instance == null)
                    _instance = new GameManager();
                return _instance;
            }
        }

        public GameManager() {
            _instance = this;
        }

        #endregion // static fields





        #region private fields

        private bool _isRunning = true;
        private Scene _currScene; // 현재 씬을 저장
        private List<Tile> _currPlayer;

        #endregion // private fields





        #region properties

        public List<Tile> CurrPlayer { get { return _currPlayer; } } // 현재 플레이어 타일
        public RuleManager RuleManager { get; set; }

        #endregion // properties





        #region public funcs

        public void Play() {
            while (_isRunning) {
                _currScene.Render();
                _currScene.Input();
                _currScene.Update();
            }
        }

        public void ChangeScene(Scene newScene) {
            _currScene = newScene; // 씬 변경
            Console.Clear();
        }

        public void ExitGame() {
            _isRunning = false; // 게임 종료
        }

        public void SetPlayer(List<Tile> player) {
            _currPlayer = player;
        }

        public void ClearPlayer() {
            _currPlayer = null; // 플레이어 초기화
        }

        public void UpdateRules() {
            RuleManager.CheckRules();
        }

        #endregion // public funcs
    }
}
