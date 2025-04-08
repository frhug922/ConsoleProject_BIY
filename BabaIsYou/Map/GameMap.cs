using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class GameMap {
        public Tile[,] Map { get; private set; }
        private int width, height;

        public GameMap(int level) {
            if (level == 1) {
                width = 33;
                height = 18;
            }
            else if (level == 2) {
                width = 24;
                height = 18;
            }
            else if (level == 3) {
                width = 33; // TODO 레벨 별로 다른 맵 사이즈
                height = 18;
            }
            else if (level == 4) {
                width = 33;
                height = 18;
            }
            else if (level == 5) {
                width = 33;
                height = 18;
            }
            Map = new Tile[width, height];

            InitializeMap();
        }

        private void InitializeMap() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    Map[x, y] = new EmptyTile(); // 기본적으로 빈 타일
                }
            }

            // TODO 레벨 별 다른 맵 형태.
            Map[2, 2] = new PlayerTile();
            Map[3, 3] = new WallTile();
            Map[4, 4] = new PushableTile();
            Map[1, 1] = new RuleTile("B");
        }

        public void PrintMap() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    Console.Write(GetTileSymbol(Map[x, y]) + " ");
                }
                Console.WriteLine();
            }
        }

        private char GetTileSymbol(Tile tile) {
            if (tile is PlayerTile) return 'P';
            if (tile is WallTile) return '#';
            if (tile is PushableTile) return 'O';
            if (tile is RuleTile) return 'T';
            return '.';
        }
    }

}
