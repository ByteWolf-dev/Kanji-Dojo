using Core;
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
                Name = "White",
                Rank = Rank.Ace,
                ColorHex = "#FFFFFF",
                RequiredScore = 0
            },
            new Belt
            {
                Name = "Yellow",
                Rank = Rank.Two,
                ColorHex = "#FACC15",
                RequiredScore = 100
            },
            new Belt
            {
                Name = "Green",
                Rank = Rank.Three,
                ColorHex = "#22C55E",
                RequiredScore = 300
            },
            new Belt
            {
                Name = "Blue",
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

        var demo = users.Single(u => u.Name == "demo");
        demo.BeltId = belts[1].Id;

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

        // JLPT N5 kanji, readings and meanings from KANJIDIC2 (EDRDG, CC BY-SA 4.0)
        // the first half is the green belt, the second half the blue belt
        var kanji = new (string Character, string Meaning, string? KunKana, string? KunRomaji, string OnKana, string OnRomaji)[]
        {
            ("一", "one", "ひと", "hito", "イチ", "ichi"),
            ("二", "two", "ふた", "futa", "ニ", "ni"),
            ("三", "three", "み", "mi", "サン", "san"),
            ("四", "four", "よん", "yon", "シ", "shi"),
            ("五", "five", "いつ", "itsu", "ゴ", "go"),
            ("六", "six", "む", "mu", "ロク", "roku"),
            ("七", "seven", "なな", "nana", "シチ", "shichi"),
            ("八", "eight", "や", "ya", "ハチ", "hachi"),
            ("九", "nine", "ここの", "kokono", "キュウ", "kyuu"),
            ("十", "ten", "とお", "too", "ジュウ", "juu"),
            ("百", "hundred", null, null, "ヒャク", "hyaku"),
            ("千", "thousand", "ち", "chi", "セン", "sen"),
            ("万", "ten thousand", null, null, "マン", "man"),
            ("円", "yen, circle", "まる", "maru", "エン", "en"),
            ("日", "day, sun", "ひ", "hi", "ニチ", "nichi"),
            ("月", "month, moon", "つき", "tsuki", "ゲツ", "getsu"),
            ("火", "fire", "ひ", "hi", "カ", "ka"),
            ("水", "water", "みず", "mizu", "スイ", "sui"),
            ("木", "tree, wood", "き", "ki", "モク", "moku"),
            ("金", "gold, money", "かね", "kane", "キン", "kin"),
            ("土", "earth, soil", "つち", "tsuchi", "ド", "do"),
            ("年", "year", "とし", "toshi", "ネン", "nen"),
            ("時", "time, hour", "とき", "toki", "ジ", "ji"),
            ("今", "now", "いま", "ima", "コン", "kon"),
            ("午", "noon", null, null, "ゴ", "go"),
            ("半", "half", "なか", "naka", "ハン", "han"),
            ("人", "person", "ひと", "hito", "ジン", "jin"),
            ("男", "man", "おとこ", "otoko", "ダン", "dan"),
            ("女", "woman", "おんな", "onna", "ジョ", "jo"),
            ("子", "child", "こ", "ko", "シ", "shi"),
            ("父", "father", "ちち", "chichi", "フ", "fu"),
            ("母", "mother", "はは", "haha", "ボ", "bo"),
            ("友", "friend", "とも", "tomo", "ユウ", "yuu"),
            ("山", "mountain", "やま", "yama", "サン", "san"),
            ("川", "river", "かわ", "kawa", "セン", "sen"),
            ("天", "heaven, sky", "あま", "ama", "テン", "ten"),
            ("雨", "rain", "あめ", "ame", "ウ", "u"),
            ("気", "spirit, air", null, null, "キ", "ki"),
            ("白", "white", "しろ", "shiro", "ハク", "haku"),
            ("名", "name", "な", "na", "メイ", "mei"),
            ("上", "up, above", "うえ", "ue", "ジョウ", "jou"),
            ("下", "down, below", "した", "shita", "カ", "ka"),
            ("中", "inside, middle", "なか", "naka", "チュウ", "chuu"),
            ("外", "outside", "そと", "soto", "ガイ", "gai"),
            ("右", "right", "みぎ", "migi", "ウ", "u"),
            ("左", "left", "ひだり", "hidari", "サ", "sa"),
            ("前", "before, front", "まえ", "mae", "ゼン", "zen"),
            ("後", "after, behind", "あと", "ato", "ゴ", "go"),
            ("間", "interval, between", "あいだ", "aida", "カン", "kan"),
            ("東", "east", "ひがし", "higashi", "トウ", "tou"),
            ("西", "west", "にし", "nishi", "セイ", "sei"),
            ("南", "south", "みなみ", "minami", "ナン", "nan"),
            ("北", "north", "きた", "kita", "ホク", "hoku"),
            ("大", "big", "おお", "oo", "ダイ", "dai"),
            ("小", "small", "ちい", "chii", "ショウ", "shou"),
            ("長", "long", "なが", "naga", "チョウ", "chou"),
            ("高", "tall, expensive", "たか", "taka", "コウ", "kou"),
            ("先", "ahead, previous", "さき", "saki", "セン", "sen"),
            ("毎", "every", null, null, "マイ", "mai"),
            ("何", "what", "なに", "nani", "カ", "ka"),
            ("本", "book, origin", "もと", "moto", "ホン", "hon"),
            ("国", "country", "くに", "kuni", "コク", "koku"),
            ("学", "study, learning", "まな", "mana", "ガク", "gaku"),
            ("校", "school", null, null, "コウ", "kou"),
            ("生", "life, birth", "い", "i", "セイ", "sei"),
            ("語", "word, language", "かた", "kata", "ゴ", "go"),
            ("電", "electricity", null, null, "デン", "den"),
            ("車", "car", "くるま", "kuruma", "シャ", "sha"),
            ("行", "go", "い", "i", "コウ", "kou"),
            ("来", "come", "く", "ku", "ライ", "rai"),
            ("出", "exit, go out", "で", "de", "シュツ", "shutsu"),
            ("入", "enter", "はい", "hai", "ニュウ", "nyuu"),
            ("見", "see", "み", "mi", "ケン", "ken"),
            ("聞", "hear, ask", "き", "ki", "ブン", "bun"),
            ("話", "talk, story", "はなし", "hanashi", "ワ", "wa"),
            ("読", "read", "よ", "yo", "ドク", "doku"),
            ("書", "write", "か", "ka", "ショ", "sho"),
            ("食", "eat, food", "た", "ta", "ショク", "shoku"),
            ("休", "rest", "やす", "yasu", "キュウ", "kyuu")
        };

        for (var i = 0; i < kanji.Length; i++)
        {
            var entry = kanji[i];

            var item = new Item
            {
                Character = entry.Character,
                Meaning = entry.Meaning,
                CharacterType = CharacterType.Kanji,
                JlptLevel = 5,
                BeltId = i < kanji.Length / 2 ? belts[2].Id : belts[3].Id
            };

            items.Add(item);

            if (entry.KunKana != null && entry.KunRomaji != null)
                readings.Add(new ItemReading
                {
                    Item = item,
                    ReadingKana = entry.KunKana,
                    ReadingRomaji = entry.KunRomaji,
                    Pronunciation = Pronunciation.KunYomi
                });

            readings.Add(new ItemReading
            {
                Item = item,
                ReadingKana = entry.OnKana,
                ReadingRomaji = entry.OnRomaji,
                Pronunciation = Pronunciation.OnYomi
            });
        }

        db.Items.AddRange(items);
        db.ItemReadings.AddRange(readings);

        await db.SaveChangesAsync();

        var cards = new List<Card>();

        var beltRanks = belts.ToDictionary(b => b.Id, b => b.Rank);

        foreach (var user in users)
        {
            var unlockedItems = items
                .Where(i => beltRanks[i.BeltId] <= beltRanks[user.BeltId]);

            var userCards = CardFactory.CreateCards(user.Id, unlockedItems, now);

            foreach (var card in userCards)
                card.DueAt = now.AddDays(-random.Next(0, 31));

            cards.AddRange(userCards);
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

                SchedulingAlgorithms.Review(card, grade, reviewDate);

                reviewDate = card.DueAt;
            }
        }

        // demo account: reviews on each of the last 14 days so it has a streak
        var demoCards = cards.Where(c => c.UserId == demo.Id).ToList();

        for (var day = 1; day <= 14; day++)
        {
            var date = now.Date.AddDays(-day).AddHours(random.Next(8, 20));

            foreach (var card in demoCards.OrderBy(_ => random.Next()).Take(random.Next(15, 25)))
            {
                var grade = random.Next(0, 10) < 9 ? random.Next(3, 6) : random.Next(0, 3);

                reviews.Add(new Review
                {
                    CardId = card.Id,
                    ReviewedAt = date,
                    Grade = grade,
                    IsCorrect = grade >= 3,
                    Answer = grade >= 3 ? itemsById[card.ItemId].Character : "unknown"
                });
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