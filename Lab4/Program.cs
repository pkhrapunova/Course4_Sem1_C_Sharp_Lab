using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ConsoleApp
{
	class Program
	{
		static void Main()
		{
			while (true)
			{
				Console.WriteLine("Выберите пункт меню:");
				Console.WriteLine("1. Строки");
				Console.WriteLine("2. Текстовый файл");
				Console.WriteLine("3. JSON файл");
				Console.WriteLine("0. Выход");

				string choice = Console.ReadLine();
				switch (choice)
				{
					case "1":
						Console.WriteLine("Введите текст:");
						string inputText = Console.ReadLine();
						string result1 = ProcessUniqueCharacters(inputText);
						Console.WriteLine("Символы, встречающиеся один раз:");
						Console.WriteLine(result1);
						break;

					case "2":
						Console.WriteLine("Введите путь к исходному файлу:");
						string inputFile = Console.ReadLine();
						Console.WriteLine("Введите путь к файлу для записи результата:");
						string outputFile = Console.ReadLine();
						ProcessFileWords(inputFile, outputFile);
						Console.WriteLine("Обработка завершена. Результат записан.");
						break;

					case "3":
						Console.WriteLine("Выберите действие:");
						Console.WriteLine("1. Создать JSON файл со студентами");
						Console.WriteLine("2. Обработать существующий JSON файл");
						string jsonChoice = Console.ReadLine();

						if (jsonChoice == "1")
						{
							Console.WriteLine("Введите путь для сохранения JSON файла:");
							string savePath = Console.ReadLine();

							// Пример коллекции студентов
							var students = new List<Student>
							{
								new Student { Name = "Иван Иванов", Group = "101", PhoneNumber = "123456789" },
								new Student { Name = "Петр Петров", Group = "101", PhoneNumber = "987654321" },
								new Student { Name = "Сергей Сергеев", Group = "102", PhoneNumber = "555555555" }
							};

							CreateJsonStudentsFile(savePath, students);
							Console.WriteLine("JSON файл успешно создан.");
						}
						else if (jsonChoice == "2")
						{
							Console.WriteLine("Введите путь к JSON файлу со студентами:");
							string jsonFile = Console.ReadLine();
							ProcessJsonStudents(jsonFile);
							Console.WriteLine("Обработка JSON завершена. Файлы созданы на рабочем столе.");
						}
						else
						{
							Console.WriteLine("Некорректный выбор.");
						}
						break;

					case "0":
						return;

					default:
						Console.WriteLine("Некорректный выбор.");
						break;
				}
			}
		}

		static string ProcessUniqueCharacters(string text)
		{
			var charCount = new Dictionary<char, int>();
			foreach (var c in text)
				if (charCount.ContainsKey(c)) charCount[c]++; else charCount[c] = 1;

			var result = text.Where(c => charCount[c] == 1);
			return new string(result.ToArray());
		}

		static void ProcessFileWords(string inputFilePath, string outputFilePath)
		{
			var lines = File.ReadAllLines(inputFilePath);
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

			File.WriteAllLines(outputFilePath, processedLines);
		}

		static int CountConsonants(string word)
		{
			string consonants = "bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ";
			int count = 0;
			foreach (var c in word)
				if (consonants.IndexOf(c) >= 0) count++;
			return count;
		}

		static void CreateJsonStudentsFile(string filePath, List<Student> students)
		{
			string json = JsonConvert.SerializeObject(students, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}

		static void ProcessJsonStudents(string jsonFilePath)
		{
			string jsonContent = File.ReadAllText(jsonFilePath);
			var students = JsonConvert.DeserializeObject<List<Student>>(jsonContent);

			string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string studentsDir = Path.Combine(desktop, "Students");
			Directory.CreateDirectory(studentsDir);

			var groups = new Dictionary<string, List<Student>>();
			foreach (var student in students)
			{
				if (!groups.ContainsKey(student.Group))
					groups[student.Group] = new List<Student>();
				groups[student.Group].Add(student);
			}

			foreach (var group in groups)
			{
				string groupFile = Path.Combine(studentsDir, group.Key + ".txt");
				var lines = group.Value.Select(s => s.Name + ", " + s.PhoneNumber).ToList();
				File.WriteAllLines(groupFile, lines);
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
