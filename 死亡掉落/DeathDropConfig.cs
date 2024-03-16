using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TShockAPI;

namespace DeathDrop;

internal class DeathDropConfig
{
	public class Monster
	{
		[JsonProperty("生物id")]
		public int NPCID { get; set; } = 0;
        [JsonProperty("完全随机掉落")]
        public bool FullRandomDrops { get; set; } = false;
        [JsonProperty("完全随机掉落排除物品ID")]
        public List<int> FullRandomExcludedItems { get; set; } = new List<int>();
        [JsonProperty("普通随机掉落物")]
        public List<int> CommonRandomDrops { get; set; } = new List<int>();
        [JsonProperty("随机掉落数量最小值")]
        public int RandomDropMinAmount { get; set; } = 1;
        [JsonProperty("随机掉落数量最大值")]
        public int RandomDropMaxAmount { get; set; } = 1;
        [JsonProperty("掉落概率")]
        public int DropChance { get; set; } = 100;

    }
    [JsonProperty("是否开启随机掉落")]
    public bool EnableRandomDrops { get; set; } = false;
    [JsonProperty("完全随机掉落")]
    public bool FullRandomDrops { get; set; } = false;
    [JsonProperty("完全随机掉落排除物品ID")]
    public List<int> FullRandomExcludedItems { get; set; } = new List<int>();
    [JsonProperty("普通随机掉落物")]
    public List<int> CommonRandomDrops { get; set; } = new List<int>();
    [JsonProperty("随机掉落概率")]
    public int RandomDropChance { get; set; } = 100;
    [JsonProperty("随机掉落数量最小值")]
    public int MinRandomDropAmount { get; set; } = 1;
    [JsonProperty("随机掉落数量最大值")]
    public int MaxRandomDropAmount { get; set; } = 1;
    [JsonProperty("是否开启自定义掉落")]
    public bool EnableCustomDrops { get; set; } = false;

    [JsonProperty("自定义掉落设置")]
    public Monster[] DeathDropSet { get; set; } = new Monster[1]
{
        new Monster()
};
    public static readonly string ConfigFilePath = Path.Combine(TShock.SavePath, "死亡掉落配置表.json");
    public static DeathDropConfig GetConfig()
    {
        DeathDropConfig DeathDropConfig2 = new DeathDropConfig();
        if (!File.Exists(ConfigFilePath))
        {
            using StreamWriter streamWriter = new StreamWriter(ConfigFilePath);
            streamWriter.WriteLine(JsonConvert.SerializeObject((object)DeathDropConfig2, (Formatting)1));
        }
        else
        {
            using StreamReader streamReader = new StreamReader(ConfigFilePath);
            DeathDropConfig2 = JsonConvert.DeserializeObject<DeathDropConfig>(streamReader.ReadToEnd());
        }
        return DeathDropConfig2;
    }
}
