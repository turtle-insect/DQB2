using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQB2
{
	class MaterialIsland
	{
		private readonly uint mAddress;

		public MaterialIsland(uint address, String name)
		{
			mAddress = address;
			Name = name;
		}

		public String Name { get; private set; }

		public uint State
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 1) & 0x6; }
			set
			{
				uint tmp = SaveData.Instance().ReadNumber(mAddress, 1) & 0xF9;
				tmp += value;
				SaveData.Instance().WriteNumber(mAddress, 1, tmp);
			}
		}
	}
}
