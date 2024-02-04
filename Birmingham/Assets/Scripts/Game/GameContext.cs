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
        /// ��ǰ�����ĵ�ͼ��Ϣ
        /// </summary>
        public GameMap CurrentMap { get; private set; }

        /// <summary>
        /// ��ǰ��Ϸ�е������Ϣ
        /// </summary>
        public IReadOnlyDictionary<PlayerEnum, GamePlayer> Players { get; private set; }


        /// <summary>
        /// ��ͼ�����ı�ʱ�ص����¼�
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
        /// ���ÿ�غ�����������ص����¼�
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
