using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleApp
{
	class Program
	{
		static void Main()
		{
			while (true)
			{
				Console.WriteLine("Меню:");
				Console.WriteLine("1. Строки");
				Console.WriteLine("2. Текстовый файл");
				Console.WriteLine("3. JSON файл");
				Console.WriteLine("0. Выход");

				string choice = Console.ReadLine();
				switch (choice)
				{
					case "1":
						Console.Write("Введите текст: ");
						string inputText = Console.ReadLine();
						string result1 = ProcessUniqueCharacters(inputText);
						Console.WriteLine("Символы, встречающиеся один раз:");
						Console.WriteLine(result1);
						break;

					case "2":
						Console.Write("Имя входного файла: ");
						string inputFileName = Console.ReadLine();

						if (!IsValidFileName(inputFileName))
						{
							Console.WriteLine("Недопустимое имя файла.");
							break;
						}

						string fullPathIn = Path.Combine(
							Directory.GetCurrentDirectory(),
							Path.ChangeExtension(inputFileName, ".txt")
						);

						if (!File.Exists(fullPathIn))
						{
							Console.WriteLine("Файл не найден.");
							break;
						}

						Console.Write("Имя выходного файла: ");
						string outputFileName = Console.ReadLine();

						if (!IsValidFileName(outputFileName))
						{
							Console.WriteLine("Недопустимое имя файла.");
							break;
						}

						string fullPathOut = Path.Combine(
							Directory.GetCurrentDirectory(),
							Path.ChangeExtension(outputFileName, ".txt")
						);

						ProcessFileWords(fullPathIn, fullPathOut);
						Console.WriteLine("Результат записан в файл: " + outputFileName + ".txt");
						break;

					case "3":
						Console.WriteLine("JSON:");
						Console.WriteLine("1. Создать JSON файл со студентами");
						Console.WriteLine("2. Обработать существующий JSON файл");
						Console.Write("Ваш выбор: ");
						string jsonChoice = Console.ReadLine();

						if (jsonChoice == "1")
						{
							Console.Write("Имя JSON-файла для сохранения: ");
							string saveFileName = Console.ReadLine();

							if (!IsValidFileName(saveFileName))
							{
								Console.WriteLine("Недопустимое имя файла.");
								break;
							}

							string savePath = Path.Combine(
								Directory.GetCurrentDirectory(),
								Path.ChangeExtension(saveFileName, ".json")
							);

							var students = new List<Student>
							{
								new Student { Name = "Иван Иванов", Group = "101", PhoneNumber = "123456789" },
								new Student { Name = "Петр Петров", Group = "101", PhoneNumber = "987654321" },
								new Student { Name = "Сергей Сергеев", Group = "102", PhoneNumber = "555555555" }
							};

							CreateJsonStudentsFile(savePath, students);
							Console.WriteLine("JSON файл успешно создан: " + saveFileName + ".json");
						}
						else if (jsonChoice == "2")
						{
							Console.Write("Имя JSON-файла для обработки: ");
							string jsonFileName = Console.ReadLine();

							if (!IsValidFileName(jsonFileName))
							{
								Console.WriteLine("Недопустимое имя файла.");
								break;
							}

							string jsonPath = Path.Combine(
								Directory.GetCurrentDirectory(),
								Path.ChangeExtension(jsonFileName, ".json")
							);

							if (!File.Exists(jsonPath))
							{
								Console.WriteLine("JSON файл не найден.");
								break;
							}

							try
							{
								ProcessJsonStudents(jsonPath);
								Console.WriteLine("Студенты распределены по группам на рабочем столе.");
							}
							catch (Exception ex)
							{
								Console.WriteLine("Ошибка обработки JSON: " + ex.Message);
							}
						}
						else
						{
							Console.WriteLine("Некорректный выбор.");
						}
						break;

					case "0":
						Console.WriteLine("Выход из программы.");
						return;

					default:
						Console.WriteLine("Некорректный выбор.");
						break;
				}
			}
		}

		static bool IsValidFileName(string name)
		{
			return !string.IsNullOrWhiteSpace(name)
				   && name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
		}

		static string ProcessUniqueCharacters(string text)
		{
			var charCount = new Dictionary<char, int>();
			foreach (var c in text)
				charCount[c] = charCount.ContainsKey(c) ? charCount[c] + 1 : 1;

			var result = text.Where(c => charCount[c] == 1);
			return new string(result.ToArray());
		}

		static void ProcessFileWords(string inputFilePath, string outputFilePath)
		{
			var lines = File.ReadAllLines(inputFilePath, Encoding.UTF8);
			var processedLines = new List<string>();

			foreach (var line in lines)
			{
				var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < words.Length; i++)
				{
					if (CountConsonants(words[i]) >= 5)
						words[i] = words[i].ToUpper();
				}
				processedLines.Add(string.Join(" ", words));
			}

			File.WriteAllLines(outputFilePath, processedLines, Encoding.UTF8);
		}

		static readonly HashSet<char> ConsonantsSet = new HashSet<char>(
			("бвгджзйклмнпрстфхцчшщБВГДЖЗЙКЛМНПРСТФХЦЧШЩ" +
			 "bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ").ToCharArray());

		static int CountConsonants(string word) =>
			word.Count(c => ConsonantsSet.Contains(c));

		static void CreateJsonStudentsFile(string filePath, List<Student> students)
		{
			string json = JsonConvert.SerializeObject(students, Formatting.Indented);
			File.WriteAllText(filePath, json, Encoding.UTF8);
		}

		static void ProcessJsonStudents(string jsonFilePath)
		{
			string jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);
			var students = JsonConvert.DeserializeObject<List<Student>>(jsonContent) ?? new List<Student>();

			string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string studentsDir = Path.Combine(desktop, "Students");
			Directory.CreateDirectory(studentsDir);

			var groups = students.GroupBy(s => s.Group);
			foreach (var group in groups)
			{
				string safeGroupName = string.Concat(
					group.Key.Where(c => !Path.GetInvalidFileNameChars().Contains(c))
				);

				string groupFile = Path.Combine(studentsDir, safeGroupName + ".txt");
				var lines = group.Select(s => $"{s.Name}, {s.PhoneNumber}");
				File.WriteAllLines(groupFile, lines, Encoding.UTF8);
			}
		}
	}

	class Student
	{
		public string Name { get; set; }
		public string Group { get; set; }
		public string PhoneNumber { get; set; }
	}
}
