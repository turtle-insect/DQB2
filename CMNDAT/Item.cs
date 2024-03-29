﻿using System.ComponentModel;

namespace CMNDAT
{
	internal class Item : INotifyPropertyChanged
	{
		readonly uint mAddress;

		public bool mCountForce { get; set; } = true;

		public Item(uint address)
		{
			mAddress = address;
		}

		public uint ID
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
			}
		}

		public uint Count
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 2, 2); }
			set
			{
				Util.WriteNumber(mAddress + 2, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
