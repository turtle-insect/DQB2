﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQB2
{
	class Skill
	{
		public bool Bag
		{
			get { return SaveData.Instance().ReadNumber(0x635, 1) == 1; }
			set { SaveData.Instance().WriteNumber(0x635, 1, value == true ? 1u : 0); }
		}

		public bool WindCape
		{
			get { return SaveData.Instance().ReadNumber(0x6A8A2, 1) == 2; }
			set { SaveData.Instance().WriteNumber(0x6A8A2, 1, value == true ? 2u : 0); }
		}

		public bool Hammer3x
		{
			get { return SaveData.Instance().ReadBit(0x506, 1); }
			set { SaveData.Instance().WriteBit(0x506, 1, value); }
		}

		public bool Hammer5x
		{
			get { return SaveData.Instance().ReadBit(0x502, 3); }
			set { SaveData.Instance().WriteBit(0x502, 3, value); }
		}

		public bool ReformIron
		{
			get { return SaveData.Instance().ReadBit(0x500, 6); }
			set { SaveData.Instance().WriteBit(0x500, 6, value); }
		}

		public bool Expression
		{
			get { return !SaveData.Instance().ReadBit(0x501, 1); }
			set { SaveData.Instance().WriteBit(0x501, 1, !value); }
		}

		public bool ThirstPotUse
		{
			get { return SaveData.Instance().ReadBit(0x504, 2); }
			set { SaveData.Instance().WriteBit(0x504, 2, value); }
		}

		public bool ThirstPot
		{
			get { return SaveData.Instance().ReadNumber(0x67D, 1) == 0xDE; }
			set { SaveData.Instance().WriteNumber(0x67D, 1, value == true ? 0xDEu : 0); }
		}

		public bool BuilderEye
		{
			get { return SaveData.Instance().ReadBit(0x502, 7); }
			set { SaveData.Instance().WriteBit(0x502, 7, value); }
		}
	}
}
