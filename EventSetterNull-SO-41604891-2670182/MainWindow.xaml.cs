using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Xaml;
using static System.Diagnostics.Debug;
using System.Windows.Markup;
using static System.Xaml.XamlNodeType;
using XamlReader = System.Xaml.XamlReader;

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

			DecompileDictionary(new object(), new RoutedEventArgs());
		}

		private void Panel_OnClick (object sender, RoutedEventArgs e)
		{
			var btn = e.OriginalSource as Button;
			var win = (Window)System.Windows.Application.LoadComponent(
				new Uri(btn.Name.Replace("_", "/") + ".xaml", UriKind.Relative));
			win.Show();
		}

		public XamlLoadLogger Logger { get; set; } = new XamlLoadLogger();

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
			//Baml2006Reader reader = new Baml2006Reader(source, settings);

			//PrintBamlStream(reader);

			//source.Seek(0, SeekOrigin.Begin);
			Window win = System.Windows.Markup.XamlReader.Load(new Baml2006Reader(source, settings)) as Window;
			win.Show();
			source.Close();

			WriteLine(System.Windows.Markup.XamlWriter.Save(win));
		}

		private void PrintBamlStream(System.Xaml.XamlReader reader, int indent = 0)
		{
			Action increaseIndent = () => indent += 1;
			Action decreaseIndent = () => indent -= 1;
			while (reader.Read())
				try
				{
					Action postIndent;
					var preIndent = postIndent = null;
					var nodeType = reader.NodeType;
					var info = "";
					switch (nodeType)
					{
						case StartObject:
							info = reader.Type?.Name;
							postIndent = increaseIndent;
							break;
						case EndObject:
							info = reader.Type?.Name;
							preIndent = decreaseIndent;
							break;
						case GetObject:
							info = (reader.SchemaContext?.GetType()).ToString();
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

					postIndent?.Invoke();

					// todo parse Markup Extensions
					var buffer = reader.Value as MemoryStream;
					if (reader.NodeType == GetObject)
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
					WriteLine("{0}\t{1}", reader.NodeType, exception);
				}
		}
	}
}
