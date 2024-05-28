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
		public int PosX { get; }
		public uint PosY { get; }
		public int PosZ { get; }

		public Part(uint baseAddress, uint chunkAddress)
		{
			ItemID = SaveData.Instance().ReadNumber(baseAddress + 8, 1);
			var tmp = SaveData.Instance().ReadNumber(baseAddress + 9, 1) & 0x1F;
			ItemID += tmp * 256;

			var dx = SaveData.Instance().ReadNumber(baseAddress + 9, 1) >> 5;
			tmp = SaveData.Instance().ReadNumber(baseAddress + 10, 1) & 0x3;
			dx += tmp << 3;

			PosY = SaveData.Instance().ReadNumber(baseAddress + 10, 1) >> 2;
			tmp = SaveData.Instance().ReadNumber(baseAddress + 11, 1) & 0x1;
			PosY += tmp << 6;

			var dz = (SaveData.Instance().ReadNumber(baseAddress + 11, 1) & 0x3E);
			dz >>= 1;

			var chunkID = SaveData.Instance().ReadNumber(chunkAddress, 1);
			chunkID += (SaveData.Instance().ReadNumber(chunkAddress + 1, 1) & 0x0F) << 8;

			// map's width = 0x24E7C9：2Byte？
			// map's height = 0x24E7CB：2Byte？
			// origin x and z = (0x800 / 2) * -1 => range = (-1024 ~ 1024)
			PosX = -1024 + (int)((chunkID % 64) * 32 + dx);
			PosZ = -1024 + (int)(chunkID / 64 * 32 + dz);
		}
	}
}
