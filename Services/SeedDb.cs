using RetroGamesLauncher.Data;
using RetroGamesLauncher.Models;
using RetroGamesLauncher.Models.Enums;

namespace RetroGamesLauncher.Services;
public static class SeedDb
{
    public static void PopulateDbInitialData(AppDbContext db)
    {
        if (!db.Games.Any())
        {
            var jogosIniciais = new[]
            {
                new GameInfo
                {
                    Title = "Super Mario World",
                    RomPath = @"Assets\Roms\Super Mario World (U) [!].smc",
                    ImagePath = @"Images\mario_thumb.jpg",
                    ScreenshotPath = @"Images\mario_large.jpg",
                    Description = "Mario e Yoshi embarcam em uma jornada para salvar a Princesa Peach e restaurar a paz na Dinosaur Land.",
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Donkey Kong Country",
                    RomPath = @"Assets\Roms\Donkey Kong Country (U) (V1.2) [!].smc",
                    ImagePath = @"Images\donkey_thumb.jpg",
                    ScreenshotPath = @"Images\donkey_large.jpg",
                    Description = "Acompanhe Donkey e Diddy Kong em uma aventura cheia de ação para recuperar sua reserva de bananas roubada.",
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Super Mario All-Stars",
                    RomPath = @"Assets\Roms\Super Mario All-Stars (U) [!].smc",
                    ImagePath = @"",
                    ScreenshotPath = @"",
                    Description = "Uma coletânea com versões aprimoradas dos clássicos Mario do NES: Mario 1, 2, 3 e The Lost Levels.",
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Aladdin",
                    RomPath = @"Assets\Roms\Aladdin (USA).sfc",
                    ImagePath = @"",
                    ScreenshotPath = @"",
                    Description = "Reviva a magia do clássico da Disney em Agrabah, enfrentando inimigos e voando no tapete mágico.",
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Street Fighter II Turbo",
                    RomPath = @"Assets\Roms\Street Fighter II Turbo (U).smc",
                    ImagePath = @"",
                    ScreenshotPath = @"",
                    Description = "Lute contra os melhores guerreiros do mundo neste clássico dos jogos de luta com combates eletrizantes.",
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Super Mario Bros",
                    RomPath = @"Assets\Roms\Super Mario Bros (E).nes",
                    ImagePath = @"",
                    ScreenshotPath = @"",
                    Description = "O início da lenda! Ajude Mario a salvar a Princesa Peach em sua primeira grande aventura pelo Reino do Cogumelo.",
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Super Mario Kart",
                    RomPath = @"Assets\Roms\Super Mario Kart (U) [!].smc",
                    ImagePath = @"",
                    ScreenshotPath = @"",
                    Description = "Corra com Mario e seus amigos em pistas malucas, usando itens e habilidades únicas para vencer.",
                    EmulatorId = Emulators.Mesen
                },
                new GameInfo
                {
                    Title = "Zombies Ate My Neighbors",
                    RomPath = @"Assets\Roms\Zombies Ate My Neighbors (U) [!].smc",
                    ImagePath = @"",
                    ScreenshotPath = @"",
                    Description = "Use armas malucas para salvar seus vizinhos de zumbis, múmias, marcianos e outras criaturas bizarras.",
                    EmulatorId = Emulators.Mesen
                },
            };


            db.Games.AddRange(jogosIniciais);
            db.SaveChanges();
        }
    }
}
