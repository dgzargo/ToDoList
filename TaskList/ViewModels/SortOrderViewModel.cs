using TaskList.BLL;
using TaskList.BLL.Enums;

namespace TaskList.ViewModels
{
    public class SortOrderViewModel
    {
        public OrderBy Order { get; set; }
        public bool? IsAscendingOrder { get; set; }
    }
}