using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace STGDAT
{
	class Strage : INotifyPropertyChanged
	{
		private readonly uint mAddress;
		public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

		public Strage(uint address, uint inside)
		{
			mAddress = address;
			for (uint i = 0; i < 30; i++)
			{
				Items.Add(new Item(inside + i * 4));
			}
		}

		public void Clear()
		{
			SaveData.Instance().Fill(mAddress, 8, 0);
			foreach (var item in Items)
			{
				item.ID = 0;
				item.Count = 0;
			}
		}

		public bool Active()
		{
			return !(SaveData.Instance().ReadNumber(mAddress, 4) == 0 && SaveData.Instance().ReadNumber(mAddress + 4, 4) == 0);
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
