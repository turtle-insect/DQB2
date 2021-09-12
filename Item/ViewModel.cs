using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
