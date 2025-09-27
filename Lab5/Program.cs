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
				new Book { Title = "Идиот", Pages = 620, AuthorId = 2 },
				new Book { Title = "Анна Каренина", Pages = 864, AuthorId = 1 },
				new Book { Title = "Мастер и Маргарита", Pages = 470, AuthorId = 3 }
			};

			var authors = new List<Author>
			{
				new Author { Id = 1, Name = "Лев Толстой", Country = "Россия" },
				new Author { Id = 2, Name = "Фёдор Достоевский", Country = "Россия" },
				new Author { Id = 3, Name = "Михаил Булгаков", Country = "Россия" }
			};

			Console.WriteLine("===== Книги =====");
			books.ForEach(b => Console.WriteLine(b));
			Console.WriteLine("\n===== Авторы =====");
			authors.ForEach(a => Console.WriteLine(a));

			Console.WriteLine("\n--- 1. Книги больше 600 страниц ---");
			var q1 = books.Where(b => b.Pages > 600);
			foreach (var b in q1) Console.WriteLine(b);

			Console.WriteLine("\n--- 2. Книги конкретного автора и страниц меньше X ---");
			Console.Write("Введите максимальное число страниц: ");
			int maxPages = int.Parse(Console.ReadLine());
			var q2 = books.Where(b => b.AuthorId == 2 && b.Pages < maxPages);
			foreach (var b in q2) Console.WriteLine(b);

			Console.WriteLine("\n--- 3. Сортировка книг по числу страниц ---");
			var q3 = books.OrderBy(b => b.Pages);
			foreach (var b in q3) Console.WriteLine(b);

			Console.WriteLine("\n--- 4. Количество книг Толстого ---");
			int count = books.Count(b => b.AuthorId == 1);
			Console.WriteLine($"Количество книг: {count}");

			Console.WriteLine("\n--- 5. Max, Average, Sum по страницам ---");
			Console.WriteLine("Макс: " + books.Max(b => b.Pages));
			Console.WriteLine("Среднее: " + books.Average(b => b.Pages));
			Console.WriteLine("Сумма: " + books.Sum(b => b.Pages));

			Console.WriteLine("\n--- 6. Использование let (возраст книги с даты выхода) ---");
			var q6 = from b in books
					 let century = (b.Pages > 700 ? "толстая" : "тонкая")
					 select $"{b.Title} — {century} книга";
			foreach (var x in q6) Console.WriteLine(x);

			Console.WriteLine("\n--- 7. Группировка книг по авторам ---");
			var q7 = from b in books
					 group b by b.AuthorId into gr
					 select new { AuthorId = gr.Key, Books = gr };
			foreach (var g in q7)
			{
				Console.WriteLine($"Автор {authors.First(a => a.Id == g.AuthorId).Name}:");
				foreach (var b in g.Books) Console.WriteLine("  " + b.Title);
			}

			Console.WriteLine("\n--- 8. Join: книги + авторы ---");
			var q8 = from b in books
					 join a in authors on b.AuthorId equals a.Id
					 select $"{b.Title} — {a.Name}";
			foreach (var x in q8) Console.WriteLine(x);

			Console.WriteLine("\n--- 9. GroupJoin: авторы + их книги ---");
			var q9 = authors.GroupJoin(books,
									   a => a.Id,
									   b => b.AuthorId,
									   (a, bs) => new { a.Name, Books = bs });
			foreach (var g in q9)
			{
				Console.WriteLine($"{g.Name}:");
				foreach (var b in g.Books) Console.WriteLine("  " + b.Title);
			}

			Console.WriteLine("\n--- 10. All: все ли книги больше 400 страниц ---");
			bool allBig = books.All(b => b.Pages > 400);
			Console.WriteLine(allBig ? "Да" : "Нет");

			Console.WriteLine("\n--- 11. Any: есть ли хотя бы одна книга Булгакова ---");
			bool anyBulgakov = books.Any(b => b.AuthorId == 3);
			Console.WriteLine(anyBulgakov ? "Да" : "Нет");
		}
	}
}
