using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace EventSetterNull_SO_41604891_2670182
{
	/// <summary>
	/// Interaction logic for BuildInCLR.xaml
	/// </summary>
	public partial class BuildInCLR : Window
	{
		public BuildInCLR()
		{
			InitializeComponent();

			var setterKey = "ButtonStyleSetters";
			var setters = new SetterBaseCollection
			{
				new EventSetter
				{
					Event = ButtonBase.ClickEvent,
					Handler = Handlers.StyleClick
				},
				new Setter {Property = FrameworkElement.HeightProperty, Value = 30d}
			};

			this.Resources.Add(setterKey, setters);

			var button1 = new Button();

			button1.SetValue(Behaviours.StyleSettersProperty, this.Resources[setterKey]);
			this.Content = button1;

			Debug.Print(XamlWriter.Save(this));
		}

		private void StyleClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("StyleClick");
		}
	}
}