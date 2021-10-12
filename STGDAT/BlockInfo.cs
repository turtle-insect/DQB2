using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STGDAT
{
	class BlockInfo : ILineAnalysis, IComparable
	{
		public uint Value { get; private set; }
		public String Name { get; private set; }

		public int CompareTo(object obj)
		{
			var dist = obj as NameValueInfo;
			if (dist == null) return 0;

			if (Value < dist.Value) return -1;
			else if (Value > dist.Value) return 1;
			else return 0;
		}

		public virtual bool Line(String[] oneLine)
		{
			if (oneLine[0].Length > 1 && oneLine[0][1] == 'x') Value = Convert.ToUInt32(oneLine[0], 16);
			else Value = Convert.ToUInt32(oneLine[0]);
			if (Value != 0) return false;

			if (oneLine[1].Length > 1 && oneLine[1][1] == 'x') Value = Convert.ToUInt32(oneLine[1], 16);
			else Value = Convert.ToUInt32(oneLine[1]);
			Name = oneLine[2];
			if (String.IsNullOrEmpty(Name)) return false;
			return true;
		}
	}
}
