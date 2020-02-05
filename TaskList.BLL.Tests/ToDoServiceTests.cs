using System;
using System.Threading.Tasks;
using Moq;
using TaskList.BLL.Services;
using TaskList.DAL.Entities;
using TaskList.DAL.Repositories;
using Xunit;

namespace TaskList.BLL.Tests
{
    public class ToDoServiceTests
    {
        [Fact]
        public async Task Create_ShouldSetCreatedDate()
        {
            // arrange
            var moq = new Mock<IRepository<ToDo>>();
            moq.Setup(r => r.Create(It.IsAny<ToDo>()))
                .Returns(Task.CompletedTask);
            var toDo = new ToDo();
            var service = new ToDoService(moq.Object);
            var now = DateTime.UtcNow;
            // act
            await service.Create(toDo);
            // assert
            Assert.InRange((now - toDo.Created).TotalSeconds, -1, 1);
            moq.Verify(r => r.Create(It.IsAny<ToDo>()), Times.Once);
        }

        [Fact]
        public async Task DeleteById_CompletedToDosMustBeSoftDeleted()
        {
            // arrange
            var moq = new Mock<IRepository<ToDo>>();
            var completed = new ToDo {IsCompleted = true};
            moq.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(completed));
            moq.Setup(r => r.Remove(It.IsAny<ToDo>()))
                .Returns(Task.CompletedTask);
            moq.Setup(r => r.Update(It.IsAny<ToDo>()))
                .Returns(Task.CompletedTask);
            var service = new ToDoService(moq.Object);
            // act
            var result = await service.DeleteById(1);
            // assert
            Assert.True(completed.IsDeleted);
            moq.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
            moq.Verify(r => r.Update(It.IsAny<ToDo>()), Times.Once);
            moq.Verify(r => r.Remove(It.IsAny<ToDo>()), Times.Never);
            Assert.Equal(completed, result);
        }

        [Fact]
        public async Task DeleteById_UncompletedToDosMustBeDeleted()
        {
            // arrange
            var moq = new Mock<IRepository<ToDo>>();
            var uncompleted = new ToDo {IsCompleted = false};
            moq.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(uncompleted));
            moq.Setup(r => r.Remove(It.IsAny<ToDo>()))
                .Returns(Task.CompletedTask);
            moq.Setup(r => r.Update(It.IsAny<ToDo>()))
                .Returns(Task.CompletedTask);
            var service = new ToDoService(moq.Object);
            // act
            var result = await service.DeleteById(1);
            // assert
            Assert.False(uncompleted.IsDeleted);
            moq.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
            moq.Verify(r => r.Remove(It.IsAny<ToDo>()), Times.Once);
            moq.Verify(r => r.Update(It.IsAny<ToDo>()), Times.Never);
        }

        [Fact]
        public async Task GetById_ShouldReturnNullIfDeleted()
        {
            // arrange
            var moq = new Mock<IRepository<ToDo>>();
            var todo = new ToDo {IsDeleted = true};
            moq.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(todo));
            var service = new ToDoService(moq.Object);
            // act
            var result = await service.GetById(1);
            // assert
            Assert.Null(result);
            moq.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnToDoIfNotDeleted()
        {
            // arrange
            var moq = new Mock<IRepository<ToDo>>();
            var todo = new ToDo {IsDeleted = false};
            moq.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(todo));
            var service = new ToDoService(moq.Object);
            // act
            var result = await service.GetById(1);
            // assert
            Assert.Equal(todo, result);
            moq.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_HaveToSaveCreatedDate()
        {
            // arrange
            var moq = new Mock<IRepository<ToDo>>();
            var todoOld = new ToDo {Created = DateTime.UtcNow - TimeSpan.FromHours(2)};
            var todoNew = new ToDo();
            moq.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(todoOld));
            moq.Setup(r => r.Update(It.IsAny<ToDo>()))
                .Returns(Task.CompletedTask);
            var service = new ToDoService(moq.Object);
            // act
            await service.Update(todoNew);
            // assert
            moq.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
            Assert.Equal(todoOld.Created, todoNew.Created);
            moq.Verify(r => r.Update(todoNew), Times.Once);
        }
    }
}