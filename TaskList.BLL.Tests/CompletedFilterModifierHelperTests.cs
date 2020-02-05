using System;
using System.Linq.Expressions;
using TaskList.BLL.Helpers;
using TaskList.DAL.Entities;
using Xunit;

namespace TaskList.BLL.Tests
{
    public class CompletedFilterModifierHelperTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void ModifyFilterIfCompletedDisallowed_ShouldReturnModifiedExpr(bool returnValue, bool isCompleted)
        {
            // arrange
            Expression<Func<ToDo, bool>> exp = todo => returnValue;
            var todo = new ToDo {IsCompleted = isCompleted};
            // act
            var returned = CompletedFilterModifierHelper.ModifyFilterIfCompletedDisallowed(exp, true);
            // assert
            var lambda = returned.Compile();
            Assert.Equal(returnValue && !isCompleted, lambda(todo));
        }

        [Fact]
        public void ModifyFilterIfCompletedDisallowed_ShouldReturnTheSameExpr()
        {
            // arrange
            Expression<Func<ToDo, bool>> exp = todo => false;
            // act
            var returned = CompletedFilterModifierHelper.ModifyFilterIfCompletedDisallowed(exp, false);
            // assert
            Assert.Equal(exp, returned);
        }
    }
}