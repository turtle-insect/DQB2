using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CMNDAT
{
	internal class People : INotifyPropertyChanged
	{
		public uint Address { get; private set; }
		public Item Weapon { get; set; }
		public Item Armor { get; set; }
		public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

		// Address[Byte] : Name
		// 229[2]:顔(顔の色、目の形、耳の形)
		// 231[2]:髪型(0x19:青兵士、0x14:ボブ)
		// 233[2]:体(0x69:青兵士、0x42:バニーガール)
		// 235[2]:目の色
		// 237[2]:髪の色
		// 239[2]:肌の色
		// 266[1]:Message Type
		// 267[1]:Voice Type

		// 部屋の好み
		// 263[1]:ひろさ
		// 264[1]:ごうかさ
		// 265[1]:ムード
		// ムード
		//   キュート = 1
		//   クール = 2
		//   ナチュラル = 3
		//   ビビット = 4
		//   えっち = 5
		//   ノーマル = 6

		// フラグ
		// 259[1:1]:戦闘参加可否
		// 301[1:1]:名前命名有無
		// 307[1:1]:装備可否
		// 302[1:4]:グラフィックロック

		public People(uint address, uint id)
		{
			Address = address;
			ID = id;
			Weapon = new Item(address + 199);
			Armor = new Item(address + 207);
			for (uint i = 0; i < 15; i++)
			{
				Items.Add(new Item(address + 32 + i * 4));
			}
		}

		public uint ID { get; private set; }

		public String Name
		{
			get { return SaveData.Instance().ReadText(Address, 30); }
			set
			{
				SaveData.Instance().WriteText(Address, 30, value);
				SaveData.Instance().WriteBit(Address + 301, 7, !String.IsNullOrEmpty(value));
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

		public uint Type
		{
			get { return SaveData.Instance().ReadNumber(Address + 144, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 144, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
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

		public uint Home
		{
			get { return SaveData.Instance().ReadNumber(Address + 275, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 275, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Home)));
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
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VoiceType)));
			}
		}

		public bool LockGraphic
		{
			get { return ! SaveData.Instance().ReadBit(Address + 302, 4); }
			set
			{
				SaveData.Instance().WriteBit(Address + 302, 4, ! value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LockGraphic)));
			}
		}

		public uint RoomSize
		{
			get { return SaveData.Instance().ReadNumber(Address + 263, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 263, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomSize)));
			}
		}

		public uint RoomGorgeous
		{
			get { return SaveData.Instance().ReadNumber(Address + 264, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 264, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomGorgeous)));
			}
		}

		public uint RoomMood
		{
			get { return SaveData.Instance().ReadNumber(Address + 265, 1); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 265, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomMood)));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public void Reload()
		{
			Name = Name;
			Sex = Sex;
			HP = HP;
			Job = Job;
			Type = Type;
			Equipment = Equipment;
			Battle = Battle;
			Home = Home;
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

			LockGraphic = LockGraphic;
			FaceType = FaceType;
			HeadType = HeadType;
			BodyType = BodyType;
			EyeColor = EyeColor;
			HairColor = HairColor;
			SkinColor = SkinColor;
			MessageType = MessageType;
			VoiceType = VoiceType;

			RoomSize = RoomSize;
			RoomGorgeous = RoomGorgeous;
			RoomMood = RoomMood;
		}

		public bool Exist()
		{
			if (!String.IsNullOrEmpty(Name)) return true;
			if (Sex != 0) return true;
			if (HP != 0) return true;
			if (Job != 0) return true;
			if (Type != 0) return true;
			if (Home != 0) return true;
			if (Island != 0) return true;
			if (Place != 0) return true;
			if (Weapon.ID != 0) return true;
			if (Weapon.Count != 0) return true;
			if (Armor.ID != 0) return true;
			if (Armor.Count != 0) return true;
			foreach (var item in Items)
			{
				if (item.ID != 0) return true;
				if (item.Count != 0) return true;
			}
			if (FaceType != 0) return true;
			if (HeadType != 0) return true;
			if (BodyType != 0) return true;
			if (EyeColor != 0) return true;
			if (HairColor != 0) return true;
			if (SkinColor != 0) return true;
			if (MessageType != 0) return true;
			if (VoiceType != 0) return true;

			if (RoomSize != 0) return true;
			if (RoomGorgeous != 0) return true;
			if (RoomMood != 0) return true;

			return false;
		}
	}
}
