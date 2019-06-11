using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.IO;
using FirebirdSql.Data;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace Servicios.Negocio
{
    public class cTasaFinanciacion
    {
        public cTasaFinanciacion()
        {

        }

        public DataSet GetTodasLasTasasdeFinanciacion()
        {
            FbParameter[] dbParams = new FbParameter[] 
			{               
                
			};
            return DBHelperFB.ExecuteDataSetSP("PROC_GET_TASAS_FINANCIACION", dbParams);
        }

    }
}