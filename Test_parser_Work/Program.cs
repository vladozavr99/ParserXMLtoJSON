using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
namespace Test_parser_Work
{
	[DataContract]
	public class Response
	{
		[DataMember]
		public string UID { get; set; }
		[DataMember(Order = 1)]
		public string Name { get; set; }

		public Response(string uid, string name)
		{
			UID = uid;
			Name = name;
		}

	}
	class Program
	{
		static void Main(string[] args)
		{
			string env_veriable = "%SPF_SCHEMA%";
			string path = Environment.ExpandEnvironmentVariables(env_veriable);
			string[] allFoundFiles = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);

			List<Response> list = new List<Response>();
			bool flag = false;
			bool jump = true;


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

								list.Add(new Response(childnode.Attributes["UID"].Value, childnode.Attributes["Name"].Value));
							}
						}
					}
				}
			}

			DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Response>));
			
			using (FileStream fs = new FileStream("response.json", FileMode.OpenOrCreate))
			{
				jsonFormatter.WriteObject(fs, list);
			}
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//////////////_____________Другой вариант вывода JSON'а. Форматирование приближенное к условию______/////////////////////////////
			string text = null;
			using (StreamReader sr = new StreamReader("response.json"))
			{
				text = sr.ReadToEnd();
			}
			string[] words = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string s in words)
			{
				using (StreamWriter sw = new StreamWriter("response2.json", flag))
				{

					if (jump == false)
					{
						sw.WriteLine(s + "," + Environment.NewLine);
						jump = true;
					}
					else
					{
						sw.WriteLine(s + ",");
						jump = false;
					}



				}
				flag = true;
			}


			Console.Read();
		}
	}
}
