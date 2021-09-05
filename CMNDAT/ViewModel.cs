using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		// トマト：0x22D57C8(4)

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
				new SolidColorBrush(Colors.LightPink),
				new SolidColorBrush(Colors.LightBlue),
				new SolidColorBrush(Colors.LightGreen),
				new SolidColorBrush(Colors.LightYellow),
			};
			for (uint i = 0; i < brushs.Count; i++)
			{
				BluePrints.Add(new BluePrint(0x166DF0 + i * Util.BluePrintSize, brushs[(int)i]));
			}

			// ストーリーで出会うキャラクタ？
			for (uint i = 0; i < Util.StoryPeopleCount; i++)
			{
				StoryPeople.Add(new Pelple(Util.StoryPeopleAddress + i * Util.PeopleSize));
			}

			// 住人
			for (uint i = 0; i < Util.ResidentCount; i++)
			{
				Residents.Add(new Pelple(Util.ResidentAddress + i * Util.PeopleSize));
			}
		}

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
	}
}
