using System;

namespace TaskList.DAL.Entities
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ImportanceEnum Importance { get; set; } = ImportanceEnum.Normal;
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }

        public int ListId { get; set; }
        public List List { get; set; }
    }

    public enum ImportanceEnum
    {
        Low,
        Normal,
        High
    }
}