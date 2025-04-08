using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    public enum TileType {
        Empty,
        Baba,
        Push,
        Wall,
        Rule,
    }

    class Tile {
        private TileType _type;
        private char _symbol;

        public TileType TileType { get { return _type; } }
        public bool IsPushable { get; private set; }
        public char Symbol { get { return _symbol; } }
        public int X { get; set; }
        public int Y { get; set; }

        public Tile(TileType type, int x, int y, bool isPushable = false) {
            X = x;
            Y = y;
            SetTile(type, isPushable);
        }

        public void SetTile(TileType type, bool isPushable = false) {
            _type = type;
            IsPushable = isPushable;

            // 타입에 따른 심볼 설정
            switch (type) {
                case TileType.Baba:
                    _symbol = 'B';
                    break;
                case TileType.Push:
                    _symbol = 'O';
                    break;
                case TileType.Rule:
                    _symbol = 'R';
                    break;
                case TileType.Wall:
                    _symbol = '#';
                    break;
                case TileType.Empty:
                default:
                    _symbol = '.';
                    break;
            }
        }
    }

}
