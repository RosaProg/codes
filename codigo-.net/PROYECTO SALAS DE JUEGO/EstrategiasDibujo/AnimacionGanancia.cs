using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestXNA.EstrategiasDibujo
{
    class AnimacionGanancia : GananciaLapsoActual
    {
        public AnimacionGanancia()
            : base(60)
        {
        }

        int numero_lapso = 0;
        public override string GetQuery()
        {
            return @"
                SELECT	UID, PosX, PosY, Ganancia
                FROM	NG_OnlineG.dbo.LT_LapsosTranscurridosMaquina WITH (NOLOCK)
	                LEFT JOIN ArgG.dbo.AT_Maquinas ON IDMaquina = Maquina
                WHERE	LapsoTranscurrido = 
                (
	                SELECT 	TOP 1 IDLapsoTranscurrido
	                FROM 	NG.dbo.LT_LapsosTranscurridos
	                WHERE	Lapso = 4
	                  AND 	NumeroLapso = " + numero_lapso + @"
	                ORDER BY IDLapsoTranscurrido DESC
                )";
        }

        public override bool Update()
        {
            numero_lapso++;
            numero_lapso = numero_lapso % 24;
            return true;
        }

        public override string GetTitle()
        {
            return numero_lapso.ToString().PadLeft(2, '0') + ":00";
        }
    }
}
