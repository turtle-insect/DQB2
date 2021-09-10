using System;
using System.Collections.ObjectModel;

namespace CMNDAT
{
	class MaterialIsland
	{
		private readonly uint mAddress;
		public ObservableCollection<BitItem> Info { get; set; } = new ObservableCollection<BitItem>();

		public MaterialIsland(uint address, String name)
		{
			mAddress = address;
			Name = name;

			for(uint i = 0; i < 48; i++)
			{
				Info.Add(new BitItem(address + i / 8, i % 8));
			}
		}

		public String Name { get; private set; }

		public uint State
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 6, 1) & 0x6; }
			set
			{
				uint tmp = SaveData.Instance().ReadNumber(mAddress + 6, 1) & 0xF9;
				tmp += value;
				SaveData.Instance().WriteNumber(mAddress + 6, 1, tmp);
			}
		}
	}
}
