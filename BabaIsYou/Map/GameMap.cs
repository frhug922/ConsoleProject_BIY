using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class GameMap {
        #region private fields

        private int _width;
        private int _height;

        #endregion // private fields





        #region properties

        public Tile[,] Map { get; private set; }

        #endregion // properties





        #region public fields
        public GameMap(int level) {
            if (level == 1) {
                _width = 33;
                _height = 18;
            }
            else if (level == 2) {
                _width = 24;
                _height = 18;
            }
            else if (level == 3) {
                _width = 33; // TODO 레벨 별로 다른 맵 사이즈
                _height = 18;
            }
            else if (level == 4) {
                _width = 33;
                _height = 18;
            }
            else if (level == 5) {
                _width = 33;
                _height = 18;
            }
            Map = new Tile[_width, _height];

            InitializeMap();
        }

        #endregion // public fields





        #region // public funcs

        public void PrintMap() {
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    Console.Write(GetTileSymbol(Map[x, y]) + " ");
                }
                Console.WriteLine();
            }
        }

        #endregion // public funcs





        #region // private funcs

        private void InitializeMap() {
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    Map[x, y] = new EmptyTile(); // 기본적으로 빈 타일
                }
            }

            // TODO 레벨 별 다른 맵 형태.
            Map[2, 2] = new PlayerTile();
            Map[3, 3] = new WallTile();
            Map[4, 4] = new PushableTile();
            Map[1, 1] = new RuleTile("B");
        }

        private char GetTileSymbol(Tile tile) {
            if (tile is PlayerTile) return 'P';
            if (tile is WallTile) return '#';
            if (tile is PushableTile) return 'O';
            if (tile is RuleTile) return 'T';
            return '.';
        }

        #endregion // private funcs
    }
}
