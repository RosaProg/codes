using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

using Microsoft.Xna.Framework;
using System.Data.Common;

namespace TestXNA.EstrategiasDibujo
{
    public class Foto : EstrategiaDibujo
    {
        string CampoTipo()
        {
            if (Tipo == 1)
                return "UltimoLlamadoAsistente";
            else if (Tipo == 2)
                return "UltimoPowerOff";
            else if (Tipo == 3)
                return "UltimaAperturaPuerta";
            else if (Tipo == 4)
                return "UltimoMenuOperador";
            else if (Tipo == 5)
                return "UltimoMenuContadores";
            else
                return "NULL";
        }


        public override string GetQuery()
        {
            string retval = "";
            if (NumeroLapso == null)
            {
                retval = @"
SELECT  UID, PosX, PosY, Angulo, ISNULL(C14,0) AS CurrentCredits, GETDATE() AS Ahora, OE.HoraInsercion AS HoraUltimoEvento, " + 
CampoTipo() + @" AS Filtro
FROM    <%=olddb%>.dbo.AT_Maquinas WITH (NOLOCK)
    LEFT JOIN <%=olddb%>.dbo.AT_OnlineContadores WITH (NOLOCK) ON UltimosContadores = IDOnlineContador
    LEFT JOIN <%=olddb%>.dbo.AT_OnlineEventos OE WITH (NOLOCK) ON UltimoEvento = IDOnlineEvento
    LEFT JOIN <%=db%>.dbo.ST_UltimosDatosMaquina UDM WITH (NOLOCK) ON IDMaquina = UDM.Maquina
WHERE   Eliminada = 0
  AND	FechaBaja > GETDATE()
  AND	FechaAlta < GETDATE();";       
            }
            else
            {
                retval = @"
SELECT 	UID, PosX, PosY, Angulo, ISNULL(C14,0), HoraInicioTeorica
FROM	" + ConfigurationSettings.AppSettings["db.oldname"] + @".dbo.AT_Maquinas WITH (NOLOCK)
	LEFT JOIN LT_LapsosTranscurridosMaquina WITH (NOLOCK) ON IDMaquina = Maquina
	LEFT JOIN LT_OnlineContadores WITH (NOLOCK) ON ContadoresIniciales = IDOnlineContadores
    LEFT JOIN NG.dbo.LT_LapsosTranscurridos WITH (NOLOCK) ON LapsoTranscurrido = IDLapsoTranscurrido
WHERE	LapsoTranscurrido = 
(
	SELECT 	TOP 1 IDLapsoTranscurrido
	FROM	NG.dbo.LT_LapsosTranscurridos WITH (NOLOCK)
	WHERE	Lapso = " + Lapso +

            (NumeroLapso == null ? "" : " AND NumeroLapso = " + NumeroLapso) + @"
	ORDER BY IDLapsoTranscurrido DESC
)
  AND   Eliminada = 0
  AND	FechaBaja > GETDATE()
  AND	FechaAlta < GETDATE()
";
            }

            return retval;
        }

        DateTime dt = DateTime.Now;
        public override void StartData()
        {
            base.StartData();
            total_maquinas = 0;
            total_en_uso = 0;
        }

        public override void PrepareData(System.Data.Common.DbDataReader dr)
        {
            dt = (DateTime)dr[5];
            total_maquinas++;
            if ((long)dr[4] != 0)
                total_en_uso++;
        }

        Vector3 blueLight = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 yellow_light = new Vector3(0.0f, 1.0f, 1.0f);

        public override Microsoft.Xna.Framework.Vector3 GetLight(System.Data.Common.DbDataReader dr)
        {
            return blueLight;                
        }

        public override Vector3 BlinkingLight(DbDataReader dr)
        {
            if (NumeroLapso != null)
                return blueLight;
            else
            {
                if (Tipo == null)
                {
                    DateTime last_event = (DateTime)dr["HoraUltimoEvento"];
                    if (DateTime.Now.Subtract(last_event).TotalSeconds < 120)
                        return yellow_light;
                    else
                        return blueLight;
                }
                else
                {
                    if (DBNull.Value == dr["Filtro"])
                        return blueLight;
                    else
                        return yellow_light;
                }
            }
        }

        public override float GetAlpha(System.Data.Common.DbDataReader dr)
        {
            if (DBNull.Value == dr[4])
                return 0.1f;
            else
            {
                long m = (long)dr[4];
                if (m == 0)
                    return 0.1f;
                else
                    return 1.0f;
            }
        }

        string Tipo2String()
        {
            if (NumeroLapso != null)
                return "";
            else if (Tipo == null)
                return "Eventos - ";
            else if (Tipo == 1)
                return "Solicitud de asistente - ";
            else if (Tipo == 2)
                return "Apagadas - ";
            else if (Tipo == 3)
                return "Puerta abierta - ";
            else if (Tipo == 4)
                return "Menu de operador - ";
            else if (Tipo == 5)
                return "Menu de contadores - ";
            else
                return "";
        }

        public override string GetTitle()
        {
            if (Loading)
                return "Foto - " + Tipo2String() + "Cargando...";
            else if (NumeroLapso == null)
                return "Foto - " + Tipo2String() + DateTime.Now.ToString();
            else
                return "Foto - " + Tipo2String() + dt.ToString();
        }

        int total_maquinas = 0;
        int total_en_uso = 0;

        string[] legends = new string[1];
        string[] empty = new string[0];
        public override string[] GetLegends()
        {
            if (Loading)
                return empty;
            else
            {
                double porcentaje = 100.0 * (double)total_en_uso / (double)total_maquinas;
                legends[0] = "Maquinas en uso " + total_en_uso + "/" + total_maquinas + " - " + Math.Round(porcentaje,2) + "%";
                return legends;
            }
        }
    }
}
