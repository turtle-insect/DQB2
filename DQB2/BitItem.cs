using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQB2
{
	class BitItem
	{
		private readonly uint mAddress;
		private readonly uint mBit;

		public BitItem(uint address, uint bit)
		{
			mAddress = address;
			mBit = bit;
		}

		public bool Value
		{
			get { return SaveData.Instance().ReadBit(mAddress, mBit); }
			set { SaveData.Instance().WriteBit(mAddress, mBit, value); }
		}
	}
}
