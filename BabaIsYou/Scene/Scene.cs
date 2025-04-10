using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    abstract class Scene {
        #region protected field

        protected ConsoleKeyInfo _keyInfo;

        #endregion // protected field





        #region public funcs

        public abstract void Render();
        public virtual void Input() {
            _keyInfo = Console.ReadKey(true);
        }
        public abstract void Update();

        #endregion // public funcs
    }
}
