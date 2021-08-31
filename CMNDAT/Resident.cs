using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CMNDAT
{
	class Resident : INotifyPropertyChanged
	{
		public uint Address { get; private set; }
		public Item Weapon { get; set; }
		public Item Armor { get; set; }

		// 258 : 性別
		// 199 : 武器 ID:2 Count:2
		// 207 : 服 ID:2 Count:2

		public Resident(uint address)
		{
			Address = address;
			Weapon = new Item(address + 199);
			Armor = new Item(address + 207);
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
			Island = Island;
			Place = Place;
			Weapon.ID = Weapon.ID;
			Weapon.Count = Weapon.Count;
			Armor.ID = Weapon.ID;
			Armor.Count = Weapon.Count;
		}
	}
}
