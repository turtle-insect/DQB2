using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DQB2
{
	class ViewModel
	{
		public Info Info { get; private set; } = Info.Instance();
		public Player Player { get; set; } = new Player();
		public Skill Skill { get; set; } = new Skill();
		public ObservableCollection<Item> Inventory { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Bag { get; set; } = new ObservableCollection<Item>();

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
	}
}
