using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace STGDAT
{
	internal class ViewModel
	{
		public Info Info { get; private set; } = Info.Instance();
		public MapGenerator Map { get; private set; } = new();

		public ICommand CommandImportFile { get; init; }
		public ICommand CommandExportFile { get; init; }
		public ICommand CommandImportOtherMapFile { get; init; }
		public ICommand CommandImportDLMapFile { get; init; }
		public ICommand CommandChoiceItem { get; init; }
		public ICommand CommandMapGeneratorUp { get; init; }
		public ICommand CommandMapGeneratorDown { get; init; }
		public ICommand CommandMapGeneratorAppend { get; init; }
		public ICommand CommandMapGeneratorRemove { get; init; }
		public ICommand CommandMapGeneratorClear { get; init; }
		public ICommand CommandMapGeneratorExecute { get; init; }

		public ObservableCollection<Strage> Boxes { get; init; } = new();
		public ObservableCollection<Strage> Cabinets { get; init; } = new();
		public ObservableCollection<Strage> ShelfChests { get; init; } = new();
		public ObservableCollection<Strage> ShelfDrawers { get; init; } = new();
		public ObservableCollection<Tableware> Tablewares { get; init; } = new();
		public ObservableCollection<Craft> Crafts { get; init; } = new();
		public ObservableCollection<Entity> Entitys { get; init; } = new();
		public ObservableCollection<BluePrint> BluePrints { get; init; } = new();

		public string FilterEntityID
		{
			get => field;
			set
			{
				field = value;
				FilterPart();
			}
		} = String.Empty;

		private List<Entity> mEntity = new();

		public uint Heart
		{
			get { return SaveData.Instance().ReadNumber(0xC0ECC, 4); }
			set { Util.WriteNumber(0xC0ECC, 4, value, 0, 99999); }
		}

		public ViewModel()
		{
			CommandImportFile = new CommandAction(ImportFile);
			CommandExportFile = new CommandAction(ExportFile);
			CommandImportOtherMapFile = new CommandAction(ImportOtherMapFile);
			CommandImportDLMapFile = new CommandAction(ImportDLMapFile);
			CommandChoiceItem = new CommandAction(ChoiceItem);
			CommandMapGeneratorUp = new CommandAction(MapGeneratorUp);
			CommandMapGeneratorDown = new CommandAction(MapGeneratorDown);
			CommandMapGeneratorAppend = new CommandAction(MapGeneratorAppend);
			CommandMapGeneratorRemove = new CommandAction(MapGeneratorRemove);
			CommandMapGeneratorClear = new CommandAction(MapGeneratorClear);
			CommandMapGeneratorExecute = new CommandAction(MapGeneratorExecute);

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
				var part = new Entity(0x24E7D1 + i * 24, 0x150E7D1 + i * 4);
				mEntity.Add(part);
				Entitys.Add(part);
			}

			for (uint i = 0; i < 5; i++)
			{
				BluePrints.Add(new BluePrint(0x2CA3C + i * 12));
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

		public void AllTablewareUnActive()
		{
			foreach (var item in Tablewares) item.Clear();
		}

		// room
		// 最大100個
		// 0x10
		// 1つ336Byte

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

		// ?????
		// 0xC8F21
		// 502Byte

		// Entity
		// 0x24E7CD : オブジェクトの数 : 3Byte
		// 0x0C7FFF：最大数
		// 0x24E7D1から開始？
		// 1つのオブジェクトは24Byteで表現されている
		// オブジェクトの数を0にするだけでオブジェクトは消える
		//
		// Chunk Index
		// https://github.com/default-kramer/HermitsHeresy/discussions/3#discussioncomment-10952063
		// map = 64(x) * 64(z) = 4096 Chunk
		// 0x24C7C1から開始
		// 1つのチャンクは2Byteで表現されている
		// 無効なChunkは0xFFFF
		// 有効なChunkは0始まりで1加算
		// IDは連続した整数である必要がある
		// マップによって最大チャンク数が設定されている？
		// = 既存のファイルサイズ分のChunk以上の生成は出来ない？
		// Chunk自体をIDとして認識させても、Y座標が0のBlockID = 0のChunkは操作出来ない？
		// 0x24E7C5(2Byte) Chunk Count？
		// 0x24E7C9(2Byte) X Max Block Count？ (64 * 32)
		// 0x24E7CB(2Byte) Z Max Block Count？ (64 * 32)
		/*
		// Chunk extension
		// filename is 『STGDAT01.BIN』
		String filename = @"*********";
		Byte[] buffer = System.IO.File.ReadAllBytes(filename);
		// append Chunk ID
		buffer[0x24DC7F] = 0x71;
		buffer[0x24DC80] = 0x01;

		// change Chunk Count
		buffer[0x24E7C5] = 0x72;
		// buffer[0x24E7C6] = 0x01;

		// ??? Chunk's block region ???
		// ??? file size +0x30000{(x=32,z=32,y=96) * 2Byte} ???
		*/

		// Chunk Entity
		// 0x150E7D1から開始？
		// 1つのチャンクは4Byteで表現されている
		// ID：0Byte + (1Byte & 0xF) << 8
		// Index：1Byte >> 4 + 2Byte << 4 + 3Byte << 12
		// X+=32：0x01増える
		// Z+=32：0x40増える
		// X = 0, Z = 0, id = 0x820

		// File Size
		// 前提
		// セーブデータの容量割り当てにはミスがある様に見える
		// 具体的には
		// (EOF - 0x183FEF0) / 0x30000 ≠ 0
		// (EOF - 0x183FEF0 + 0x110) / 0x30000 = 0
		// ファイルサイズが0x110(272)Byte小さく設定されている
		// mistake？
		// 0x183FEF0 + (Chunk Count + (Chunk Count == 700 ? 0 : 1 )) * 0x30000 - 0x110

		// Block
		// 0x183FEF0 - EOF
		// 座標の考え方
		// x = 32, z = 32, y = 96を一塊にしたチャンク方式
		// yは岩盤地点も含めて保存されている
		// 1つの空間を2Byteで表現している
		// 結果、一つのチャンクを0x30000(96*32*32*2)で表現している
		// 例
		// 0層：岩盤、1層：土、2層：草原の土
		// ブロック以外のオブジェクト無し
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

		private void ImportFile(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Import(dlg.FileName);
		}

		private void ExportFile(Object? parameter)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Export(dlg.FileName);
		}

		private void ImportOtherMapFile(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().OtherMap(dlg.FileName);
		}

		private void ImportDLMapFile(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().DLMap(dlg.FileName);
		}

		private void ChoiceItem(Object? obj)
		{
			Item? item = obj as Item;
			if (item == null) return;

			var window = new ChoiceWindow();
			window.ID = item.ID;
			window.ShowDialog();
			item.ID = window.ID;

			item.Count = item.ID == 0 ? 0 : 1u;
		}

		private void MapGeneratorUp(Object? obj)
		{
			Map.Front(MapGeneratorIndex(obj));
        }

		private void MapGeneratorDown(Object? obj)
		{
			Map.Back(MapGeneratorIndex(obj));
		}

		private void MapGeneratorAppend(Object? obj)
		{
			Map.Append();
		}

		private void MapGeneratorRemove(Object? obj)
		{
			Map.RemoveAt(MapGeneratorIndex(obj));
		}

		private void MapGeneratorClear(Object? obj)
		{
			Map.Clear();
		}

		private void MapGeneratorExecute(Object? obj)
		{
			Map.Execution();
		}

		private int MapGeneratorIndex(Object? obj)
		{
			if (obj == null) return -1;

			var str = obj.ToString();
			if (str == null) return -1;

			int index = int.Parse(str);
			return index;
		}

		private void FilterPart()
		{
			Entitys.Clear();
			uint id;
			uint.TryParse(FilterEntityID, out id);

			foreach (var entity in mEntity)
			{
				if (string.IsNullOrEmpty(FilterEntityID) || entity.ItemID == id) Entitys.Add(entity);
			}
		}
	}
}
