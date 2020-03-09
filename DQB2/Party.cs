using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQB2
{
	class Party
	{
		private readonly uint mAddress;

		public Party(uint address)
		{
			mAddress = address;
		}

		public uint ID
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress, 2, value);
				SaveData.Instance().WriteNumber(mAddress + 2, 1, value == 0 ? 0 : 1u);
			}
		}
	}
}
