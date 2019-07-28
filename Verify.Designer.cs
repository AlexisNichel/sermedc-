using System.Windows.Forms;

namespace Sermed
{
    partial class Form2
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
            this.label2 = new System.Windows.Forms.Label();
            this.bartimeout = new System.Windows.Forms.ProgressBar();
            this.btncancel = new System.Windows.Forms.Button();
            this.btncancel2 = new System.Windows.Forms.Button();
            this.btnEnroll = new System.Windows.Forms.Button();
            this.lblstep = new System.Windows.Forms.Label();
            this.flecha1 = new System.Windows.Forms.PictureBox();
            this.step3picture = new System.Windows.Forms.PictureBox();
            this.step2picture = new System.Windows.Forms.PictureBox();
            this.step1picture = new System.Windows.Forms.PictureBox();
            this.huella2 = new System.Windows.Forms.PictureBox();
            this.dedo2 = new System.Windows.Forms.PictureBox();
            this.huella1 = new System.Windows.Forms.PictureBox();
            this.dedo1 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flecha2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.flecha1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.step3picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.step2picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.step1picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.huella2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dedo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.huella1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dedo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flecha2)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(350, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(984, 78);
            this.label2.TabIndex = 4;
            this.label2.Text = "Coloque la huella del dedo marcado";
            // 
            // bartimeout
            // 
            this.bartimeout.Location = new System.Drawing.Point(336, 824);
            this.bartimeout.Name = "bartimeout";
            this.bartimeout.Size = new System.Drawing.Size(1020, 28);
            this.bartimeout.TabIndex = 5;
            this.bartimeout.Visible = false;
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(742, 901);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(207, 69);
            this.btncancel.TabIndex = 6;
            this.btncancel.Text = "Cancelar";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Visible = false;
            this.btncancel.Click += new System.EventHandler(this.btn_cancel_click);
            // 
            // btncancel2
            // 
            this.btncancel2.Location = new System.Drawing.Point(336, 901);
            this.btncancel2.Name = "btncancel2";
            this.btncancel2.Size = new System.Drawing.Size(501, 69);
            this.btncancel2.TabIndex = 11;
            this.btncancel2.Text = "Cancelar";
            this.btncancel2.UseVisualStyleBackColor = true;
            this.btncancel2.Visible = false;
            this.btncancel2.Click += new System.EventHandler(this.Btncancel2_Click);
            // 
            // btnEnroll
            // 
            this.btnEnroll.BackColor = System.Drawing.Color.DarkGreen;
            this.btnEnroll.ForeColor = System.Drawing.Color.White;
            this.btnEnroll.Location = new System.Drawing.Point(855, 901);
            this.btnEnroll.Name = "btnEnroll";
            this.btnEnroll.Size = new System.Drawing.Size(501, 69);
            this.btnEnroll.TabIndex = 12;
            this.btnEnroll.Text = "Enrolar";
            this.btnEnroll.UseVisualStyleBackColor = false;
            this.btnEnroll.Visible = false;
            this.btnEnroll.Click += new System.EventHandler(this.BtnEnroll_Click);
            // 
            // lblstep
            // 
            this.lblstep.AutoSize = true;
            this.lblstep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblstep.ForeColor = System.Drawing.Color.Maroon;
            this.lblstep.Location = new System.Drawing.Point(600, 250);
            this.lblstep.Name = "lblstep";
            this.lblstep.Size = new System.Drawing.Size(472, 37);
            this.lblstep.TabIndex = 14;
            this.lblstep.Text = "Coloque el dedo seleccionado";
            this.lblstep.Visible = false;
            // 
            // flecha1
            // 
            this.flecha1.BackgroundImage = global::sermed.Properties.Resources.descarga;
            this.flecha1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flecha1.Location = new System.Drawing.Point(483, 447);
            this.flecha1.Name = "flecha1";
            this.flecha1.Size = new System.Drawing.Size(140, 132);
            this.flecha1.TabIndex = 17;
            this.flecha1.TabStop = false;
            this.flecha1.Visible = false;
            // 
            // step3picture
            // 
            this.step3picture.BackgroundImage = global::sermed.Properties.Resources.asda;
            this.step3picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.step3picture.Location = new System.Drawing.Point(1283, 346);
            this.step3picture.Name = "step3picture";
            this.step3picture.Size = new System.Drawing.Size(358, 340);
            this.step3picture.TabIndex = 16;
            this.step3picture.TabStop = false;
            this.step3picture.Visible = false;
            // 
            // step2picture
            // 
            this.step2picture.BackgroundImage = global::sermed.Properties.Resources.asda;
            this.step2picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.step2picture.Location = new System.Drawing.Point(676, 346);
            this.step2picture.Name = "step2picture";
            this.step2picture.Size = new System.Drawing.Size(358, 340);
            this.step2picture.TabIndex = 15;
            this.step2picture.TabStop = false;
            this.step2picture.Visible = false;
            // 
            // step1picture
            // 
            this.step1picture.BackgroundImage = global::sermed.Properties.Resources.asda;
            this.step1picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.step1picture.Location = new System.Drawing.Point(73, 346);
            this.step1picture.Name = "step1picture";
            this.step1picture.Size = new System.Drawing.Size(358, 340);
            this.step1picture.TabIndex = 13;
            this.step1picture.TabStop = false;
            this.step1picture.Visible = false;
            // 
            // huella2
            // 
            this.huella2.BackgroundImage = global::sermed.Properties.Resources.fin1;
            this.huella2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.huella2.Location = new System.Drawing.Point(867, 456);
            this.huella2.Name = "huella2";
            this.huella2.Size = new System.Drawing.Size(100, 123);
            this.huella2.TabIndex = 10;
            this.huella2.TabStop = false;
            this.huella2.Visible = false;
            // 
            // dedo2
            // 
            this.dedo2.BackgroundImage = global::sermed.Properties.Resources.fin12;
            this.dedo2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dedo2.Location = new System.Drawing.Point(867, 456);
            this.dedo2.Name = "dedo2";
            this.dedo2.Size = new System.Drawing.Size(100, 123);
            this.dedo2.TabIndex = 9;
            this.dedo2.TabStop = false;
            this.dedo2.Click += new System.EventHandler(this.Dedo2_Click);
            // 
            // huella1
            // 
            this.huella1.BackgroundImage = global::sermed.Properties.Resources.ssss;
            this.huella1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.huella1.Location = new System.Drawing.Point(994, 250);
            this.huella1.Name = "huella1";
            this.huella1.Size = new System.Drawing.Size(78, 116);
            this.huella1.TabIndex = 8;
            this.huella1.TabStop = false;
            // 
            // dedo1
            // 
            this.dedo1.BackgroundImage = global::sermed.Properties.Resources.Sin_título_11;
            this.dedo1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dedo1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dedo1.Location = new System.Drawing.Point(994, 250);
            this.dedo1.Name = "dedo1";
            this.dedo1.Size = new System.Drawing.Size(78, 116);
            this.dedo1.TabIndex = 7;
            this.dedo1.TabStop = false;
            this.dedo1.Click += new System.EventHandler(this.Dedo1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::sermed.Properties.Resources.hands;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(336, 214);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1020, 570);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // flecha2
            // 
            this.flecha2.BackgroundImage = global::sermed.Properties.Resources.descarga;
            this.flecha2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flecha2.Location = new System.Drawing.Point(1092, 456);
            this.flecha2.Name = "flecha2";
            this.flecha2.Size = new System.Drawing.Size(140, 132);
            this.flecha2.TabIndex = 18;
            this.flecha2.TabStop = false;
            this.flecha2.Visible = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1720, 1048);
            this.ControlBox = false;
            this.Controls.Add(this.flecha2);
            this.Controls.Add(this.flecha1);
            this.Controls.Add(this.step3picture);
            this.Controls.Add(this.step2picture);
            this.Controls.Add(this.lblstep);
            this.Controls.Add(this.step1picture);
            this.Controls.Add(this.btnEnroll);
            this.Controls.Add(this.btncancel2);
            this.Controls.Add(this.huella2);
            this.Controls.Add(this.dedo2);
            this.Controls.Add(this.huella1);
            this.Controls.Add(this.dedo1);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.bartimeout);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Validar";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.flecha1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.step3picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.step2picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.step1picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.huella2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dedo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.huella1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dedo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flecha2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar bartimeout;
        private System.Windows.Forms.Button btncancel;
        private PictureBox dedo1;
        private PictureBox huella1;
        private PictureBox dedo2;
        private PictureBox huella2;
        private Button btncancel2;
        private Button btnEnroll;
        private PictureBox step1picture;
        private Label lblstep;
        private PictureBox step2picture;
        private PictureBox step3picture;
        private PictureBox flecha1;
        private PictureBox flecha2;
    }
}