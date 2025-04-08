using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    abstract class Scene {
        protected ConsoleKeyInfo _keyInfo;
        public abstract void Render();
        public virtual void Input() {
            _keyInfo = Console.ReadKey(true);
        }
        public abstract void Update();
    }
}
