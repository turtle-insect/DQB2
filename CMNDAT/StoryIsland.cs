using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNDAT
{
    class StoryIsland
    {
        private readonly uint mAddress;
        private readonly String mName;

        public StoryIsland(uint address, String name)
        {
            mAddress = address;
            mName = name;
        }

        public String Name
        {
            get => mName;
        }

        public bool Map
        {
            get => SaveData.Instance().ReadBit(mAddress, 0);
            set => SaveData.Instance().WriteBit(mAddress, 0, value);
        }

		public bool Move
		{
			get => SaveData.Instance().ReadBit(mAddress, 1);
			set => SaveData.Instance().WriteBit(mAddress, 1, value);
		}

		public bool New
		{
			get => SaveData.Instance().ReadBit(mAddress, 2);
			set => SaveData.Instance().WriteBit(mAddress, 2, value);
		}

		public bool Clear
		{
			get => SaveData.Instance().ReadBit(mAddress, 3);
			set => SaveData.Instance().WriteBit(mAddress, 3, value);
		}
	}
}
