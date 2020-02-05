using TaskList.BLL;
using TaskList.BLL.Helpers;
using TaskList.DAL.Entities;

namespace TaskList.ViewModels
{
    public static class SortOrderViewModelHelper
    {
        public static void Sort(this List list, SortOrderViewModel viewModel = null)
        {
            if (viewModel is null) return;
            if (viewModel.IsAscendingOrder.HasValue)
            {
                list.SortToDoList(viewModel.Order, viewModel.IsAscendingOrder.Value);
            }
            else
            {
                list.SortToDoList(viewModel.Order);
            }
        }
    }
}