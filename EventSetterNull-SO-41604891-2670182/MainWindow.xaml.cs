using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventSetterNull_SO_41604891_2670182
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow ()
		//public MyStyleSetters setters { get; set; }
		{
			//setters = new MyStyleSetters();

			InitializeComponent();

			//setters.Add(new Setter
			//{
			//	Property = FrameworkElement.HeightProperty,
			//	Value = this.Resources["ButtonHeight"]
			//});

			//setters.Add(new Setter
			//{
			//	Property = FrameworkElement.MarginProperty,
			//	Value = this.Resources["ButtonMargin"]
			//});

			//setters.Add(new EventSetter
			//{
			//	Event = ButtonBase.ClickEvent,
			//	Handler = (RoutedEventHandler)StyleClick
			//});

		}

		private void StyleClick (object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("StyleClick");
		}
	}
}
