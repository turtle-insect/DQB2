using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNDAT
{
	class Util
	{
		public const uint CraftAddress = 0x22C5C0;

		public const uint BluePrintSize = 0x30008;
		public const uint PeopleSize = 608;

		public const uint ResidentAddress = 0x102A68;
		public const uint ResidentCount = 60;

		public const uint StoryPeopleAddress = 0x6ACC8;
		public const uint StoryPeopleCount = 1023;

		public static void WriteNumber(uint address, uint size, uint value, uint min, uint max)
		{
			if (value < min) value = min;
			if (value > max) value = max;
			SaveData.Instance().WriteNumber(address, size, value);
		}
	}
}
