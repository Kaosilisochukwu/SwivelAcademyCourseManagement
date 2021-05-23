using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SwivelAcademyCourseManagement.API.Controllers;
using SwivelAcademyCourseManagement.Data.Contracts;
using SwivelAcademyCourseManagement.Domain.DTOs;
using SwivelAcademyCourseManagement.Domain.Exceptions;
using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SwivelAcademyCourseManagement.Test.IntegrationTest
{
    public class StudentControllerTests
    {
        private readonly Mock<IStudentRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        public StudentControllerTests()
        {
            _mockRepo = new Mock<IStudentRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async void EnpointsReturnBadRequestOnValidationError()
        {
            //Arrange
            var studentController = new StudentController(_mockRepo.Object, _mockMapper.Object);
            studentController.ModelState.AddModelError("error", "errors");

            //Act
            var returnType = await studentController.RegisterStudent(new StudentToRegisterDTO());

            //Assert
            Assert.IsType<BadRequestObjectResult>(returnType);
        }

        [Fact]
        public async void RegisterCourse_ReturnsCreatedWhenThereIsASuccessfulTransaction()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<Student>(It.IsAny<StudentToRegisterDTO>())).Returns(new Student());
            var studentController = new StudentController(_mockRepo.Object, _mockMapper.Object);

            //Act
            var returnType = await studentController.RegisterStudent(new StudentToRegisterDTO());
            
            //Assert
            Assert.IsType<CreatedResult>(returnType);
        }

        [Fact]
        public async void RegisterCourse_ReturnsBadRequestThereIsASuccessfulTransaction()
        {

            //Arrange
            _mockMapper.Setup(m => m.Map<Student>(It.IsAny<CourseToAddDTO>())).Returns(new Student());
            _mockRepo.Setup(m => m.Insert(It.IsAny<Student>())).Throws<AppUserException>();
            var studentController = new StudentController(_mockRepo.Object, _mockMapper.Object);

            //Act
            var returnType = await studentController.RegisterStudent(new StudentToRegisterDTO());

            //Assert
            Assert.IsType<BadRequestObjectResult>(returnType);
        }
    }
}
