namespace Domain.Faculties;

public class Faculty
{
    public FacultyId Id { get; }
    public string Name { get; private set; }
    public DateTime UpdatedAt { get; private set; }


    private Faculty(FacultyId id, string name, DateTime updatedAt)
    {
        Id = id;
        Name = name;
        UpdatedAt = updatedAt;
    }

    public static Faculty New(FacultyId id, string name)
        => new(id, name, DateTime.UtcNow);
    
    public void UpdateDetails(string name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }
}