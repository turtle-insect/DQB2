using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINKDATA
{
	internal class IDXzrc
	{
		public ObservableCollection<IDXzrcChunk> Chunks { get; private set; } = new ObservableCollection<IDXzrcChunk>();
		public UInt32 UncompressedSize { get; private set; }
		public void Create(String filename)
		{
			if (!System.IO.File.Exists(filename)) return;

			Chunks.Clear();
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			UInt32 blockCount = BitConverter.ToUInt32(buffer, 4);
			UInt32 offset = 0xC + blockCount * 4;
			for (int count = 0; count < blockCount; count++)
			{
				if (offset % 0x80 != 0) offset = (offset / 0x80 + 1) * 0x80;
				UInt32 size = BitConverter.ToUInt32(buffer, 0xC + count * 4);
				var block = new IDXzrcChunk(size, offset);
				Chunks.Add(block);
				offset += size;
			}
		}
	}
}
