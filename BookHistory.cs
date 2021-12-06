using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace TestSqliteGeckofx60
{
	public struct Book
	{
		public string ID;
		public string TitleBestForUserDisplay;
		public string FolderPath;
	}

	[Table("books")]
	public class BookHistoryBook
	{
		[PrimaryKey]
		[Column("id")]
		public string Id { get; set; }

		// todo: books have many names... could stick them all in here, or make a new table for names, or?
		[Column("name")] public string Name { get; set; }

	}

	public enum BookHistoryEventType
	{
		CheckIn
	}

	[Table("events")]
	public class BookHistoryEvent
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Column("userid")] public string UserId { get; set; }
		[Column(name: "username")] public string UserName { get; set; }
		[Column("type")] public BookHistoryEventType Type { get; set; }
		[Column("user_message")] public string Message { get; set; }
		[Column("date")] public DateTime When { get; set; }

		[Indexed]
		[Column("book_id")] public string BookId { get; set; }
	}

	public class BookHistory
	{
		private static SQLiteConnection GetConnection(string folderPath)
		{
			SQLiteConnection db = null;
			var path = GetDatabasePath(folderPath);
			Console.WriteLine("DEBUG: BookHistory.GetConnection(): calling new SQLiteConnection(\"{0}\")", path);
			db = new SQLiteConnection(path);
			if (db == null)
				throw new ApplicationException("Could not open history db for" + path);
			db.CreateTable<BookHistoryBook>();
			db.CreateTable<BookHistoryEvent>();
			Console.WriteLine("DEBUG: good connection achieved");
			return db;
		}

		private static string GetDatabasePath(string bookFolder)
		{
			return Path.Combine(bookFolder, "history.db");
		}

		public static void AddEvent(Book book, BookHistoryEventType eventType, string message = "")
		{
			try
			{
				using (var db = GetConnection(book.FolderPath))
				{
					var bookRecord = db.Table<BookHistoryBook>().FirstOrDefault(b => b.Id == book.ID);
					if (bookRecord == null)
					{
						bookRecord = new BookHistoryBook
						{
							Id = book.ID,
							Name = book.TitleBestForUserDisplay
						};
						db.Insert(bookRecord);
					}

					var evt = new BookHistoryEvent()
					{
						BookId = book.ID,
						Message = message,
						UserId = "mcconnel.steve@gmail.com",
						UserName = "Steve",
						Type = eventType,
						When = DateTime.Now
					};

					db.Insert(evt);
					db.Close();
				}
			}
			catch (Exception e)
			{

				Console.WriteLine("Problem writing book history for {0}: {1}",
					$"folder={book.FolderPath}", e);
			}
		}

		public static List<BookHistoryEvent> GetHistory(Book book)
		{
			{
				using (var db = GetConnection(book.FolderPath))
				{
					var events = db.Table<BookHistoryEvent>().ToList();
					db.Close();
					return events;
				}
			}
		}
	}
}
