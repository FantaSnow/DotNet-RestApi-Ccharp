
using Domain.Courses;
using Domain.Users;

namespace Domain.CoursesUsers;

public class CourseUser
{
    public CourseUserId Id { get; }
    
    public CourseId CourseId { get; private set; }
    public Course? Course { get; private set; }
    
    public UserId UserId { get; private set;  }
    public User? User { get; private set;  } 

    public int Raiting { get; private set;  }   
    public DateTime JoinAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsJoined { get; private set; }
    
    private CourseUser(CourseUserId id, CourseId courseId, UserId userId, int raiting, DateTime joinAt, DateTime endAt, DateTime updatedAt, bool isJoined)
    {
        Id = id;
        CourseId = courseId;
        UserId = userId;
        Raiting = raiting;
        JoinAt = joinAt;
        EndAt = endAt;
        UpdatedAt = updatedAt;
        IsJoined = isJoined;
    }    
    
    public static CourseUser New(CourseUserId id, CourseId courseId, UserId userId, int raiting, DateTime joinAt, DateTime endAt)
        => new( id, courseId, userId, raiting, joinAt, endAt,DateTime.UtcNow, true);
    
    public void UpdateDetails(CourseId courseId, UserId userId, int raiting, DateTime joinAt, DateTime endAt)
    {
        CourseId = courseId;
        UserId = userId;
        Raiting = raiting;
        JoinAt = joinAt;
        EndAt = endAt;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void EndCourseForAll()
    {
        IsJoined = false;
    }
    
    public void UpdateRaitingForEnd(int raiting)
    {
        Raiting = raiting;
        UpdatedAt = DateTime.UtcNow;
        EndAt = DateTime.UtcNow;
        IsJoined = false;
    }
}