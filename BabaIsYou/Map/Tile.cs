using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    public enum TileType {
        Empty,
        Rule,
        Object,
    }

    public class Tile {
        #region Properties

        public TileType TileType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; } // 타일의 이름 (예: "ROCK", "BABA", "WALL")

        #endregion // Properties





        #region Constructors

        public Tile(TileType tileType, int x, int y, string name) {
            TileType = tileType;
            X = x;
            Y = y;
            Name = name;
        }

        public Tile(Tile other) {
            Name = other.Name;
            TileType = other.TileType;
            X = other.X;
            Y = other.Y;
        }

        #endregion // Constructors





        #region public funcs

        public void SetPosition(int newX, int newY) {
            this.X = newX;
            this.Y = newY;
        }

        #endregion // public funcs
    }
}

