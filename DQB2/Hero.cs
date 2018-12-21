using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQB2
{
	class Hero
	{
		public String Name
		{
			get { return SaveData.Instance().ReadText(0xCD, 12); }
			set { SaveData.Instance().WriteText(0xCD, 12, value); }
		}
	}
}
