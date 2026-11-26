# Kanji Dojo 漢字道場

A Japanese trainer for hiragana, katakana and the JLPT N5 kanji. Reviews are scheduled with spaced repetition (SM-2, the same idea Anki uses), you type romaji and it turns into kana while you type, and every level is a belt colour. To rank up you have to beat a short boss fight.

**Live demo:** https://kanji.lykos.work (click "Try the demo" on the login page)

## Features

- **Spaced repetition** – every card is scheduled with SM-2. Wrong answers reset the interval, the ease factor changes with how hard the card was.
- **Romaji → kana input** – type `konnichiha` and get こんにちは. Handles ん, small っ, combined syllables (きょ, しゃ…) and katakana with long vowel marks (`ko-hi-` → コーヒー).
- **Three card types** – recognition (type the reading), meaning (type the English meaning) and writing (pick the right character).
- **Belts** – white (hiragana), yellow (katakana), green and blue (N5 kanji). Every belt unlocks new cards.
- **Boss fight** – once your score is high enough you can challenge the boss: 10 characters from your belt, 80% to pass. Graded on the server.
- **Streaks** – current and longest streak of days with at least one review.
- **Demo account** with a few weeks of progress already made.

## Tech stack

| | |
|---|---|
| Backend | .NET 10, ASP.NET Core, C# |
| Data | EF Core (code first, migrations), SQL Server |
| Frontend | Angular 21 (standalone components, signals), Angular Material |
| Tests | xUnit (backend), Vitest (frontend) |
| Deployment | Docker Compose, nginx, Cloudflare tunnel |

## Project structure

```
backend/backend
├── api      controllers, DTOs + mappers, auth, seeding
├── Core     SM-2, boss fight rules, streaks, card creation (no database code)
├── Domain   entities, DbContext, migrations
└── Tests    xUnit tests for everything in Core
frontend     Angular app
```

## Decisions

- **The rules live in Core.** SM-2, the boss fight, streaks and card creation are plain C# with no EF Core or ASP.NET in them, so they're easy to test. The controllers only load data, call Core and save.
- **Records for results.** `BossFightResult` and `BossFightAnswer` are records: they are values that never change after they're created, and you get equality for free, which makes the tests simple (`Assert.Equal(new BossFightResult(8, 10, true), result)`).
- **Controllers instead of minimal APIs.** With eight resources, grouping the endpoints per controller with constructor injection kept things organised. For a small service with a few endpoints I'd use minimal APIs.
- **Cookie auth instead of JWT.** The frontend and API are served from the same origin, so an HttpOnly cookie is simpler and can't be read by JavaScript.
- **Romaji conversion on the client, grading of the boss fight on the server.** Conversion has to happen while you type. The boss fight decides your belt, so the server checks the answers against the readings itself.
- **Streaks are calculated from reviews.** No extra table: the distinct review days are queried and the streak is counted in Core.

## Running locally

Requirements: .NET 10 SDK, Node 22, SQL Server LocalDB.

```bash
# backend (creates and seeds the database on first start)
cd backend/backend/api
dotnet run --launch-profile https

# reset the database to the seed data
dotnet run --launch-profile https -- --seed

# frontend (proxies /api to the backend)
cd frontend
npm install
npm start
```

Open http://localhost:4200 and log in with `demo` / `TestPassword123!`.

## Tests

```bash
cd backend/backend && dotnet test
cd frontend && npx ng test --watch=false
```

## Deployment

```bash
cp .env.example .env    # set a real database password
docker compose up -d --build
```

The app is served on port 10100. nginx serves the Angular build and proxies `/api` to the API container, the API migrates the database on start and seeds it when it's empty.

## Credits

Kanji readings and meanings are taken from [KANJIDIC2](https://www.edrdg.org/wiki/index.php/KANJIDIC_Project), the property of the [Electronic Dictionary Research and Development Group](https://www.edrdg.org/), used in conformance with the Group's [licence](https://www.edrdg.org/edrdg/licence.html) (CC BY-SA 4.0).
