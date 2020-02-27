using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DQB2
{
	class ViewModel
	{
		public ObservableCollection<Item> Inventory { get; set; } = new ObservableCollection<Item>();
		public Player Player { get; set; } = new Player();

		public ViewModel()
		{
			for (uint i = 0; i < 15; i++)
			{
				Inventory.Add(new Item(0x55B28D + i * 4));
			}
		}
	}
}
