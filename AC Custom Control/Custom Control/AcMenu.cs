using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace AC_Control
{
    public partial class AcMenu : UserControl
    {

        #region Private Fields

        private readonly Dictionary<int, DataTable> _subMenu;

        private Color _cellColor;

        private DataColumn _dataColumn;

        private DataRow _dataRow;

        private DataTable _dtMenu, _dtSubMenu;

        private int _itemIndex;

        private Color _panelColor;

        private bool _showSubMenu, _closingSubMenu, _focusMouseControl, _loaded;

        private string _title;

        private Color _titleColor;

        #endregion Private Fields

        #region Public Constructors

        public AcMenu()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Opaque, true);
            _titleColor = Color.Black;
            _cellColor = Color.DodgerBlue;
            _panelColor = Color.MediumSeaGreen;
            _subMenu = new Dictionary<int, DataTable>();
            Size = panel1.Size;
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void CellClickEventHandler(object sender, DataGridViewCellMouseEventArgs e);

        public delegate void CellEnterEventHandler(object sender, DataGridViewCellEventArgs e);

        #endregion Public Delegates

        #region Public Events

        public event CellClickEventHandler CellClick;

        public event CellEnterEventHandler CellEnter;

        #endregion Public Events

        #region Public Properties

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color CellColor
        {
            get =>  _cellColor;
            set
            {
                if (_cellColor != value)
                {
                    _cellColor = value;
                }
            }
        }

        [Category("AC Control")]
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsSubItem { get; set; }

        [Category("AC Control")]
        [Browsable(false)]
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Item ItemSelected { get; set; }

        [Category("AC Control")]
        [Browsable(false)]
        [DefaultValue(true)]
        public bool LeaveFocus { get; set; }

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "MediumSeaGreen")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color PanelColor
        {
            get => _panelColor;
            set
            {
                if (_panelColor != value)
                {
                    _panelColor = value;
                    label1.BackColor = _panelColor;
                    splitter1.BackColor = _panelColor;
                    splitter2.BackColor = _panelColor;
                }
            }
        }

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue("Title")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                label1.Text = _title;
            }
        }

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Black")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color TitleColor
        {
            get => _titleColor;
            set
            {
                if (_titleColor != value)
                {
                    _titleColor = value;
                    label1.ForeColor = _titleColor;
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void AddItemOrSubItem(
            string value,
            int reg = -1,
            int orderBy = 1,
            bool hasSubMenu = false,
            int codMenu = -1)
        {
            if (codMenu > -1)
            {
                CreateDataTable(ref _dtSubMenu);

                if (reg == -1)
                {
                    _dtSubMenu.Columns[0].AutoIncrement = true;
                    _dtSubMenu.Columns[0].AutoIncrementStep = 1;
                }

                _dataRow = _dtSubMenu.NewRow();

                if (reg > -1)
                {
                    _dataRow[0] = reg;
                }

                _dataRow[1] = value;
                _dataRow[2] = hasSubMenu;
                _dataRow[3] = codMenu;

                _dtSubMenu.Rows.Add(_dataRow);

                if (orderBy > -1)
                {
                    _dtSubMenu.DefaultView.Sort = _dtSubMenu.Columns[orderBy].ColumnName;
                }

                if (!_subMenu.ContainsKey(codMenu))
                {
                    _subMenu.Add(codMenu, _dtSubMenu);
                }

                return;
            }

            CreateDataTable(ref _dtMenu);

            if (reg == -1)
            {
                _dtMenu.Columns[0].AutoIncrement = true;
                _dtMenu.Columns[0].AutoIncrementStep = 1;
            }

            _dataRow = _dtMenu.NewRow();

            if (reg > -1)
            {
                _dataRow[0] = reg;
            }

            _dataRow[1] = value;
            _dataRow[2] = hasSubMenu;
            _dataRow[3] = codMenu;

            _dtMenu.Rows.Add(_dataRow);

            if (orderBy > -1)
            {
                _dtMenu.DefaultView.Sort = _dtMenu.Columns[orderBy].ColumnName;
            }

            dataGridView1.DataSource = _dtMenu;
            dataGridView1.Height = dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            panel1.Height = 2 + splitter2.Height + dataGridView1.Height;
        }

        public void AlterItemOrSubItem(
            string value,
            int reg = -1,
            int codItem = -1)
        {
            if (codItem > -1)
            {
                if (!_subMenu.ContainsKey(codItem)) return;

                foreach (DataRow row in _subMenu[codItem].Rows)
                {
                    if ((int)row[0] == reg)
                    {
                        row[1] = value;
                        break;
                    }
                }

                return;
            }

            if (dataGridView1.DataSource == null) return;

            foreach (DataRow row in dataGridView1.Rows)
            {
                if ((int)row[0] == reg)
                {
                    row[1] = value;
                    break;
                }
            }
        }

        public void ClearDataTable()
        {
            _dtMenu.Clear();
            _dtSubMenu.Clear();
            _subMenu.Clear();
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
        }

        public object GetColumnValue(int _row, int _column, int _codItem = -1)
        {
            try
            {
                if (_codItem > -1 && IsSubItem)
                {
                    return dataGridView2.Rows[_row].Cells[_column].Value;
                }
                return dataGridView1.Rows[_row].Cells[_column].Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Exibe controle de acordo com localização informada manualmente
        /// </summary>
        /// <param name="point">Localização do controle</param>
        public void ShowOrHide(Point point = default)
        {
            if (!Visible)
            {
                Location = point;
                Visible = true;
                return;
            }
            _itemIndex = 0;
            _loaded = false;
            dataGridView2.Visible = false;
            Visible = false;
        }

        #endregion Public Methods

        #region Private Methods

        private void AcMenu_Leave(object sender, EventArgs e)
        {
            ShowOrHide();
        }

        private void AcMenu_MouseEnter(object sender, EventArgs e)
        {
            dataGridView2.Hide();
            Size = panel1.Size;
        }

        private void AcMenu_SizeChanged(object sender, EventArgs e)
        {
            if (!_showSubMenu)
            {
                panel1.Size = Size;
            }
        }

        private void AcMenu_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                dataGridView2.Visible = false;
                dataGridView1.Focus();
                dataGridView1.ClearSelection();
            }
        }

        private void CloseSubMenu()
        {
            dataGridView2.Hide();
            Size = panel1.Size;
            dataGridView1.Focus();
        }

        private void CreateDataTable(
            ref DataTable dataTable,
            string tbl = "itens",
            string column1 = "codigo",
            string column2 = "texto",
            string column3 = "subMenu",
            string column4 = "codMenu",
            List<string> columns = null)
        {
            if (dataTable != null) return;

            var dataTable1 = new DataTable(tbl);
            _dataColumn = new DataColumn()
            {
                DataType = Type.GetType("System.Int32"),
                ColumnName = column1,
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1
            };
            dataTable1.Columns.Add(_dataColumn);
            _dataColumn = new DataColumn()
            {
                DataType = Type.GetType("System.String"),
                ColumnName = column2
            };
            dataTable1.Columns.Add(_dataColumn);
            _dataColumn = new DataColumn()
            {
                DataType = Type.GetType("System.Boolean"),
                ColumnName = column3
            };
            dataTable1.Columns.Add(_dataColumn);
            _dataColumn = new DataColumn
            {
                DataType = Type.GetType("System.Int32"),
                ColumnName = column4
            };
            dataTable1.Columns.Add(_dataColumn);
            if (columns is object)
            {
                foreach (var column in columns)
                {
                    _dataColumn = new DataColumn()
                    {
                        DataType = Type.GetType("System.String"),
                        ColumnName = column
                    };
                    dataTable1.Columns.Add(_dataColumn);
                }
            }
            dataTable = dataTable1;
        }

        private void DataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (dataGridView.Name.Equals("dataGridView2"))
            {
                _closingSubMenu = true;
                IsSubItem = true;
                CloseSubMenu();
                ItemSelected = new Item((int)dataGridView2.Rows[e.RowIndex].Cells[0].Value,
                    dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            }
            else
            {
                IsSubItem = false;
                ItemSelected = new Item((int)dataGridView1.Rows[e.RowIndex].Cells[0].Value,
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            }
            Visible = false;
            CellClick?.Invoke(this, e);
        }

        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            dataGridView.Columns[0].HeaderText = "Código";
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[0].Width = 0;
            dataGridView.Columns[1].HeaderText = "Texto";
            dataGridView.Columns[1].Visible = true;
            dataGridView.Columns[1].Width = dataGridView1.Width;
            dataGridView.Columns[2].HeaderText = "SubItem";
            dataGridView.Columns[2].Visible = false;
            dataGridView.Columns[2].Width = 0;
            dataGridView.Columns[3].HeaderText = "CodItem";
            dataGridView.Columns[3].Visible = false;
            dataGridView.Columns[3].Width = 0;
            dataGridView.CurrentCell = null;
            dataGridView.ClearSelection();
        }

        private void DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.Handled = true;
                    DataGridView_CellMouseClick(dataGridView1,
                        new DataGridViewCellMouseEventArgs(1, _itemIndex, 0, 0, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)));
                    break;

                case Keys.Right:
                    dataGridView2.Focus();
                    break;

                case Keys.Up:
                    if (_itemIndex == 0)
                    {
                        _itemIndex = dataGridView1.Rows.Count;
                    }
                    DataGridView1_RowEnter(dataGridView1, new DataGridViewCellEventArgs(1, --_itemIndex));
                    break;

                case Keys.Down:
                    if (!_loaded)
                    {
                        _itemIndex = -1;
                        _loaded = true;
                    }
                    else if (_itemIndex + 1 == dataGridView1.Rows.Count)
                    {
                        _itemIndex = -1;
                    }
                    DataGridView1_RowEnter(dataGridView1, new DataGridViewCellEventArgs(1, ++_itemIndex));
                    //Cursor.Position = new Point(Location.X, Cursor.Position.Y);
                    break;
            }
        }

        private void DataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                _closingSubMenu = true;
                CloseSubMenu();
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                DataGridView_CellMouseClick(dataGridView2,
                    new DataGridViewCellMouseEventArgs(1, dataGridView2.CurrentRow.Index, 0, 0, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)));
            }
        }

        private void DataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            _focusMouseControl = true;
            SelectMenu(e.RowIndex);
            CellEnter?.Invoke(this, e);
        }

        private void DataGridView1_MouseLeave(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void DataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!_closingSubMenu) SelectMenu(_itemIndex);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            _closingSubMenu = false;
        }

        private void DataGridView2_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            _focusMouseControl = true;
            IsSubItem = true;
            dataGridView2.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = _cellColor;
            dataGridView2.Rows[e.RowIndex].Selected = true;
            CellEnter?.Invoke(this, e);
        }

        private void DataGridView2_MouseLeave(object sender, EventArgs e)
        {
            _focusMouseControl = false;
            CloseSubMenu();
        }

        private void DataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_focusMouseControl) return;
            IsSubItem = true;
            dataGridView2.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = _cellColor;
            dataGridView2.Rows[e.RowIndex].Selected = true;
            CellEnter?.Invoke(this, e);
        }
        private void SelectMenu(int _index)
        {
            _showSubMenu = true;
            _closingSubMenu = false;
            dataGridView1.Rows[_index].DefaultCellStyle.SelectionForeColor = _cellColor;
            dataGridView1.Rows[_index].Selected = true;
            dataGridView2.CurrentCell = null;
            if ((bool)dataGridView1.Rows[_index].Cells[2].Value)
            {
                dataGridView2.Location = new Point(panel1.Right - 5, (dataGridView1.Top + 10) + (_index * dataGridView1.Rows[_index].Height));
                ShowSubMenu((int)dataGridView1.Rows[_index].Cells[0].Value);
                return;
            }
            CloseSubMenu();
        }

        private void ShowSubMenu(int codMenu)
        {
            if (!_subMenu.ContainsKey(codMenu))
            {
                return;
            }
            dataGridView2.DataSource = _subMenu[codMenu];
            dataGridView2.Width = 5 + dataGridView2.Columns.GetColumnsWidth(DataGridViewElementStates.Visible);
            dataGridView2.Height = 3 + dataGridView2.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            Width = dataGridView2.Right;
            Height = dataGridView2.Bottom;
            dataGridView2.Visible = true;
            dataGridView2.ClearSelection();
        }

        #endregion Private Methods

        #region Public Structs

        public struct Item
        {

            #region Public Constructors

            public Item(int cod, string value)
            {
                Cod = cod;
                Value = value;
            }

            #endregion Public Constructors

            #region Public Properties

            public int Cod { get; set; }
            public string Value { get; set; }

            #endregion Public Properties

        }

        #endregion Public Structs

    }
}