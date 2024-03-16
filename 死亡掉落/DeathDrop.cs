using System;
using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace DeathDrop
{
    [ApiVersion(2, 1)]
    public class DeathDrop : TerrariaPlugin
    {
        public static Random RandomGenerator = new Random();

        public override string Author => "大豆子，肝帝熙恩更新优化";

        public override string Description => "自定义怪物死亡随机掉落物";

        public override string Name => "死亡随机掉落";

        public override Version Version => new Version(1, 0, 1);

        public DeathDrop(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            ServerApi.Hooks.NpcKilled.Register(this, NPCDead);
        }

        private void NPCDead(NpcKilledEventArgs args)
        {
            int npcNetID = args.npc.netID;
            Vector2 npcPosition = args.npc.position;
            TSPlayer player = args.npc.lastInteraction != 255 && args.npc.lastInteraction >= 0
                ? TShock.Players[args.npc.lastInteraction]
                : null;

            try
            {
                DeathDropConfig config = DeathDropConfig.GetConfig();

                if (config.EnableRandomDrops)
                {
                    int itemId;
                    if (config.FullRandomDrops)
                    {
                        // 从1到5452中随机选择一个物品ID
                        itemId = RandomGenerator.Next(1, 5453);
                        // 检查这个ID是否在排除列表中
                        while (config.FullRandomExcludedItems.Contains(itemId))
                        {
                            // 如果是在排除列表中，则重新选择
                            itemId = RandomGenerator.Next(1, 5453);
                        }
                    }
                    else
                    {
                        // 否则从配置的随机掉落物列表中随机选择一个物品 ID
                        int randomIndex = RandomGenerator.Next(config.CommonRandomDrops.Count);
                        itemId = config.CommonRandomDrops[randomIndex];
                    }
                    if (Candorp(config.RandomDropChance))
                    {
                        Item item = TShock.Utils.GetItemById(itemId);

                            int dropAmount = RandomGenerator.Next(config.MinRandomDropAmount, config.MaxRandomDropAmount); // 随机生成1到物品最大堆叠数量的物品数量
                        int itemNumber = Item.NewItem(
                            null,
                            (int)args.npc.position.X,
                            (int)args.npc.position.Y,
                            item.width,
                            item.height,
                            item.type,
                            dropAmount
                        );
                        if (player != null)
                        {
                            player.SendData(PacketTypes.SyncExtraValue, null, itemNumber);
                            player.SendData(PacketTypes.ItemOwner, null, itemNumber);
                            player.SendData(PacketTypes.TweakItem, null, itemNumber, 255f, 63f);
                        }
                    }
                }

                if (!config.EnableCustomDrops)
                {
                    return;
                }

                foreach (DeathDropConfig.Monster monster in config.DeathDropSet)
                {
                    if (monster.NPCID == npcNetID && Candorp(monster.DropChance))
                    {
                        int dropItemId;
                        if (monster.FullRandomDrops)
                        {
                            // 从1到5452中随机选择一个物品ID
                            dropItemId = RandomGenerator.Next(1, 5453);
                            // 检查这个ID是否在排除列表中
                            while (monster.FullRandomExcludedItems.Contains(dropItemId))
                            {
                                // 如果是在排除列表中，则重新选择
                                dropItemId = RandomGenerator.Next(1, 5453);
                            }
                        }
                        else
                        {
                            // 否则从配置的随机掉落物列表中随机选择一个物品 ID
                            int randomIndex = RandomGenerator.Next(monster.CommonRandomDrops.Count);
                            dropItemId = monster.CommonRandomDrops[randomIndex];
                        }
                        // 从怪物.掉落物列表中随机选择一个物品 ID
                        Item dropItem = TShock.Utils.GetItemById(dropItemId);
                        int dropAmount = RandomGenerator.Next(monster.RandomDropMinAmount, monster.RandomDropMaxAmount + 1);
                        int dropItemNumber = Item.NewItem(
                            null,
                            (int)npcPosition.X,
                            (int)npcPosition.Y,
                            dropItem.width,
                            dropItem.height,
                            dropItem.type,
                            dropAmount
                        );
                        if (player != null)
                        {
                            player.SendData(PacketTypes.SyncExtraValue, null, dropItemNumber);
                            player.SendData(PacketTypes.ItemOwner, null, dropItemNumber);
                            player.SendData(PacketTypes.TweakItem, null, dropItemNumber, 255f, 63f);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"死亡掉落插件发生异常：{ex.Message}");
            }
        }

        public static bool Candorp(int probability)
        {
            return RandomGenerator.Next(100) <= probability;
        }
    }
}
