using Domain.Faculties;

namespace Tests.Data;

public static class FacultiesData
{
    public static Faculty MainFaculty => Faculty.New(FacultyId.New(), "Main Test Faculty");
}