using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithListOfUsers()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.ViewData.Model);
            Assert.Equal(UserController.userlist, model);
        }

        [Fact]
        public void Details_ReturnsViewResult_WithUser()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 3, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(user);

            // Act
            var result = controller.Details(3);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal(user, model);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Name = "Test User", Email = "test@example.com" };

            // Act
            var result = controller.Create(user);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Contains(user, UserController.userlist);
        }

        [Fact]
        public void Edit_Get_ReturnsViewResult_WithUser()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 2, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(user);

            // Act
            var result = controller.Edit(2);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal(user, model);
        }

        [Fact]
        public void Edit_Get_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 11, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(user);
            var updatedUser = new User { Id = 11, Name = "Updated User", Email = "updated@example.com" };

            // Act
            var result = controller.Edit(11, updatedUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            var editedUser = UserController.userlist.FirstOrDefault(u => u.Id == 11);
            Assert.Equal("Updated User", editedUser.Name);
            Assert.Equal("updated@example.com", editedUser.Email);
        }

        [Fact]
        public void Delete_Get_ReturnsViewResult_WithUser()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 5, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(user);

            // Act
            var result = controller.Delete(5);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal(user, model);
        }

        [Fact]
        public void Delete_Get_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Post_RedirectsToIndex_WhenUserDeleted()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 6, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(user);

            // Act
            var result = controller.Delete(6, null);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.DoesNotContain(user, UserController.userlist);
        }

        [Fact]
        public void Search_ReturnsViewResult_WithMatchingUsers()
        {
            // Arrange
            var controller = new UserController();
            var user1 = new User { Id = 8, Name = "Gary", Email = "alice@example.com" };
            var user2 = new User { Id = 9, Name = "David", Email = "bob@example.com" };
            UserController.userlist.Add(user1);
            UserController.userlist.Add(user2);

            // Act
            var result = controller.Search("Gary");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.ViewData.Model);
            Assert.Single(model);
            Assert.Contains(user1, model);
        }

        [Fact]
        public void Search_ReturnsViewResult_WithNoUsers_WhenNoMatch()
        {
            // Arrange
            var controller = new UserController();
            var user1 = new User { Id = 21, Name = "Alice", Email = "alice@example.com" };
            var user2 = new User { Id = 22, Name = "Bob", Email = "bob@example.com" };
            UserController.userlist.Add(user1);
            UserController.userlist.Add(user2);

            // Act
            var result = controller.Search("Charlie");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.ViewData.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Search_ReturnsViewResult_WithMatchingUsers_ByEmail()
        {
            // Arrange
            var controller = new UserController();
            var user1 = new User { Id = 31, Name = "Eve", Email = "eve@example.com" };
            var user2 = new User { Id = 32, Name = "Frank", Email = "frank@example.com" };
            UserController.userlist.Add(user1);
            UserController.userlist.Add(user2);

            // Act
            var result = controller.Search("eve@example.com");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.ViewData.Model);
            Assert.Single(model);
            Assert.Contains(user1, model);
        }
    }
}
