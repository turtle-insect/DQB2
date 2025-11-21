using System;

namespace LINKDATA
{
	internal class IDXzrcChunk
	{
		public UInt32 Size { get; init; }
		public UInt32 Offset { get; init; }

		public IDXzrcChunk(UInt32 size, UInt32 offset)
		{
			Size = size;
			Offset = offset;
		}
	}
}
