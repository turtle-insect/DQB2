﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;

namespace CMNDAT
{
	class BluePrint : INotifyPropertyChanged
	{
		public uint Address { get; private set; }
		public Brush Brush { get; private set; }

		public BluePrint(uint address, Brush brush)
		{
			Address = address;
			Brush = brush;
		}

		public uint X
		{
			get { return SaveData.Instance().ReadNumber(Address + 0x30000, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 0x30000, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X)));
			}
		}

		public uint Y
		{
			get { return SaveData.Instance().ReadNumber(Address + 0x30002, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 0x30002, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y)));
			}
		}

		public uint Z
		{
			get { return SaveData.Instance().ReadNumber(Address + 0x30004, 2); }
			set
			{
				SaveData.Instance().WriteNumber(Address + 0x30004, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Z)));
			}
		}

		public void Reload()
		{
			X = X;
			Y = Y;
			Z = Z;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}