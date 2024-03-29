﻿using System.Collections.ObjectModel;

namespace STGDAT
{
	internal class Strage
	{
		private readonly uint mAddress;
		public ObservableCollection<Item> Items { get; private set; } = new ObservableCollection<Item>();

		public Strage(uint info, uint inside)
		{
			mAddress = info;
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
	}
}
