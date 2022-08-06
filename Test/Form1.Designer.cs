
namespace Test
{
    partial class Form1
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
            this.acMenu1 = new AC_Control.AcMenu();
            this.campo1 = new AC_Control.Campo();
            this.acButton1 = new AC_Control.AcButton();
            this.SuspendLayout();
            // 
            // acMenu1
            // 
            this.acMenu1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acMenu1.LeaveFocus = false;
            this.acMenu1.Location = new System.Drawing.Point(259, 74);
            this.acMenu1.Name = "acMenu1";
            this.acMenu1.Size = new System.Drawing.Size(170, 93);
            this.acMenu1.TabIndex = 3;
            this.acMenu1.Title = null;
            this.acMenu1.Visible = false;
            // 
            // campo1
            // 
            this.campo1.BackColor = System.Drawing.Color.Transparent;
            this.campo1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.campo1.Location = new System.Drawing.Point(12, 27);
            this.campo1.Name = "campo1";
            this.campo1.ShowButton = false;
            this.campo1.Size = new System.Drawing.Size(120, 41);
            this.campo1.TabIndex = 2;
            // 
            // acButton1
            // 
            this.acButton1.BackColor = System.Drawing.Color.White;
            this.acButton1.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.acButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.acButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.acButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.acButton1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.acButton1.Location = new System.Drawing.Point(211, 43);
            this.acButton1.Name = "acButton1";
            this.acButton1.Size = new System.Drawing.Size(85, 25);
            this.acButton1.TabIndex = 1;
            this.acButton1.Text = "acButton1";
            this.acButton1.ToolTip = null;
            this.acButton1.UseVisualStyleBackColor = false;
            this.acButton1.Click += new System.EventHandler(this.acButton1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.acMenu1);
            this.Controls.Add(this.campo1);
            this.Controls.Add(this.acButton1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private AC_Control.AcButton acButton1;
        private AC_Control.Campo campo1;
        private AC_Control.AcMenu acMenu1;
    }
}