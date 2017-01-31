using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Xaml;
using XamlReader = System.Xaml.XamlReader;

namespace EventSetterNull_SO_41604891_2670182
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			//Debug.WriteLine(Logger.LogMemberTry((App)(Application.Current), 
			//	new[] { "ParkingHwnd", "_appIsShutdown", "Events" }));

			DecompileDictionary(new object(), new RoutedEventArgs());
		}

		private void Panel_OnClick(object sender, RoutedEventArgs e)
		{
			var btn = e.OriginalSource as Button;
			var win = (Window) System.Windows.Application.LoadComponent(
				new Uri(btn.Name.Replace("_", "/") + ".xaml", UriKind.Relative));
			win.Show();
		}

		public XamlLoadLogger Logger = new XamlLoadLogger();

		private void DecompileDictionary(object sender, RoutedEventArgs e)
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
			var path =
				System.IO.Path.Combine(System.IO.Path.GetFullPath(@"..\..\obj\Debug"),
					@"BuildInXaml.baml");
			if (!File.Exists(path)) return;
			var source = new FileStream(path, FileMode.Open);
			XamlReaderSettings settings = new XamlReaderSettings
			{
				LocalAssembly = System.Reflection.Assembly.LoadFile(assembly)
			};
			//Baml2006Reader reader = new Baml2006Reader(source, settings);

			//PrintBamlStream(reader);

			//source.Seek(0, SeekOrigin.Begin);
			Window win =
				System.Windows.Markup.XamlReader.Load(new Baml2006Reader(source, settings))
					as Window;
			win.Show();
			source.Close();

			Debug.WriteLine(System.Windows.Markup.XamlWriter.Save(win));
		}

		private void PrintBamlStream(System.Xaml.XamlReader reader, int indent = 0)
		{
			Action increaseIndent = () => indent += 1;
			Action decreaseIndent = () => indent -= 1;
			while (reader.Read())
				try
				{
					Action postIndent;
					var preIndent = postIndent = () =>{};
					var nodeType = reader.NodeType;
					var info = "";
					switch (nodeType)
					{
						case XamlNodeType.StartObject:
							info = reader.Type != null ? reader.Type.Name : null;
							postIndent = increaseIndent;
							break;
						case XamlNodeType.EndObject:
							info = reader.Type != null ? reader.Type.Name : null;
							preIndent = decreaseIndent;
							break;
						case XamlNodeType.GetObject:
							info = reader.SchemaContext?.GetType().ToString();
							postIndent = increaseIndent;
							break;
						case XamlNodeType.StartMember:
						case XamlNodeType.EndMember:
							info = reader.Member != null ? reader.Member.Name : null;
							switch (nodeType)
							{
								case XamlNodeType.StartMember:
									postIndent = increaseIndent;
									break;
								case XamlNodeType.EndMember:
									preIndent = decreaseIndent;
									break;
							}
							break;
						case XamlNodeType.Value:
							info = reader.Value?.ToString();
							break;
						case XamlNodeType.None:
							info = "None";
							break;
						case XamlNodeType.NamespaceDeclaration:
							info = reader.Namespace?.Namespace;
							break;
					}

					preIndent();

					Debug.WriteLine("Line: {0,4}{1}\t{2}\t{3}",
						((System.Xaml.IXamlLineInfo) reader).LineNumber,
						new string('\t', indent), nodeType, info);

					postIndent();

					// todo parse Markup Extensions
					var buffer = reader.Value as MemoryStream;
					if (reader.NodeType == XamlNodeType.GetObject)
					{
						var rootIndent = indent;
						using (XamlReader subReader = reader.ReadSubtree())
						{
							subReader.Read();
							PrintBamlStream(subReader, indent);
						}
						indent = rootIndent;
					}
				}
				catch (Exception exception)
				{
					Debug.WriteLine("{0}\t{1}", reader.NodeType, exception);
				}
		}
	}
}