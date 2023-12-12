namespace OldSchool.Domain.Models;

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int SchoolId { get; set; }
    public Gender Gender { get; set; }
    public List<Score> Scores  { get; set; }
}

public record Score(string Subject, double Mark);

public enum Gender
{
    Male,
    Female
}