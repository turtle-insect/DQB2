using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CMNDAT
{
	class Resident
	{
		private readonly uint mAddress;
		public Item Weapon { get; set; }
		public Item Dress { get; set; }

		// 258 : 性別
		// 199 : 武器 ID:2 Count:2
		// 207 : 服 ID:2 Count:2

		public Resident(uint address)
		{
			mAddress = address;
			Weapon = new Item(address + 199);
			Dress = new Item(address + 207);
		}

		public uint Sex
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 258, 1); }
			set { SaveData.Instance().WriteNumber(mAddress + 258, 1, value); }
		}
	}
}
