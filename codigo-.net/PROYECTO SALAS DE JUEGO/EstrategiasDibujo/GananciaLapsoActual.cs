using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using Microsoft.Xna.Framework;

namespace TestXNA.EstrategiasDibujo
{
    class GananciaLapsoActual : EstrategiaDibujo
    {
        int duracion;
        decimal limite;
        public GananciaLapsoActual(int duracion)
        {
            this.duracion = duracion;

            if (duracion == 1440)
                this.limite = 800;
            else
                this.limite = 300;
        }

        public override string GetQuery()
        {
            return @"
                SELECT  M.UID, PosX, PosY, GananciaAcumulada
                FROM    AT_Maquinas M WITH (NOLOCK) 
                    LEFT JOIN " + ConfigurationSettings.AppSettings["db.name"] + @".dbo.SV_LapsosTranscurridosMaquina LTM ON M.UID = LTM.UID
                WHERE   Eliminada = 0
                  AND   DuracionMinutos = " + duracion + ";";
        }

        public override void StartData()
        {
        }

        public override void PrepareData(System.Data.Common.DbDataReader dr)
        {
        }

        Vector3 greenLight = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 redLight = new Vector3(1.0f, 0.0f, 0.0f);

        public override Vector3 GetLight(System.Data.Common.DbDataReader dr)
        {
            decimal ganancia = (decimal)dr[3];
            return ((ganancia >= 0) ? greenLight : redLight);
        }

        public override float GetAlpha(System.Data.Common.DbDataReader dr)
        {
            decimal ganancia = (decimal)dr[3];

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
    }
}
