using System;

namespace TestXNA
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Punto de entrada.
        /// </summary>
        static void Main(string[] args)
        {
            using (MapaTermico game = new MapaTermico())
            {
                game.Run();
            }
        }
    }
#endif
}

