using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class Util {

    }

    class Vector2 {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2(int x, int y) {
            X = x;
            Y = y;
        }

        public static Vector2 Up => new Vector2(0, -1);
        public static Vector2 Down => new Vector2(0, 1);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Right => new Vector2(1, 0);
    }
}
