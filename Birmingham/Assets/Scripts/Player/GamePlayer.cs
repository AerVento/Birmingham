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
        /// 玩家最小收入
        /// </summary>
        public const int MINIMUM_INCOME = -10;
        
        /// <summary>
        /// 玩家最大收入
        /// </summary>
        public const int MAXIMUM_INCOME = 30;

        public static event PlayerOperationEvent OnPlayerOperated;

        private TechTypeVector<TechTree> _trees;

        /// <summary>
        /// 当前玩家
        /// </summary>
        public PlayerEnum Current {  get; private set; }

        /// <summary>
        /// 玩家的经济点数
        /// </summary>
        public int Economy { get; private set; } = 10;

        /// <summary>
        /// 玩家每回合能拿到的钱
        /// </summary>
        public int Income
        {
            get
            {
                // 这部分实际上是地图上的格子形状决定的，所以公式看上去有点魔法。
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
        /// 玩家分数
        /// </summary>
        public uint Score { get; private set; }

        /// <summary>
        /// 玩家当前金钱。小于等于0时则游戏失败。
        /// </summary>
        public uint Money { get; set; }

        /// <summary>
        /// 玩家免费的科研点数
        /// </summary>
        public ushort FreeUpgrade { get; set; } = 0;

        /// <summary>
        /// 玩家当前手中的卡牌
        /// </summary>
        public List<GameCard> Cards { get; private set; }

        /// <summary>
        /// 玩家当前自己的科技树
        /// </summary>
        public TechTypeVector<TechTree> Trees => _trees;

        public GamePlayer()
        {
            _trees = SingletonSOManager.Instance.GetSOFile<TechTreeSO>("TechTree").GetTree();
        }

        /// <summary>
        /// 科研的内部方法
        /// </summary>
        private ProgramMessage UpgraderInternal(IDictionary<TechType, ushort> counts)
        {
            // 原子性：先检查一下有没有一个科技已经没有可研发的了。如果有那所有的研发操作都不进行。
            foreach (TechType type in counts.Keys)
            {
                if (_trees[type].Root == null)
                    return ProgramMessage.Failure(-1, $"{type}类型已没有可研发的科技了。");
                ushort total = _trees[type].Total;
                if (total < counts[type])
                    return ProgramMessage.Failure(-1, $"{type}类型余下的建筑数量（{total}）不足以支持{counts[type]}次研发了。");
            }

            List<(IReadOnlyTechNode Tech, ushort Count)> developed = new();

            // 检查无误后再进行升级
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
        /// 建造一个建筑
        /// </summary>
        /// <param name="city">城市名</param>
        /// <param name="index">城市格子</param>
        /// <param name="tech">建造建筑的类型</param>
        /// <param name="card">使用的卡牌</param>
        /// <param name="isCheckOnly">是否只是检查而不真的建造</param>
        /// <returns>是否成功建造</returns>
        public ProgramMessage CreateBuilding(CityName city, ushort index, TechType type, GameCard card, bool isCheckOnly = false)
        {
            // 先检查当前科技树是否还有可以建造的建筑
            if (_trees[type].Root == null)
                return ProgramMessage.Failure(-1, $"当前类别{type}已经没有可以建造的建筑了。");

            var map = GameManager.Instance.Context.CurrentMap;

            ProgramMessage result;
            switch (card.Type)
            {
                case CardType.CommonLocation:
                case CardType.UniversalLocation:
                    // 如果是地点卡，不检查工业区
                    result = map.CreateBuilding(Current, type, city, index, _trees[type].Root, false, isCheckOnly);
                    break;
                case CardType.CommonResource:
                case CardType.UniversalResource:
                    // 如果是资源卡，则需要检查工业区
                    result = map.CreateBuilding(Current, type, city, index, _trees[type].Root, true, isCheckOnly);
                    break;
                default:
                    return ProgramMessage.Failure(-1, $"Unknown card type: {card.Type}.");
            }

            // 如果建造失败了，就返回错误信息
            if (!result.IsSuccess)
                return result;

            // 如果真的建造了的话，则需要挪动科技树
            if (!isCheckOnly)
            {
                // 同等级建筑仍有剩余
                if (_trees[type].Root.Data.Count > 1)
                    _trees[type].Root.Data.Count--;
                else
                {
                    // 将下一级的建筑挪动到科技树的根上
                    _trees[type].Root = _trees[type].Root.Next;
                }

                // 将此卡移除
                Cards.Remove(card);
                OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, map.GetBuilding(city, index)));
            }

            return ProgramMessage.Success();
        }

        /// <summary>
        /// 建造一个道路
        /// </summary>
        /// <param name="cityA">城市A</param>
        /// <param name="cityB">城市B</param>
        /// <param name="roadType">道路类型</param>
        /// <param name="card">使用的卡牌</param>
        /// <param name="isCheckOnly">是否只是检查而不真的建造</param>
        /// <returns></returns>
        public ProgramMessage CreateRoad(CityName cityA, CityName cityB, RoadType roadType, GameCard card, bool isCheckOnly = false)
        {
            var map = GameManager.Instance.Context.CurrentMap;

            // 建造道路一定需要工业区判断，这里加上工业区判断 
            ProgramMessage result = map.CreateRoad(Current, cityA, cityB, roadType, true, isCheckOnly);

            // 如果建造失败了，就返回错误信息
            if (!result.IsSuccess)
                return result;

            // 如果真的建造了的话，则需要一些额外操作
            if (!isCheckOnly)
            {
                Cards.Remove(card);
                OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, map.GetRoad(cityA, cityB)));
            }
            return ProgramMessage.Success();
        }

        /// <summary>
        /// 不使用卡牌的研发操作，但需要消耗免费研究点。
        /// </summary>
        /// <param name="counts">升级的科技列表。如果含有这个科技类型的Key，则对该科技类型进行Value次研发操作。</param>
        public ProgramMessage Upgrade(IDictionary<TechType, ushort> counts)
        {
            if(counts.Count > FreeUpgrade)
                return ProgramMessage.Failure(-1, $"缺少{counts.Count - FreeUpgrade}个免费科研点以进行科研。");

            return UpgraderInternal(counts);
        }

        /// <summary>
        /// 研发操作
        /// </summary>
        /// <param name="counts">升级的科技列表。如果含有这个科技类型的Key，则对该科技类型进行Value次研发操作。</param>
        /// <param name="card">使用的卡牌</param>
        public ProgramMessage Upgrade(IDictionary<TechType, ushort> counts, GameCard card)
        {
            Cards.Remove(card);
            return UpgraderInternal(counts);
        }

        /// <summary>
        /// 将一座或多座城市的经济建筑上的物品卖掉。
        /// </summary>
        /// <param name="contexts">卖物品操作的相关信息</param>
        /// <param name="card">使用的卡牌</param>
        /// <returns></returns>
        public ProgramMessage Sell(IEnumerable<SellContext> contexts, GameCard card)
        {
            var map = GameManager.Instance.Context.CurrentMap;
            
            // 原子性：先检查一下能否进行售卖操作。
            foreach(SellContext context in contexts)
            {
                if(!context.Building.Type.IsEconomy())
                    return ProgramMessage.Failure(-1, $"城市{context.Building.City}的格子{context.Building.Index}处的建筑不是经济建筑。");

                if (!Cities.IsMarket(context.Destination))
                    return ProgramMessage.Failure(-1, $"城市{context.Destination}不是交易所。");
                
                if(map.IsConnected(context.Building.City, context.Destination))
                    return ProgramMessage.Failure(-1, $"没有道路与交易所{context.Destination}相连。");

                Market market = map.GetCity<Market>(context.Destination);
                if (market.GetSelling(context.Index).IsSelling(context.Building.Type))
                    return ProgramMessage.Failure(-1, $"交易所{context.Destination}的格子{context.Index}处不售卖${context.Building.Type}类型的物品。");


                // 使用交易所的酒
                if (context.WineProvider == null)
                {
                    // 目标交易所格子处没有奖励酒
                    if (!market.IsBonusAvailable(context.Index))
                        return ProgramMessage.Failure(-1, $"将物品卖到交易所{context.Destination}失败：没有酒。");
                }
                else
                {
                    // 给酒的建筑没有酒
                    if (context.WineProvider.Type != TechType.Wine)
                        return ProgramMessage.Failure(-1, $"城市{context.WineProvider.City}的格子{context.WineProvider.Index}处不是酒厂。");

                    // 酒不够
                    if (context.WineProvider.ItemNeeds[TechType.Wine] < context.Building.ItemNeeds[TechType.Wine])
                        return ProgramMessage.Failure(-1, $"城市{context.WineProvider.City}的格子{context.WineProvider.Index}处的酒厂没有足够的酒。");
                }

            }

            foreach (SellContext context in contexts)
            {
                Market market = map.GetCity<Market>(context.Destination);
                if (context.WineProvider == null)
                {
                    // 使用酒
                    market.SetBonusAvailable(context.Index, false);
                    // 获得奖励
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
                    // 使用酒厂的酒
                    context.WineProvider.ItemNeeds[TechType.Wine] -= context.Building.ItemNeeds[TechType.Wine];
                    // 如果酒厂的酒归0而进入可收益状态，应该是由酒厂建设玩家对象监听卖出动作，进而改变各项收益。所以这里不设置酒厂建设玩家的收益。
                }

                // 进行卖出事项

                // 不再需要酒了
                context.Building.ItemNeeds[TechType.Wine] = 0;
                
                // 调整卖出玩家的收益
                Score += context.Building.Tech.Score;
                Economy += context.Building.Tech.EconomyValue;

            }

            Cards.Remove(card);
            OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, contexts.ToList()));
            return ProgramMessage.Success();
        }

        /// <summary>
        /// 贷款操作
        /// </summary>
        /// <param name="card">使用的卡牌</param>
        public ProgramMessage Loan(GameCard card)
        {
            if (Income - 3 < MINIMUM_INCOME)
                return ProgramMessage.Failure(-1, "收入过少，不能贷款！");

            // 每回合收入-3，直接获得30元。
            Income -= 3;
            Money += 30;
            OnPlayerOperated.Invoke(this, new PlayerOperationContext(Current, Operation.Loan));
            Cards.Remove(card);
            return ProgramMessage.Success();
        }

        /// <summary>
        /// 侦查。丢弃手中任意三张卡，自选两张万能卡
        /// </summary>
        /// <param name="drop">丢弃的卡</param>
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
