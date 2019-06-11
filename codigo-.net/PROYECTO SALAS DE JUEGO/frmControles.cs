using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TestXNA.EstrategiasDibujo;

namespace TestXNA
{
    public partial class frmControles : Form
    {
        MapaTermico m;
        public frmControles(MapaTermico m)
        {
            this.m = m;
            InitializeComponent();
        }

        bool loading = true;
        private void frmControles_Load(object sender, EventArgs e)
        {
            try
            {
                cmdGrafico.SelectedIndex = 0;
                cmbLapso.SelectedIndex = 0;
                loading = false;
                UpdateGrafico();
            }
            catch (Exception ex)
            {
                int k = 0;
            }
        }

        EstrategiaDibujo e = new Foto();
        void UpdateGrafico()
        {
            if (loading)
                return;

            if (cmdGrafico.SelectedIndex == 0)
                e = new Foto();
            else if (cmdGrafico.SelectedIndex == 1)
                e = new Ganancia(numLimite);
            else if (cmdGrafico.SelectedIndex == 2)
                e = new TiempoUso(true);
            else if (cmdGrafico.SelectedIndex == 3)
                e = new TiempoUso(false);
            else
                throw new Exception("blgblg");

            e.Lapso = cmbLapso.SelectedIndex + 1;
            UpdateAnimacion();
            m.SetTipoMapa(e);
            /*
            int duracion = 0;
            if (cmbLapso.SelectedIndex == 0 || cmbLapso.SelectedIndex == -1)
                duracion = 1440;
            else if (cmbLapso.SelectedIndex == 1)
                duracion = 60;
            else
                throw new Exception("BLGBLG");

            EstrategiaDibujo e = null;
            if (cmdGrafico.SelectedIndex == 0 || cmdGrafico.SelectedIndex == -1)
                e = new GananciaLapsoActual(duracion);
            else if (cmdGrafico.SelectedIndex == 1)
                e = new TiempoUsoLapsoActual(true, duracion, 10000);
            else if (cmdGrafico.SelectedIndex == 2)
                e = new TiempoUsoLapsoActual(false, duracion, 10000);
            else if (cmdGrafico.SelectedIndex == 3)
                e = new AnimacionGanancia();
            else if (cmdGrafico.SelectedIndex == 4)
                e = new Foto();
            else
                throw new Exception("BLGBLG");

            m.SetTipoMapa(e);
            */
        }

        private void cmdGrafico_SelectedIndexChanged(object sender, EventArgs e)
        {
            numLimite.Enabled = (cmdGrafico.SelectedIndex == 1);
            UpdateGrafico();
        }

        private void frmControles_FormClosed(object sender, FormClosedEventArgs e)
        {
            m.Close();
            Application.Exit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void cmbLapso_SelectedIndexChanged(object sender, EventArgs __e)
        {
            e.Lapso = cmbLapso.SelectedIndex + 1;
            trackCuadro.Value = 0;
            if (cmbLapso.SelectedIndex == 0)
                trackCuadro.Maximum = 0;
            else if (cmbLapso.SelectedIndex == 1 || cmbLapso.SelectedIndex == 2)
                trackCuadro.Maximum = 48;
            else if (cmbLapso.SelectedIndex == 3)
                trackCuadro.Maximum = 24;
            else if (cmbLapso.SelectedIndex == 4)
                trackCuadro.Maximum = 12 * 24;

            UpdateGrafico();
        }

        private void chkAnimar_CheckedChanged(object sender, EventArgs e)
        {
            trackCuadro.Enabled = radAnimacion.Checked || radFijo.Enabled;
            UpdateAnimacion();
            m.UpdateMapa();
        }

        void UpdateAnimacion()
        {
            if (radUltimo.Checked)
                e.NumeroLapso = null;
            else
            {
                if (radAnimacion.Checked)
                {
                    if (trackCuadro.Value >= (trackCuadro.Maximum - 1))
                        trackCuadro.Value = 0;
                    else
                        trackCuadro.Value++;
                }

                e.NumeroLapso = (uint?)trackCuadro.Value;
            }
        }

        private void tmrAnimacion_Tick(object sender, EventArgs __e)
        {
            UpdateAnimacion();
            m.UpdateMapa();
        }

        private void trackCuadro_Scroll(object sender, EventArgs e)
        {
            UpdateAnimacion();
            m.UpdateMapa();
        }

        private void numLimite_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radUltimo_CheckedChanged(object sender, EventArgs __e)
        {
            tmrAnimacion.Enabled = false;
            UpdateAnimacion();
            m.UpdateMapa();
        }

        private void radAnimacion_CheckedChanged(object sender, EventArgs e)
        {
            tmrAnimacion.Enabled = true;
            UpdateAnimacion();
            m.UpdateMapa();
        }

        private void radFijo_CheckedChanged(object sender, EventArgs e)
        {
            tmrAnimacion.Enabled = false;
            UpdateAnimacion();
            m.UpdateMapa();
        }

        private void frmControles_Resize(object sender, EventArgs e)
        {
            m.ShowGearIcon = (WindowState == FormWindowState.Minimized);
        }
    }
}
