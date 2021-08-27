using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STGDAT
{
	class ViewModel
	{
		public uint Heart
		{
			get { return SaveData.Instance().ReadNumber(0xC0ECC, 4); }
			set { Util.WriteNumber(0xC0ECC, 4, value, 0, 99999); }
		}

		// map info.
		// からっぽ島
		// 0x183FEF0 - 0x5D3FEF0くらいが書き換え可能
		// 座標の考え方
		// x = 32, z = 32, y = 96を一塊にしたチャンク方式
		// yは岩盤地点も含めて保存されている
		// 1つの空間を2Byteで表現している
		// 結果、一つのチャンクを0x30000(96*32*32*2)で表現している
		// 0x00 = 空気(air)
		// 0x01 = 岩盤
		// 0x02 = 土
		// 0x03 = 草原の草
		// 0x0A = こいグレー岩
		// 0x28 = 城のカベ

		// ブロック以外のオブジェクト
		// 0x24E7CD : オブジェクトの数 : 3Byte
		// 0x0C7FFF：最大数
		// 1つのオブジェクトは24Byteで表現されている
		// カウントを0にするだけでオブジェクトは消える
		// 0x24E7F1から開始


		// 例
		// 0層：岩盤、1層：土、2層：草原の土
		// ブロック以外のオブジェクト無し
		/*
		int address = 0x24E7CD;
		buffer[address] = 0;
		buffer[address + 1] = 0;
		buffer[address + 2] = 0;

		for(int ch = 0; ch < 368; ch++)
		{
			address = 0x183FEF0 + ch * 0x30000;
			for(int y = 0; y < 96; y++)
			{
				Byte block = 0;
				if (y == 0) block = 1;
				else if (y == 1) block = 2;
				else if (y == 2) block = 3;

				for(int i = 0; i < 32 * 32; i++)
				{
					buffer[address] = block;
					buffer[address + 1] = 0;
					address += 2;
				}
			}
		}
		*/
	}
}
