using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Test_parser_Work
{

	public class response
	{
		public string UID { get; set; }

		public string Name { get; set;}

		public response(){}

	}
	class Program
	{
		static void Main(string[] args)
		{

			response[] Response = new response[] { };

			string env_veriable = "%SPF_SCHEMA%";
			string path = Environment.ExpandEnvironmentVariables(env_veriable);
			Console.WriteLine(path);
			string[] allFoundFiles = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);
			Console.WriteLine(allFoundFiles[6]);
			
			

			foreach (string directory in allFoundFiles)
			{				
				XmlDocument file = new XmlDocument();
				file.Load(directory);

				foreach (XmlNode node in file.DocumentElement)
				{
					if (node.Name == "EnumEnum")
					{
						foreach (XmlNode childnode in node.ChildNodes)
						{
							if (childnode.Name == "IObject")
							{
								string UID = "UID: " + childnode.Attributes["UID"].Value;
								string Name = "Name: " + childnode.Attributes["Name"].Value;
								
								Console.WriteLine(UID);
								Console.WriteLine(Name);
							}

						}
					}
				}
			}
			Console.Read();
		}
	}
}
