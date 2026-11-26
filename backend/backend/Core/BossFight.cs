namespace Core;

public record BossFightAnswer(int ItemId, string Answer);

public record BossFightResult(int Correct, int Total, bool Passed);

public static class BossFight
{
    public const int QuestionCount = 10;
    public const double PassRate = 0.8;

    public static BossFightResult Grade(
        IReadOnlyCollection<BossFightAnswer> answers,
        IReadOnlyDictionary<int, List<string>> readingsByItem)
    {
        var correct = answers.Count(a =>
            readingsByItem.TryGetValue(a.ItemId, out var readings) &&
            readings.Any(r => Kana.Matches(a.Answer, r)));

        return new BossFightResult(
            correct,
            answers.Count,
            answers.Count > 0 && correct >= answers.Count * PassRate);
    }
}
