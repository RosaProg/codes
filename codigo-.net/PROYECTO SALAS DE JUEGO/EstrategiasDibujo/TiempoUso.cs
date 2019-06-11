using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Common;

using Microsoft.Xna.Framework;

namespace TestXNA.EstrategiasDibujo
{
    class TiempoUso : EstrategiaDibujo
    {
        bool contadores = false;
        public TiempoUso(bool contadores)
        {
            this.contadores = contadores;
        }

        public override string GetQuery()
        {
            if (NumeroLapso == null)
            {
                return @"
SELECT	UID, PosX, PosY, ISNULL(Angulo,0) AS Angulo, " + (contadores ? "TiempoUsoCreditos" : "TiempoUsoEventos") + @"
    FROM	<%=olddb%>.dbo.AT_Maquinas WITH (NOLOCK)
	    LEFT JOIN <%=db%>.dbo.ST_LapsosTranscurridosMaquina S WITH (NOLOCK) ON IDMaquina = Maquina
	    LEFT JOIN <%=db%>.dbo.LT_LapsosTranscurridosMaquina LTM WITH (NOLOCK) ON S.LapsoTranscurridoMaquina = LTM.IDLapsoTranscurridoMaquina
WHERE	Lapso = " + Lapso + ";";
            }
            else
            {
                return @"
                SELECT	UID, PosX, PosY, Angulo, " + (contadores ? "TiempoUsoCreditos" : "TiempoUsoEventos") + @", HoraInicioTeorica
                FROM	<%=db%>.dbo.LT_LapsosTranscurridosMaquina WITH (NOLOCK)
	                LEFT JOIN <%=olddb%>.dbo.AT_Maquinas ON IDMaquina = Maquina
                    LEFT JOIN <%=padron%>.dbo.LT_LapsosTranscurridos ON LapsoTranscurrido = IDLapsoTranscurrido
                WHERE	LapsoTranscurrido = 
                (
	                SELECT 	TOP 1 IDLapsoTranscurrido
	                FROM 	<%=padron%>.dbo.LT_LapsosTranscurridos
	                WHERE	Lapso = " + Lapso + @"
	                  AND 	NumeroLapso = " + NumeroLapso + @"
	                ORDER BY IDLapsoTranscurrido DESC
                )";
            }
        }

        public override void PrepareData(DbDataReader dr)
        {
        }

        Vector3 blueLight = new Vector3(0.0f, 0.0f, 1.0f);
        public override Vector3 GetLight(DbDataReader dr)
        {
            return blueLight;
        }

        public override float GetAlpha(DbDataReader dr)
        {
            int limite = 1000;
            if (Lapso == 1)
                limite = 5000;
            else if (Lapso == 2 || Lapso == 3)
                limite = 500;
            else if (Lapso == 4)
                limite = 1000;
            else
                limite = 100;

            float alpha = 1.0f;
            int uso = (int)dr[3];
            if (uso > limite)
                alpha = 1.0f;
            else if (uso < (long)((double)limite * 0.1))
                alpha = 0.1f;
            else
                alpha = (float)uso / (float)limite;

            return alpha;
        }

        string GetName()
        {
            return "Tiempo de uso (" + (contadores ? "contadores" : "eventos") + ")";
        }

        DateTime dt;
        public override string GetTitle()
        {
            if (Loading)
                return GetName() + " - Cargando...";
            else if (NumeroLapso == null)
                return GetName() + " - " + DateTime.Now.ToString();
            else
                return GetName() + " - " + dt.ToString();
        }
    }
}
