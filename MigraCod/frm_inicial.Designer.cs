namespace MigraCod
{
    partial class frm_inicial
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
            this.bt_intermediario = new System.Windows.Forms.Button();
            this.rtb_de = new System.Windows.Forms.RichTextBox();
            this.rtb_intermediário = new System.Windows.Forms.RichTextBox();
            this.lb_de = new System.Windows.Forms.Label();
            this.lb_para = new System.Windows.Forms.Label();
            this.rtb_para = new System.Windows.Forms.RichTextBox();
            this.lb_intermediario = new System.Windows.Forms.Label();
            this.lb_distancia = new System.Windows.Forms.Label();
            this.tb_distancia = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bt_intermediario
            // 
            this.bt_intermediario.Location = new System.Drawing.Point(464, 369);
            this.bt_intermediario.Name = "bt_intermediario";
            this.bt_intermediario.Size = new System.Drawing.Size(75, 23);
            this.bt_intermediario.TabIndex = 0;
            this.bt_intermediario.Text = "MIGRAR";
            this.bt_intermediario.UseVisualStyleBackColor = true;
            this.bt_intermediario.Click += new System.EventHandler(this.bt_intermediario_Click);
            // 
            // rtb_de
            // 
            this.rtb_de.Location = new System.Drawing.Point(37, 44);
            this.rtb_de.Name = "rtb_de";
            this.rtb_de.Size = new System.Drawing.Size(290, 290);
            this.rtb_de.TabIndex = 2;
            this.rtb_de.Text = "";
            // 
            // rtb_intermediário
            // 
            this.rtb_intermediário.Location = new System.Drawing.Point(363, 44);
            this.rtb_intermediário.Name = "rtb_intermediário";
            this.rtb_intermediário.Size = new System.Drawing.Size(290, 290);
            this.rtb_intermediário.TabIndex = 3;
            this.rtb_intermediário.Text = "";
            // 
            // lb_de
            // 
            this.lb_de.AutoSize = true;
            this.lb_de.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_de.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lb_de.Location = new System.Drawing.Point(79, 12);
            this.lb_de.Name = "lb_de";
            this.lb_de.Size = new System.Drawing.Size(185, 20);
            this.lb_de.TabIndex = 4;
            this.lb_de.Text = "De: Código NATURAL";
            // 
            // lb_para
            // 
            this.lb_para.AutoSize = true;
            this.lb_para.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_para.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lb_para.Location = new System.Drawing.Point(759, 12);
            this.lb_para.Name = "lb_para";
            this.lb_para.Size = new System.Drawing.Size(139, 20);
            this.lb_para.TabIndex = 5;
            this.lb_para.Text = "Para: Código C#";
            // 
            // rtb_para
            // 
            this.rtb_para.Location = new System.Drawing.Point(691, 44);
            this.rtb_para.Name = "rtb_para";
            this.rtb_para.Size = new System.Drawing.Size(438, 290);
            this.rtb_para.TabIndex = 6;
            this.rtb_para.Text = "";
            // 
            // lb_intermediario
            // 
            this.lb_intermediario.AutoSize = true;
            this.lb_intermediario.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_intermediario.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lb_intermediario.Location = new System.Drawing.Point(431, 12);
            this.lb_intermediario.Name = "lb_intermediario";
            this.lb_intermediario.Size = new System.Drawing.Size(183, 20);
            this.lb_intermediario.TabIndex = 7;
            this.lb_intermediario.Text = "Conjunto das Classes";
            // 
            // lb_distancia
            // 
            this.lb_distancia.AutoSize = true;
            this.lb_distancia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_distancia.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lb_distancia.Location = new System.Drawing.Point(37, 369);
            this.lb_distancia.Name = "lb_distancia";
            this.lb_distancia.Size = new System.Drawing.Size(97, 13);
            this.lb_distancia.TabIndex = 8;
            this.lb_distancia.Text = "Valor Distância:";
            // 
            // tb_distancia
            // 
            this.tb_distancia.Location = new System.Drawing.Point(141, 361);
            this.tb_distancia.Name = "tb_distancia";
            this.tb_distancia.Size = new System.Drawing.Size(58, 20);
            this.tb_distancia.TabIndex = 9;
            // 
            // frm_inicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 420);
            this.Controls.Add(this.tb_distancia);
            this.Controls.Add(this.lb_distancia);
            this.Controls.Add(this.lb_intermediario);
            this.Controls.Add(this.rtb_para);
            this.Controls.Add(this.lb_para);
            this.Controls.Add(this.lb_de);
            this.Controls.Add(this.rtb_intermediário);
            this.Controls.Add(this.rtb_de);
            this.Controls.Add(this.bt_intermediario);
            this.Name = "frm_inicial";
            this.Text = "Migrador de NATURAL para C#";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_intermediario;
        private System.Windows.Forms.RichTextBox rtb_de;
        private System.Windows.Forms.RichTextBox rtb_intermediário;
        private System.Windows.Forms.Label lb_de;
        private System.Windows.Forms.Label lb_para;
        private System.Windows.Forms.RichTextBox rtb_para;
        private System.Windows.Forms.Label lb_intermediario;
        private System.Windows.Forms.Label lb_distancia;
        private System.Windows.Forms.TextBox tb_distancia;
    }
}

