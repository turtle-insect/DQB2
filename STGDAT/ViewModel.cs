using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace STGDAT
{
	internal class ViewModel
	{
		public Info Info { get; private set; } = Info.Instance();
		public MapGenerator Map { get; private set; } = new MapGenerator();
		public ObservableCollection<Strage> Boxes { get; private set; } = new ObservableCollection<Strage>();
		public ObservableCollection<Strage> Cabinets { get; private set; } = new ObservableCollection<Strage>();
		public ObservableCollection<Strage> ShelfChests { get; private set; } = new ObservableCollection<Strage>();
		public ObservableCollection<Strage> ShelfDrawers { get; private set; } = new ObservableCollection<Strage>();
		public ObservableCollection<Tableware> Tablewares { get; private set; } = new ObservableCollection<Tableware>();
		public ObservableCollection<Craft> Crafts { get; private set; } = new ObservableCollection<Craft>();
		public ObservableCollection<Part> Parts { get; private set; } = new ObservableCollection<Part>();
		public string FilterPartID { get; set; } = "";
		private List<Part> mPart = new List<Part>();

		public uint Heart
		{
			get { return SaveData.Instance().ReadNumber(0xC0ECC, 4); }
			set { Util.WriteNumber(0xC0ECC, 4, value, 0, 99999); }
		}

		public ViewModel()
		{
			FilterPartID = "";
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

			for (uint i = 0; i < 32; i++)
			{
				Tablewares.Add(new Tableware(0xFC75 + i * 8, 0x24893C + i * 4));
			}

			for (uint i = 0; i < 128; i++)
			{
				Crafts.Add(new Craft(0x10EA5 + i * 52));
			}

			uint count = SaveData.Instance().ReadNumber(0x24E7CD, 3);
			for (uint i = 1; i < count; i++)
			{
				var part = new Part(0x24E7D1 + i * 24, 0x150E7D1 + i * 4);
				mPart.Add(part);
				Parts.Add(part);
			}
		}

		public void AllStorageUnActive()
		{
			foreach (var item in Boxes) item.Clear();
			foreach (var item in Cabinets) item.Clear();
			foreach (var item in ShelfChests) item.Clear();
			foreach (var item in ShelfDrawers) item.Clear();
			SaveData.Instance().WriteNumber(0x28708, 4, 0);
		}

		public void FilterPart()
		{
			Parts.Clear();

			uint id;
			if (uint.TryParse(FilterPartID, out id) == false) return;

			foreach (var part in mPart)
			{
				if (part.ItemID == id) Parts.Add(part);
			}
		}

		public void AllTablewareUnActive()
		{
			foreach (var item in Tablewares) item.Clear();
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

		// 超スーパーカー
		// 0x13D8D
		// 1つ10Byte


		// 『xxxのはた』のドット
		// 0x357DDDA
		// x = 16, y = 16

		// たき火等
		// 1つ52Byte
		// 0埋めで無し
		// 最大128個
		// 0x10EA5

		// 利用可能な家具
		// 1つ8Byte
		// 0埋めで無し

		// 食器(1288)
		// 最大32個
		// 0xFC75
		// 中身
		// 0x24893C

		// 銅ジョッキ(2207)
		// 最大32個(256)
		// 0x103FD
		// 中身
		// 0x24C034

		// 収納系
		// 0x28708：マップ内の個数：1Byte
		// 一つの収納に30個のアイテム
		// ■収納箱(2045)＋収納ロッカー(121)＋がらくた倉庫(1379)
		// 最大32個
		// 0xF565：8Byte
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
		// 0x24E7D0から開始？
		// チャンクは0x150E7D1から開始？
		// 1つのチャンクは4Byteで表現されている
		// ID：0Byte + (1Byte & 0xF) << 8
		// Index：1Byte >> 4 + 2Byte << 4 + 3Byte << 12
		// X+=32：0x01増える
		// Z+=32：0x40増える
		// X = 0, Z = 0, id = 0x820

		// 例
		// 0層：岩盤、1層：土、2層：草原の土
		// ブロック以外のオブジェクト無し

		// ?????
		// 0xC8F21
		// 502Byte

		/*
		// filename is 『STGDATxx.BIN』
		String filename = @"*********";
		Byte[] buffer = System.IO.File.ReadAllBytes(filename);
		int address = 0x24E7CD;

		// If you want to clear objects, enable it.
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
