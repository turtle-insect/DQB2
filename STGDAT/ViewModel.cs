namespace STGDAT
{
	class ViewModel
	{
		public Info Info { get; private set; } = Info.Instance();
		public MapGenerator Map { get; set; } = new MapGenerator();
		public uint Heart
		{
			get { return SaveData.Instance().ReadNumber(0xC0ECC, 4); }
			set { Util.WriteNumber(0xC0ECC, 4, value, 0, 99999); }
		}

		// map info.
		// からっぽ島
		// 0x183FEF0 - 0x86DFEF0くらいが書き換え可能そう
		// = 590チャンク
		// 横 = 可変
		// かいたく島(大きい)
		// 0x183FEF0 - 0x3B0FEF0くらいが書き換え可能そう
		// = 185チャンク
		// 座標の考え方
		// x = 32, z = 32, y = 96を一塊にしたチャンク方式
		// yは岩盤地点も含めて保存されている
		// 1つの空間を2Byteで表現している
		// 結果、一つのチャンクを0x30000(96*32*32*2)で表現している

		// 『xxxのはた』のドット
		// 0x357DDDA
		// x = 16, y = 16

		// 収納箱の数
		// 0x28708

		// ブロック以外のオブジェクト
		// 0x24E7CD : オブジェクトの数 : 3Byte
		// 0x0C7FFF：最大数
		// 1つのオブジェクトは24Byteで表現されている
		// オブジェクトの数を0にするだけでオブジェクトは消える
		// 0x24E7F1から開始


		// 例
		// 0層：岩盤、1層：土、2層：草原の土
		// ブロック以外のオブジェクト無し
		/*
		String filename = @"*********";
		Byte[] buffer = System.IO.File.ReadAllBytes(filename);
		int address = 0x24E7CD;
		//buffer[address] = 0;
		//buffer[address + 1] = 0;
		//buffer[address + 2] = 0;

		for (int ch = 0; ch < 185; ch++)
		{
			address = 0x183FEF0 + ch * 0x30000;
			for (int y = 0; y < 96; y++)
			{
				Byte block = 0;
				if (y == 0) block = 1;
				else if (y == 1) block = 2;
				else if (y == 2) block = 3;

				for (int i = 0; i < 32 * 32; i++)
				{
					buffer[address] = block;
					buffer[address + 1] = 0;
					address += 2;
				}
			}
		}
		System.IO.File.WriteAllBytes(filename + "_", buffer);
		*/
	}
}
