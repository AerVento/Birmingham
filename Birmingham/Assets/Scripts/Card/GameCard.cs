using Game.Map;
using Game.Tech;
using UnityEngine;

namespace Game.Card
{
    /// <summary>
    /// 卡牌类型
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// 普通地点卡
        /// </summary>
        CommonLocation,

        /// <summary>
        /// 普通资源卡
        /// </summary>
        CommonResource,

        /// <summary>
        /// 万能地点卡
        /// </summary>
        UniversalLocation,

        /// <summary>
        /// 万能资源卡
        /// </summary>
        UniversalResource,
    }

    [System.Serializable]
    public struct GameCard
    {
        /// <summary>
        /// 万能地点卡
        /// </summary>
        public static GameCard UniversalLocation => new GameCard()
        {
            _type = CardType.UniversalLocation
        };

        /// <summary>
        /// 万能资源卡
        /// </summary>
        public static GameCard UniversalResource => new GameCard()
        {
            _type = CardType.UniversalResource
        };

        [SerializeField]
        [Tooltip("卡牌的类型")]
        private CardType _type;


        [SerializeField]
        [Tooltip("普通地点卡对应的城市")]
        private CityName _city;

        [SerializeField]
        [Tooltip("普通资源卡中能建造的资源类型")]
        private TechTypeVector<bool> _resource;

        /// <summary>
        /// 卡牌类型
        /// </summary>
        public CardType Type => _type;

        /// <summary>
        /// 如果卡牌类型是普通地点卡，则该属性代表卡牌的城市地点。
        /// </summary>
        public CityName City
        {
            get
            {
                if(_type == CardType.CommonLocation)
                    return _city;
                throw new System.InvalidOperationException($"Trying to access the property \"City\" of a non-common-location card. This card is type {_type}");
            }
        }

        /// <summary>
        /// 如果卡牌类型是普通资源卡，则该属性代表卡牌所表示的资源类型。
        /// </summary>
        public IReadOnlyTechTypeVector<bool> Resource
        {
            get
            {
                if (_type == CardType.CommonResource)
                    return _resource;
                throw new System.InvalidOperationException($"Trying to access the property \"City\" of a non-common-resource card. This card is type {_type}");
            }
        }
    }
}