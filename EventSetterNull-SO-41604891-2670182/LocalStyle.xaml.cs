using System.Diagnostics;
using System.Windows;

namespace EventSetterNull_SO_41604891_2670182
{
	/// <summary>
	/// Interaction logic for LocalStyle.xaml
	/// </summary>
	public partial class LocalStyle : Window
	{
		private void StyleClick (object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("StyleClick");
		}
	}
}
