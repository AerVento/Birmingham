using Framework.Singleton;
using Game.Map;
using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameContext
    {
        /// <summary>
        /// 当前所处的地图信息
        /// </summary>
        public GameMap CurrentMap { get; private set; }

        /// <summary>
        /// 当前游戏中的玩家信息
        /// </summary>
        public IReadOnlyDictionary<PlayerEnum, GamePlayer> Players { get; private set; }


        /// <summary>
        /// 地图发生改变时回调的事件
        /// </summary>
        public event MapChangingEvent OnMapChanged 
        {
            add
            {
                CurrentMap.OnMapChanged += value;
            }
            remove
            {
                CurrentMap.OnMapChanged -= value;
            }
        }

        /// <summary>
        /// 玩家每回合做出操作后回调的事件
        /// </summary>
        public event PlayerOperationEvent OnPlayerOperated
        {
            add
            {
                GamePlayer.OnPlayerOperated += value;
            }
            remove
            {
                GamePlayer.OnPlayerOperated -= value;
            }
        }


        public GameContext(GameMap currentMap, IReadOnlyDictionary<PlayerEnum, GamePlayer> players)
        {
            CurrentMap = currentMap;
            Players = players;
        }

    }

}
