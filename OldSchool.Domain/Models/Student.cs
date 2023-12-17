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

public class Score
{
    public string Subject { get; set; }
    public double Mark { get; set; }
    public int StudentId { get; set; }
    public int  ScoreId { get; set; }

    public Score()
    {
        
    }
    public Score(string subject, double mark, int studentId, int scoreId)
    {
        Subject = subject;
        Mark = mark;
        StudentId = studentId;
        ScoreId = scoreId;
    }
}

public enum Gender
{
    Male,
    Female
}