using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using Domain.CoursesUsers;
using Domain.Faculties;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CourseUserRepository(ApplicationDbContext context) : ICourseUserRepository, ICourseUserQueries
{
    public async Task<IReadOnlyList<CourseUser>> GetAll(CancellationToken cancellationToken)
    {
        return await context.CourseUsers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<CourseUser>> GetAllCourses(CourseId courseId, CancellationToken cancellationToken)
    {
        return await context.CourseUsers
            .AsNoTracking()
            .Where(c => c.CourseId == courseId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<CourseUser>> UpdateAllCourses(List<CourseUser> courses, CancellationToken cancellationToken)
    {
        context.CourseUsers
            .UpdateRange(courses);
        await context.SaveChangesAsync(cancellationToken);  

        return courses;       
    }

    public async Task<Option<CourseUser>> GetById(CourseUserId id, CancellationToken cancellationToken)
    {
        var entity = await context.CourseUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<CourseUser>() : Option.Some(entity);
    }

    public async Task<CourseUser> Add(CourseUser course, CancellationToken cancellationToken)
    {
        await context.CourseUsers.AddAsync(course, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return course;
    }

    public async Task<CourseUser> Update(CourseUser course, CancellationToken cancellationToken)
    {
        context.CourseUsers.Update(course);

        await context.SaveChangesAsync(cancellationToken);

        return course;
    }

    public async Task<Option<CourseUser>> GetByCourseIdAndUserId(CourseId courseId, UserId userId, CancellationToken cancellationToken)
    {
        var entity = await context.CourseUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId, cancellationToken);

        return entity == null ? Option.None<CourseUser>() : Option.Some(entity);
    }
    

    public async Task<CourseUser> Delete(CourseUser course, CancellationToken cancellationToken)
    {
        context.CourseUsers.Remove(course);

        await context.SaveChangesAsync(cancellationToken);

        return course;
    }
}
