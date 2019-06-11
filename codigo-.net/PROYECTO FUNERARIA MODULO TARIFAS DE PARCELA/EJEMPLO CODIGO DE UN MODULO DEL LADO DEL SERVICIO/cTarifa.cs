using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using FirebirdSql.Data;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;
namespace Servicios.Negocio
{
    public class cTarifa
    {
        public DataSet getTarifasxcontrato(DateTime FECHA_CONTRATO, Int32  COD_TIPO_CONTRATO, int COD_ZONA)
        {
            FbParameter[] dbParams = new FbParameter[]
			{    
                DBHelperFB.MakeParam("@FECHA_CONTRATO", FbDbType.Date , 0, FECHA_CONTRATO),
                DBHelperFB.MakeParam("@COD_TIPO_CONTRATO", FbDbType.Double    , 0, COD_TIPO_CONTRATO),
                DBHelperFB.MakeParam("@COD_ZONA", FbDbType.Integer  , 0, COD_ZONA),
			};
            return DBHelperFB.ExecuteDataSetSP("PROC_GET_TARIFA_X_TIPO_CONT", dbParams);
        }

        public DataSet GetCondVentaxTarifa(int COD_LISTA_PRECIO)
        {
            FbParameter[] dbParams = new FbParameter[]
			{    
                DBHelperFB.MakeParam("@COD_LISTA_PRECIO", FbDbType.Integer  , 0, COD_LISTA_PRECIO),
			};
            return DBHelperFB.ExecuteDataSetSP("PROC_CONDICION_VENTA_X_TARIFA", dbParams);
        }

        public DataSet GetCondVentaxTarifaxCondicionVenta(int COD_CONDICION_VENTA)
        {
            FbParameter[] dbParams = new FbParameter[]
			{    
                DBHelperFB.MakeParam("@COD_CONDICION_VENTA", FbDbType.Integer  , 0, COD_CONDICION_VENTA),
			};
            return DBHelperFB.ExecuteDataSetSP("PROC_GET_DET_CONDICION_VENTA", dbParams);
        }

        public DataSet AplicarCondicionVenta(double CAPITAL, int CANT_CUOTAS, int PERIODO_CUOTA, string TIEMPO_PERIODO_CUOTA, decimal TASA_INTERES, string TIEMPO_INTERES, string TIPO_INTERES, DateTime FECHA_INICIO)
        {
            FbParameter[] dbParams = new FbParameter[]
			{    
                DBHelperFB.MakeParam("@CAPITAL", FbDbType.Double  , 20, CAPITAL),
                DBHelperFB.MakeParam("@CANT_CUOTAS", FbDbType.Integer   , 8, CANT_CUOTAS),
                DBHelperFB.MakeParam("@PERIODO_CUOTA", FbDbType.Integer   , 8, PERIODO_CUOTA),
                DBHelperFB.MakeParam("@TIEMPO_PERIODO_CUOTA", FbDbType.VarChar    , 20, TIEMPO_PERIODO_CUOTA),
                DBHelperFB.MakeParam("@TASA_INTERES", FbDbType.Decimal       , 20, TASA_INTERES),
                DBHelperFB.MakeParam("@TIEMPO_INTERES", FbDbType.VarChar      , 20, TIEMPO_INTERES),
                DBHelperFB.MakeParam("@TIPO_INTERES", FbDbType.VarChar       , 20, TIPO_INTERES) ,
                DBHelperFB.MakeParam("@FECHA_INICIO", FbDbType.Date        , 0, FECHA_INICIO) 
			};
            return DBHelperFB.ExecuteDataSetSP("PROC_APLICAR_COND_VENTA", dbParams);
        }

        public DataSet GetMantenimientoxTarifa(int COD_LISTA_PRECIO, DateTime FECHA_INICIO)
        {
            FbParameter[] dbParams = new FbParameter[]
			{    
                DBHelperFB.MakeParam("@COD_LISTA_PRECIO", FbDbType.Integer  , 0, COD_LISTA_PRECIO),
                DBHelperFB.MakeParam("@FECHA_INICIO", FbDbType.Date     , 0, FECHA_INICIO),
			};
            return DBHelperFB.ExecuteDataSetSP("PROC_MANT_X_TARIFA", dbParams);
        }

        public DataSet GetTarifaxCodListaPrecioyCodCondicionVena(int COD_LISTA_PRECIO, int COD_CONDICION_VENTA_ENT)
        {
            FbParameter[] dbParams = new FbParameter[]
			{    
                DBHelperFB.MakeParam("@COD_LISTA_PRECIO", FbDbType.Integer  , 0, COD_LISTA_PRECIO),
                DBHelperFB.MakeParam("@COD_CONDICION_VENTA_ENT", FbDbType.Integer  , 0, COD_CONDICION_VENTA_ENT),
			};
            return DBHelperFB.ExecuteDataSetSP("PROC_COND_VENTA_X_TARIFA_X_COND", dbParams);

        }
    }
}