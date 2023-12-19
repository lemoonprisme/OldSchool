using OldSchool.Domain.Models;

namespace OldSchool.Application.Tests;

public class ScoreBuilder
{
    private Score _score;

    public ScoreBuilder()
    {
        _score = new Score();
        _score.ScoreId = 1;
        _score.StudentId = 1;
        _score.Mark = 2;
        _score.Subject = "Math";
    }

    public ScoreBuilder SetMark(int mark)
    {
        _score.Mark = mark;
        return this;
    }
    public ScoreBuilder SetSubject(string subject)
    {
        _score.Subject = subject;
        return this;
    }
    public ScoreBuilder SetStudentId(int id)
    {
        _score.StudentId = id;
        return this;
    }
    public ScoreBuilder SetScoreId(int id)
    {
        _score.ScoreId = id;
        return this;
    }

    public Score Build()
    {
        return _score;
    }
}