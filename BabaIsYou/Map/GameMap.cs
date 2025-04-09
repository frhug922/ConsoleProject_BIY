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
        private Action _winCallback;

        #endregion // private fields





        #region properties

        public Stack<Tile>[,] Map { get; private set; }
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        #endregion // properties





        #region public fields
        public GameMap(int level, Action winCallback) {
            if (level == 1) {
                _width = level1.GetLength(0);
                _height = level1.GetLength(1);
                InitializeMap(level1);
            }
            else if (level == 2) {
                // TODO 레벨 별 다른 맵 적용
            }
            else if (level == 3) {

            }
            else if (level == 4) {

            }
            else if (level == 5) {

            }

            _winCallback = winCallback;
        }

        #endregion // public fields





        #region // public funcs

        public void Move(Vector2 direction) {
            List<string> controlledObjects = RuleManager.Instance.GetControlledObjects(); // 현재 조작 가능한 오브젝트들 가져오기

            if (controlledObjects.Count == 0) {
                return; // 조작할 오브젝트가 없으면 아무 일도 안 함
            }

            // 먼저, 움직일 수 있는 타일들을 찾음
            List<Tile> movableTiles = new List<Tile>();

            for (int i = 0; i < controlledObjects.Count; i++) {
                for (int y = 0; y < _height; y++) {
                    for (int x = 0; x < _width; x++) {
                        Tile tile = Map[x, y].Peek();

                        if (tile.TileType == TileType.Rule) {
                            continue; // 룰 타일은 무시
                        }

                        // 조작 가능한 오브젝트에 따라 타일을 추가
                        if (controlledObjects.Contains("BABA") && tile.Name == "B"
                            || controlledObjects.Contains("ROCK") && tile.Name == "R"
                            || controlledObjects.Contains("WALL") && tile.Name == "W"
                            || controlledObjects.Contains("FLAG") && tile.Name == "F"
                            ) {
                            movableTiles.Add(tile); // "B" 타일을 움직일 수 있음
                        }
                    }
                }
            }

            // 이동 우선순위 설정
            if (direction.Y > 0) {
                movableTiles = movableTiles.OrderByDescending(t => t.Y).ToList(); // 아래로 이동 시 Y가 큰 순서
            }
            else if (direction.Y < 0) {
                movableTiles = movableTiles.OrderBy(t => t.Y).ToList(); // 위로 이동 시 Y가 작은 순서
            }
            else if (direction.X > 0) {
                movableTiles = movableTiles.OrderByDescending(t => t.X).ToList(); // 오른쪽 이동 시 X가 큰 순서
            }
            else if (direction.X < 0) {
                movableTiles = movableTiles.OrderBy(t => t.X).ToList(); // 왼쪽 이동 시 X가 작은 순서
            }

            bool isWin = false;

            // 이동 로직
            foreach (Tile tile in movableTiles) {
                int newX = tile.X + direction.X;
                int newY = tile.Y + direction.Y;

                if (!IsInBounds(newX, newY)) {
                    continue;
                }

                Tile targetTile = Map[newX, newY].Count > 0 ? Map[newX, newY].Peek() : null;

                // STOP 오브젝트인지 확인
                if (targetTile != null && RuleManager.Instance.HasRule(targetTile.Name, "IS", "STOP") && targetTile.TileType != TileType.Rule) {
                    continue;
                }

                // PUSH 오브젝트인지 확인
                if (targetTile != null && RuleManager.Instance.HasRule(targetTile.Name, "IS", "PUSH") || targetTile.TileType == TileType.Rule) {
                    List<Tile> pushTiles = new List<Tile>();
                    int checkX = newX, checkY = newY;

                    while (IsInBounds(checkX, checkY)) {
                        Tile checkTile = Map[checkX, checkY].Count > 0 ? Map[checkX, checkY].Peek() : null;

                        if (checkTile == null || checkTile.TileType == TileType.Empty) {
                            break;
                        }

                        if (RuleManager.Instance.HasRule(checkTile.Name, "IS", "STOP") && checkTile.TileType != TileType.Rule) {
                            pushTiles.Clear();
                            break;
                        }

                        if (RuleManager.Instance.HasRule(checkTile.Name, "IS", "PUSH") || checkTile.TileType == TileType.Rule) {
                            pushTiles.Add(checkTile);
                        }
                        checkX += direction.X;
                        checkY += direction.Y;
                    }

                    if (pushTiles.Count == 0 || !IsInBounds(checkX, checkY)) {
                        continue;
                    }

                    for (int i = pushTiles.Count - 1; i >= 0; i--) {
                        Tile pushTile = pushTiles[i];
                        int moveX = pushTile.X + direction.X;
                        int moveY = pushTile.Y + direction.Y;

                        Map[pushTile.X, pushTile.Y].Pop();
                        pushTile.SetPosition(moveX, moveY);
                        Map[moveX, moveY].Push(pushTile);
                    }
                }

                Map[tile.X, tile.Y].Pop();
                tile.SetPosition(newX, newY);
                Map[newX, newY].Push(tile);

                // 승리 조건 확인
                if (Map[newX, newY].Any(x => RuleManager.Instance.HasRule(x.Name, "IS", "WIN"))) {
                    isWin = true;
                }
            }

            if (isWin) {
                _winCallback(); // 승리 조건 충족 시 콜백 호출
            }
            UpdateRules();
        }

        public void PrintMap() {
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    if (Map[x, y].Peek().TileType == TileType.Rule) {
                        Util.SetConsoleColor(Map[x, y].Peek().Name);
                    }
                    else {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(Map[x, y].Peek().Name.First() + " ");
                }
                Console.WriteLine();
            }
        }

        #endregion // public funcs





        #region // private funcs

        private void InitializeMap(string[,] level) {
            _height = level.GetLength(0);
            _width = level.GetLength(1);
            Map = new Stack<Tile>[_width, _height];

            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    Map[x, y] = new Stack<Tile>();
                    Map[x, y].Push(new Tile(TileType.Empty, x, y, ".")); // 초기화
                }
            }

            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    string tileStr = level[y, x];

                    if (string.IsNullOrEmpty(tileStr) || tileStr == ".") { // 빈 타일
                        //Map[x, y] = new Tile(TileType.Empty, x, y, ".");
                        // Do Nothing
                    }
                    else if (tileStr == "W" || tileStr == "B" || tileStr == "R" || tileStr == "F") { // 오브젝트 타일. 추후 생기면 추가해야함.
                        //Map[x, y] = new Tile(TileType.Object, x, y, tileStr);
                        Map[x, y].Push(new Tile(TileType.Object, x, y, tileStr));
                    }
                    else {
                        //Map[x, y] = new Tile(TileType.Rule, x, y, tileStr);
                        Map[x, y].Push(new Tile(TileType.Rule, x, y, tileStr));
                    }
                }
            }

            UpdateRules();
        }

        private void UpdateRules() {
            RuleManager.Instance.ClearRules(); // 기존 규칙 초기화

            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    Tile tile = Map[x, y].Peek();

                    if (tile.TileType == TileType.Rule) {
                        TryAddRule(x, y); // 규칙 추가 시도
                    }
                }
            }
        }

        private void TryAddRule(int x, int y) {
            // 가로 방향 규칙 추가 (예: BABA IS YOU)
            if (IsInBounds(x + 2, y)) {
                Tile first = Map[x, y].Peek();
                Tile second = Map[x + 1, y].Peek();
                Tile third = Map[x + 2, y].Peek();

                if (second.Name == "IS" && IsValidSubject(first.Name) && IsValidAttribute(third.Name)) {
                    RuleManager.Instance.AddRule(first.Name, second.Name, third.Name);
                }
            }

            // 세로 방향 규칙 추가 (예: ROCK IS PUSH)
            if (IsInBounds(x, y + 2)) {
                Tile first = Map[x, y].Peek();
                Tile second = Map[x, y + 1].Peek();
                Tile third = Map[x, y + 2].Peek();

                if (second.Name == "IS" && IsValidSubject(first.Name) && IsValidAttribute(third.Name)) {
                    RuleManager.Instance.AddRule(first.Name, second.Name, third.Name);
                }
            }
        }

        private bool IsInBounds(int x, int y) {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }

        private bool IsValidSubject(string name) {
            return name == "BABA" || name == "ROCK" || name == "WALL" || name == "FLAG";
        }

        private bool IsValidAttribute(string name) {
            return name == "YOU" || name == "PUSH" || name == "STOP" || name == "WIN";
        }

        #endregion // private funcs





        #region // Map Data

        string[,] level1 = {
            // 1    2    3    4    5    6    7    8    9    10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27   28   29   30   31   32   33
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 1
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 2
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 3
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 4
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 5
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 6
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "BABA", "IS", "YOU", ".", ".", ".", ".", ".", "FLAG", "IS", "WIN", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 7
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 8
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 9
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "R", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 10
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "B", ".", ".", ".", "R", ".", ".", ".", "F", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 11
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "R", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 12
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 13
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 14
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "WALL", "IS", "STOP", ".", ".", ".", ".", ".", "ROCK", "IS", "PUSH", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 15
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 16
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 17
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", },}; // 18

        #endregion // Map Data
    }
}
