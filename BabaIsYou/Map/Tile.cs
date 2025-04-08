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
        private bool _isPushable;
        protected char _symbol;

        public TileType TileType { get { return _type; } }
        public bool IsPushable { get { return _isPushable; } set { _isPushable = value; } }
        public char Symbol { get { return _symbol; } }
        public int X { get; set; }
        public int Y { get; set; }

        public Tile(TileType type, int x, int y, bool isPushable = false) {
            _type = type;
            X = x;
            Y = y;
            _isPushable = isPushable;
            SetTile(type);
        }

        public void SetTile(TileType type) {
            if (TileType.Baba == type) {
                _symbol = 'B';
            }
            else if (TileType.Push == type) {
                _symbol = 'O';
            }
            else if (TileType.Wall == type) {
                _symbol = '#';
            }
            else {
                _symbol = '.';
            }
        }
    }

    class RuleTile : Tile {
        public string RuleText { get; private set; }

        public RuleTile(string ruleText, int x, int y) : base(TileType.Rule, x, y, true) {
            RuleText = ruleText;
            this._symbol = this.RuleText[0]; // 룰 타일의 첫 글자를 심볼로 사용
        }

        public void SetRule(string newRule) {
            RuleText = newRule;
        }
    }
}

