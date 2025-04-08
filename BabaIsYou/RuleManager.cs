using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class RuleManager {
        private Tile[,] _map;
        private int _width, _height;

        public RuleManager(GameMap map) {
            _map = map.Map;
            _width = map.Width;
            _height = map.Height;

            GameManager.Instance.RuleManager = this;
        }

        public void CheckRules() {
            List<Tile> ruleTiles = new List<Tile>();

            // 1. 모든 Rule 타일 찾기
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    if (_map[x, y].TileType == TileType.Rule) {
                        ruleTiles.Add(_map[x, y]);
                    }
                }
            }

            // 2. "Baba Is You" 확인
            foreach (Tile tile in ruleTiles) {
                if (tile.Symbol == 'B') {
                    Tile isTile = FindTileAt(tile.X + 1, tile.Y, 'I');
                    Tile youTile = FindTileAt(tile.X + 2, tile.Y, 'Y');

                    if (isTile != null && youTile != null) {
                        Console.WriteLine("Baba Is You 규칙이 활성화됨!");
                        GameManager.Instance.SetPlayer(FindTileFromType(TileType.Baba));
                        return;
                    }
                }
            }

            // "Baba Is You" 없으면 플레이어 없음
            Console.WriteLine("Baba Is You 규칙이 없어서 플레이어가 없음!");
            GameManager.Instance.ClearPlayer();
        }

        private List<Tile> FindTileFromType(TileType tileType) {
            List<Tile> tiles = new List<Tile>();
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    if (_map[x, y].TileType == tileType) {
                        tiles.Add(_map[x, y]);
                    }
                }
            }
            return tiles;
        }

        private Tile FindTileAt(int x, int y, char symbol) {
            if (x >= 0 && x < _width && y >= 0 && y < _height) {
                Tile tile = _map[x, y];
                if (tile.Symbol == symbol) return tile;
            }
            return null;
        }
    }
}
