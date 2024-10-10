using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Courses;
using Domain.Faculties;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Courses;

public class CourseControllerTests(IntegrationTestWebFactory factory)
    : BaseIntegrationTest(factory), IAsyncLifetime
{
    private readonly Course _mainCourse = CoursesData.MainCourse;

    [Fact]
    public async Task ShouldCreateCourse()
    {
        // Arrange
        var newName = "TestCourseName";
        var newMaxStudents = 10;
        var request = new CourseDto(
            Id: null,
            Name: newName,
            StartAt: DateTime.UtcNow,
            FinishAt: DateTime.UtcNow.AddDays(7),
            MaxStudents: newMaxStudents,
            UpdatedAt: null);

        // Act
        var response = await Client.PostAsJsonAsync("courses", request);
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var fromResponse = await response.ToResponseModel<CourseDto>();
        var courseId = new CourseId(fromResponse.Id!.Value);

        var fromDataBase = await Context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
        fromDataBase.Should().NotBeNull();

        fromDataBase!.Name.Should().Be("TestCourseName");
        fromDataBase!.MaxStudents.Should().Be(10);

    }

    
    [Fact]
    public async Task ShouldUpdateCourse()
    {
        // Arrange
        var newName = "NewTestCourseName";
        var newMaxStudents = 11;
        var request = new CourseDto(
            Id: _mainCourse.Id!.Value,
            Name: newName,
            StartAt: DateTime.UtcNow,
            FinishAt: DateTime.UtcNow.AddDays(7),
            MaxStudents: newMaxStudents,
            UpdatedAt: null);


        // Act
        var response = await Client.PutAsJsonAsync("courses", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var fromResponse = await response.ToResponseModel<CourseDto>();

        var fromDataBase = await Context.Courses
            .FirstOrDefaultAsync(x => x.Id == new CourseId(fromResponse.Id!.Value));

        fromDataBase.Should().NotBeNull();

        fromDataBase!.Name.Should().Be(newName);
        fromDataBase!.MaxStudents.Should().Be(newMaxStudents);

    }
    [Fact]
    public async Task ShouldNotCreateCourseBecauseNameDuplicated()
    {
        // Arrange
        var newMaxStudents = 11;
        var request = new CourseDto(
            Id: Guid.NewGuid(),
            Name: _mainCourse.Name,
            StartAt: DateTime.UtcNow,
            FinishAt: DateTime.UtcNow.AddDays(7),
            MaxStudents: newMaxStudents,
            UpdatedAt: null);

        // Act
        var response = await Client.PostAsJsonAsync("courses", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotUpdateCourseBecauseCourseNotFound()
    {
        // Arrange
        var newMaxStudents = 11;
        var request = new CourseDto(
            Id: Guid.NewGuid(),
            Name: _mainCourse.Name,
            StartAt: DateTime.UtcNow,
            FinishAt: DateTime.UtcNow.AddDays(7),
            MaxStudents: newMaxStudents,
            UpdatedAt: null);

        // Act
        var response = await Client.PutAsJsonAsync("courses", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    

    public async Task InitializeAsync()
    {
        await Context.Courses.AddAsync(_mainCourse);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Courses.RemoveRange(Context.Courses);

        await SaveChangesAsync();
    }
}