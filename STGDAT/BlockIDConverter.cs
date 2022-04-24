using System;
using System.Globalization;
using System.Windows.Data;

namespace STGDAT
{
	internal class BlockIDConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			for (int i = 0; i < Info.Instance().Block.Count; i++)
			{
				if (Info.Instance().Block[i].Value == id) return i;
			}

			return -1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int id = (int)value;
			return Info.Instance().Block[id].Value;
		}
	}
}
