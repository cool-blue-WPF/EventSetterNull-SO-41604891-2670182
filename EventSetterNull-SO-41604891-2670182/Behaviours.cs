using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace EventSetterNull_SO_41604891_2670182
{
	public static class Behaviours
	{
		#region AP StyleSetters

		public static readonly DependencyProperty StyleSettersProperty =
			DependencyProperty.RegisterAttached(
				"StyleSetters", typeof(SetterBaseCollection),
				typeof(Behaviours),
				new PropertyMetadata(default(SetterBaseCollection),
					ButtonSettersChanged));

		private static void ButtonSettersChanged (DependencyObject d,
			DependencyPropertyChangedEventArgs args)
		{
			var fe = d as FrameworkElement;
			if (fe == null) return;
			var ui = d as UIElement;

			var newValue = args.NewValue as SetterBaseCollection;
			if (newValue != null)
			{
				foreach (var member in newValue)
				{
					var setter = member as Setter;
					if(setter != null)
					{
						fe.SetValue(setter.Property, setter.Value);
						continue;
					}
					var eventSetter = member as EventSetter;
					if (eventSetter == null) continue;
					if (ui == null || eventSetter.Event == null) continue;
					ui.AddHandler(eventSetter.Event, eventSetter.Handler);
				}
			}
		}

		public static void SetStyleSetters(DependencyObject element,
			SetterBaseCollection value)
		{
			element.SetValue(StyleSettersProperty, value);
		}

		public static SetterBaseCollection GetStyleSetters (
			DependencyObject element)
		{
			return (SetterBaseCollection)element
				.GetValue(StyleSettersProperty);
		}

		#endregion
	}

	public class MyStyleSetters : List<SetterBase>
	{
	}

	public class TestList : List<string>
	{
		
	}
}