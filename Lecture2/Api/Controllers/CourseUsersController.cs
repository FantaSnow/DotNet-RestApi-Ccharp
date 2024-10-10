using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.CourseUsers.Commands;
using Domain.CoursesUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("courseUsers")]
[ApiController]
public class CourseUsersController(ISender sender, ICourseUserQueries courseUserQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseUserDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await courseUserQueries.GetAll(cancellationToken);

        return entities.Select(CourseUserDto.FromDomainModel).ToList();
    }
    
    [HttpGet("{courseUserId:guid}")]
    public async Task<ActionResult<CourseUserDto>> Get([FromRoute] Guid courseUserId, CancellationToken cancellationToken)
    {
        var entity = await courseUserQueries.GetById(new CourseUserId(courseUserId), cancellationToken);

        return entity.Match<ActionResult<CourseUserDto>>(
            u => CourseUserDto.FromDomainModel(u),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CourseUserDto>> Create(
        [FromBody] CourseUserDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCourseUserCommand
        {
            CourseId = request.CourseId!.Value,
            UserId = request.UserId!.Value,
            Raiting = request.Raiting,
            JoinAt = request.JoinAt,
            EndAt = request.EndAt,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            f => CourseUserDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("{courseId:guid}")]
    public async Task<ActionResult<CourseUserDto>> Delete([FromRoute] Guid courseUserId, CancellationToken cancellationToken)
    {
        var input = new DeleteCourseUserCommand
        {
            CourseUserId = courseUserId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            u => CourseUserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }
    
    [HttpPut]
    public async Task<ActionResult<CourseUserDto>> Update(
        [FromBody] CourseUserDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCourseUserCommand
        {
            Id = request.Id!.Value,
            CourseId = request.CourseId!.Value,
            UserId = request.UserId!.Value,
            Raiting = request.Raiting,
            JoinAt = request.JoinAt,
            EndAt = request.EndAt
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            f => CourseUserDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpPut ("RaitingEdit")]
    public async Task<ActionResult<CourseUserDto>> EndCourseForUser(
        Guid courseUserId, int raiting,
        CancellationToken cancellationToken)
    {
        var input = new EndCourseUserCommand
        {
            Id = courseUserId,
            Raiting = raiting
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            f => CourseUserDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    [HttpPut ("EndCourseForAll")]
    public async Task<ActionResult<List<CourseUserDto>>> EndCourseForUser(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        var input = new EndCourseForAllCommand()
        {
            Id = courseId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Select(CourseUserDto.FromDomainModel).ToList();
    }
}