using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CMNDAT
{
	class Scenery : INotifyPropertyChanged
	{
		private readonly uint mAddress;

		public Scenery(uint address)
		{
			mAddress = address;
		}

		public bool Visit
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 1) == 1; }
			set
			{
				SaveData.Instance().WriteNumber(mAddress, 1, value == true ? 1u : 0);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visit)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
