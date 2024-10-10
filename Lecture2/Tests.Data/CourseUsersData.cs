using Domain.Courses;
using Domain.CoursesUsers;
using Domain.Faculties;
using Domain.Users;

namespace Tests.Data;

public static class CourseUsersData
{
    public static CourseUser MainCourseUser(CourseId courseId , UserId userId)
        => CourseUser.New(CourseUserId.New(), courseId, userId, 45, DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
}