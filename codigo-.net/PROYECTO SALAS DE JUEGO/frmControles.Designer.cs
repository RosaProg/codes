namespace TestXNA
{
    partial class frmControles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.numLimite = new System.Windows.Forms.NumericUpDown();
            this.cmdGrafico = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.radFijo = new System.Windows.Forms.RadioButton();
            this.radAnimacion = new System.Windows.Forms.RadioButton();
            this.radUltimo = new System.Windows.Forms.RadioButton();
            this.cmbLapso = new System.Windows.Forms.ComboBox();
            this.trackCuadro = new System.Windows.Forms.TrackBar();
            this.tmrAnimacion = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimite)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackCuadro)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(348, 226);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.numLimite);
            this.tabPage1.Controls.Add(this.cmdGrafico);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(340, 200);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Grafico";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(305, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "$";
            // 
            // numLimite
            // 
            this.numLimite.Location = new System.Drawing.Point(240, 66);
            this.numLimite.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numLimite.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numLimite.Name = "numLimite";
            this.numLimite.Size = new System.Drawing.Size(65, 20);
            this.numLimite.TabIndex = 1;
            this.numLimite.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numLimite.ValueChanged += new System.EventHandler(this.numLimite_ValueChanged);
            // 
            // cmdGrafico
            // 
            this.cmdGrafico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdGrafico.FormattingEnabled = true;
            this.cmdGrafico.Items.AddRange(new object[] {
            "Foto",
            "Ganancia",
            "Tiempo de uso (contadores)",
            "Tiempo de uso (eventos)"});
            this.cmdGrafico.Location = new System.Drawing.Point(10, 39);
            this.cmdGrafico.Name = "cmdGrafico";
            this.cmdGrafico.Size = new System.Drawing.Size(313, 21);
            this.cmdGrafico.TabIndex = 0;
            this.cmdGrafico.SelectedIndexChanged += new System.EventHandler(this.cmdGrafico_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.radFijo);
            this.tabPage2.Controls.Add(this.radAnimacion);
            this.tabPage2.Controls.Add(this.radUltimo);
            this.tabPage2.Controls.Add(this.cmbLapso);
            this.tabPage2.Controls.Add(this.trackCuadro);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(340, 200);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Animacion";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // radFijo
            // 
            this.radFijo.AutoSize = true;
            this.radFijo.Location = new System.Drawing.Point(10, 93);
            this.radFijo.Name = "radFijo";
            this.radFijo.Size = new System.Drawing.Size(41, 17);
            this.radFijo.TabIndex = 4;
            this.radFijo.Text = "Fijo";
            this.radFijo.UseVisualStyleBackColor = true;
            this.radFijo.CheckedChanged += new System.EventHandler(this.radFijo_CheckedChanged);
            // 
            // radAnimacion
            // 
            this.radAnimacion.AutoSize = true;
            this.radAnimacion.Location = new System.Drawing.Point(10, 57);
            this.radAnimacion.Name = "radAnimacion";
            this.radAnimacion.Size = new System.Drawing.Size(74, 17);
            this.radAnimacion.TabIndex = 3;
            this.radAnimacion.Text = "Animacion";
            this.radAnimacion.UseVisualStyleBackColor = true;
            this.radAnimacion.CheckedChanged += new System.EventHandler(this.radAnimacion_CheckedChanged);
            // 
            // radUltimo
            // 
            this.radUltimo.AutoSize = true;
            this.radUltimo.Checked = true;
            this.radUltimo.Location = new System.Drawing.Point(10, 24);
            this.radUltimo.Name = "radUltimo";
            this.radUltimo.Size = new System.Drawing.Size(80, 17);
            this.radUltimo.TabIndex = 2;
            this.radUltimo.TabStop = true;
            this.radUltimo.Text = "Ultimo valor";
            this.radUltimo.UseVisualStyleBackColor = true;
            this.radUltimo.CheckedChanged += new System.EventHandler(this.radUltimo_CheckedChanged);
            // 
            // cmbLapso
            // 
            this.cmbLapso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLapso.FormattingEnabled = true;
            this.cmbLapso.Items.AddRange(new object[] {
            "Dia",
            "1/2 hora (+15min)",
            "1/2 hora",
            "Hora",
            "5 min"});
            this.cmbLapso.Location = new System.Drawing.Point(211, 105);
            this.cmbLapso.Name = "cmbLapso";
            this.cmbLapso.Size = new System.Drawing.Size(121, 21);
            this.cmbLapso.TabIndex = 1;
            this.cmbLapso.SelectedIndexChanged += new System.EventHandler(this.cmbLapso_SelectedIndexChanged);
            // 
            // trackCuadro
            // 
            this.trackCuadro.Location = new System.Drawing.Point(10, 146);
            this.trackCuadro.Name = "trackCuadro";
            this.trackCuadro.Size = new System.Drawing.Size(322, 42);
            this.trackCuadro.TabIndex = 0;
            this.trackCuadro.Scroll += new System.EventHandler(this.trackCuadro_Scroll);
            // 
            // tmrAnimacion
            // 
            this.tmrAnimacion.Enabled = true;
            this.tmrAnimacion.Interval = 10000;
            this.tmrAnimacion.Tick += new System.EventHandler(this.tmrAnimacion_Tick);
            // 
            // frmControles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 228);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmControles";
            this.Opacity = 0.7D;
            this.Text = "Configuracion";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmControles_FormClosed);
            this.Load += new System.EventHandler(this.frmControles_Load);
            this.Resize += new System.EventHandler(this.frmControles_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimite)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackCuadro)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox cmdGrafico;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cmbLapso;
        private System.Windows.Forms.TrackBar trackCuadro;
        private System.Windows.Forms.Timer tmrAnimacion;
        private System.Windows.Forms.NumericUpDown numLimite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radFijo;
        private System.Windows.Forms.RadioButton radAnimacion;
        private System.Windows.Forms.RadioButton radUltimo;
    }
}