using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Card
{
    [CreateAssetMenu(fileName = "NewCardDeckSOFile", menuName = "SO/CardDeckSO")]
    public class CardDeckSO : ScriptableObject
    {
        [SerializeField]
        [Tooltip("��Ϸ�еĿ���")]
        private List<GameCard> _deck;

        /// <summary>
        /// ���һ�����顣
        /// </summary>
        public List<GameCard> Deck => new List<GameCard>(_deck);
    }
}
