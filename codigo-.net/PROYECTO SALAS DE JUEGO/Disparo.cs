using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestXNA_blabla
{
    class Disparo
    {
        public Disparo()
        {
            Mostrar = false;
        }

        public bool Mostrar { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private int veces_desplazado = 0;

        public void Mover(int dx, int dy)
        {
            X += dx;
            Y += dy;

            veces_desplazado++;
            if (veces_desplazado > 150)
                Mostrar = false;
        }

        public void Disparar(int x, int y)
        {
            Mostrar = true;
            veces_desplazado = 0;
            X = x;
            Y = y;
        }
    }
}
