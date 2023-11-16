using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Lab3
{
	public class Graduate : INotifyPropertyChanged
	{
		public Graduate() { }
		public Graduate(string name, string faculty, string specialty, int gradYear)
		{ 
			Name = name;
			Faculty = faculty;
			Specialty = specialty;
			GradYear = gradYear;
		}
		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private string _faculty;
		public string Faculty
		{
			get { return _faculty; }
			set
			{
				_faculty = value;
				OnPropertyChanged(nameof(Faculty));
			}
		}

		private string _specialty;
		public string Specialty
		{
			get { return _specialty; }
			set
			{
				_specialty = value;
				OnPropertyChanged(nameof(Specialty));
			}
		}

		private int _gradYear;
		public int GradYear
		{
			get { return _gradYear; }
			set
			{
				_gradYear = value;
				OnPropertyChanged(nameof(GradYear));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}