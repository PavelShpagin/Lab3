using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    static class RootDir 
    {
		public static string Get(string path)
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string directory = baseDirectory;
			while (!Directory.GetFiles(directory, "*.sln").Any())
			{
				string parentDirectory = Directory.GetParent(directory).FullName;
				if (parentDirectory == directory)
				{
					break;
				}
				directory = parentDirectory;
			}

			return Path.Combine(directory, "Lab3", path);
		}
	}
}
