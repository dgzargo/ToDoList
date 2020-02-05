using System;
using System.Linq.Expressions;
using TaskList.DAL.Entities;

namespace TaskList.BLL.Helpers
{
    public static class CompletedFilterModifierHelper
    {
        public static Expression<Func<ToDo, bool>> ModifyFilterIfCompletedDisallowed(
            Expression<Func<ToDo, bool>> filter,
            bool disallowCompleted)
        {
            if (disallowCompleted)
            {
                var param = filter.Parameters[0];

                var e1 = Expression.Invoke(filter, param);

                Expression<Func<ToDo, bool>> completedFilter = todo => todo.IsCompleted == false;
                var e2 = Expression.Invoke(completedFilter, param);

                var expr = Expression.And(e1, e2);
                filter = Expression.Lambda<Func<ToDo, bool>>(expr, param);
            }

            return filter;
        }
    }
}