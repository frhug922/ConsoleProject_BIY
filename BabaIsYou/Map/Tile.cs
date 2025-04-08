using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    // 모든 타일의 기본 클래스
    abstract class Tile {
        public virtual bool IsSolid => false;  // 충돌 여부 (기본값: false)
        public virtual bool IsPushable => false; // 밀 수 있는지 여부
    }

    // 빈 타일 (바닥)
    class EmptyTile : Tile { }

    // 벽 타일
    class WallTile : Tile {
        public override bool IsSolid => true; // 벽은 충돌함
    }

    // 플레이어 타일
    class PlayerTile : Tile {
        public override bool IsSolid => true; // 플레이어는 공간을 차지함
    }

    // 밀 수 있는 오브젝트 타일 (예: 바위)
    class PushableTile : Tile {
        public override bool IsSolid => true; // 다른 오브젝트와 충돌
        public override bool IsPushable => true; // 밀 수 있음
    }

    // 텍스트 타일 (규칙 변경용)
    class RuleTile : Tile {
        public string RuleText { get; }

        public RuleTile(string ruleText) {
            RuleText = ruleText;
        }
    }

}
