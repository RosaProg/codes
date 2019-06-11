using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace TestXNA.EstrategiasDibujo
{
    class Ganancia : EstrategiaDibujo
    {
        NumericUpDown num;
        public Ganancia(NumericUpDown num)
        {
            this.num = num;
        }

        public override string GetQuery()
        {
            if (NumeroLapso == null)
            {
                return @"
SELECT	UID, PosX, PosY, ISNULL(Angulo,0) AS Angulo, SUM(LTMP.Ganancia)
    FROM	<%=olddb%>.dbo.AT_Maquinas WITH (NOLOCK)
	    LEFT JOIN <%=db%>.dbo.ST_LapsosTranscurridosMaquina S WITH (NOLOCK) ON IDMaquina = Maquina
	    LEFT JOIN <%=db%>.dbo.LT_LapsosTranscurridosMaquina LTM WITH (NOLOCK) ON S.LapsoTranscurridoMaquina = LTM.IDLapsoTranscurridoMaquina
	    LEFT JOIN <%=db%>.dbo.LT_LapsosTranscurridosMaquina_Periodos LTMP WITH (NOLOCK) ON LTMP.LapsoTranscurridoMaquina = LTM.IDLapsoTranscurridoMaquina
WHERE	Lapso = " + Lapso + @"
GROUP BY UID, PosX, PosY, Angulo";
            }
            else
            {
                return @"
                SELECT	UID, PosX, PosY, Angulo, Ganancia, HoraInicioTeorica
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

        bool start = true;
        public override void StartData()
        {
            limite = num.Value;
            start = false;
        }

        DateTime dt;
        public override void PrepareData(System.Data.Common.DbDataReader dr)
        {
            if (NumeroLapso != null)
                try
                {
                    dt = (DateTime)dr[5];
                }
                catch (Exception ex)
                {
                }
        }

        Vector3 greenLight = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 redLight = new Vector3(1.0f, 0.0f, 0.0f);
        public override Microsoft.Xna.Framework.Vector3 GetLight(System.Data.Common.DbDataReader dr)
        {
            decimal ganancia = 0m;
            try
            {
                ganancia = (decimal)dr[4];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
            }

            if (ganancia < 0m)
                return redLight;
            else
                return greenLight;
        }

        decimal limite = 800;
        public override float GetAlpha(System.Data.Common.DbDataReader dr)
        {
            decimal ganancia = 0m;

            try
            {
                ganancia = (decimal)dr[4];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
            }

            float alpha = 1.0f;
            if (ganancia > limite || ganancia < -limite)
                alpha = 1.0f;
            else if (ganancia > 0 && ganancia < 0.05m * limite)
                alpha = 0.05f;
            else if (ganancia < 0 && ganancia > -0.05m * limite)
                alpha = 0.05f;
            else if (ganancia > 0)
                alpha = (float)(ganancia / limite);
            else if (ganancia < 0)
                alpha = (float)(-ganancia / limite);
            else
            {
                alpha = 0.05f;
            }

            return alpha;
        }

        public override string GetTitle()
        {
            if (start)
                return "Ganancia - Cargando...";
            else if (NumeroLapso == null)
                return "Ganancia - " + DateTime.Now.ToString();
            else
                return "Ganancia - " + dt.ToString();
        }
    }
}
