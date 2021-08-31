using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace CMNDAT
{
	class Resident : INotifyPropertyChanged
	{
		public uint Address { get; private set; }
		public Item Weapon { get; set; }
		public Item Armor { get; set; }
		public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

		// 212 - 243：キャラスキン？
		// 261 - 266：セリフ？


		public Resident(uint address)
		{
			Address = address;
			Weapon = new Item(address + 199);
			Armor = new Item(address + 207);
			for(uint i = 0; i < 15; i++)
			{
				Items.Add(new Item(address + 32 + i * 4));
			}
		}

		public String Name
		{
			get { return SaveData.Instance().ReadText(Address, 120); }
			set
			{
				SaveData.Instance().WriteText(Address, 120, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
			}
		}

		public uint Sex
		{
			get { return SaveData.Instance().ReadNumber(Address + 258, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 258, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sex)));
			}
		}

		public uint Job
		{
			get { return SaveData.Instance().ReadNumber(Address + 271, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 271, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Job)));
			}
		}

		public uint Island
		{
			get { return SaveData.Instance().ReadNumber(Address + 223, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 223, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Island)));
			}
		}

		public uint Place
		{
			get { return SaveData.Instance().ReadNumber(Address + 324, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 324, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Place)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void Reload()
		{
			Name = Name;
			Sex = Sex;
			Job = Job;
			Island = Island;
			Place = Place;
			Weapon.ID = Weapon.ID;
			Weapon.Count = Weapon.Count;
			Armor.ID = Armor.ID;
			Armor.Count = Armor.Count;
			foreach (var item in Items)
			{
				item.ID = item.ID;
				item.Count = item.Count;
			}
		}
	}
}
