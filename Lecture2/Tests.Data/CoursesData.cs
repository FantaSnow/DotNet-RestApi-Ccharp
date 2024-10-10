using Domain.Courses;
using Domain.Faculties;

namespace Tests.Data;

public static class CoursesData
{
    public static Course MainCourse => Course.New(CourseId.New(), "Test Name", DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5), 20);
}