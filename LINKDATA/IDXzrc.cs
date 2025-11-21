using System;
using System.Collections.ObjectModel;

namespace LINKDATA
{
	internal class IDXzrc
	{
		public ObservableCollection<IDXzrcChunk> Chunks { get; private set; } = new();
		public UInt32 SplitSize { get; private set; }
		public UInt32 UncompressedSize { get; private set; }
		public void Read(String filename)
		{
			Chunks.Clear();
			if (!System.IO.File.Exists(filename)) return;

			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			if (BitConverter.ToUInt32(buffer, 0) == 0) return;

			SplitSize = BitConverter.ToUInt32(buffer, 0);
			UInt32 blockCount = BitConverter.ToUInt32(buffer, 4);
			UncompressedSize = BitConverter.ToUInt32(buffer, 8);
			UInt32 offset = 0x0C + blockCount * 4;
			for (int count = 0; count < blockCount; count++)
			{
				if (offset % 0x80 != 0) offset = (offset / 0x80 + 1) * 0x80;
				UInt32 size = BitConverter.ToUInt32(buffer, 0x0C + count * 4);
				var block = new IDXzrcChunk(size, offset);
				Chunks.Add(block);
				offset += size;
			}
		}
	}
}
