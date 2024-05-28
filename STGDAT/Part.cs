using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace STGDAT
{
	internal class Part
	{
		public uint ItemID { get; }
		public uint PosX { get; }
		public uint PosY { get; }
		public uint PosZ { get; }

		public Part(uint address)
		{
			ItemID = SaveData.Instance().ReadNumber(address + 8, 1);
			var tmp = SaveData.Instance().ReadNumber(address + 9, 1) & 0x1F;
			ItemID += tmp * 256;

			PosX = SaveData.Instance().ReadNumber(address + 9, 1) >> 5;
			tmp = SaveData.Instance().ReadNumber(address + 10, 1) & 0x3;
			PosX += tmp << 3;

			PosY = SaveData.Instance().ReadNumber(address + 10, 1) >> 2;
			tmp = SaveData.Instance().ReadNumber(address + 11, 1) & 0x1;
			PosY += tmp << 6;

			PosZ = (SaveData.Instance().ReadNumber(address + 11, 1) & 0x3E);
			PosZ >>= 1;
		}
	}
}
