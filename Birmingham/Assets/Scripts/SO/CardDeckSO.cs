using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Card
{
    [CreateAssetMenu(fileName = "NewCardDeckSOFile", menuName = "SO/CardDeckSO")]
    public class CardDeckSO : ScriptableObject
    {
        [SerializeField]
        [Tooltip("游戏中的卡组")]
        private List<GameCard> _deck;

        /// <summary>
        /// 获得一个卡组。
        /// </summary>
        public List<GameCard> Deck => new List<GameCard>(_deck);
    }
}
