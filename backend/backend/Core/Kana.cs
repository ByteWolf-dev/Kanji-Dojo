namespace Core;

public static class Kana
{
    public static string ToHiragana(string text)
    {
        // katakana and hiragana blocks are 0x60 apart
        return new string(text
            .Select(c => c is >= '\u30A1' and <= '\u30F6' ? (char)(c - 0x60) : c)
            .ToArray());
    }

    public static bool Matches(string answer, string expected)
    {
        return string.Equals(
            ToHiragana(answer.Trim()),
            ToHiragana(expected.Trim()),
            StringComparison.OrdinalIgnoreCase);
    }
}
