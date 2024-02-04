using Framework.Message;
using Framework.SO;
using Game.Card;
using Game.Map;
using Game.Tech;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Player
{
    public class GamePlayer
    {
        /// <summary>
        /// �����С����
        /// </summary>
        public const int MINIMUM_INCOME = -10;
        
        /// <summary>
        /// ����������
        /// </summary>
        public const int MAXIMUM_INCOME = 30;

        public static event PlayerOperationEvent OnPlayerOperated;

        private TechTypeVector<TechTree> _trees;

        /// <summary>
        /// ��ǰ���
        /// </summary>
        public PlayerEnum Current {  get; private set; }

        /// <summary>
        /// ��ҵľ��õ���
        /// </summary>
        public int Economy { get; private set; } = 10;

        /// <summary>
        /// ���ÿ�غ����õ���Ǯ
        /// </summary>
        public int Income
        {
            get
            {
                // �ⲿ��ʵ�����ǵ�ͼ�ϵĸ�����״�����ģ����Թ�ʽ����ȥ�е�ħ����
                if (Economy <= 10)
                    return Economy - 10;
                else if (Economy > 10 && Economy <= 30)
                    return (Economy + 1) / 2 - 5;
                else if (Economy > 30 && Economy <= 60)
                    return (Economy + 2) / 3;
                else if(Economy > 60 && Economy <= 96)
                    return (Economy + 3) / 4 + 5;
                else
                    return (Economy + 2) / 3 + 7;
            }

            private set
            {
                if (value <= 0)
                    Economy = value + 10;
                else if (value > 0 && value <= 10)
                    Economy = 2 * value + 10;
                else if (value > 10 && value <= 20)
                    Economy = 3 * value;
                else if (value > 20 && value <= 29)
                    Economy = 4 * value - 20;
                else
                    Economy = 3 * value + 9;
            }
        }

        /// <summary>
        /// ��ҷ���
        /// </summary>
        public uint Score { get; private set; }

        /// <summary>
        /// ��ҵ�ǰ��Ǯ��С�ڵ���0ʱ����Ϸʧ�ܡ�
        /// </summary>
        public uint Money { get; set; }

        /// <summary>
        /// �����ѵĿ��е���
        /// </summary>
        public ushort FreeUpgrade { get; set; } = 0;

        /// <summary>
        /// ��ҵ�ǰ���еĿ���
        /// </summary>
        public List<GameCard> Cards { get; private set; }

        /// <summary>
        /// ��ҵ�ǰ�Լ��ĿƼ���
        /// </summary>
        public TechTypeVector<TechTree> Trees => _trees;

        public GamePlayer()
        {
            _trees = SingletonSOManager.Instance.GetSOFile<TechTreeSO>("TechTree").GetTree();
        }

        /// <summary>
        /// ���е��ڲ�����
        /// </summary>
        private ProgramMessage UpgraderInternal(IDictionary<TechType, ushort> counts)
        {
            // ԭ���ԣ��ȼ��һ����û��һ���Ƽ��Ѿ�û�п��з����ˡ�����������е��з������������С�
            foreach (TechType type in counts.Keys)
            {
                if (_trees[type].Root == null)
                    return ProgramMessage.Failure(-1, $"{type}������û�п��з��ĿƼ��ˡ�");
                ushort total = _trees[type].Total;
                if (total < counts[type])
                    return ProgramMessage.Failure(-1, $"{type}�������µĽ���������{total}��������֧��{counts[type]}���з��ˡ�");
            }

            List<(IReadOnlyTechNode Tech, ushort Count)> developed = new();

            // ���������ٽ�������
            foreach (TechType type in counts.Keys)
            {
                TechNode root = _trees[type].Root;
                ushort count = counts[type];
                while (count > 0)
                {
                    if (root.Data.Count > count)
                    {
                        root.Data.Count -= count;
                        developed.Add((root, count));
                        count = 0;
                    }
                    else
                    {
                        count -= root.Data.Count;
                        developed.Add((root, root.Data.Count));
                        root = root.Next;
                    }
                }
                _trees[type].Root = root;
            }

            OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, developed));
            return ProgramMessage.Success();
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="city">������</param>
        /// <param name="index">���и���</param>
        /// <param name="tech">���콨��������</param>
        /// <param name="card">ʹ�õĿ���</param>
        /// <param name="isCheckOnly">�Ƿ�ֻ�Ǽ�������Ľ���</param>
        /// <returns>�Ƿ�ɹ�����</returns>
        public ProgramMessage CreateBuilding(CityName city, ushort index, TechType type, GameCard card, bool isCheckOnly = false)
        {
            // �ȼ�鵱ǰ�Ƽ����Ƿ��п��Խ���Ľ���
            if (_trees[type].Root == null)
                return ProgramMessage.Failure(-1, $"��ǰ���{type}�Ѿ�û�п��Խ���Ľ����ˡ�");

            var map = GameManager.Instance.Context.CurrentMap;

            ProgramMessage result;
            switch (card.Type)
            {
                case CardType.CommonLocation:
                case CardType.UniversalLocation:
                    // ����ǵص㿨������鹤ҵ��
                    result = map.CreateBuilding(Current, type, city, index, _trees[type].Root, false, isCheckOnly);
                    break;
                case CardType.CommonResource:
                case CardType.UniversalResource:
                    // �������Դ��������Ҫ��鹤ҵ��
                    result = map.CreateBuilding(Current, type, city, index, _trees[type].Root, true, isCheckOnly);
                    break;
                default:
                    return ProgramMessage.Failure(-1, $"Unknown card type: {card.Type}.");
            }

            // �������ʧ���ˣ��ͷ��ش�����Ϣ
            if (!result.IsSuccess)
                return result;

            // �����Ľ����˵Ļ�������ҪŲ���Ƽ���
            if (!isCheckOnly)
            {
                // ͬ�ȼ���������ʣ��
                if (_trees[type].Root.Data.Count > 1)
                    _trees[type].Root.Data.Count--;
                else
                {
                    // ����һ���Ľ���Ų�����Ƽ����ĸ���
                    _trees[type].Root = _trees[type].Root.Next;
                }

                // ���˿��Ƴ�
                Cards.Remove(card);
                OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, map.GetBuilding(city, index)));
            }

            return ProgramMessage.Success();
        }

        /// <summary>
        /// ����һ����·
        /// </summary>
        /// <param name="cityA">����A</param>
        /// <param name="cityB">����B</param>
        /// <param name="roadType">��·����</param>
        /// <param name="card">ʹ�õĿ���</param>
        /// <param name="isCheckOnly">�Ƿ�ֻ�Ǽ�������Ľ���</param>
        /// <returns></returns>
        public ProgramMessage CreateRoad(CityName cityA, CityName cityB, RoadType roadType, GameCard card, bool isCheckOnly = false)
        {
            var map = GameManager.Instance.Context.CurrentMap;

            // �����·һ����Ҫ��ҵ���жϣ�������Ϲ�ҵ���ж� 
            ProgramMessage result = map.CreateRoad(Current, cityA, cityB, roadType, true, isCheckOnly);

            // �������ʧ���ˣ��ͷ��ش�����Ϣ
            if (!result.IsSuccess)
                return result;

            // �����Ľ����˵Ļ�������ҪһЩ�������
            if (!isCheckOnly)
            {
                Cards.Remove(card);
                OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, map.GetRoad(cityA, cityB)));
            }
            return ProgramMessage.Success();
        }

        /// <summary>
        /// ��ʹ�ÿ��Ƶ��з�����������Ҫ��������о��㡣
        /// </summary>
        /// <param name="counts">�����ĿƼ��б������������Ƽ����͵�Key����ԸÿƼ����ͽ���Value���з�������</param>
        public ProgramMessage Upgrade(IDictionary<TechType, ushort> counts)
        {
            if(counts.Count > FreeUpgrade)
                return ProgramMessage.Failure(-1, $"ȱ��{counts.Count - FreeUpgrade}����ѿ��е��Խ��п��С�");

            return UpgraderInternal(counts);
        }

        /// <summary>
        /// �з�����
        /// </summary>
        /// <param name="counts">�����ĿƼ��б������������Ƽ����͵�Key����ԸÿƼ����ͽ���Value���з�������</param>
        /// <param name="card">ʹ�õĿ���</param>
        public ProgramMessage Upgrade(IDictionary<TechType, ushort> counts, GameCard card)
        {
            Cards.Remove(card);
            return UpgraderInternal(counts);
        }

        /// <summary>
        /// ��һ����������еľ��ý����ϵ���Ʒ������
        /// </summary>
        /// <param name="contexts">����Ʒ�����������Ϣ</param>
        /// <param name="card">ʹ�õĿ���</param>
        /// <returns></returns>
        public ProgramMessage Sell(IEnumerable<SellContext> contexts, GameCard card)
        {
            var map = GameManager.Instance.Context.CurrentMap;
            
            // ԭ���ԣ��ȼ��һ���ܷ��������������
            foreach(SellContext context in contexts)
            {
                if(!context.Building.Type.IsEconomy())
                    return ProgramMessage.Failure(-1, $"����{context.Building.City}�ĸ���{context.Building.Index}���Ľ������Ǿ��ý�����");

                if (!Cities.IsMarket(context.Destination))
                    return ProgramMessage.Failure(-1, $"����{context.Destination}���ǽ�������");
                
                if(map.IsConnected(context.Building.City, context.Destination))
                    return ProgramMessage.Failure(-1, $"û�е�·�뽻����{context.Destination}������");

                Market market = map.GetCity<Market>(context.Destination);
                if (market.GetSelling(context.Index).IsSelling(context.Building.Type))
                    return ProgramMessage.Failure(-1, $"������{context.Destination}�ĸ���{context.Index}��������${context.Building.Type}���͵���Ʒ��");


                // ʹ�ý������ľ�
                if (context.WineProvider == null)
                {
                    // Ŀ�꽻�������Ӵ�û�н�����
                    if (!market.IsBonusAvailable(context.Index))
                        return ProgramMessage.Failure(-1, $"����Ʒ����������{context.Destination}ʧ�ܣ�û�оơ�");
                }
                else
                {
                    // ���ƵĽ���û�о�
                    if (context.WineProvider.Type != TechType.Wine)
                        return ProgramMessage.Failure(-1, $"����{context.WineProvider.City}�ĸ���{context.WineProvider.Index}�����ǾƳ���");

                    // �Ʋ���
                    if (context.WineProvider.ItemNeeds[TechType.Wine] < context.Building.ItemNeeds[TechType.Wine])
                        return ProgramMessage.Failure(-1, $"����{context.WineProvider.City}�ĸ���{context.WineProvider.Index}���ľƳ�û���㹻�ľơ�");
                }

            }

            foreach (SellContext context in contexts)
            {
                Market market = map.GetCity<Market>(context.Destination);
                if (context.WineProvider == null)
                {
                    // ʹ�þ�
                    market.SetBonusAvailable(context.Index, false);
                    // ��ý���
                    switch (market.BonusType)
                    {
                        case MarketBonusType.Money:
                            Money += market.BonusValue;
                            break;
                        case MarketBonusType.Score:
                            Score += market.BonusValue;
                            break;
                        case MarketBonusType.Economy:
                            Economy += market.BonusValue;
                            break;
                        case MarketBonusType.Upgrade:
                            FreeUpgrade += market.BonusValue;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // ʹ�þƳ��ľ�
                    context.WineProvider.ItemNeeds[TechType.Wine] -= context.Building.ItemNeeds[TechType.Wine];
                    // ����Ƴ��ľƹ�0�����������״̬��Ӧ�����ɾƳ�������Ҷ���������������������ı�������档�������ﲻ���þƳ�������ҵ����档
                }

                // ������������

                // ������Ҫ����
                context.Building.ItemNeeds[TechType.Wine] = 0;
                
                // ����������ҵ�����
                Score += context.Building.Tech.Score;
                Economy += context.Building.Tech.EconomyValue;

            }

            Cards.Remove(card);
            OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, contexts.ToList()));
            return ProgramMessage.Success();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="card">ʹ�õĿ���</param>
        public ProgramMessage Loan(GameCard card)
        {
            if (Income - 3 < MINIMUM_INCOME)
                return ProgramMessage.Failure(-1, "������٣����ܴ��");

            // ÿ�غ�����-3��ֱ�ӻ��30Ԫ��
            Income -= 3;
            Money += 30;
            OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, Operation.Loan));
            Cards.Remove(card);
            return ProgramMessage.Success();
        }

        /// <summary>
        /// ��顣���������������ſ�����ѡ�������ܿ�
        /// </summary>
        /// <param name="drop">�����Ŀ�</param>
        public ProgramMessage Investigate(IEnumerable<GameCard> drop)
        {
            foreach (GameCard card in drop)
            {
                Cards.Remove(card);
            }

            Cards.Add(GameCard.UniversalLocation);
            Cards.Add(GameCard.UniversalResource);

            OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, Operation.Investigate));
            return ProgramMessage.Success();
        }
    }

}
