using Domain.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(KanjiDbContext db)
    {
        await using var transaction =
            await db.Database.BeginTransactionAsync();

        await db.Reviews.ExecuteDeleteAsync();
        await db.Cards.ExecuteDeleteAsync();
        await db.ItemReadings.ExecuteDeleteAsync();
        await db.Items.ExecuteDeleteAsync();
        await db.Users.ExecuteDeleteAsync();
        await db.Belts.ExecuteDeleteAsync();

        var random = new Random(42);
        var now = DateTime.UtcNow;
        var hasher = new PasswordHasher<User>();

        var belts = new[]
        {
            new Belt
            {
                Name = "Ace",
                Rank = Rank.Ace,
                ColorHex = "#FFFFFF",
                RequiredScore = 0
            },
            new Belt
            {
                Name = "Two",
                Rank = Rank.Two,
                ColorHex = "#FACC15",
                RequiredScore = 100
            },
            new Belt
            {
                Name = "Three",
                Rank = Rank.Three,
                ColorHex = "#22C55E",
                RequiredScore = 300
            },
            new Belt
            {
                Name = "Four",
                Rank = Rank.Four,
                ColorHex = "#2563EB",
                RequiredScore = 600
            }
        };

        db.Belts.AddRange(belts);
        await db.SaveChangesAsync();

        var usernames = new[]
        {
            "admin",
            "testuser",
            "demo",
            "akira",
            "yuki",
            "haruto",
            "sakura",
            "ren",
            "hana",
            "kaito"
        };

        var users = usernames.Select((name, index) =>
            new User
            {
                Name = name,
                BeltId = belts[index % belts.Length].Id
            }).ToList();

        foreach (var user in users)
            user.PasswordHash = hasher.HashPassword(
                user,
                "TestPassword123!"
            );

        db.Users.AddRange(users);
        await db.SaveChangesAsync();

        string[] hiragana =
        {
            "あ", "い", "う", "え", "お",
            "か", "き", "く", "け", "こ",
            "さ", "し", "す", "せ", "そ",
            "た", "ち", "つ", "て", "と",
            "な", "に", "ぬ", "ね", "の",
            "は", "ひ", "ふ", "へ", "ほ",
            "ま", "み", "む", "め", "も",
            "や", "ゆ", "よ",
            "ら", "り", "る", "れ", "ろ",
            "わ", "を", "ん"
        };

        string[] katakana =
        {
            "ア", "イ", "ウ", "エ", "オ",
            "カ", "キ", "ク", "ケ", "コ",
            "サ", "シ", "ス", "セ", "ソ",
            "タ", "チ", "ツ", "テ", "ト",
            "ナ", "ニ", "ヌ", "ネ", "ノ",
            "ハ", "ヒ", "フ", "ヘ", "ホ",
            "マ", "ミ", "ム", "メ", "モ",
            "ヤ", "ユ", "ヨ",
            "ラ", "リ", "ル", "レ", "ロ",
            "ワ", "ヲ", "ン"
        };

        string[] romaji =
        {
            "a", "i", "u", "e", "o",
            "ka", "ki", "ku", "ke", "ko",
            "sa", "shi", "su", "se", "so",
            "ta", "chi", "tsu", "te", "to",
            "na", "ni", "nu", "ne", "no",
            "ha", "hi", "fu", "he", "ho",
            "ma", "mi", "mu", "me", "mo",
            "ya", "yu", "yo",
            "ra", "ri", "ru", "re", "ro",
            "wa", "wo", "n"
        };

        var items = new List<Item>();
        var readings = new List<ItemReading>();

        for (var i = 0; i < hiragana.Length; i++)
        {
            var item = new Item
            {
                Character = hiragana[i],
                CharacterType = CharacterType.Hiragana,
                BeltId = belts[0].Id
            };

            items.Add(item);

            readings.Add(new ItemReading
            {
                Item = item,
                ReadingKana = hiragana[i],
                ReadingRomaji = romaji[i],
                Pronunciation = Pronunciation.Standard
            });
        }

        for (var i = 0; i < katakana.Length; i++)
        {
            var item = new Item
            {
                Character = katakana[i],
                CharacterType = CharacterType.Katakana,
                BeltId = belts[1].Id
            };

            items.Add(item);

            readings.Add(new ItemReading
            {
                Item = item,
                ReadingKana = katakana[i],
                ReadingRomaji = romaji[i],
                Pronunciation = Pronunciation.Standard
            });
        }

        var kanji = new[]
        {
            ("水", "Water", "みず", "mizu", "スイ", "sui"),
            ("火", "Fire", "ひ", "hi", "カ", "ka"),
            ("木", "Tree", "き", "ki", "モク", "moku"),
            ("金", "Gold", "かね", "kane", "キン", "kin"),
            ("土", "Earth", "つち", "tsuchi", "ド", "do"),
            ("日", "Sun", "ひ", "hi", "ニチ", "nichi"),
            ("月", "Moon", "つき", "tsuki", "ゲツ", "getsu"),
            ("山", "Mountain", "やま", "yama", "サン", "san"),
            ("川", "River", "かわ", "kawa", "セン", "sen"),
            ("人", "Person", "ひと", "hito", "ジン", "jin"),
            ("口", "Mouth", "くち", "kuchi", "コウ", "kou"),
            ("目", "Eye", "め", "me", "モク", "moku"),
            ("耳", "Ear", "みみ", "mimi", "ジ", "ji"),
            ("手", "Hand", "て", "te", "シュ", "shu"),
            ("足", "Foot", "あし", "ashi", "ソク", "soku"),
            ("雨", "Rain", "あめ", "ame", "ウ", "u"),
            ("空", "Sky", "そら", "sora", "クウ", "kuu"),
            ("花", "Flower", "はな", "hana", "カ", "ka"),
            ("犬", "Dog", "いぬ", "inu", "ケン", "ken"),
            ("猫", "Cat", "ねこ", "neko", "ビョウ", "byou")
        };

        foreach (var entry in kanji)
        {
            var item = new Item
            {
                Character = entry.Item1,
                Meaning = entry.Item2,
                CharacterType = CharacterType.Kanji,
                BeltId = belts[2].Id
            };

            items.Add(item);

            readings.Add(new ItemReading
            {
                Item = item,
                ReadingKana = entry.Item3,
                ReadingRomaji = entry.Item4,
                Pronunciation = Pronunciation.KunYomi
            });

            readings.Add(new ItemReading
            {
                Item = item,
                ReadingKana = entry.Item5,
                ReadingRomaji = entry.Item6,
                Pronunciation = Pronunciation.OnYomi
            });
        }

        db.Items.AddRange(items);
        db.ItemReadings.AddRange(readings);

        await db.SaveChangesAsync();

        var cards = new List<Card>();

        foreach (var user in users)
        {
            var availableCards = items
                .SelectMany(item =>
                    Enum.GetValues<CardType>()
                        .Where(type =>
                            type != CardType.Meaning ||
                            item.CharacterType == CharacterType.Kanji)
                        .Select(type => new
                        {
                            Item = item,
                            Type = type
                        }))
                .OrderBy(_ => random.Next())
                .Take(150);

            foreach (var entry in availableCards)
                cards.Add(new Card
                {
                    UserId = user.Id,
                    ItemId = entry.Item.Id,
                    CardType = entry.Type,
                    EaseFactor = 2.5,
                    IntervalDays = 0,
                    Repetitions = 0,
                    DueAt = now.AddDays(-random.Next(0, 31))
                });
        }

        db.Cards.AddRange(cards);
        await db.SaveChangesAsync();

        var reviews = new List<Review>();

        var itemsById = items.ToDictionary(i => i.Id);

        var readingsByItem = readings
            .GroupBy(r => r.ItemId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var card in cards)
        {
            var item = itemsById[card.ItemId];

            var reviewCount = random.Next(0, 7);

            var reviewDate = card.DueAt;

            for (var i = 0; i < reviewCount; i++)
            {
                if (reviewDate > now)
                    break;

                var grade = random.Next(0, 6);

                var isCorrect = grade >= 3;

                var correctAnswer = card.CardType switch
                {
                    CardType.Writing => item.Character,
                    CardType.Meaning => item.Meaning ?? item.Character,
                    _ => readingsByItem[item.Id][0].ReadingRomaji
                };

                reviews.Add(new Review
                {
                    CardId = card.Id,
                    ReviewedAt = reviewDate,
                    Grade = grade,
                    IsCorrect = isCorrect,
                    Answer = isCorrect
                        ? correctAnswer
                        : "unknown"
                });

                if (grade >= 3)
                {
                    card.IntervalDays = card.Repetitions switch
                    {
                        0 => 1,
                        1 => 6,
                        _ => (int)Math.Ceiling(
                            card.IntervalDays * card.EaseFactor)
                    };

                    card.Repetitions++;
                }
                else
                {
                    card.Repetitions = 0;
                    card.IntervalDays = 1;
                }

                card.EaseFactor = Math.Max(
                    1.3,
                    card.EaseFactor + 0.1 -
                    (5 - grade) *
                    (0.08 + (5 - grade) * 0.02)
                );

                card.LastReviewAt = reviewDate;

                card.DueAt = reviewDate.AddDays(
                    card.IntervalDays
                );

                reviewDate = card.DueAt;
            }
        }

        db.Reviews.AddRange(reviews);

        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        Console.WriteLine("Database successfully seeded.");
        Console.WriteLine($"Belts: {belts.Length}");
        Console.WriteLine($"Users: {users.Count}");
        Console.WriteLine($"Items: {items.Count}");
        Console.WriteLine($"Readings: {readings.Count}");
        Console.WriteLine($"Cards: {cards.Count}");
        Console.WriteLine($"Reviews: {reviews.Count}");
    }
}