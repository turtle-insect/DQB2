using System;
using System.Globalization;
using System.Windows.Data;

namespace CMNDAT
{
	internal class MaterialIslandStateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint state = (uint)value;
			return state / 2;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int)value * 2;
		}
	}
}
