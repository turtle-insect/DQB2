﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DQB2
{
	class Item : INotifyPropertyChanged
	{
		readonly uint mAddress;

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
			set { Util.WriteNumber(mAddress + 2, 2, value, 0, 999); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
