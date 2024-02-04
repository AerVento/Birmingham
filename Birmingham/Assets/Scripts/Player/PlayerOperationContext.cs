using Game.Map;
using Game.Tech;
using System.Collections.Generic;

namespace Game.Player
{
    /// <summary>
    /// 玩家操作枚举
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// 玩家建造了建筑
        /// </summary>
        CreateBuilding,

        /// <summary>
        /// 玩家建造了一条路
        /// </summary>
        CreateRoad,

        /// <summary>
        /// 玩家进行了一次研发
        /// </summary>
        Upgrade,

        /// <summary>
        /// 玩家进行了一次售卖
        /// </summary>
        Sell,

        /// <summary>
        /// 玩家进行了贷款
        /// </summary>
        Loan,

        /// <summary>
        /// 玩家进行了侦查
        /// </summary>
        Investigate,
    }

    /// <summary>
    /// 玩家一次回合操作的委托函数
    /// </summary>
    /// <param name="player">玩家对象</param>
    /// <param name="context">上下文信息</param>
    public delegate void PlayerOperationEvent(GamePlayer player, PlayerOperationContext context);

    /// <summary>
    /// 当玩家进行了一次回合操作时，用于回调的上下文。
    /// </summary>
    public class PlayerOperationContext
    { 
        /// <summary>
        /// 是由哪一个玩家进行了操作。
        /// </summary>
        public PlayerEnum Player { get; private set; }

        /// <summary>
        /// 玩家此回合进行的操作
        /// </summary>
        public Operation Operation { get; private set; }

        /// <summary>
        /// 如果玩家此回合进行了建造建筑，则该属性代表该玩家建造的建筑信息。
        /// </summary>
        public Building Building { get; private set; }

        /// <summary>
        /// 如果玩家此回合进行了建造道路，则该属性代表该玩家建造的道路信息。
        /// </summary>
        public Road Road { get; private set; }

        /// <summary>
        /// 如果玩家此回合进行了研发，则该属性代表该玩家所研发的科技类型以及个数。
        /// </summary>
        public List<(IReadOnlyTechNode Tech, ushort Count)> Techs { get; private set; } 

        /// <summary>
        /// 如果玩家此回合进行了售卖操作，则该属性代表该玩家所有的售卖操作。List中的一个元素代表将一个建筑上的物品卖掉的售卖操作。
        /// </summary>
        public List<SellContext> SellContexts { get; private set; }

        /// <summary>
        /// 普通的构造函数，只需给定一个操作。
        /// </summary>
        /// <param name="player"></param>
        /// <param name="operation"></param>
        public PlayerOperationContext(PlayerEnum player, Operation operation)
        {
            Player = player;
            Operation = operation;
        }

        /// <summary>
        /// 创建建筑操作
        /// </summary>
        /// <param name="player"></param>
        /// <param name="building"></param>
        public PlayerOperationContext(PlayerEnum player, Building building) : this(player, Operation.CreateBuilding)
        {
            Building = building;
        }

        /// <summary>
        /// 创建道路操作
        /// </summary>
        /// <param name="player"></param>
        /// <param name="road"></param>
        public PlayerOperationContext(PlayerEnum player, Road road) : this(player, Operation.CreateRoad)
        {
            Road = road;
        }

        /// <summary>
        /// 科研操作
        /// </summary>
        /// <param name="player"></param>
        /// <param name="techs"></param>
        public PlayerOperationContext(PlayerEnum player, List<(IReadOnlyTechNode Tech, ushort Count)> techs) : this(player, Operation.Upgrade)
        {
            Techs = techs;
        }

        /// <summary>
        /// 卖商品操作
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sellContexts"></param>
        public PlayerOperationContext(PlayerEnum player, List<SellContext> sellContexts) : this(player, Operation.Sell)
        {
            SellContexts = sellContexts;
        }
    }
}