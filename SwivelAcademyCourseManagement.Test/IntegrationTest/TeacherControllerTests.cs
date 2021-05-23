using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SwivelAcademyCourseManagement.API.Controllers;
using SwivelAcademyCourseManagement.Data.Contracts;
using SwivelAcademyCourseManagement.Domain.DTOs;
using SwivelAcademyCourseManagement.Domain.Exceptions;
using SwivelAcademyCourseManagement.Domain.Models;
using Xunit;

namespace SwivelAcademyCourseManagement.Test.IntegrationTest
{
    public class TeacherControllerTests
    {
        private readonly Mock<ITeacherRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        public TeacherControllerTests()
        {
            _mockRepo = new Mock<ITeacherRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async void EnpointsReturnBadRequestOnValidationError()
        {
            //Arrange
            var teacherController = new TeacherController(_mockRepo.Object, _mockMapper.Object);
            teacherController.ModelState.AddModelError("error", "errors");

            //Act            
            var returnType = await teacherController.Registerteacher(new TeacherToRegisterDTO());

            //Assert
            Assert.IsType<BadRequestObjectResult>(returnType);
        }

        [Fact]
        public async void RegisterCourse_ReturnsCreatedWhenThereIsASuccessfulTransaction()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<Teacher>(It.IsAny<TeacherToRegisterDTO>())).Returns(new Teacher());
            var teacherController = new TeacherController(_mockRepo.Object, _mockMapper.Object);

            //Act
            var returnType = await teacherController.Registerteacher(new TeacherToRegisterDTO());

            //Assert
            Assert.IsType<CreatedResult>(returnType);
        }

        [Fact]
        public async void RegisterCourse_ReturnsBadRequestThereIsASuccessfulTransaction()
        {

            //Arrange
            _mockMapper.Setup(m => m.Map<Student>(It.IsAny<CourseToAddDTO>())).Returns(new Student());
            _mockRepo.Setup(m => m.Insert(It.IsAny<Teacher>())).Throws<AppUserException>();
            var teacherController = new TeacherController(_mockRepo.Object, _mockMapper.Object);

            //Act
            var returnType = await teacherController.Registerteacher(new TeacherToRegisterDTO());

            //Assert
            Assert.IsType<BadRequestObjectResult>(returnType);
        }
    }
}
