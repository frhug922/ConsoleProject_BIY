﻿using System;
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
                //_width = 33;
                //_height = 18;
                _width = level1.GetLength(0);
                _height = level1.GetLength(1);
                InitializeMap(level1);
            }
            else if (level == 2) {
                //_width = 24;
                //_height = 18;
            }
            else if (level == 3) {
                //_width = 33; // TODO 레벨 별로 다른 맵 사이즈
                //_height = 18;
            }
            else if (level == 4) {
                //_width = 33;
                //_height = 18;
            }
            else if (level == 5) {
                //_width = 33;
                //_height = 18;
            }
        }

        public void Move(Vector2 direction) {
            // 1. 현재 바바의 위치 찾기
            List<Tile> playerTiles = GameManager.Instance.CurrPlayer;

            if (playerTiles.Count == 0) {
                Console.WriteLine("플레이어가 존재하지 않습니다!");
                return;
            }

            Tile playerTile = playerTiles[0]; // 현재는 바바가 하나라고 가정
            int currentX = playerTile.X;
            int currentY = playerTile.Y;

            // 2. 이동할 새로운 좌표 계산
            int newX = currentX + (int)direction.X;
            int newY = currentY + (int)direction.Y;

            // 3. 맵 범위 검사
            if (newX < 0 || newX >= _width || newY < 0 || newY >= _height) {
                return;
            }

            // 4. 이동할 위치의 타일 가져오기
            Tile targetTile = Map[newX, newY];

            // 5. 이동 가능 여부 체크
            if (!targetTile.IsPushable && targetTile.TileType != TileType.Empty) {
                return;
            }

            // 6. 밀 수 있는 타일들을 리스트로 저장
            List<Tile> pushTiles = new List<Tile>();
            int checkX = newX;
            int checkY = newY;

            while (checkX >= 0 && checkX < _width && checkY >= 0 && checkY < _height) {
                Tile checkTile = Map[checkX, checkY];

                if (!checkTile.IsPushable) {
                    break; // 밀 수 없는 타일이 나오면 종료
                }

                pushTiles.Add(checkTile);
                checkX += (int)direction.X;
                checkY += (int)direction.Y;
            }

            // 7. 밀리는 타일의 끝부분이 맵을 벗어나면 이동 불가
            if (checkX < 0 || checkX >= _width || checkY < 0 || checkY >= _height) {
                return;
            }

            // 8. 밀리는 타일의 끝부분이 빈 공간이 아니라면 이동 불가
            if (Map[checkX, checkY].TileType != TileType.Empty) {
                return;
            }

            // 9. 모든 밀리는 타일을 이동
            for (int i = pushTiles.Count - 1; i >= 0; i--) {
                Tile pushTile = pushTiles[i];
                int moveX = pushTile.X + (int)direction.X;
                int moveY = pushTile.Y + (int)direction.Y;
                Map[moveX, moveY] = pushTile;
                pushTile.X = moveX;
                pushTile.Y = moveY;
            }

            // 10. 플레이어 이동
            Map[newX, newY] = playerTile;
            Map[currentX, currentY] = new Tile(TileType.Empty, currentX, currentY);
            playerTile.X = newX;
            playerTile.Y = newY;
        }

        #endregion // public fields





        #region // public funcs

        public void PrintMap() {
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    if (Map[x, y].TileType == TileType.Rule) {
                        if (Map[x, y].Symbol == 'B') {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                        }
                        else if (Map[x, y].Symbol == 'I') {
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        else if (Map[x, y].Symbol == 'Y') {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                        }
                        else if (Map[x, y].Symbol == 'S') {
                            //TODO
                        }
                    }
                    else {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write(Map[x, y].Symbol + " ");
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

                    if (string.IsNullOrEmpty(tileStr)) {
                        Map[x, y] = new Tile(TileType.Empty, x, y); // 빈 타일
                    }
                    else if (tileStr == "#") {
                        Map[x, y] = new Tile(TileType.Wall, x, y); // 벽 (밀 수 없음)
                    }
                    else if (tileStr == "O") {
                        Map[x, y] = new Tile(TileType.Push, x, y); // 오브젝트 (못밂)
                    }
                    else if (tileStr == "B") {
                        Map[x, y] = new Tile(TileType.Baba, x, y); // 바바
                    }
                    else if (tileStr == "BABA" || tileStr == "IS" || tileStr == "YOU") {
                        Map[x, y] = new RuleTile(tileStr, x, y); // 룰 타일
                    }
                    else {
                        Console.WriteLine($"알 수 없는 타일: {tileStr} at ({x},{y})");
                        Map[x, y] = new Tile(TileType.Empty, x, y);
                    }
                }
            }
        }


        #endregion // private funcs





        #region // Map Data

        //char[,] level1 = {
        //    { 'R', 'R', 'R', '.', '.' },
        //    { '.', '.', '.', '.', '.' },
        //    { '.', '.', 'B', '.', '.' },
        //    { '.', '#', '.', '#', '.' },
        //    { '.', '.', '.', '.', '.' }};



        string[,] level1 = {
            { "", "", "", "", "" },
            { "BABA", "IS", "YOU", "", "" },
            { "", "", "", "", "" },
            { "B", "", "", "", "" },
            { "", "", "#", "O", "" }};

        #endregion // Map Data
    }
}
