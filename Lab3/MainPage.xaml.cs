using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace Lab3
{
	public partial class MainPage : ContentPage
	{
		string filePath;
		public LINQ Linq = new LINQ();
		public Graduate FilterGraduate { get; set; }
		public ObservableCollection<Graduate> GraduateObjects { get; set; }
		private List<string> _nameOptions;
		public List<string> NameOptions
		{
			get { return _nameOptions; }
			set
			{
				_nameOptions = value;
				OnPropertyChanged(nameof(NameOptions));
			}
		}
		private List<string> _facultyOptions;
		public List<string> FacultyOptions
		{
			get { return _facultyOptions; }
			set
			{
				_facultyOptions = value;
				OnPropertyChanged(nameof(FacultyOptions));
			}
		}
		private List<string> _specialtyOptions;
		public List<string> SpecialtyOptions 
		{
			get { return _specialtyOptions; }
			set 
			{
				_specialtyOptions = value;
				OnPropertyChanged(nameof(SpecialtyOptions));
			}
		}
		private List<int> _gradYearOptions;
		public List<int> GradYearOptions
		{
			get { return _gradYearOptions; }
			set 
			{ 
				_gradYearOptions = value;
				OnPropertyChanged(nameof(GradYearOptions));
			}
		}
		public MainPage()
		{
			FilterGraduate = new Graduate();
			GraduateObjects = new ObservableCollection<Graduate>();
			NameOptions = new List<string>();
			FacultyOptions = new List<string>();
			SpecialtyOptions = new List<string>();
			GradYearOptions = new List<int>();

			InitializeComponent();

			BindingContext = this;
		}

		private async void OpenJSON_ButtonClicked(object sender, EventArgs e)
		{
			try
			{
				var result = await FilePicker.PickAsync();
				if (result != null)
				{
					if (result.FileName.EndsWith(".json"))
					{
						filePath = result.FullPath;
						GraduateObjects.Clear();
						foreach (var graduate in Linq.Search(filePath, FilterGraduate))
						{
							GraduateObjects.Add(graduate);
						}
						UpdateOptions();
					}
					else
					{
						await DisplayAlert("Error", "Invalid format", "OK");
					}
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"{ex.Message}", "OK");
			}
		}

		private void Info_ButtonClicked(object sender, EventArgs e)
		{
			DisplayAlert("Info", "-Author: Pavel Andrew Shpagin\n-Course: 2\n-Group: K-27\n-Release year: 2023\n-Description:\nClick 'Open JSON' button to choose a JSON file on your computer, the contents will be displayed. Click 'Add' button to add a new item to JSON; you can redact and delete items from your JSON. Click 'Save JSON' button to save the file. Linq algorithm is used for search.", "OK");
		}

		private void Clear_ButtonClicked(object sender, EventArgs e)
		{
			FilterGraduate = new Graduate();

			NamePicker.SelectedItem = null;
			FacultyPicker.SelectedItem = null;
			SpecialtyPicker.SelectedItem = null;
			GradYearPicker.SelectedItem = null;
		}

		private void Reset_ButtonClicked(object sender, EventArgs e)
		{
			if (filePath != null)
			{
				GraduateObjects.Clear();
				foreach (var graduate in Linq.Search(filePath, new Graduate()))
				{
					GraduateObjects.Add(graduate);
				}
				UpdateOptions();
			}
			else
			{
				DisplayAlert("Error", "No JSON has been uploaded yet", "OK");
			}
		}

		private void Search_ButtonClicked(object sender, EventArgs e)
		{
			var graduateCopies = new List<Graduate>(GraduateObjects);
			GraduateObjects.Clear();
			foreach (var graduate in Linq.Update(graduateCopies, FilterGraduate))
			{
				GraduateObjects.Add(graduate);
			}
		}

		private void Close_ButtonClicked(object sender, EventArgs e)
		{
			var button = (ImageButton)sender;
			var item = (Graduate)button.CommandParameter;
			GraduateObjects.Remove(item);
			UpdateOptions();
		}

		private async void RedactName_ButtonClicked(object sender, EventArgs e)
		{
			var result = await this.DisplayPromptAsync("Modify Name", "Enter new name:", "Save", "Cancel");

			if (result != null)
			{
				var button = (ImageButton)sender;
				var item = (Graduate)button.CommandParameter;
				item.Name = result;
				UpdateOptions();
			}
		}

		private async void RedactFaculty_ButtonClicked(object sender, EventArgs e)
		{
			var result = await this.DisplayPromptAsync("Modify Faculty", "Enter new faculty:", "Save", "Cancel");

			if (result != null)
			{
				var button = (ImageButton)sender;
				var item = (Graduate)button.CommandParameter;
				item.Faculty = result;
				UpdateOptions();
			}
		}

		private async void RedactSpecialty_ButtonClicked(object sender, EventArgs e)
		{
			var result = await this.DisplayPromptAsync("Modify Specialty", "Enter new specialty:", "Save", "Cancel");

			if (result != null)
			{
				var button = (ImageButton)sender;
				var item = (Graduate)button.CommandParameter;
				item.Specialty = result;
				UpdateOptions();
			}
		}

		private async void RedactGradYear_ButtonClicked(object sender, EventArgs e)
		{
			var result = await this.DisplayPromptAsync("Modify Faculty", "Enter new graduation year:", "Save", "Cancel");

			if (result != null)
			{
				var button = (ImageButton)sender;
				var item = (Graduate)button.CommandParameter;
				if (int.TryParse(result, out int intResult))
				{
					if (intResult > 0)
					{
						item.GradYear = intResult;
						UpdateOptions();
					}
					else
					{
						DisplayAlert("Error", "Not a positive number", "OK");
					}
				}
				else 
				{
					DisplayAlert("Error", "Not a number", "OK");
				}
			}
		}

		private async void Add_ButtonClicked(object sender, EventArgs e)
		{
			var name = await this.DisplayPromptAsync("Add new object", "Enter name", "Next", "Cancel");
			if (name != null)
			{
				var faculty = await this.DisplayPromptAsync("Add new object", "Enter faculty", "Next", "Cancel");
				if (faculty != null)
				{
					var specialty = await this.DisplayPromptAsync("Add new object", "Enter specialty", "Next", "Cancel");
					if (specialty != null)
					{
						var gradYear = await this.DisplayPromptAsync("Add new object", "Enter graduation year", "Add", "Cancel");
						while (gradYear != null)
						{
							if (int.TryParse(gradYear, out int intGradYear))
							{
								if (intGradYear > 0)
								{
									GraduateObjects.Insert(0, new Graduate(name, faculty, specialty, intGradYear));
									UpdateOptions();
									break;
								}
								else
								{
									await DisplayAlert("Error", "Not a positive number", "OK");
									gradYear = await this.DisplayPromptAsync("Add new object", "Enter graduation year", "Add", "Cancel");
								}
							}
							else
							{
								await DisplayAlert("Error", "Not a number", "OK");
								gradYear = await this.DisplayPromptAsync("Add new object", "Enter graduation year", "Add", "Cancel");
							}
						}
					}
				}
			}
		}

		private async void SaveJSON_ButtonClicked(object sender, EventArgs e)
		{
			if (GraduateObjects.Count > 0)
			{
				var options = new JsonSerializerOptions
				{
					WriteIndented = true
				};

				string json = JsonSerializer.Serialize(GraduateObjects, options);
				var path = RootDir.Get("new_json.json");
				File.WriteAllText(path, json);
				filePath = path;
				await DisplayAlert("Success!", $"JSON has been saved in '{path}'", "OK");
			}
			else
			{
				await DisplayAlert("Error", $"No objects to save in JSON", "OK");
			}
		}

		private void UpdateOptions() 
		{
			NameOptions = GraduateObjects.Select(obj => obj.Name).Distinct().ToList();
			FacultyOptions = GraduateObjects.Select(obj => obj.Faculty).Distinct().ToList();
			SpecialtyOptions = GraduateObjects.Select(obj => obj.Specialty).Distinct().ToList();
			GradYearOptions = GraduateObjects.Select(obj => obj.GradYear).Distinct().OrderBy(name => name).ToList();
		}
	}
}