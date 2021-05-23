using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SwivelAcademyCourseManagement.API.Controllers;
using SwivelAcademyCourseManagement.Data.Contracts;
using SwivelAcademyCourseManagement.Domain.DTOs;
using SwivelAcademyCourseManagement.Domain.Exceptions;
using SwivelAcademyCourseManagement.Domain.Maps;
using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace SwivelAcademyCourseManagement.Test.IntegrationTest
{
    public class CourseControllerTests
    {
        private readonly Mock<ICourseRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        public CourseControllerTests()
        {
            _mockRepo = new Mock<ICourseRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async void EnpointsReturnBadRequestOnValidationError()
        {

            //Arrange
            var courseController = new CourseController(_mockRepo.Object, _mockMapper.Object);
            courseController.ModelState.AddModelError("error", "errors");

            //Act
            var returnType = await courseController.RegisterCourse(new CourseToAddDTO());

            //Assert
            Assert.IsType<BadRequestObjectResult>(returnType);
        }

        [Fact]
        public async void RegisterCourse_ReturnsCreatedWhenThereIsASuccessfulTransaction()
        {

            //Arrange
            _mockMapper.Setup(m => m.Map<Course>(It.IsAny<CourseToAddDTO>())).Returns(new Course());
            var courseController = new CourseController(_mockRepo.Object, _mockMapper.Object);

            //Act
            var returnType = await courseController.RegisterCourse(new CourseToAddDTO());

            //Assert
            Assert.IsType<CreatedResult>(returnType);
        }

        [Fact]
        public async void RegisterCourse_ReturnsBadRequestThereIsASuccessfulTransaction()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<Course>(It.IsAny<CourseToAddDTO>())).Returns(new Course());
            _mockRepo.Setup(m => m.Insert(It.IsAny<Course>())).Throws<CoursesException>();
            var courseController = new CourseController(_mockRepo.Object, _mockMapper.Object);

            //Act
            var returnType = await courseController.RegisterCourse(new CourseToAddDTO());

            //Assert
            Assert.IsType<BadRequestObjectResult>(returnType);
        }

        
    }
}
