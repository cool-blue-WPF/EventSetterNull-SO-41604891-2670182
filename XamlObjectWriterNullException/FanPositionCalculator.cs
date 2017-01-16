using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace XamlObjectWriterNullException
{

	public class FanPositionCalculator : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter,
			CultureInfo culture)
		{
			//var list = values?[0] as IList;
			//if(list == null) return DependencyProperty.UnsetValue;
			//int count = list.Count;
			//int itemIndex = list.IndexOf(values[1]);

			int count = (values[0] as IList).Count;
			int itemIndex = (values[0] as IList).IndexOf(values[1]);

			double indexFromCenter = itemIndex - count / 2;

			//multiply by the degrees we want for each card
			return indexFromCenter * 3;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
			CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
