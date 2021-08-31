using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

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
		public ObservableCollection<Resident> Residents { get; set; } = new ObservableCollection<Resident>();
		public ObservableCollection<Party> Party { get; set; } = new ObservableCollection<Party>();
		public ObservableCollection<BluePrint> BluePrints { get; set; } = new ObservableCollection<BluePrint>();


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

			for (uint i = 0; i < 4; i++)
			{
				BluePrints.Add(new BluePrint(0x166DF0 + i * Util.BluePrintSize));
			}

			// 住人
			// 0x102A68
			// 608Byte
			for (int i = 0; i < 60; i++)
			{
				Residents.Add(new Resident((uint)(0x102A68 + i * Util.ResidentSize)));
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
	}
}
