using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Todo
{
	public class TodoItemDatabase
	{
		readonly SQLiteAsyncConnection database;

		public TodoItemDatabase(string dbPath)
		{
            //Default setting of connection is to store dates as ticks.  We want this as code does rely on it.
			database = new SQLiteAsyncConnection(dbPath);
			database.CreateTableAsync<TodoItem>().Wait();
		}

		public Task<List<TodoItem>> GetItemsAsync()
		{
            DateTime dateTime1 = DateTime.Now.AddDays(1).Date;
            DateTime dateTime2 = DateTime.Now.AddDays(3).Date;

            //Even though dates are stored as ticks in linq we can still deal in dates.
            //I assume because we are not dealing with the DB and the data is now in c# memory structures.
            //return database.Table<TodoItem>()
            //    .Where(x => x.CreatedDate >= dateTime1)
            //    .Where(x => x.CreatedDate <= dateTime2)
            //    .ToListAsync();

            //Or we can do raw SQL but we need to convert our dates in c# to the right format .... ticks.
            //How easy is that.
            return database.QueryAsync<TodoItem>($"SELECT * FROM [TodoItem] WHERE [CreatedDate] BETWEEN {dateTime1.Ticks} AND {dateTime2.Ticks}");
		}

		public Task<List<TodoItem>> GetItemsNotDoneAsync()
		{
			return database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
		}

		public Task<TodoItem> GetItemAsync(int id)
		{
			return database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
		}

		public Task<int> SaveItemAsync(TodoItem item)
		{
			if (item.ID != 0)
			{
				return database.UpdateAsync(item);
			}
			else {
				return database.InsertAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(TodoItem item)
		{
			return database.DeleteAsync(item);
		}
	}
}

