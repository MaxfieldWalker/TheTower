using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    /// <summary>
    /// プレイヤーの数やゲームの難易度などを格納する
    /// </summary>
    public class GameInfo
    {
        public readonly GamePlayerMode PlayerMode;
        public readonly GameLevel Lavel;

        public GameInfo(GamePlayerMode playerMode, GameLevel level)
        {
            this.PlayerMode = playerMode;
            this.Lavel = level;
        }
    }
}
