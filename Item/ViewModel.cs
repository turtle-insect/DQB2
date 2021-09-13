using System;
using System.Collections.ObjectModel;

namespace Item
{
	class ViewModel
	{
		public String ItemFilterWord { get; set; }
		public ObservableCollection<uint> Items { get; set; } = new ObservableCollection<uint>();

		public ViewModel()
		{
			CreateItemList();
		}

		public void CreateItemList()
		{
			Items.Clear();
			foreach(var info in Info.Instance().Item)
			{
				if (info.Value == 0) continue;
				if (String.IsNullOrEmpty(ItemFilterWord) || info.Name.IndexOf(ItemFilterWord) >= 0)
				{
					Items.Add(info.Value);
				}
			}
		}
	}
}
