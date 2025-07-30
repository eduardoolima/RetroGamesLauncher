using RetroGamesLauncher.Data;
using RetroGamesLauncher.Models;
using RetroGamesLauncher.Models.Enums;

namespace RetroGamesLauncher.Services;
public static class SeedDb
{
    public static void PopulateDbInitialData(AppDbContext db)
    {
        if (!db.GameGenders.Any())
        {
            var initialGenders = new[]
            {
                new GameGender("Ação/Aventura"),
                new GameGender("Plataforma"),
                new GameGender("RPG"),
                new GameGender("Luta"),
                new GameGender("Shoot 'em Up"),
                new GameGender("Esportes"),
                new GameGender("Beat 'em Up / Briga de Rua"),
                new GameGender("Puzzle"),
                new GameGender("FPS (First-Person Shooter)"),
                new GameGender("Estratégia"),
                new GameGender("Estratégia em Tempo Real (RTS)"),
                new GameGender("Estratégia por Turnos (TBS)"),
                new GameGender("Simulação"),
                new GameGender("Jogo Indie"),
                new GameGender("Tiro"),
            };
            db.GameGenders.AddRange(initialGenders);
            db.SaveChanges();
        }
        if (!db.Games.Any())
        {
            var genders = db.GameGenders.ToDictionary(g => g.Gender, g => g);
            var initialGames = new[]
            {
                new GameInfo
                {
                    Title = "Super Mario World",
                    RomPath = @"Assets\Roms\Super Mario World (U) [!].smc",
                    ImagePath = @"Assets\Images\GamesCover\SuperMarioWorld.jpeg",
                    ScreenshotPath = @"Assets\Images\GamesScreenshot\SuperMarioWorld.jpeg",
                    Description = "Mario e Yoshi embarcam em uma jornada para salvar a Princesa Peach e restaurar a paz na Dinosaur Land.",
                    Gender = genders["Plataforma"],
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Donkey Kong Country",
                    RomPath = @"Assets\Roms\Donkey Kong Country (U) (V1.2) [!].smc",
                    ImagePath = @"Assets\Images\GamesCover\DonkeyKongCountry.jpeg",
                    ScreenshotPath = @"Assets\Images\GamesScreenshot\donkeyKongCountry.jpeg",
                    Description = "Acompanhe Donkey e Diddy Kong em uma aventura cheia de ação para recuperar sua reserva de bananas roubada.",
                    Gender = genders["Plataforma"],
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Super Mario All-Stars",
                    RomPath = @"Assets\Roms\Super Mario All-Stars (U) [!].smc",
                    ImagePath = @"Assets\Images\GamesCover\Super_Mario_All_Stars.jpeg",
                    ScreenshotPath = @"Assets\Images\GamesScreenshot\superMarioAllStars.jpeg",
                    Description = "Uma coletânea com versões aprimoradas dos clássicos Mario do NES: Mario 1, 2, 3 e The Lost Levels.",
                    Gender = genders["Plataforma"],
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Aladdin",
                    RomPath = @"Assets\Roms\Aladdin (USA).sfc",
                    ImagePath = @"Assets\Images\GamesCover\aladdin-snes-scaled.jpeg",
                    ScreenshotPath = @"Assets\Images\GamesScreenshot\Aladdin SNES Super Nintendo.jpeg",
                    Description = "Reviva a magia do clássico da Disney em Agrabah, enfrentando inimigos e voando no tapete mágico.",
                    Gender = genders["Ação/Aventura"],
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Street Fighter II Turbo",
                    RomPath = @"Assets\Roms\Street Fighter II Turbo (U).smc",
                    ImagePath = @"Assets\Images\GamesCover\streetfighterturbo-snes-sq-1642199243723.jpeg",
                    ScreenshotPath = @"Assets\Images\GamesScreenshot\StreetFighter.jpeg",
                    Description = "Lute contra os melhores guerreiros do mundo neste clássico dos jogos de luta com combates eletrizantes.",
                    Gender = genders["Luta"],
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Super Mario Bros",
                    RomPath = @"Assets\Roms\Super Mario Bros (E).nes",
                    ImagePath = @"Assets\Images\GamesCover\Super_Mario_Bros._box.jpeg",
                    ScreenshotPath = @"Assets\Images\GamesScreenshot\SuperMarioBros.jpeg",
                    Description = "O início da lenda! Ajude Mario a salvar a Princesa Peach em sua primeira grande aventura pelo Reino do Cogumelo.",
                    Gender = genders["Plataforma"],
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Super Mario Kart",
                    RomPath = @"Assets\Roms\Super Mario Kart (U) [!].smc",
                    ImagePath = @"Assets\Images\GamesCover\SuperMarioKart.jpeg",
                    ScreenshotPath = @"Assets\Images\GamesScreenshot\super-mario-kart-snes.jpeg",
                    Description = "Corra com Mario e seus amigos em pistas malucas, usando itens e habilidades únicas para vencer.",
                    Gender = genders["Esportes"],
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Zombies Ate My Neighbors",
                    RomPath = @"Assets\Roms\Zombies Ate My Neighbors (U) [!].smc",
                    ImagePath = @"Assets\Images\GamesCover\Zombies Ate My Neighbors.jpeg",
                    ScreenshotPath = @"Assets\Images\GamesScreenshot\ZombiesAteMyNeighbors.jpeg",
                    Description = "Use armas malucas para salvar seus vizinhos de zumbis, múmias, marcianos e outras criaturas bizarras.",
                    Gender = genders["Ação/Aventura"],
                    EmulatorId = Emulators.Mesen
                },
            };
            db.Games.AddRange(initialGames);
            db.SaveChanges();
        }
    }
}
