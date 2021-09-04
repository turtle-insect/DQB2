using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace STGDAT
{
	class MapGenerator
	{
		public bool AllObjectClear { get; set; } = false;
		public ObservableCollection<Layer> Layers { get; set; } = new ObservableCollection<Layer>();

		public MapGenerator()
		{
			// テンプレとして生成.
			Layers.Add(new Layer() { Height = 93, Block = 0, Option = 0 });
			Layers.Add(new Layer() { Height = 1, Block = 3, Option = 0 });
			Layers.Add(new Layer() { Height = 1, Block = 2, Option = 0 });
			Layers.Add(new Layer() { Height = 1, Block = 1, Option = 0 });
		}

		public void Clear()
		{
			Layers.Clear();
		}

		public void RemoveAt(int index)
		{
			if (Layers.Count <= index) return;
			Layers.RemoveAt(index);
		}

		public void Front(int index)
		{
			if (index <= 0) return;
			Layer layer = Layers.ElementAt(index);
			Layers.RemoveAt(index);
			Layers.Insert(index - 1, layer);
		}

		public void Back(int index)
		{
			if (index >= Layers.Count - 1) return;
			Layer layer = Layers.ElementAt(index);
			Layers.RemoveAt(index);
			Layers.Insert(index + 1, layer);
		}

		public void Append()
		{
			Layers.Insert(0, new Layer() { Height = 1, Block = 0, Option = 0 });
		}

		public void Execution()
		{
			if (AllObjectClear)
			{
				SaveData.Instance().WriteNumber(0x24E7CD, 3, 0);
				// 収納箱
				//SaveData.Instance().WriteNumber(0x28708, 1, 0);
				//SaveData.Instance().Fill(0xF565, 264, 0x00);
			}

			List<uint> BlockList = new List<uint>();
			for (int index = Layers.Count - 1; index >= 0; index--)
			{
				for (int y = 0; BlockList.Count < 96 && y < Layers[index].Height; y++)
				{
					BlockList.Add(Layers[index].Block);
				}
			}

			for (uint ch = 0;; ch++)
			{
				uint address = 0x183FEF0 + ch * 0x30000;
				if (SaveData.Instance().Length() < address + 0x30000) break;

				foreach (var block in BlockList)
				{
					for (int i = 0; i < 32 * 32; i++)
					{
						SaveData.Instance().WriteNumber(address, 2, block);
						address += 2;
					}
				}
			}
		}
	}
}
