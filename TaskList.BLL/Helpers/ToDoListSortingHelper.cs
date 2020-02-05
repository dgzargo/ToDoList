using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.BLL.Enums;
using TaskList.DAL.Entities;

namespace TaskList.BLL.Helpers
{
    public static class ToDoListSortingHelper
    {
        public static IEnumerable<ToDo> SelectOrder<TKey>(IEnumerable<ToDo> enumerable, Func<ToDo, TKey> selector,
            IComparer<TKey> comparer = null, bool isAscendingOrder = true)
        {
            return isAscendingOrder
                ? enumerable.OrderBy(selector, comparer)
                : enumerable.OrderByDescending(selector, comparer);
        }

        public static IEnumerable<ToDo> OrderBySelector(this IEnumerable<ToDo> enumerable, OrderBy orderBy,
            bool isAscendingOrder = true)
        {
            return orderBy switch
            {
                OrderBy.Importance => SelectOrder(enumerable, todo => todo.Importance),
                OrderBy.DueDate => SelectOrder(enumerable, todo => todo.Deadline),
                OrderBy.CompletedFlag => SelectOrder(enumerable, todo => todo.IsCompleted),
                OrderBy.Alphabetically => SelectOrder(enumerable, todo => todo.Title),
                OrderBy.CreationDate => SelectOrder(enumerable, todo => todo.Created),
                _ => throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy,
                    "such order field option isn't implemented")
            };
        }

        public static void SortToDoList(this List list, OrderBy orderBy, bool isAscendingOrder = true)
        {
            list.Tasks = list.Tasks.OrderBySelector(orderBy, isAscendingOrder).ToHashSet();
        }
    }
}