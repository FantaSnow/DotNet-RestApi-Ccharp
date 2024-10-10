using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Courses;
using Domain.CoursesUsers;
using Domain.Faculties;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.CourseUsers;

public class CourseUsersControllerTests : BaseIntegrationTest, IAsyncLifetime
{
        private readonly Course _mainCourse = CoursesData.MainCourse;
        private readonly Faculty _mainFaculty = FacultiesData.MainFaculty;
        private readonly CourseUser _mainCourseUser;
        private readonly User _mainUser;
        private readonly User _secondUser;


    public CourseUsersControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _secondUser = UsersData.MainUser(_mainFaculty.Id);
        _mainUser = UsersData.MainUser(_mainFaculty.Id);
        _mainCourseUser = CourseUsersData.MainCourseUser(_mainCourse.Id, _mainUser.Id);
    }
    [Fact]
    public async Task ShouldCreateCourseUser()
    {
        // Arrange
        var newRaiting = 10;
        var request = new CourseUserDto(
            Id: null,
            CourseId: _mainCourse.Id.Value,
            Course: null, 
            UserId: _secondUser.Id.Value,
            User: null,
            Raiting: newRaiting,
            JoinAt: DateTime.UtcNow,
            EndAt: DateTime.UtcNow.AddMonths(1),
            UpdatedAt: null,
            IsJoined: true
            );

        // Act
        var response = await Client.PostAsJsonAsync("courseUsers", request);
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var fromResponse = await response.ToResponseModel<CourseUserDto>();
        var courseUserId = new CourseUserId(fromResponse.Id!.Value);

        var fromDataBase = await Context.CourseUsers.FirstOrDefaultAsync(x => x.Id == courseUserId);
        fromDataBase.Should().NotBeNull();

        fromDataBase!.CourseId.Should().Be(_mainCourse.Id);
        fromDataBase!.UserId.Should().Be(_secondUser.Id);
        fromDataBase!.Raiting.Should().Be(10);
        fromDataBase!.IsJoined.Should().Be(true);

    }

    
    [Fact]
    public async Task ShouldUpdateCourse()
    {
        // Arrange
        var newRaiting = 12;
        var request = new CourseUserDto(
            Id: _mainCourseUser.Id.Value,
            CourseId: _mainCourse.Id.Value,
            Course: null, 
            UserId: _mainUser.Id.Value,
            User: null,
            Raiting: newRaiting,
            JoinAt: DateTime.UtcNow,
            EndAt: DateTime.UtcNow.AddMonths(1),
            UpdatedAt: null,
            IsJoined: true
        );
    
    
        // Act
        var response = await Client.PutAsJsonAsync("courseUsers", request);
    
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    
        var fromResponse = await response.ToResponseModel<CourseUserDto>();
    
        var fromDataBase = await Context.CourseUsers
            .FirstOrDefaultAsync(x => x.Id == new CourseUserId(fromResponse.Id!.Value));
    
        fromDataBase.Should().NotBeNull();
    
        fromDataBase!.Raiting.Should().Be(newRaiting);
    
    }
    [Fact]
    public async Task ShouldNotCreateCourseUserBecauseUserIdAndCourseIdAlreadyExists()
    {
        // Arrange
        var newRaiting = 12;
        var request = new CourseUserDto(
            Id: Guid.NewGuid(),
            CourseId: _mainCourse.Id.Value,
            Course: null, 
            UserId: _mainUser.Id.Value,
            User: null,
            Raiting: newRaiting,
            JoinAt: DateTime.UtcNow,
            EndAt: DateTime.UtcNow.AddMonths(1),
            UpdatedAt: null,
            IsJoined: true
        );
    
        // Act
        var response = await Client.PostAsJsonAsync("courseUsers", request);
    
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async Task ShouldNotUpdateCourseBecauseCourseNotFound()
    {
        // Arrange
        var request = new CourseUserDto(
            Id: Guid.NewGuid(),
            CourseId: _mainCourse.Id.Value,
            Course: null, 
            UserId: _mainUser.Id.Value,
            User: null,
            Raiting: 44,
            JoinAt: DateTime.UtcNow,
            EndAt: DateTime.UtcNow.AddMonths(1),
            UpdatedAt: null,
            IsJoined: true
        );
        // Act
        var response = await Client.PutAsJsonAsync("courseUsers", request);
    
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    

    public async Task InitializeAsync()
    {
        await Context.Courses.AddAsync(_mainCourse);
        await Context.Faculties.AddAsync(_mainFaculty);
        await Context.Users.AddAsync(_secondUser);
        await Context.Users.AddAsync(_mainUser);
        await Context.CourseUsers.AddAsync(_mainCourseUser);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Courses.RemoveRange(Context.Courses);
        Context.Users.RemoveRange(Context.Users);
        Context.Faculties.RemoveRange(Context.Faculties);
        Context.CourseUsers.RemoveRange(Context.CourseUsers);

        await SaveChangesAsync();
    }
}