using System.Windows;

namespace SO_41650679_2670182
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Activities CurrentActivities { get; set; }

		public MainWindow ()
		{
			CurrentActivities = new Activities
			{
				IsIndeterminate = true
			};
			InitializeComponent();
		}

	}
		public class Activities
		{
			private bool _isIndeterminate;

			public bool IsIndeterminate	
			{
				get { return _isIndeterminate; }
				set { _isIndeterminate = value; }
			}
		}
}
