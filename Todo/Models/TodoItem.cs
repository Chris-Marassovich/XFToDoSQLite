using SQLite;
using System;

namespace Todo
{
	public class TodoItem
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Done { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

