using Game.Map;
using Game.Tech;
using UnityEngine;

namespace Game.Card
{
    /// <summary>
    /// ��������
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// ��ͨ�ص㿨
        /// </summary>
        CommonLocation,

        /// <summary>
        /// ��ͨ��Դ��
        /// </summary>
        CommonResource,

        /// <summary>
        /// ���ܵص㿨
        /// </summary>
        UniversalLocation,

        /// <summary>
        /// ������Դ��
        /// </summary>
        UniversalResource,
    }

    [System.Serializable]
    public struct GameCard
    {
        /// <summary>
        /// ���ܵص㿨
        /// </summary>
        public static GameCard UniversalLocation => new GameCard()
        {
            _type = CardType.UniversalLocation
        };

        /// <summary>
        /// ������Դ��
        /// </summary>
        public static GameCard UniversalResource => new GameCard()
        {
            _type = CardType.UniversalResource
        };

        [SerializeField]
        [Tooltip("���Ƶ�����")]
        private CardType _type;


        [SerializeField]
        [Tooltip("��ͨ�ص㿨��Ӧ�ĳ���")]
        private CityName _city;

        [SerializeField]
        [Tooltip("��ͨ��Դ�����ܽ������Դ����")]
        private TechTypeVector<bool> _resource;

        /// <summary>
        /// ��������
        /// </summary>
        public CardType Type => _type;

        /// <summary>
        /// ���������������ͨ�ص㿨��������Դ����Ƶĳ��еص㡣
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
        /// ���������������ͨ��Դ����������Դ���������ʾ����Դ���͡�
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