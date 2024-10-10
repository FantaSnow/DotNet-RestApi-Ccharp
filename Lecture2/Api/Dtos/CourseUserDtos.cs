
using Domain.CoursesUsers;

namespace Api.Dtos;

public record CourseUserDto(
    Guid? Id,
    Guid? CourseId,
    CourseDto? Course,
    Guid? UserId,
    UserDto? User,
    int Raiting,
    DateTime JoinAt,
    DateTime EndAt,
    DateTime? UpdatedAt,
    bool IsJoined)
{
    public static CourseUserDto FromDomainModel(CourseUser courseUser)
        => new(
            Id: courseUser.Id.Value,
            CourseId: courseUser.CourseId.Value,
            Course: courseUser.Course == null ? null : CourseDto.FromDomainModel(courseUser.Course),
            UserId: courseUser.UserId.Value,
            User: courseUser.User == null ? null : UserDto.FromDomainModel(courseUser.User),
            Raiting: courseUser.Raiting,
            JoinAt: courseUser.JoinAt,
            EndAt: courseUser.EndAt,
            UpdatedAt: courseUser.UpdatedAt,
            IsJoined:courseUser.IsJoined
            );
}