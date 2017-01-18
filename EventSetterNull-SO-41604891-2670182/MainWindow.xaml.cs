using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Xaml;
using static System.Diagnostics.Debug;
using static System.Xaml.XamlNodeType;

namespace EventSetterNull_SO_41604891_2670182
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow ()
		{
			InitializeComponent();
		}

		private void Panel_OnClick (object sender, RoutedEventArgs e)
		{
			Button btn = e.OriginalSource as Button;
			var win = (Window)System.Windows.Application.LoadComponent(
				new Uri(btn.Name.Replace("_", "/") + ".xaml", UriKind.Relative));
			win.Show();
		}

		private void DecompileDictionary(object sender, RoutedEventArgs e)
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
			var path = System.IO.Path.Combine(System.IO.Path.GetFullPath(@"..\..\obj\Debug"),
													@"BuildInXaml.baml");
			if (!File.Exists(path)) return;
			var source = new FileStream(path, FileMode.Open);
			XamlReaderSettings settings = new XamlReaderSettings
			{
				LocalAssembly = System.Reflection.Assembly.LoadFile(assembly)
			};
			Baml2006Reader reader = new Baml2006Reader(source, settings);

			PrintBamlStream(reader, settings);

			Window win = System.Windows.Markup.XamlReader.Load(reader) as Window;

			WriteLine(System.Windows.Markup.XamlWriter.Save(win));
		}

		private void PrintBamlStream(XamlReader reader, XamlReaderSettings settings)
		{
			var indent = 0;
			Action preIndent, postIndent;
			Action increaseIndent = () => indent += 1;
			Action decreaseIndent = () => indent -= 1;
			while (reader.Read())
				try
				{
					var nodeType = reader.NodeType;
					var info = "";
					preIndent = postIndent = null;
					switch (nodeType)
					{
						case StartObject:
						case EndObject:
							info = reader.Type?.Name;
							switch (nodeType)
							{
								case StartObject:
									postIndent = increaseIndent;
									break;
								case EndObject:
									preIndent = decreaseIndent;
									break;
							}
							break;
						case GetObject:
							info = reader.Member?.Name;
							postIndent = increaseIndent;
							break;
						case StartMember:
						case EndMember:
							info = reader.Member?.Name;
							switch (nodeType)
							{
								case StartMember:
									postIndent = increaseIndent;
									break;
								case EndMember:
									preIndent = decreaseIndent;
									break;
							}
							break;
						case Value:
							info = reader.Value?.ToString();
							break;
						case None:
							info = "None";
							break;
						case XamlNodeType.NamespaceDeclaration:
							info = reader.Namespace?.Namespace;
							break;
					}

					preIndent?.Invoke();

					WriteLine("Line: {0,4}{1}\t{2}\t{3}",
						((System.Xaml.IXamlLineInfo)reader).LineNumber,
						new string('\t', indent), nodeType, info);
					// todo parse Markup Extensions
					//var buffer = reader.Value as MemoryStream;
					//if (nodeType == Value && buffer != null)
					//{
					//	PrintBamlStream(new XamlXmlReader(buffer, reader.SchemaContext), 
					//		settings);
					//}

					postIndent?.Invoke();

				}
				catch (Exception exception)
				{
					WriteLine("{0}\t{1}", reader.NodeType, exception);
				}
		}

		private void PrintBamlStream2(Baml2006Reader reader, XamlReaderSettings settings)
		{
			
		}
	}
}
