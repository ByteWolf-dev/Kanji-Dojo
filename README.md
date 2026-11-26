# Kanji Dojo 漢字道場

I wanted to learn hiragana, katakana and some basic kanji, and every app I tried either felt like a boring flashcard deck or wanted a subscription. So I built my own and made it a dojo: you start as a white belt and have to fight your way up.

Live: https://kanji.lykos.work (there's a "Try the demo" button on the login page, so you don't need an account)

## How it works

You get a few reviews every day. Cards you know well come back less often, cards you get wrong come back soon. That's SM-2, the same algorithm Anki is based on.

For readings you type romaji and it turns into kana while you type, so `konnichiha` becomes こんにちは. It handles the annoying cases too, like ん (`n`, `nn` or `n'`), small っ from double letters (`kitte` → きって) and long vowels in katakana (`ko-hi-` → コーヒー).

There are three kinds of cards:
- see the character, type the reading
- see the character, type the meaning
- see the romaji (or the meaning for kanji), pick the right character

The belts:

| Belt | What you learn | Correct answers needed |
|---|---|---|
| White | hiragana | - |
| Yellow | katakana | 100 |
| Green | first half of the N5 kanji | 300 |
| Blue | second half of the N5 kanji | 600 |

Once you have enough correct answers you can fight the boss: 10 characters from your current belt and you need 8 right. Win and you get the next belt with all its new cards. There's also a streak counter, because apparently I only do things if there's a streak.

The demo account already has two weeks of reviews and can fight the boss right away.

## Built with

- Backend: .NET 10, ASP.NET Core, EF Core, SQL Server
- Frontend: Angular 21 with Angular Material
- Tests: xUnit and Vitest
- Hosting: Docker Compose on my own server, behind a Cloudflare tunnel

```
backend/backend
  api      controllers, DTOs, auth, seeding
  Core     SM-2, boss fight, streaks, card creation
  Domain   entities, DbContext, migrations
  Tests    xUnit tests for Core
frontend   Angular app
```

## Some choices I made

All the actual rules (SM-2, the boss fight, streaks) are in `Core`, which is plain C# without EF Core or ASP.NET. That made them easy to test, and the controllers just load data, call Core and save.

The boss fight is graded on the server. Your belt depends on it, so I didn't want the browser to decide if you passed. The romaji conversion is the other way around and runs in the browser, because it has to happen on every key press.

I used cookies for login instead of JWT. The frontend and API are on the same domain anyway, and an HttpOnly cookie can't be read by JavaScript.

Streaks don't have their own table. They're counted from the days you did reviews, so they can't get out of sync.

I went with controllers instead of minimal APIs because there are quite a few endpoints and it was easier to keep them grouped that way.

## Running it locally

You need the .NET 10 SDK, Node 22 and SQL Server LocalDB.

```bash
cd backend/backend/api
dotnet run --launch-profile https              # creates and seeds the database on first start
dotnet run --launch-profile https -- --seed    # resets it to the seed data

cd frontend
npm install
npm start
```

Then open http://localhost:4200 and log in with `demo` / `TestPassword123!`.

Tests:

```bash
cd backend/backend && dotnet test
cd frontend && npx ng test --watch=false
```

## Deploying

```bash
cp .env.example .env    # put a real database password in there
docker compose up -d --build
```

It runs on port 10100. nginx serves the Angular app and passes `/api` on to the API, and the API sets up the database by itself on the first start.

## Credits

The kanji readings and meanings come from [KANJIDIC2](https://www.edrdg.org/wiki/index.php/KANJIDIC_Project), which belongs to the [Electronic Dictionary Research and Development Group](https://www.edrdg.org/) and is used under their [licence](https://www.edrdg.org/edrdg/licence.html) (CC BY-SA 4.0).
