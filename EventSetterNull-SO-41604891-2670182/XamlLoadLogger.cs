using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xaml;
using static System.Diagnostics.Debug;
using static System.Xaml.XamlNodeType;
using XamlReader = System.Xaml.XamlReader;


namespace EventSetterNull_SO_41604891_2670182
{
	public class XamlLoadLogger
	{
		private XamlNodeType currentNodeType;
		private int _indent = 0;
		private static readonly Action<XamlLoadLogger> IncreaseIndent = 
			(XamlLoadLogger inst) => inst._indent += 1;
		private static readonly Action<XamlLoadLogger> DecreaseIndent = 
			(XamlLoadLogger inst) => inst._indent -= 1;

		private static readonly int tabStop = 4;
		private static readonly string tab = new string(' ', tabStop);
		private string Padding => new string(' ', _indent * tabStop);

		private string LogReader (System.Xaml.XamlReader reader)
		{
			var output = "";
			var info = "";
			var tab = "    ";
			Action<XamlLoadLogger> postIndent;
			var preIndent = postIndent = null;
			var nodeType = reader.NodeType;

			switch (nodeType)
			{
				case StartObject:
					info = reader.Type?.Name;
					postIndent = IncreaseIndent;
					break;
				case EndObject:
					info = reader.Type?.Name;
					preIndent = DecreaseIndent;
					break;
				case GetObject:
					info = (reader.SchemaContext?.GetType()).ToString();
					postIndent = IncreaseIndent;
					break;
				case StartMember:
				case EndMember:
					info = reader.Member?.Name;
					switch (nodeType)
					{
						case StartMember:
							postIndent = IncreaseIndent;
							break;
						case EndMember:
							preIndent = DecreaseIndent;
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

			preIndent?.Invoke(this);

			output = string.Format("{1,-8}{2,4}{3}{0}{4}{0}{5}", tab, "Line: ",
				((System.Xaml.IXamlLineInfo)reader).LineNumber,
				Padding, nodeType, info);

			postIndent?.Invoke(this);

			return output;
		}

		public string Log (string caller, object arg)
		{
			if (arg == null) return caller + ": Log Error: nul arg";
			if (arg is XamlReader) return LogReader((XamlReader) arg);
			return caller + ": unknown arg";
		}

		public string LogMember(object host, string member, string label = "")
		{
			var pre = new string(' ', 12) + Padding + label;
			try
			{
				return pre + host.GetType().GetProperty(member).GetValue(host);
			}
			catch (Exception e)
			{
				return pre + (host?.ToString() ?? "null");
			}
		}
		public string LogMemberList<T> (List<T> host, string label = "", int indx = -1)
		{
			var pre = new string(' ', 12) + Padding + label;
			pre += $"{(indx > -1 ? "[" + indx + "]" : ""),-6}";
			try
			{
				return pre + host[indx];
			}
			catch (Exception e)
			{
				return pre + (host?.ToString() ?? "null");
			}
		}
	}
}
