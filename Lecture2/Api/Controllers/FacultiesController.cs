using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Faculties.Commands;
using Application.Users.Commands;
using Domain.Faculties;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("faculties")]
[ApiController]
public class FacultiesController(ISender sender, IFacultyQueries facultyQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FacultyDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await facultyQueries.GetAll(cancellationToken);

        return entities.Select(FacultyDto.FromDomainModel).ToList();
    }
    
    [HttpGet("{facultyId:guid}")]
    public async Task<ActionResult<FacultyDto>> Get([FromRoute] Guid facultyId, CancellationToken cancellationToken)
    {
        var entity = await facultyQueries.GetById(new FacultyId(facultyId), cancellationToken);

        return entity.Match<ActionResult<FacultyDto>>(
            u => FacultyDto.FromDomainModel(u),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<FacultyDto>> Create(
        [FromBody] FacultyDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateFacultyCommand
        {
            Name = request.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<FacultyDto>>(
            f => FacultyDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("{facultyId:guid}")]
    public async Task<ActionResult<FacultyDto>> Delete([FromRoute] Guid facultyId, CancellationToken cancellationToken)
    {
        var input = new DeleteFacultyCommand
        {
            FacultyId = facultyId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<FacultyDto>>(
            u => FacultyDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }
    
    [HttpPut]
    public async Task<ActionResult<FacultyDto>> Update(
        [FromBody] FacultyDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateFacultyCommand
        {
            FacultyId = request.Id!.Value,
            Name = request.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<FacultyDto>>(
            f => FacultyDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
}