using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STGDAT
{
	internal class BluePrint
	{
		private readonly uint mAddress;

		public BluePrint(uint address)
		{
			mAddress = address;
		}

		public uint ID
		{
			get => SaveData.Instance().ReadNumber(mAddress, 2);
		}

		public int X
		{
			get => (int)SaveData.Instance().ReadNumber(mAddress + 2, 2) - 1024;
		}

		public int Y
		{
			get => (int)SaveData.Instance().ReadNumber(mAddress + 4, 2) - 1024;
		}

		public int Z
		{
			get => (int)SaveData.Instance().ReadNumber(mAddress + 6, 2);
		}
	}
}
