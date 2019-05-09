using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ScientificReport.BLL.Services;
using ScientificReport.Controllers;
using ScientificReport.DAL.DbContext;
using ScientificReport.DAL.Entities;
using ScientificReport.DAL.Entities.UserProfile;
using ScientificReport.DAL.Roles;
using Xunit;

namespace ScientificReport.Test.ControllersTests
{
	public class TaskControllerTests
	{
		private static readonly List<Department> TestDepartments = new List<Department>
			{TestData.Department1, TestData.Department2};

		private static async Task<DepartmentController> MockController(IEnumerable<string> userRoles)
		{
			var context = new Mock<ScientificReportDbContext>();
			context.Setup(item => item.Departments).Returns(MockProvider.GetMockSet(TestDepartments.AsQueryable()).Object);
			
			var departmentService = new Mock<DepartmentService>(context.Object);

			var userService = new Mock<UserProfileService>(context.Object);
			userService.Object.CreateItem(TestData.User1);

			var scientificWorkService = new Mock<ScientificWorkService>(context.Object);

			var store = new Mock<IUserStore<UserProfile>>();
			var userManager = new Mock<UserManager<UserProfile>>(store.Object, null, null, null, null, null, null, null, null);
			userManager.Object.UserValidators.Add(new UserValidator<UserProfile>());
			userManager.Object.PasswordValidators.Add(new PasswordValidator<UserProfile>());
			await userManager.Object.AddToRolesAsync(TestData.User1, userRoles);

			return new DepartmentController(departmentService.Object, userService.Object, scientificWorkService.Object, userManager.Object);
		}

		[Fact]
		public async Task IndexTest()
		{
			var controller = await MockController(new []{UserProfileRole.Administrator});
			var view = controller.Index();
			
			Console.Out.WriteLine(view.ToString());
			
			Assert.Equal(TestDepartments, (view as ViewResult)?.ViewData.Model);
		}
	}
}
