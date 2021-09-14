using System.Collections.ObjectModel;

namespace STGDAT
{
	class ViewModel
	{
		public Info Info { get; private set; } = Info.Instance();
		public MapGenerator Map { get; set; } = new MapGenerator();
		public ObservableCollection<Strage> Boxes { get; set; } = new ObservableCollection<Strage>();
		public ObservableCollection<Strage> Cabinets { get; set; } = new ObservableCollection<Strage>();
		public ObservableCollection<Strage> ShelfChests { get; set; } = new ObservableCollection<Strage>();
		public ObservableCollection<Strage> ShelfDrawers{ get; set; } = new ObservableCollection<Strage>();

		public uint Heart
		{
			get { return SaveData.Instance().ReadNumber(0xC0ECC, 4); }
			set { Util.WriteNumber(0xC0ECC, 4, value, 0, 99999); }
		}

		public ViewModel()
		{
			for (uint i = 0; i < 32; i++)
			{
				Boxes.Add(new Strage(0xF565 + i * 8, 0x2467CC + i * 120));
			}

			for (uint i = 0; i < 16; i++)
			{
				Cabinets.Add(new Strage(0xFF75 + i * 8, 0x248C24 + i * 120));
			}

			for (uint i = 0; i < 16; i++)
			{
				ShelfChests.Add(new Strage(0x1027D + i * 8, 0x24A9B4 + i * 120));
			}

			for (uint i = 0; i < 16; i++)
			{
				ShelfDrawers.Add(new Strage(0x102FD + i * 8, 0x24B134 + i * 120));
			}
		}

		public void AllStorageUnActive()
		{
			foreach (var item in Boxes) item.Clear();
			foreach (var item in Cabinets) item.Clear();
			foreach (var item in ShelfChests) item.Clear();
			foreach (var item in ShelfDrawers) item.Clear();
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


		// 利用可能な家具
		// 1つ8Byte
		// 0埋めで無し

		// 食器(1288)
		// 最大32個
		// 0xFC75

		// 銅ジョッキ(2207)
		// 最大32個(256)
		// 0x103FD

		// 収納系
		// 一つの収納に30個のアイテム
		// ■収納箱(2045)＋収納ロッカー(121)＋がらくた倉庫(1379)
		// 最大32個
		// 0xF565
		// 中身
		// 0x2467CC
		// ■大きなクローゼット(1380)＋タンス+キャビネット(1302)
		// 最大16個
		// 0xFF75
		// 中身
		// 0x248C24
		// ■たなのタンス(2229)
		// 最大16個
		// 0x1027D
		// 中身
		// 0x24A9B4
		// ■たなのひきだし(2230)
		// 最大16個
		// 0x102FD
		// 中身
		// 0x24B134

		// 食事テーブルの数
		// 0x144E7F

		// ブロック以外のオブジェクト
		// 0x24E7CD : オブジェクトの数 : 3Byte
		// 0x0C7FFF：最大数
		// 1つのオブジェクトは24Byteで表現されている
		// オブジェクトの数を0にするだけでオブジェクトは消える
		// 0x24E7F1から開始

		// 例
		// 0層：岩盤、1層：土、2層：草原の土
		// ブロック以外のオブジェクト無し

		// ?????
		// 0xC8F21
		// 502Byte

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
