using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqBooks
{
	class Book
	{
		public string Title { get; set; }
		public int Pages { get; set; }
		public int AuthorId { get; set; }

		public override string ToString() => $"{Title}, {Pages} стр.";
	}

	class Author
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }

		public override string ToString() => $"{Name} ({Country})";
	}

	class Program
	{
		static void Main()
		{
			var books = new List<Book>
			{
				new Book { Title = "Война и мир", Pages = 1225, AuthorId = 1 },
				new Book { Title = "Преступление и наказание", Pages = 671, AuthorId = 2 },
				new Book { Title = "Анна Каренина", Pages = 864, AuthorId = 1 },
				new Book { Title = "Мастер и Маргарита", Pages = 470, AuthorId = 3 },
				new Book { Title = "Белая гвардия", Pages = 510, AuthorId = 3 },
				new Book { Title = "Евгений Онегин", Pages = 384, AuthorId = 4 },
				new Book { Title = "Мёртвые души", Pages = 352, AuthorId = 5 }
			};

			var authors = new List<Author>
			{
				new Author { Id = 1, Name = "Лев Толстой", Country = "Россия" },
				new Author { Id = 2, Name = "Фёдор Достоевский", Country = "Россия" },
				new Author { Id = 3, Name = "Михаил Булгаков", Country = "Россия" },
				new Author { Id = 4, Name = "Александр Пушкин", Country = "Россия" },
				new Author { Id = 5, Name = "Николай Гоголь", Country = "Россия" }
			};


			Console.WriteLine("\nКниги:");
			books.ForEach(b => Console.WriteLine(b));
			Console.WriteLine("\nАвторы:");
			authors.ForEach(a => Console.WriteLine(a));

			Console.WriteLine("\nЗадание 1. Фильтрация по одному критерию: книги больше 600 страниц");
			var q1 = books.Where(b => b.Pages > 600);
			foreach (var b in q1) Console.WriteLine(b);

			Console.WriteLine("\nЗадание 2. Фильтрация по двум критериям: книги Достоевского и меньше заданного числа страниц");
			Console.Write("Введите максимальное число страниц: ");
			int maxPages = int.Parse(Console.ReadLine());
			var q2 = books.Where(b => b.AuthorId == 2 && b.Pages < maxPages);

			if (q2.Any())
			{
				foreach (var b in q2)
					Console.WriteLine(b);
			}
			else
			{
				Console.WriteLine("Ничего не найдено");
			}


			Console.WriteLine("\nЗадание 3. Сортировка: книги по числу страниц (возрастание)");
			var q3 = books.OrderBy(b => b.Pages);
			foreach (var b in q3) Console.WriteLine(b);

			Console.WriteLine("\nЗадание 4. Размер выборки: количество книг Льва Толстого");
			int count = books.Count(b => b.AuthorId == 1);
			Console.WriteLine($"Количество книг Толстого: {count}");

			Console.WriteLine("\nЗадание 5. Агрегаты Max, Average, Sum по страницам");
			Console.WriteLine("Максимум: " + books.Max(b => b.Pages));
			Console.WriteLine("Среднее: " + books.Average(b => b.Pages));
			Console.WriteLine("Сумма: " + books.Sum(b => b.Pages));


			Console.WriteLine("\nЗадание 6. Использование let: определить, толстая или тонкая книга");
			var q6 = from b in books
					 let size = (b.Pages > 700 ? "толстая" : "тонкая")
					 select $"{b.Title} — {size} книга";
			foreach (var x in q6) Console.WriteLine(x);

			Console.WriteLine("\nЗадание 7. Группировка книг по авторам");
			var q7 = from b in books
					 group b by b.AuthorId into gr
					 select new { AuthorId = gr.Key, Books = gr };
			foreach (var g in q7)
			{
				Console.WriteLine($"Автор {authors.First(a => a.Id == g.AuthorId).Name}:");
				foreach (var b in g.Books) Console.WriteLine("  " + b.Title);
			}

			Console.WriteLine("\nЗадание 8. Join: объединение книг с авторами");
			var q8 = from b in books
					 join a in authors on b.AuthorId equals a.Id
					 select $"{b.Title} — {a.Name}";
			foreach (var x in q8) Console.WriteLine(x);

			Console.WriteLine("\nЗадание 9. GroupJoin: авторы и список их книг");
			var q9 = authors.GroupJoin(books,
									   a => a.Id,
									   b => b.AuthorId,
									   (a, bs) => new { a.Name, Books = bs });
			foreach (var g in q9)
			{
				Console.WriteLine($"{g.Name}:");
				foreach (var b in g.Books) Console.WriteLine("  " + b.Title);
			}

			Console.WriteLine("\nЗадание 10. Проверка All: все ли книги больше 400 страниц?");
			bool allBig = books.All(b => b.Pages > 400);
			Console.WriteLine(allBig ? "Да, все больше 400" : "Нет, есть меньше 400");

			Console.WriteLine("\nЗадание 11. Проверка Any: есть ли хотя бы одна книга Булгакова?");
			bool anyBulgakov = books.Any(b => b.AuthorId == 3);
			Console.WriteLine(anyBulgakov ? "Да, есть" : "Нет, нету");
		}
	}
}
