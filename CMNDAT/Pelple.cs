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
	class Pelple : INotifyPropertyChanged
	{
		public uint Address { get; private set; }
		public Item Weapon { get; set; }
		public Item Armor { get; set; }
		public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

		// 229:顔(2)(顔の色、目の形、耳の形)
		// 231:髪型(2)(0x19:青兵士、0x14:ボブ)
		// 233:体(2)(0x69:青兵士、0x42:バニーガール)
		// 235:目の色(2)
		// 237:髪の色(2)
		// 239:肌の色(2)
		// 266:Message Type(1)
		// 267:Voice Type(1)
		// 301(7):名前命名


		public Pelple(uint address)
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
			get { return SaveData.Instance().ReadText(Address, 30); }
			set
			{
				SaveData.Instance().WriteText(Address, 30, value);
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

		public uint HP
		{
			get { return SaveData.Instance().ReadNumber(Address + 146, 2); }
			set
			{
				Util.WriteNumber(Address + 146, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HP)));
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

		public bool Equipment
		{
			get { return !SaveData.Instance().ReadBit(Address + 307, 1); }
			set
			{
				SaveData.Instance().WriteBit(Address + 307, 1, !value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Equipment)));
			}
		}

		public bool Battle
		{
			get { return !SaveData.Instance().ReadBit(Address + 259, 1); }
			set
			{
				SaveData.Instance().WriteBit(Address + 259, 1, !value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Battle)));
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

		public uint FaceType
		{
			get { return SaveData.Instance().ReadNumber(Address + 229, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 229, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FaceType)));
			}
		}

		public uint HeadType
		{
			get { return SaveData.Instance().ReadNumber(Address + 231, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 231, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadType)));
			}
		}

		public uint BodyType
		{
			get { return SaveData.Instance().ReadNumber(Address + 233, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 233, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BodyType)));
			}
		}

		public uint EyeColor
		{
			get { return SaveData.Instance().ReadNumber(Address + 235, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 235, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EyeColor)));
			}
		}

		public uint HairColor
		{
			get { return SaveData.Instance().ReadNumber(Address + 237, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 237, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HairColor)));
			}
		}

		public uint SkinColor
		{
			get { return SaveData.Instance().ReadNumber(Address + 239, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 239, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SkinColor)));
			}
		}

		public uint MessageType
		{
			get { return SaveData.Instance().ReadNumber(Address + 266, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 266, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageType)));
			}
		}

		public uint VoiceType
		{
			get { return SaveData.Instance().ReadNumber(Address + 267, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 267, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageType)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void Reload()
		{
			Name = Name;
			Sex = Sex;
			HP = HP;
			Job = Job;
			Equipment = Equipment;
			Battle = Battle;
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

			FaceType = FaceType;
			HeadType = HeadType;
			BodyType = BodyType;
			EyeColor = EyeColor;
			HairColor = HairColor;
			SkinColor = SkinColor;
			MessageType = MessageType;
			VoiceType = VoiceType;
		}
	}
}
