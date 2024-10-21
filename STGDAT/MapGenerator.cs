using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace STGDAT
{
	internal class MapGenerator
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
			if (index < 0) return;
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
			if (index < 0) return;
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
				SaveData.Instance().WriteNumber(0x24E7CD, 3, 1);
				SaveData.Instance().Fill(0x24E7D1, 24 * 0xC8000, 0);
				for (uint i = 0; i < 0xC8000; i++)
				{
					SaveData.Instance().WriteNumber(0x24E7D1 + i * 24 + 12, 3, i * 16);
				}
				//SaveData.Instance().WriteNumber(0x28708, 1, 0);
				//SaveData.Instance().Fill(0xF565, 264, 0x00);
			}

			List<uint> BlockList = new List<uint>();
			for (int layerIndex = Layers.Count - 1; layerIndex >= 0; layerIndex--)
			{
				for (int y = 0; BlockList.Count < 96 && y < Layers[layerIndex].Height; y++)
				{
					BlockList.Add(Layers[layerIndex].Block);
				}
			}

			var chunkCount = SaveData.Instance().ReadNumber(0x24E7C5, 2);
			for (uint chunkIndex = 0; chunkIndex < chunkCount; chunkIndex++)
			{
				uint address = Util.CalcChunkAddress(chunkIndex);
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
