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
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        #endregion // properties





        #region public fields
        public GameMap(int level) {
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
                        Tile tile = Map[x, y];

                        if (tile.TileType == TileType.Rule) {
                            continue; // 룰 타일은 무시
                        }

                        // 조작 가능한 오브젝트에 따라 타일을 추가
                        if (controlledObjects.Contains("BABA") && tile.Name == "B") {
                            movableTiles.Add(tile); // "B" 타일을 움직일 수 있음
                        }
                        else if (controlledObjects.Contains("ROCK") && tile.Name == "O") {
                            movableTiles.Add(tile); // "O" 타일을 움직일 수 있음
                        }
                        else if (controlledObjects.Contains("WALL") && tile.Name == "#") {
                            movableTiles.Add(tile); // 벽도 움직일 수 있도록 처리
                        }
                        else if (controlledObjects.Contains("FLAG") && tile.Name == "F") {
                            movableTiles.Add(tile); // "F" (플래그)도 움직일 수 있도록 처리
                        }
                    }
                }
            }

            // 이동을 시도하는 로직
            foreach (Tile tile in movableTiles) {
                int newX = tile.X + direction.X;
                int newY = tile.Y + direction.Y;

                if (!IsInBounds(newX, newY)) {
                    continue; // 맵 밖으로 나가면 이동하지 않음
                }

                Tile targetTile = Map[newX, newY];

                // Stop 오브젝트인지 확인
                if (RuleManager.Instance.HasRule(targetTile.Name, "IS", "STOP")) {
                    continue;
                }

                // PUSH 오브젝트인지 확인
                if (RuleManager.Instance.HasRule(targetTile.Name, "IS", "PUSH")) {
                    List<Tile> pushTiles = new List<Tile>(); // 밀릴 타일들을 저장
                    int checkX = newX, checkY = newY;

                    // 연쇄적으로 밀릴 수 있는지 확인
                    while (IsInBounds(checkX, checkY)) {
                        Tile checkTile = Map[checkX, checkY];

                        if (checkTile.TileType == TileType.Empty) {
                            break; // 빈 공간을 만나면 밀 수 있음
                        }

                        if (!RuleManager.Instance.HasRule(checkTile.Name, "IS", "PUSH")) {
                            pushTiles.Clear(); // 밀 수 없는 타일을 만나면 전체 취소
                            break;
                        }

                        pushTiles.Add(checkTile);
                        checkX += direction.X;
                        checkY += direction.Y;
                    }

                    // 밀릴 공간이 없으면 이동 취소
                    if (pushTiles.Count == 0 || !IsInBounds(checkX, checkY)) {
                        continue;
                    }

                    // 밀기 수행 (역순으로 처리하여 마지막부터 밀어야 함)
                    for (int i = pushTiles.Count - 1; i >= 0; i--) {
                        Tile pushTile = pushTiles[i];
                        int moveX = pushTile.X + direction.X;
                        int moveY = pushTile.Y + direction.Y;

                        Map[moveX, moveY] = pushTile;
                        pushTile.SetPosition(moveX, moveY);
                    }

                    // 원래 움직이려던 타일도 이동
                    Map[newX, newY] = tile;
                    tile.SetPosition(newX, newY);
                    Map[tile.X, tile.Y] = new Tile(TileType.Empty, tile.X, tile.Y, ".");
                }
                else {
                    // 그냥 이동 가능하면 이동
                    Map[newX, newY] = tile;
                    tile.SetPosition(newX, newY);
                    Map[tile.X, tile.Y] = new Tile(TileType.Empty, tile.X, tile.Y, ".");
                }
            }

            UpdateRules();
        }


        public void PrintMap() {
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    if (Map[x, y].TileType == TileType.Rule) {
                        // TODO 룰 타일 배경 색깔 변경
                    }
                    else {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write(Map[x, y].Name.First() + " ");
                }
                Console.WriteLine();
            }
        }

        #endregion // public funcs





        #region // private funcs

        private void InitializeMap(string[,] level) {
            _height = level.GetLength(0);
            _width = level.GetLength(1);
            Map = new Tile[_width, _height];

            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    string tileStr = level[y, x];

                    if (string.IsNullOrEmpty(tileStr) || tileStr == ".") { // 빈 타일
                        Map[x, y] = new Tile(TileType.Empty, x, y, ".");
                    }
                    else if (tileStr == "#" || tileStr == "B" || tileStr == "O" || tileStr == "F") { // 오브젝트 타일. 추후 생기면 추가해야함.
                        Map[x, y] = new Tile(TileType.Object, x, y, tileStr);
                    }
                    else {
                        Map[x, y] = new Tile(TileType.Rule, x, y, tileStr);
                    }
                }
            }

            UpdateRules();
        }

        public void UpdateRules() {
            RuleManager.Instance.ClearRules(); // 기존 규칙 초기화

            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    Tile tile = Map[x, y];

                    if (tile.TileType == TileType.Rule) {
                        TryAddRule(x, y); // 규칙 추가 시도
                    }
                }
            }
        }

        private void TryAddRule(int x, int y) {
            // 가로 방향 규칙 추가 (예: BABA IS YOU)
            if (IsInBounds(x + 2, y)) {
                Tile first = Map[x, y];
                Tile second = Map[x + 1, y];
                Tile third = Map[x + 2, y];

                if (second.Name == "IS" && IsValidSubject(first.Name) && IsValidAttribute(third.Name)) {
                    RuleManager.Instance.AddRule(first.Name, second.Name, third.Name);
                }
            }

            // 세로 방향 규칙 추가 (예: ROCK IS PUSH)
            if (IsInBounds(x, y + 2)) {
                Tile first = Map[x, y];
                Tile second = Map[x, y + 1];
                Tile third = Map[x, y + 2];

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
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", "BABA", "IS", "YOU", }, // 1
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 2
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 3
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 4
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 5
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 6
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 7
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 8
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 9
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "O", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 10
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "B", ".", ".", ".", "O", ".", ".", ".", "F", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 11
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "O", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 12
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 13
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 14
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 15
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 16
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", }, // 17
            { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." ,".", ".", ".", ".", ".", ".", ".", ".", ".", ".", },}; // 18

        #endregion // Map Data
    }
}
