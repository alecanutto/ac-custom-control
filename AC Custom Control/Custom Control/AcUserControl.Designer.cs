
namespace AC_Control
{
    partial class Campo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.acContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.desfazerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.recortarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excluirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.selecionarTudoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.acLabel = new AC_Control.AcLabel();
            this.acTextBox = new AC_Control.AcTextBox();
            this.acComboBox = new AC_Control.AcComboBox();
            this.acContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // acContextMenuStrip
            // 
            this.acContextMenuStrip.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.acContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.desfazerToolStripMenuItem,
            this.toolStripSeparator1,
            this.recortarToolStripMenuItem,
            this.copiarToolStripMenuItem,
            this.colarToolStripMenuItem,
            this.excluirToolStripMenuItem,
            this.toolStripSeparator2,
            this.selecionarTudoToolStripMenuItem});
            this.acContextMenuStrip.Name = "acContextMenuStrip";
            this.acContextMenuStrip.ShowImageMargin = false;
            this.acContextMenuStrip.Size = new System.Drawing.Size(153, 148);
            this.acContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // desfazerToolStripMenuItem
            // 
            this.desfazerToolStripMenuItem.Name = "desfazerToolStripMenuItem";
            this.desfazerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.desfazerToolStripMenuItem.Text = "Desfazer";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // recortarToolStripMenuItem
            // 
            this.recortarToolStripMenuItem.Name = "recortarToolStripMenuItem";
            this.recortarToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recortarToolStripMenuItem.Text = "Recortar";
            // 
            // copiarToolStripMenuItem
            // 
            this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
            this.copiarToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copiarToolStripMenuItem.Text = "Copiar";
            // 
            // colarToolStripMenuItem
            // 
            this.colarToolStripMenuItem.Name = "colarToolStripMenuItem";
            this.colarToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.colarToolStripMenuItem.Text = "Colar";
            // 
            // excluirToolStripMenuItem
            // 
            this.excluirToolStripMenuItem.Name = "excluirToolStripMenuItem";
            this.excluirToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.excluirToolStripMenuItem.Text = "Excluir";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // selecionarTudoToolStripMenuItem
            // 
            this.selecionarTudoToolStripMenuItem.Name = "selecionarTudoToolStripMenuItem";
            this.selecionarTudoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selecionarTudoToolStripMenuItem.Text = "Selecionar tudo";
            // 
            // acLabel
            // 
            this.acLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.acLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.acLabel.Location = new System.Drawing.Point(-3, 0);
            this.acLabel.Name = "acLabel";
            this.acLabel.Size = new System.Drawing.Size(120, 16);
            this.acLabel.TabIndex = 1;
            this.acLabel.Text = "Título";
            this.acLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // acTextBox
            // 
            this.acTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.acTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.acTextBox.Font = new System.Drawing.Font("Arial", 9F);
            this.acTextBox.Location = new System.Drawing.Point(0, 16);
            this.acTextBox.MinimumSize = new System.Drawing.Size(4, 23);
            this.acTextBox.Name = "acTextBox";
            this.acTextBox.Size = new System.Drawing.Size(120, 25);
            this.acTextBox.TabIndex = 2;
            this.acTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.acTextBox.Enter += new System.EventHandler(this.TextBox_Enter);
            this.acTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            this.acTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            this.acTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            this.acTextBox.Leave += new System.EventHandler(this.TextBox_Leave);
            this.acTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBox_PreviewKeyDown);
            // 
            // acComboBox
            // 
            this.acComboBox.DropDownHeight = 60;
            this.acComboBox.DropDownWidth = 10;
            this.acComboBox.Font = new System.Drawing.Font("Arial", 9F);
            this.acComboBox.FormattingEnabled = true;
            this.acComboBox.IntegralHeight = false;
            this.acComboBox.Location = new System.Drawing.Point(0, 16);
            this.acComboBox.Name = "acComboBox";
            this.acComboBox.Size = new System.Drawing.Size(120, 25);
            this.acComboBox.TabIndex = 0;
            this.acComboBox.TabStop = false;
            this.acComboBox.DropDown += new System.EventHandler(this.ComboBox_DropDown);
            this.acComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            this.acComboBox.DropDownClosed += new System.EventHandler(this.ComboBox_DropDownClosed);
            this.acComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ComboBox_KeyDown);
            this.acComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox_KeyPress);
            this.acComboBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ComboBox_MouseClick);
            // 
            // Campo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.acLabel);
            this.Controls.Add(this.acTextBox);
            this.Controls.Add(this.acComboBox);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Campo";
            this.Size = new System.Drawing.Size(120, 41);
            this.SizeChanged += new System.EventHandler(this.Campo_SizeChanged);
            this.Enter += new System.EventHandler(this.Campo_Enter);
            this.Leave += new System.EventHandler(this.Campo_Leave);
            this.Resize += new System.EventHandler(this.Campo_Resize);
            this.acContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AcTextBox acTextBox;
        private AcComboBox acComboBox;
        private AcLabel acLabel;
        private System.Windows.Forms.ContextMenuStrip acContextMenuStrip;
        private System.Windows.Forms.ToolTip acToolTip;
        private System.Windows.Forms.ToolStripMenuItem desfazerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem recortarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copiarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excluirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem selecionarTudoToolStripMenuItem;
    }
}
