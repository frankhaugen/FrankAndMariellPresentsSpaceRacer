using Stride.Engine;

namespace FrankAndMariellPresentsSpaceRacer
{
    class FrankAndMariellPresentsSpaceRacerApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
