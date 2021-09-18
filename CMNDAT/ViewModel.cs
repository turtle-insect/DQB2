using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CMNDAT
{
	class ViewModel
	{
		public Info Info { get; private set; } = Info.Instance();
		public Player Player { get; set; } = new Player();
		public Skill Skill { get; set; } = new Skill();
		public ObservableCollection<Item> Inventory { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Bag { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<MaterialIsland> Island { get; set; } = new ObservableCollection<MaterialIsland>();
		public ObservableCollection<Pelple> Residents { get; set; } = new ObservableCollection<Pelple>();
		public ObservableCollection<Pelple> StoryPeople { get; set; } = new ObservableCollection<Pelple>();
		public ObservableCollection<Party> Party { get; set; } = new ObservableCollection<Party>();
		public ObservableCollection<BluePrint> BluePrints { get; set; } = new ObservableCollection<BluePrint>();
		public ObservableCollection<Craft> Crafts { get; set; } = new ObservableCollection<Craft>();
		public ObservableCollection<Crop> Crops { get; set; } = new ObservableCollection<Crop>();
		public ObservableCollection<Scenery> Sceneries { get; set; } = new ObservableCollection<Scenery>();

		// サムネ
		// 0x10D - 0x2A40F

		// シドー
		// 武器
		// 0x6AD8F(2)

		// 野菜
		// 収穫した数(4)
		// 収穫ステータス(4)
		//   収穫可能:0x0C
		//   収穫済み:0x0D
		// 
		// 小麦：0x22D568(4)
		// キャベツ：0x22D578(4)
		// ネギ：0x22D598(4)
		// カボチャ：0x22D5C0(4)
		// トマト：0x22D5C8(4)

		// 赤の設計図：0x22CD2D
		// 未作成：0x00
		// 作成済み：0x03

		// クラフトテーブル new
		// 590  0x227C9F アイスキャンディ
		// 1420 0x2271DE 城の矢よけ石
		// 2125 0x227DB3 八尺玉
		// 2389 0x227D0E 城のまどかざり

		// ビルダー100景
		// ジメジメ：0x230B6D - 0x230BD0
		// 100Byte
		// 未訪問:0x00
		// 訪問済み:0x01

		public ViewModel()
		{
			for (uint i = 0; i < 15; i++)
			{
				Inventory.Add(new Item(0x55B28D + i * 4));
			}

			for (uint i = 0; i < 420; i++)
			{
				Bag.Add(new Item(0x55B2C9 + i * 4));
			}

			for(uint i = 0; i < Info.MaterialIsland.Count; i++)
			{
				Island.Add(new MaterialIsland(0x69F8D + i * 7, Info.MaterialIsland[(int)i].Name));
			}

			for(uint i = 0; i < 3; i++)
			{
				Party.Add(new Party(0x6A9DC + i * 4));
			}

			var brushs = new List<Brush>{
				new SolidColorBrush(Colors.White),
				new SolidColorBrush(Colors.LightPink),
				new SolidColorBrush(Colors.LightBlue),
				new SolidColorBrush(Colors.LightGreen),
				new SolidColorBrush(Colors.LightYellow),
			};
			for (uint i = 0; i < brushs.Count; i++)
			{
				BluePrints.Add(new BluePrint(0x136DE8 + i * Util.BluePrintSize, brushs[(int)i]));
			}
			// move online blue print
			BluePrints.Move(0, 4);

			// ストーリーで出会うキャラクタ？
			CreateStoryPeople();

			// 住人
			CreateResident();

			// クラフト
			CreateCraft();

			// 収穫物
			for (int i = 0; i < Info.Crop.Count; i++)
			{
				Crops.Add(new Crop(0x22D568 + Info.Crop[i].Value * 8, Info.Crop[i].Value));
			}

			// ビルダー100景
			for (uint i = 0; i < 100; i++)
			{
				Sceneries.Add(new Scenery(0x230B6D + i));
			}
		}

		public int StoryPeopleFilter { get; set; }
		public int ResidentFilter { get; set; }
		public int ResidentExist { get; set; }
		public String CraftNameFilter { get; set; }

		public uint From
		{
			get { return SaveData.Instance().ReadNumber_Header(0xC9, 1); }
			set { SaveData.Instance().WriteNumber_Header(0xC9, 1, value); }
		}

		public uint To
		{
			get { return SaveData.Instance().ReadNumber_Header(0xC8, 1); }
			set { SaveData.Instance().WriteNumber_Header(0xC8, 1, value); }
		}

		public uint MiniMedal
		{
			get { return SaveData.Instance().ReadNumber(0x226E40, 1); }
			set { Util.WriteNumber(0x226E40, 1, value, 0, 90); }
		}

		public uint MiniMedal_deposit
		{
			get { return SaveData.Instance().ReadNumber(0x226E44, 1); }
			set { Util.WriteNumber(0x226E44, 1, value, 0, 90); }
		}

		public bool Car_Light
		{
			get { return SaveData.Instance().ReadBit(0x506, 5); }
			set { SaveData.Instance().WriteBit(0x506, 5, value); }
		}

		public bool Car_Fly
		{
			get { return SaveData.Instance().ReadBit(0x506, 6); }
			set { SaveData.Instance().WriteBit(0x506, 6, value); }
		}

		public bool Car_Beam
		{
			get { return SaveData.Instance().ReadBit(0x506, 7); }
			set { SaveData.Instance().WriteBit(0x506, 7, value); }
		}

		public BitmapSource Thumbnail
		{
			get
			{
				int size = 320 * 180 * 3;
				Byte[] pixel = new Byte[size];
				
				for(int i = 0; i < size; i++)
				{
					pixel[i] = (Byte)SaveData.Instance().ReadNumber_Header(0x10D + (uint)i, 1);
				}
				BitmapSource thumbnail = BitmapSource.Create(320, 180, 96, 96, PixelFormats.Bgr24, null, pixel, 960);
				return thumbnail;
			}
		}

		public bool MaterialBonusCord
		{
			get { return SaveData.Instance().ReadBit(0x22C75A, 4); }
			set { SaveData.Instance().WriteBit(0x22C75A, 4, value); }
		}

		public bool MaterialBonusGrassFibre
		{
			get { return SaveData.Instance().ReadBit(0x22CD8A, 4); }
			set { SaveData.Instance().WriteBit(0x22CD8A, 4, value); }
		}

		public bool MaterialBonusWood
		{
			get { return SaveData.Instance().ReadBit(0x22C757, 4); }
			set { SaveData.Instance().WriteBit(0x22C757, 4, value); }
		}

		public bool MaterialBonusDryGrass
		{
			get { return SaveData.Instance().ReadBit(0x22CD3E, 4); }
			set { SaveData.Instance().WriteBit(0x22CD3E, 4, value); }
		}

		 // かいたく島の地図表示
		public bool PioneerLand
		{
			get { return SaveData.Instance().ReadBit(0x22E785, 3); }
			set { SaveData.Instance().WriteBit(0x22E785, 3, value); }
		}


		public void CreateStoryPeople()
		{
			StoryPeople.Clear();
			uint filter = Info.StoryIsland[StoryPeopleFilter].Value;
			for (uint i = 0; i < Util.StoryPeopleCount; i++)
			{
				var item = new Pelple(Util.StoryPeopleAddress + i * Util.PeopleSize, i + 1);
				if (StoryPeopleFilter == 0 || item.Island == filter)
				{
					StoryPeople.Add(item);
				}
			}
		}

		public void CreateResident()
		{
			Residents.Clear();
			uint filter = Info.StoryIsland[ResidentFilter].Value;
			for (uint i = 0; i < Util.ResidentCount; i++)
			{
				var item = new Pelple(Util.ResidentAddress + i * Util.PeopleSize, i + 1024);
				if (ResidentFilter == 0 || item.Island == filter)
				{
					if(ResidentExist == 0 ||
						(ResidentExist == 1 && item.Exist() == true) ||
						(ResidentExist == 2 && item.Exist() == false)
						)
					Residents.Add(item);
				}
			}
		}

		public void CreateCraft()
		{
			Crafts.Clear();
			for (uint i = 0; i < Info.Instance().Item.Count; i++)
			{
				var item = Info.Instance().Item[(int)i];
				if (item.Value == 0) continue;

				if (String.IsNullOrEmpty(CraftNameFilter) || item.Name.IndexOf(CraftNameFilter) >= 0)
				{
					Crafts.Add(new Craft(Util.CraftAddress + item.Value, item.Value));
				}
			}
		}
	}
}
