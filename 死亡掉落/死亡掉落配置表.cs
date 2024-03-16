using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace 死亡掉落;

internal class 死亡掉落配置表
{
	public class 怪物
	{
		public int 怪物ID { get; set; } = 0;

        public List<int> 掉落物列表 = new List<int>();

        public int 随机掉落数量最小值 { get; set; } = 1;

        public int 随机掉落数量最大值 { get; set; } = 1;

		public int 掉落概率 { get; set; } = 1000;

	}

	public string 说明2 = "需要用到的功能改为true；为确保插件正常运行，请手动将随机掉落物也调0";

	public bool 是否开启随机掉落 = false;
    public bool 完全随机掉落 = false;
    public List<int> 完全随机掉落排除物品ID = new List<int>();

    public List<int> 普通随机掉落物 = new List<int>();
	public int 随机掉落概率 = 100;
    public int 随机掉落数量最小值 = 1;
    public int 随机掉落数量最大值 = 1;
    public int 总掉落百分比 = 100;

    public bool 是否开启自定义掉落 = false;

    private const string path = "tshock/死亡掉落配置表.json";

	public 怪物[] 自定义掉落设置 { get; set; } = new 怪物[1]
	{
		new 怪物()
	};


	public static 死亡掉落配置表 GetConfig()
	{
		死亡掉落配置表 死亡掉落配置表2 = new 死亡掉落配置表();
		if (!File.Exists("tshock/死亡掉落配置表.json"))
		{
			using StreamWriter streamWriter = new StreamWriter("tshock/死亡掉落配置表.json");
			streamWriter.WriteLine(JsonConvert.SerializeObject((object)死亡掉落配置表2, (Formatting)1));
		}
		else
		{
			using StreamReader streamReader = new StreamReader("tshock/死亡掉落配置表.json");
			死亡掉落配置表2 = JsonConvert.DeserializeObject<死亡掉落配置表>(streamReader.ReadToEnd());
		}
		return 死亡掉落配置表2;
	}
}
