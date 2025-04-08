using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    class Program {
        static void Main(string[] args) {
            Start();
            GameManager.Instance.ChangeScene(new TitleScene()); // 시작 씬을 타이틀 씬으로 설정
            GameManager.Instance.Play();
        }

        static void Start() {
            Console.CursorVisible = false; // 커서 숨기기
            Console.SetWindowSize(80, 30);  // 콘솔 창 크기 설정
            Console.SetBufferSize(80, 50);  // 콘솔 버퍼 크기 설정 (세로 길이를 더 크게)
        }
    }
}
