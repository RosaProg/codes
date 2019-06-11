using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace TestXNA.EstrategiasDibujo
{
    class TiempoUsoLapsoActual : EstrategiaDibujo
    {
        bool creditos;
        int duracion;
        int limite;
        public TiempoUsoLapsoActual(bool creditos, int duracion, int limite)
        {
            this.creditos = creditos;
            this.duracion = duracion;
            
            if (duracion == 1440)
                this.limite = 10000;
            else
                this.limite = 1000;
        }

        public override string GetQuery()
        {
            return @"
                SELECT  M.UID, PosX, PosY, TiempoUso" + (creditos ? "Creditos" : "Eventos") + @"
                FROM    AT_Maquinas M WITH (NOLOCK) 
                    LEFT JOIN NG_OnlineGualeguay.dbo.SV_LapsosTranscurridosMaquina LTM ON M.UID = LTM.UID
                WHERE   Eliminada = 0
                  AND   DuracionMinutos = " + duracion + ";";
        }

        public override void StartData()
        {
        }

        public override void PrepareData(System.Data.Common.DbDataReader dr)
        {
        }

        Vector3 blueLight = new Vector3(0.0f, 0.0f, 1.0f);

        public override Vector3 GetLight(System.Data.Common.DbDataReader dr)
        {
            return blueLight;
        }

        public override float GetAlpha(System.Data.Common.DbDataReader dr)
        {
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
    }
}
