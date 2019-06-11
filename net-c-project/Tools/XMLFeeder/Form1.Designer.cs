namespace ProXmlFeeder
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtXmlFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Savebtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.LogPath = new System.Windows.Forms.Button();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtXmlFileName
            // 
            this.txtXmlFileName.Location = new System.Drawing.Point(53, 15);
            this.txtXmlFileName.Name = "txtXmlFileName";
            this.txtXmlFileName.Size = new System.Drawing.Size(607, 20);
            this.txtXmlFileName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = " ";
            // 
            // Savebtn
            // 
            this.Savebtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Savebtn.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.Savebtn.FlatAppearance.BorderSize = 0;
            this.Savebtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.Savebtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.BlueViolet;
            this.Savebtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Savebtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Savebtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Savebtn.Location = new System.Drawing.Point(750, 11);
            this.Savebtn.Margin = new System.Windows.Forms.Padding(0);
            this.Savebtn.Name = "Savebtn";
            this.Savebtn.Size = new System.Drawing.Size(75, 27);
            this.Savebtn.TabIndex = 4;
            this.Savebtn.Text = "Save";
            this.Savebtn.UseVisualStyleBackColor = false;
            this.Savebtn.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(51, 76);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(615, 20);
            this.textBox1.TabIndex = 5;
            // 
            // LogPath
            // 
            this.LogPath.Location = new System.Drawing.Point(685, 72);
            this.LogPath.Name = "LogPath";
            this.LogPath.Size = new System.Drawing.Size(121, 28);
            this.LogPath.TabIndex = 6;
            this.LogPath.Text = "Upload Log Path";
            this.LogPath.UseVisualStyleBackColor = true;
            this.LogPath.Click += new System.EventHandler(this.LogPath_Click);
           // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(0, 102);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(988, 640);
            this.txtConsole.TabIndex = 3;
            this.txtConsole.Text = "";
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(672, 11);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 27);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
//            this.BackgroundImage = global::ProXmlFeeder.Properties.Resources.RePlay_athletesBackground;
            this.ClientSize = new System.Drawing.Size(988, 742);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.LogPath);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Savebtn);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtXmlFileName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtXmlFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Savebtn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button LogPath;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.Button btnStart;
    }
}

