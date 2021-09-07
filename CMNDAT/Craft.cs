using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CMNDAT
{
	class Craft : INotifyPropertyChanged
	{
		private readonly uint mAddress;

		public Craft(uint address, uint id)
		{
			mAddress = address;
			ID = id;
		}

		public uint ID { get; private set; }

		public bool Recipe
		{
			get { return SaveData.Instance().ReadBit(mAddress, 0); }
			set
			{
				SaveData.Instance().WriteBit(mAddress, 0, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Recipe)));
			}
		}

		public bool Build
		{
			get { return SaveData.Instance().ReadBit(mAddress, 1); }
			set
			{
				SaveData.Instance().WriteBit(mAddress, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Build)));
			}
		}

		public bool New
		{
			get { return !SaveData.Instance().ReadBit(mAddress, 2); }
			set
			{
				SaveData.Instance().WriteBit(mAddress, 2, !value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(New)));
			}
		}

		public bool Infinite
		{
			get { return SaveData.Instance().ReadBit(mAddress, 4); }
			set
			{
				SaveData.Instance().WriteBit(mAddress, 4, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Infinite)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
