using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNDAT
{
	internal class Player
	{
		public String Name
		{
			get { return SaveData.Instance().ReadText_Header(0xCD, 12); }
			set { SaveData.Instance().WriteText_Header(0xCD, 12, value); }
		}

		public uint Lv
		{
			get { return SaveData.Instance().ReadNumber(0x6A9CF, 1); }
			set { Util.WriteNumber(0x6A9CF, 1, value, 1, 99); }
		}

		public uint Exp
		{
			get { return SaveData.Instance().ReadNumber(0x6A9D1, 4); }
			set { Util.WriteNumber(0x6A9D1, 4, value, 0, 9999999); }
		}

		public uint HP
		{
			get { return SaveData.Instance().ReadNumber(0x6A890, 2); }
			set { Util.WriteNumber(0x6A890, 2, value, 1, 999); }
		}

		public uint HPPlus
		{
			get { return SaveData.Instance().ReadNumber(0x6A892, 2); }
			set { Util.WriteNumber(0x6A892, 2, value, 0, 999); }
		}

		public uint Hungry
		{
			get { return SaveData.Instance().ReadNumber(0x6A896, 2) / 100; }
			set { Util.WriteNumber(0x6A896, 2, value * 100, 0, 10000); }
		}

		public uint Attack
		{
			get { return SaveData.Instance().ReadNumber(0x6A898, 2); }
			set { Util.WriteNumber(0x6A898, 2, value, 1, 9999); }
		}

		public uint Defense
		{
			get { return SaveData.Instance().ReadNumber(0x6A89A, 2); }
			set { Util.WriteNumber(0x6A89A, 2, value, 1, 9999); }
		}

		public Item Weapon { get; private set; } = new Item(0x55B959);
		public Item Armor { get; private set; } = new Item(0x55B989);
		public Item Shield { get; private set; } = new Item(0x55B985);
		public Item Hammer { get; private set; } = new Item(0x55B95D);
	}
}
