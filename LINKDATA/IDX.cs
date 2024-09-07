using System;

namespace LINKDATA
{
	internal class IDX
	{
		public UInt64 Offset { get; set; }
		public UInt64 UncompressedSize { get; set; }
		public UInt64 CompressedSize { get; set; }
		public UInt64 IsCompressed { get; set; }
		public int GameIndex { get; set; } = -1;

		public int Index { get; private set; }

		public IDX(int index) => Index = index;
	}
}
