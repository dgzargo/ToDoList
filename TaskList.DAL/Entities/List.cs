using System.Collections.Generic;

namespace TaskList.DAL.Entities
{
    public class List
    {
        public List()
        {
            Tasks = new HashSet<ToDo>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<ToDo> Tasks { get; set; }
    }
}