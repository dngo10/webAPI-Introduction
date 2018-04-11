namespace TodoApi.Models{
    public class TodoItem{
        public long Id { get; set; } // The database generates the Id when a TodoItem is created.
        public string Name { get; set; }
        public bool IsComplete{get; set;}

    }
}