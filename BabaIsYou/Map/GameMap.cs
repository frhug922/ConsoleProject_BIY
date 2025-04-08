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

        #endregion // properties





        #region public fields
        public GameMap(int level) {
            if (level == 1) {
                //_width = 33;
                //_height = 18;
                _width = level1.GetLength(0);
                _height = level1.GetLength(1);
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
            Map = new Tile[_width, _height];

            InitializeMap(level);
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

            if (targetTile.TileType == TileType.Push) {
                int pushX = newX + (int)direction.X;
                int pushY = newY + (int)direction.Y;

                if (pushX < 0 || pushX >= _width || pushY < 0 || pushY >= _height) {
                    return;
                }

                Tile pushTargetTile = Map[pushX, pushY];

                if (pushTargetTile.TileType == TileType.Empty) {
                    // 밀기 성공: 밀리는 타일 이동
                    Map[pushX, pushY] = targetTile;
                    Map[newX, newY] = playerTile;
                    Map[currentX, currentY] = new Tile(TileType.Empty, currentX, currentY);
                    targetTile.X = pushX;
                    targetTile.Y = pushY;
                }
                else {
                    return;
                }
            }
            else {
                // 일반 이동 (빈 공간일 경우)
                Map[newX, newY] = playerTile;
                Map[currentX, currentY] = new Tile(TileType.Empty, currentX, currentY);
            }

            // 6. 플레이어 위치 업데이트
            playerTile.X = newX;
            playerTile.Y = newY;
        }


        #endregion // public fields





        #region // public funcs

        public void PrintMap() {
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    Console.Write(Map[x, y].Symbol + " ");
                }
                Console.WriteLine();
            }
        }

        #endregion // public funcs





        #region // private funcs

        private void InitializeMap(int level) {
            //for (int y = 0; y < _height; y++) {
            //    for (int x = 0; x < _width; x++) {
            //        Map[x, y] = new Tile(TileType.Empty, x, y); // 기본적으로 빈 타일
            //    }
            //}

            //// TODO 레벨 별 다른 맵 형태.
            ////Map[2, 2] = new BabaTile();
            ////Map[3, 3] = new WallTile();
            ////Map[4, 4] = new PushableTile();
            ////Map[1, 1] = new RuleTile("B");

            //Map[2, 2].SetTile(TileType.Baba);
            //Map[3, 3].SetTile(TileType.Push, true);
            //Map[4, 4].SetTile(TileType.Push, false);

            //GameManager.Instance.SetPlayer(FindTileFromType(TileType.Baba));

            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    char tileChar = level1[y, x]; // 문자 기반 데이터에서 가져오기
                    Tile newTile;

                    switch (tileChar) {
                        case 'B':
                            newTile = new Tile(TileType.Baba, x, y);
                            break;
                        case '#':
                            newTile = new Tile(TileType.Wall, x, y, false); // 벽 (밀 수 없음)
                            break;
                        case 'O':
                            newTile = new Tile(TileType.Push, x, y, true); // 오브젝트 (밀 수 있음)
                            break;
                        case 'R':
                            newTile = new Tile(TileType.Rule, x, y, true); // 룰 (밀 수 있음)
                            break;
                        default:
                            newTile = new Tile(TileType.Empty, x, y);
                            break;
                    }

                    Map[x, y] = newTile;
                }
            }
            GameManager.Instance.SetPlayer(FindTileFromType(TileType.Baba));
        }

        private List<Tile> FindTileFromType(TileType tileType) {
            List<Tile> tiles = new List<Tile>();
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    if (Map[x, y].TileType == tileType) {
                        tiles.Add(Map[x, y]);
                    }
                }
            }
            return tiles;
        }

        #endregion // private funcs





        #region // Map Data

        char[,] level1 = {
            { '.', '.', '.', '.', '.' },
            { '.', '#', '#', '#', '.' },
            { '.', '#', 'B', 'O', '.' },
            { '.', '#', 'R', '#', '.' },
            { '.', '.', '.', '.', '.' }};

        #endregion // Map Data
    }
}
