using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Lab3
{
	public class LINQ
	{

		public List<Graduate> Search(string filePath, Graduate filterGraduate) 
		{
			List<Graduate> result;
			using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(filePath))) 
			{
				result = (from graduate in document.RootElement.EnumerateArray()
							  let Name = graduate.GetProperty("Name").GetString()
							  let Faculty = graduate.GetProperty("Faculty").GetString()
							  let Specialty = graduate.GetProperty("Specialty").GetString()
							  let GradYear = graduate.GetProperty("GradYear").GetInt32()
							  where (Name == filterGraduate.Name || filterGraduate.Name == null) &&
							  (Faculty == filterGraduate.Faculty || filterGraduate.Faculty == null) &&
							  (Specialty == filterGraduate.Specialty || filterGraduate.Specialty == null) &&
							  (GradYear == filterGraduate.GradYear || filterGraduate.GradYear == 0)
							  select new Graduate
							  {
								  Name = Name,
								  Faculty = Faculty,
								  Specialty = Specialty,
								  GradYear = GradYear
							  }).ToList();
			}
			return result;
		}

		public List<Graduate> Update(List<Graduate> graduates, Graduate filterGraduate)
		{
			var result = (from graduate in graduates
						  where (graduate.Name == filterGraduate.Name || filterGraduate.Name == null) &&
						  (graduate.Faculty == filterGraduate.Faculty || filterGraduate.Faculty == null) &&
						  (graduate.Specialty == filterGraduate.Specialty || filterGraduate.Specialty == null) &&
						  (graduate.GradYear == filterGraduate.GradYear || filterGraduate.GradYear == 0)
						  select new Graduate
						  {
							  Name = graduate.Name,
							  Faculty = graduate.Faculty,
							  Specialty = graduate.Specialty,
							  GradYear = graduate.GradYear
						  }).ToList();

			return result;
		}

	}
}
