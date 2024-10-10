namespace Domain.CoursesUsers;

public record CourseUserId(Guid Value)
{
    public static CourseUserId Empty => new(Guid.Empty);
    public static CourseUserId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}