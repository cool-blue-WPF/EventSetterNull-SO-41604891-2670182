using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SO_41650679_2670182.Converters
{
	class BoolToVisibilityMultiParamConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Check for design mode.
			if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
			{
				return Visibility.Visible;
			}

			if (value is bool && targetType == typeof(Visibility))
			{
				var arr = parameter as Array;
				if (null != arr && arr.Length == 2)
				{
					bool valueEqTrue = (bool)value;
					if (valueEqTrue)
					{
						return arr.GetValue(0);
					}
					return arr.GetValue(1);
				}
			}
			return Visibility.Visible;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
