using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Faculties;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Faculties;

public class FacultiesControllerTests(IntegrationTestWebFactory factory)
    : BaseIntegrationTest(factory), IAsyncLifetime
{
    private readonly Faculty _mainFaculty = FacultiesData.MainFaculty;

    [Fact]
    public async Task ShouldCreateFaculty()
    {
        // Arrange
        var facultyName = "From Test Faculty";
        var request = new FacultyDto(
            Id: null,
            Name: facultyName,
            UpdatedAt: null);

        // Act
        var response = await Client.PostAsJsonAsync("faculties", request);
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var facultyFromResponse = await response.ToResponseModel<FacultyDto>();
        var facultyId = new FacultyId(facultyFromResponse.Id!.Value);

        var facultyFromDataBase = await Context.Faculties.FirstOrDefaultAsync(x => x.Id == facultyId);
        facultyFromDataBase.Should().NotBeNull();

        facultyFromDataBase!.Name.Should().Be(facultyName);
    }
    [Fact]
    public async Task ShouldDeleteFaculty()
    {
        // Arrange
        var facultyId = _mainFaculty.Id.Value;

        // Act
        var response = await Client.DeleteAsync($"faculties/{facultyId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue(); 
        response.Should().NotBeNull(); 

        var facultyAfterDelete = await Context.Faculties.FirstOrDefaultAsync(x => x.Id == new FacultyId(facultyId));
        facultyAfterDelete.Should().BeNull();

    }

    [Fact]
    public async Task ShouldNotDeleteFacultyBecauseOfNotFound()
    {
        // Arrange
        var facultyId =  Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"faculties/{facultyId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse(); 
        
        var facultyAfterDelete = await Context.Faculties.FirstOrDefaultAsync(x => x.Id == new FacultyId(facultyId));
        facultyAfterDelete.Should().BeNull();
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }
    
    [Fact]
    public async Task ShouldGetFacultyById()
    {
        // Arrange
        var facultyId = _mainFaculty.Id.Value;

        // Act
        var response = await Client.GetAsync($"faculties/{facultyId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var facultyFromResponse = await response.ToResponseModel<FacultyDto>();
        facultyFromResponse.Should().NotBeNull();
        facultyFromResponse.Id.Should().Be(facultyId);
        facultyFromResponse.Name.Should().Be(_mainFaculty.Name);
    }
    [Fact]
    public async Task ShouldNotGetFacultyByIdBecauseNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid(); 

        // Act
        var response = await Client.GetAsync($"faculties/{facultyId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    [Fact]
    public async Task ShouldUpdateFaculty()
    {
        // Arrange
        var newFacultyName = "New Faculty Name";
        var request = new FacultyDto(
            Id: _mainFaculty.Id.Value,
            Name: newFacultyName,
            UpdatedAt: null);

        // Act
        var response = await Client.PutAsJsonAsync("faculties", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var facultyFromResponse = await response.ToResponseModel<FacultyDto>();

        var facultyFromDataBase = await Context.Faculties
            .FirstOrDefaultAsync(x => x.Id == new FacultyId(facultyFromResponse.Id!.Value));

        facultyFromDataBase.Should().NotBeNull();

        facultyFromDataBase!.Name.Should().Be(newFacultyName);
    }

    [Fact]
    public async Task ShouldNotCreateFacultyBecauseNameDuplicated()
    {
        // Arrange
        var request = new FacultyDto(
            Id: null,
            Name: _mainFaculty.Name,
            UpdatedAt: null);

        // Act
        var response = await Client.PostAsJsonAsync("faculties", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotUpdateFacultyBecauseFacultyNotFound()
    {
        // Arrange
        var request = new FacultyDto(
            Id: Guid.NewGuid(),
            Name: "New Faculty Name",
            UpdatedAt: null);

        // Act
        var response = await Client.PutAsJsonAsync("faculties", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    

    public async Task InitializeAsync()
    {
        await Context.Faculties.AddAsync(_mainFaculty);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Faculties.RemoveRange(Context.Faculties);

        await SaveChangesAsync();
    }
}