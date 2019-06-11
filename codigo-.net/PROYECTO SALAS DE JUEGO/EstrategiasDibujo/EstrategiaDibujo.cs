using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Common;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Windows.Forms;

namespace TestXNA.EstrategiasDibujo
{
    public abstract class EstrategiaDibujo
    {
        private uint? numeroLapso = null;

        protected bool Updated { get; private set; }
        public uint? NumeroLapso
        {
            get { return numeroLapso; }
            set { numeroLapso = value; Updated = true; }
        }

        int lapso = 1;
        public int Lapso
        {
            get { return lapso; }
            set { lapso = value; }
        }
        

        public abstract string  GetQuery();

        public virtual void StartData()
        {
            Loading = true;
        }

        public abstract void PrepareData(DbDataReader dr);

        public bool Loading { get; private set; }
        public virtual void EndData()
        {
            Loading = false;
        }

        public abstract Vector3 GetLight(DbDataReader dr);
        public abstract float GetAlpha(DbDataReader dr);

        public virtual bool Update()
        {
            bool retval = Updated;
            Updated = false;
            return retval || numeroLapso == null;
        }

        public virtual string GetTitle()
        {
            return "";
        }

        string[] legends = new string[0];
        public virtual string[] GetLegends()
        {
            return legends;
        }

        public virtual Vector3 BlinkingLight(DbDataReader dr)
        {
            return GetLight(dr);            
        }

        public virtual int? Tipo
        {
            get;
            set;
        }
    }
}
