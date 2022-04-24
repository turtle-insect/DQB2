using System;
using System.Globalization;
using System.Windows.Data;

namespace CMNDAT
{
	internal class IslandIDConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			for (int i = 0; i < Info.Instance().StoryIsland.Count; i++)
			{
				if (id == Info.Instance().StoryIsland[i].Value) return i;
			}

			return -1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Info.Instance().StoryIsland[(int)value].Value;
		}
	}
}
