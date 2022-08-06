using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AC_Control
{
    public partial class AcDataGridView : DataGridView
    {
        #region Private Fields

        private bool _showCheckBox = false;

        #endregion Private Fields

        #region Public Constructors

        public AcDataGridView()
        {
            BackgroundColor = Color.White;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeColumns = false;
            AllowUserToResizeRows = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            RowHeadersVisible = false;
            EnableHeadersVisualStyles = false;
            ReadOnly = true;
            AutoSize = false;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            GridColor = Color.DimGray;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9f, FontStyle.Bold);
            ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(185, 209, 234);
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DefaultCellStyle.Font = new Font("Arial", 9f, FontStyle.Bold);
        }

        #endregion Public Constructors

        #region Public Properties

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool ShowCheckBox
        {
            get => _showCheckBox;
            set => _showCheckBox = value;
        }

        #endregion Public Properties

        #region Public Methods

        public List<DataGridViewRow> GetSelectedRows()
        {
            var rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow item in Rows)
            {
                if ((bool)item.Cells["checkBox"].Value) rows.Add(item);
            }
            return rows;
        }

        public void SelectCheckBox(int rowIndex)
        {
            if (!Columns[0].Name.Contains("checkBox")) return;

            DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)Rows[rowIndex].Cells[0];
            chk.Value = !(chk.Value != null && (bool)chk.Value);
        }

        #endregion Public Methods

        #region Private Methods

        private void AddCheckBox()
        {
            if (Columns["checkBox"] != null) return;

            var checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "checkBox",
                Name = "checkBox",
                HeaderText = "",
                DisplayIndex = 0,
                Width = 20
            };

            Columns.Insert(0, checkBoxColumn);
        }

        #endregion Private Methods

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            ClearSelection();
            if (_showCheckBox) AddCheckBox();
            base.OnDataBindingComplete(e);
        }

    }
}