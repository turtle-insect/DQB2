using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINKDATA
{
	internal class IDXzrcChunk
	{
		public UInt32 FileSize { get; private set; }
		public UInt32 Offset { get; private set; }

		public IDXzrcChunk(UInt32 size, UInt32 offset)
		{
			FileSize = size;
			Offset = offset;
		}
	}
}
