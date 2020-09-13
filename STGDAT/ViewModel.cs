using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STGDAT
{
	class ViewModel
	{
		public uint Heart
		{
			get { return SaveData.Instance().ReadNumber(0xC0ECC, 4); }
			set { Util.WriteNumber(0xC0ECC, 4, value, 0, 99999); }
		}
	}
}
