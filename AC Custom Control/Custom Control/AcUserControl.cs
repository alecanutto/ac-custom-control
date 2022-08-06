using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AC_Control
{
    public partial class Campo : UserControl
    {
        #region Campo

        public Campo()
        {
            DoubleBuffered = true;
            InitializeComponent();
            BackColor = Color.Transparent;
            acLabel.BackColor = Color.Transparent;
        }

        private void Campo_Resize(object sender, EventArgs e)
        {
            if (_xResize || (_xDesigner && (_multiline || _isCombobox))) Refresh_Control();
        }

        private void Campo_SizeChanged(object sender, EventArgs e)
        {
            // ===========BLOQUEIA ALTERAÇÃO DE TAMANHO DO CAMPO EM MODO DESIGNER============='
            if (_xDesigner && !_xResize && !_multiline && (Height != acComboBox.Height + (_hideTitle ? 0 : acLabel.Height)))
            {
                Height = acComboBox.Height + (_hideTitle ? 0 : acLabel.Height);
            }
        }

        private void Campo_Enter(object sender, EventArgs e)
        {
            if (!EnabledText && !TextReadOnly)
            {
                acTextBox.BackColor = BackColorDisabled;
            }
            else
            {
                acTextBox.BackColor = BackColorFocused;
                acComboBox.BorderColor = BorderColorFocused;
            }
        }

        private void Campo_Leave(object sender, EventArgs e)
        {
            if (EnabledText)
            {
                acTextBox.BackColor = Color.White;
                acComboBox.BorderColor = BorderColor;
            }
            else if (!TextReadOnly)
            {
                acTextBox.BackColor = BackColorDisabled;
            }
        }

        #endregion Campo

        #region Enumeradores

        public enum TipoDeCampo
        {
            Texto,
            Data,
            DataHora,
            Moeda,
            Numerico,
            SemEspecial,
            Telefone,
            CPFCNPJ
        }

        #endregion Enumeradores

        #region Variaveis

        private ContentAlignment _alignTitle = ContentAlignment.TopLeft;
        private HorizontalAlignment _alinha = HorizontalAlignment.Left;
        private Color _backColor = Color.Transparent;
        private Color _borderColor = Color.DimGray;
        private Color _borderColorFocused = Color.DodgerBlue;
        private bool _borderStyleFixed = false;
        private CharacterCasing _characterCasing = CharacterCasing.Upper;
        private DataColumn _dataColumn;
        private DataRow _dataRow;
        private DataTable _dataTable;
        private bool _enabled = true;
        private Font _font = new Font("Arial", 9f);
        private Color _foreColor = Color.Black;
        private string _formatoCampo = "";
        private bool _gotFocus = false;
        private bool _hideTitle = false;
        private int _indexAnt = -1;
        private bool _isCombobox = false;
        private bool _isControl = false;
        private int _maxItemVisible = 4;
        private bool _mostraBtn = false;
        private bool _mouseClicked = false;
        private bool _multiline = false;
        private bool _passwordChar = false;
        private bool _pasting = false;
        private int _position = 0;
        private bool _readOnly = false;
        private string _textoAnt;
        private string _textoSql = "";
        private TipoDeCampo _tipoCampo = TipoDeCampo.Texto;
        private string _titulo = "Titulo";
        private string _toolTip = "";
        private int _xCodigo = -1;
        private bool _xResize = false;
        private string _xTexto = "";

        // Evita selecionar index da combobox quando preenche datasource
        private bool _load = true;

        // UTILIZADO PARA CONTROLE DE CONSTRUÇÃO
        private readonly bool _xDesigner = Assembly.GetExecutingAssembly().GetName().Name.Equals("AC_Control");

        public event KeyDown_ComboEventHandler KeyDown_Combo;

        public delegate void KeyDown_ComboEventHandler(object sender, KeyEventArgs e);

        public new event KeyDownEventHandler KeyDown;

        public delegate void KeyDownEventHandler(object sender, KeyEventArgs e);

        public new event KeyPressEventHandler KeyPress;

        public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);

        public event SelectedIndexEventHandler SelectedIndex;

        public delegate void SelectedIndexEventHandler(object sender, EventArgs e);

        public new event TextChangedEventHandler TextChanged;

        public delegate void TextChangedEventHandler(object sender, EventArgs e);

        public new event PreviewKeyDownEventHandler PreviewKeyDown;

        public delegate void PreviewKeyDownEventHandler(object sender, PreviewKeyDownEventArgs e);

        public new event LostFocusEventHandler LostFocus;

        public delegate void LostFocusEventHandler(object sender, EventArgs e);

        public new event GotFocusEventHandler GotFocus;

        public delegate void GotFocusEventHandler(object sender, EventArgs e);

        #endregion Variaveis

        #region Propridades Nativas Desativadas

        // ==================== COMENTEI POIS ESTAVA COMPROMENTENDO O COMPORTAMENTO DO CONTROLE

        // <Browsable(False)> Public Overrides Property AllowDrop As Boolean
        // <Browsable(False)> Public Overrides Property Anchor As AnchorStyles
        // <Browsable(False)> Public Overrides Property AutoScroll As Boolean
        // <Browsable(False)> Public Overrides Property AutoSize As Boolean
        // <Browsable(False)> Public Overrides Property AutoValidate As AutoValidate
        // <Browsable(False)> Public Overrides Property BackgroundImage As Image
        // <Browsable(False)> Public Overrides Property BackgroundImageLayout As ImageLayout
        // <Browsable(False)> Public Overrides Property ContextMenuStrip As ContextMenuStrip
        // <Browsable(False)> Public Overrides Property Cursor As Cursor
        // <Browsable(False)> Public Overrides Property Dock As DockStyle
        // <Browsable(False)> Public Overrides Property MaximumSize As Size
        // <Browsable(False)> Public Overrides Property MinimumSize As Size
        // <Browsable(False)> Public Overrides Property RightToLeft As RightToLeft

        #endregion Propridades Nativas Desativadas

        #region Propridades Ativas

        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define se o controle usará máscara.")]
        public bool EnabledMask { get; set; } = false;

        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define se controle será um combobox.")]
        public bool IsCombobox
        {
            get
            {
                return _isCombobox;
            }

            set
            {
                if (_isCombobox != value)
                {
                    _isCombobox = value;
                    if (_isCombobox)
                    {
                        if (MultiLine)
                        {
                            MultiLine = false;
                        }

                        if (PasswordChar)
                        {
                            PasswordChar = false;
                        }

                        if (!ShowButton)
                        {
                            ShowButton = true;
                        }
                    }
                    else
                    {
                        ShowButton = false;
                    }
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(4)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a quantidade máxima de itens visiveis na combo.")]
        public int MaxItensVisible
        {
            get
            {
                return _maxItemVisible;
            }

            set
            {
                _maxItemVisible = value;
                if (acComboBox.DropDownHeight != _maxItemVisible * acComboBox.ItemHeight)
                {
                    acComboBox.DropDownHeight = Convert.ToInt32(_maxItemVisible * acComboBox.ItemHeight);
                }
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o status do botão.")]
        public bool ShowButton
        {
            get
            {
                return _mostraBtn;
            }

            set
            {
                if (_mostraBtn != value)
                {
                    _mostraBtn = value;
                    if (_xDesigner)
                    {
                        Refresh_Control();
                    }
                    else
                    {
                        _xResize = true;
                    }
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o controle como somente leitura.")]
        public bool TextReadOnly
        {
            get
            {
                return _readOnly;
            }

            set
            {
                _readOnly = value;
                EnabledText = !_readOnly;
            }
        }

        [Browsable(true)]
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a tag do controle.")]
        public string TextTag { get; set; } = "";

        [Browsable(true)]
        [DefaultValue(typeof(Color), "Transparent")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a cor do controle.")]
        public Color Back_Color
        {
            get
            {
                return _backColor;
            }

            set
            {
                _backColor = value;
                acLabel.BackColor = _backColor;
            }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Color), "214, 224, 237")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a cor de fundo quando o controle está desabilitado.")]
        public Color BackColorDisabled { get; set; } = Color.FromArgb(214, 224, 237);

        [Browsable(true)]
        [DefaultValue(typeof(Color), "255, 255, 150")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a cor de fundo quando o controle recebe o foco.")]
        public Color BackColorFocused { get; set; } = Color.FromArgb(255, 255, 150);

        [Browsable(true)]
        [DefaultValue(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define se o campo será liberado para digitação.")]
        public bool BlockText { get; set; } = true;

        [Browsable(true)]
        [DefaultValue(typeof(Color), "DimGray")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a cor da borda do controle.")]
        [NotifyParentProperty(true)]
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }

            set
            {
                _borderColor = value;
                acLabel.BorderColor = _borderColor;
                acTextBox.BorderColor = _borderColor;
                acComboBox.BorderColor = _borderColor;
            }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a cor da borda Do controle quando recebe o foco.")]
        [NotifyParentProperty(true)]
        public Color BorderColorFocused
        {
            get
            {
                return _borderColorFocused;
            }

            set
            {
                if (IsCombobox)
                {
                    _borderColorFocused = Color.DodgerBlue;
                }
                else
                {
                    _borderColorFocused = value;
                }

                acTextBox.BorderColorFocused = _borderColorFocused;
                acComboBox.BorderColorFocused = _borderColorFocused;
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define borda do controle.")]
        public bool BorderStyleFixed
        {
            get
            {
                return _borderStyleFixed;
            }

            set
            {
                if (_borderStyleFixed != value)
                {
                    _borderStyleFixed = value;
                    if (_borderStyleFixed)
                    {
                        acLabel.Location = new Point(0, 0);
                        acLabel.Height += 2;
                        acLabel.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else
                    {
                        acLabel.Location = new Point(-3, 0);
                        acLabel.Height -= 2;
                        acLabel.BorderStyle = BorderStyle.None;
                    }

                    acLabel.Width = Width + (_borderStyleFixed ? 0 : 3);
                    if (Height != acLabel.Height + acTextBox.Height)
                    {
                        Height = acLabel.Height + acTextBox.Height;
                    }
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(CharacterCasing.Upper)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Obtém ou define se o controle TextBox modifica As maiúsculas e minúsculas de caracteres quando eles são digitados.")]
        public CharacterCasing CharacterCasing
        {
            get
            {
                return _characterCasing;
            }

            set
            {
                _characterCasing = value;
                acTextBox.CharacterCasing = _characterCasing;
            }
        }

        [Browsable(false)]
        [DefaultValue(-1)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Retorna o codigo Do registro selecionado na lista.")]
        public int Codigo
        {
            get
            {
                if (!Found())
                {
                    _xCodigo = -1;
                }

                return _xCodigo;
            }

            set
            {
                if (_xCodigo == value)
                {
                    return;
                }

                _xCodigo = value;
                if (_xCodigo == -1 || _dataTable is null)
                {
                    if (BlockText)
                    {
                        Texto = "";
                    }

                    return;
                }

                var dr = _dataTable.Select(_dataTable.Columns[0].ColumnName + "=" + _xCodigo);
                if (dr.Length > 0)
                {
                    if (!Texto.Equals(dr[0][DisplayMember]))
                    {
                        Texto = dr[0][DisplayMember].ToString();
                    }
                }
                else
                {
                    Texto = "";
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Retorna a quantodade de itens na lista.")]
        public string Count
        {
            get
            {
                return acComboBox.Items.Count.ToString();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Retorna o datasource da combobox.")]
        public DataTable GetDataSource
        {
            get
            {
                return (DataTable)acComboBox.DataSource;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Retorna o valor do displaymember.")]
        public string DisplayMember
        {
            get
            {
                return acComboBox.DisplayMember;
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o status do controle.")]
        public bool EnabledText
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
                if (!_enabled)
                {
                    if (TextReadOnly)
                    {
                        acTextBox.BackColor = Color.White;
                    }
                    else
                    {
                        acTextBox.BackColor = BackColorDisabled;
                    }
                }
                else
                {
                    acTextBox.BackColor = Color.White;
                }

                acTextBox.Enabled = _enabled;
                acComboBox.Enabled = _enabled;
                acTextBox.TabStop = _enabled;
                TabStop = _enabled;
            }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Font), "Arial, 9")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a fonte do controle.")]
        public Font Font_Control
        {
            get
            {
                return _font;
            }

            set
            {
                if (_font.Name.Equals(value.Name) && _font.Size.Equals(value.Size) && _font.Style.Equals(value.Style))
                {
                    return;
                }

                _font = value;
                acLabel.Tag = (object)acLabel.Width;
                acLabel.AutoSize = true;
                if (!_font.Bold)
                {
                    acLabel.Font = new Font(_font.Name, _font.Size, FontStyle.Bold);
                }
                else
                {
                    acLabel.Font = _font;
                }

                acComboBox.Font = _font;
                acTextBox.Font = _font;
                _xResize = true;
                if (Height != acLabel.Height + acTextBox.Height)
                {
                    Height = acLabel.Height + acTextBox.Height;
                }

                _xResize = false;
                MaxItensVisible = _maxItemVisible; //recupera mesma quantidade ao alterar fonte
                acLabel.AutoSize = false;
                acLabel.Width = int.Parse(acLabel.Tag.ToString());
            }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Color), "Black")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define a cor da fonte do controle.")]
        public Color Fore_Color
        {
            get
            {
                return _foreColor;
            }

            set
            {
                _foreColor = value;
                acLabel.ForeColor = _foreColor;
            }
        }

        [Browsable(true)]
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o formato do controle.")]
        public string Format
        {
            get
            {
                return _formatoCampo;
            }

            set
            {
                if (_formatoCampo != value)
                {
                    _formatoCampo = value;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Retorna o texto convertido para double")]
        public double GetNumberValue
        {
            get
            {
                if (Functions.IsNumeric(Texto))
                {
                    return Convert.ToDouble(Texto);
                }
                else
                {
                    return Convert.ToDouble("0");
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define se o titulo do controle será visivel")]
        public bool HideTitle
        {
            get
            {
                return _hideTitle;
            }

            set
            {
                if (_hideTitle == value)
                {
                    return;
                }

                _hideTitle = value;
                if (_xDesigner)
                {
                    Refresh_Control();
                }
                else
                {
                    _xResize = true;
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define se a combo terá multiplas linhas")]
        public bool MultiLine
        {
            get
            {
                return _multiline;
            }

            set
            {
                if (value != _multiline)
                {
                    acTextBox.AcceptsTab = value;
                    if (IsCombobox)
                    {
                        _multiline = false;
                    }
                    else
                    {
                        _multiline = value;
                    }

                    acTextBox.Multiline = _multiline;
                    if (_xDesigner)
                    {
                        Refresh_Control();
                    }
                    else
                    {
                        _xResize = true;
                    }
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Obtém ou define um valor que indica se o texto no controle TextBox deve aparecer como o caractere de senha padrão")]
        public bool PasswordChar
        {
            get
            {
                return _passwordChar;
            }

            set
            {
                _passwordChar = value;
                if (_passwordChar)
                {
                    acTextBox.UseSystemPasswordChar = true;
                    acTextBox.PasswordChar = '*';
                }
                else
                {
                    acTextBox.UseSystemPasswordChar = false;
                    acTextBox.PasswordChar = default;
                }
            }
        }

        [Browsable(false)]
        [DefaultValue(0)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("UserControl")]
        [Description("Define seleção do textbox")]
        public object SelectionStart
        {
            get
            {
                return acTextBox.SelectionStart;
            }

            set
            {
                acTextBox.SelectionStart = int.Parse(value.ToString());
            }
        }

        [Browsable(false)]
        [DefaultValue(-1)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Seleciona o index da combobox")]
        public int SelectIndex
        {
            get
            {
                return acComboBox.SelectedIndex;
            }

            set
            {
                // ---------------------EVITA ERRO AO SETAR INDEX QUE NÃO EXISTE NA LISTA
                if (value < acComboBox.Items.Count)
                {
                    acComboBox.SelectedIndex = value;
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(HorizontalAlignment.Left)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o alinhamento horizontal do controle.")]
        public HorizontalAlignment TextAlignment
        {
            get
            {
                return _alinha;
            }

            set
            {
                _alinha = value;
                acTextBox.TextAlign = _alinha;
            }
        }

        [Browsable(true)]
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        [Localizable(true)]
        [Category("UserControl")]
        [Description("Retorna o texto do campo.")]
        public string Texto
        {
            get
            {
                FieldFormat();
                return _xTexto;
            }

            set
            {
                _xTexto = Functions.NotNull(value);
                _xTexto = Exists(_xTexto); // Evita setar texto que não existe no formato (númerico e sem especial)
                FieldFormat();
                if (!acTextBox.Text.ToUpper().Equals(_xTexto.ToUpper()))
                {
                    acTextBox.Text = _xTexto;
                }
                else if (string.IsNullOrEmpty(acTextBox.Text))
                {
                    acComboBox.SelectedIndex = -1;
                }
                else
                {
                    acComboBox.SelectedIndex = acComboBox.FindString(acTextBox.Text);
                }

                // permite a pesquisa por nome (copiar/colar) e corrige o selectindex da combobox
                if (_pasting)
                {
                    acComboBox.Text = _xTexto;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Retorna o texto do campo no formato sql.")]
        public string TextSQL
        {
            get
            {
                switch (Field_TextType)
                {
                    case TipoDeCampo.Data:
                        {
                            if (!string.IsNullOrEmpty(Texto) && DateTime.TryParse(Texto, out _))
                            {
                                _textoSql = "'" + DateTime.Parse(Texto).ToString("yyyy/MM/dd") + "'";
                            }
                            else
                            {
                                _textoSql = "NULL";
                            }
                            break;
                        }

                    case TipoDeCampo.DataHora:
                        {
                            if (_xTexto.Split(' ').Count() == 2)
                            {
                                string dt = _xTexto.Split(' ')[0];
                                string hr = _xTexto.Split(' ')[1];
                                _textoSql = "'" + DateTime.Parse(dt).ToString("yyyy/MM/dd") + " " + hr + "'";
                            }
                            else
                            {
                                _textoSql = "NULL";
                            }

                            break;
                        }

                    case TipoDeCampo.Moeda:
                        {
                            _textoSql = NumSQL(Texto, Format.Split('.')[1].Length);
                            break;
                        }

                    case TipoDeCampo.Numerico:
                        {
                            _textoSql = Functions.NotNull(Texto, "NULL");
                            break;
                        }

                    default:
                        {
                            _textoSql = "'" + Texto + "'";
                            break;
                        }
                }

                return _textoSql;
            }
        }

        [DefaultValue(TipoDeCampo.Texto)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o tipo do campo.")]
        public TipoDeCampo Field_TextType
        {
            get
            {
                return _tipoCampo;
            }

            set
            {
                if (_tipoCampo == value)
                {
                    return;
                }

                _tipoCampo = value;
                switch (_tipoCampo)
                {
                    case TipoDeCampo.Data:
                        {
                            MaxLenght = 10;
                            IsCombobox = false;
                            EnabledMask = true;
                            TextAlignment = HorizontalAlignment.Center;
                            Format = "##/##/####";
                            break;
                        }

                    case TipoDeCampo.DataHora:
                        {
                            MaxLenght = 19;
                            IsCombobox = false;
                            EnabledMask = true;
                            TextAlignment = HorizontalAlignment.Center;
                            Format = "##/##/#### ##:##:##";
                            break;
                        }

                    case TipoDeCampo.Moeda:
                        {
                            MaxLenght = 15;
                            EnabledMask = false;
                            TextAlignment = HorizontalAlignment.Right;
                            Format = "###,###,###,##0.00";
                            break;
                        }

                    case TipoDeCampo.Numerico:
                        {
                            MaxLenght = 15;
                            if (!EnabledMask)
                            {
                                Format = "0123456789";
                            }

                            break;
                        }

                    case TipoDeCampo.SemEspecial:
                        {
                            EnabledMask = false;
                            Format = "ABCDEFGHIJKLMNOPQRSTWYUVXZ1234567890";
                            break;
                        }

                    case TipoDeCampo.Telefone:
                        {
                            MaxLenght = 14;
                            IsCombobox = false;
                            EnabledMask = true;
                            Format = "(##)#####-####";
                            break;
                        }

                    case TipoDeCampo.CPFCNPJ:
                        {
                            MaxLenght = 18;
                            IsCombobox = false;
                            EnabledMask = true;
                            Format = "##.###.###/####-##";
                            break;
                        }
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(255)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o tamanho maximo do campo.")]
        public int MaxLenght { get; set; } = 255;

        [Browsable(true)]
        [DefaultValue("Titulo")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o titulo do controle.")]
        public string Title
        {
            get
            {
                return _titulo;
            }

            set
            {
                _titulo = value;
                acLabel.Text = _titulo;
            }
        }

        [Browsable(true)]
        [DefaultValue(ContentAlignment.TopLeft)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o alinhamento horizontal do titulo do controle.")]
        public ContentAlignment TitleAlignment
        {
            get
            {
                return _alignTitle;
            }

            set
            {
                if (_alignTitle != value)
                {
                    _alignTitle = value;
                    acLabel.TextAlign = _alignTitle;
                }
            }
        }

        [Browsable(true)]
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("UserControl")]
        [Description("Define o tooltip do controle.")]
        public string Tooltip
        {
            get
            {
                return _toolTip;
            }

            set
            {
                _toolTip = value;
                acToolTip.SetToolTip(acTextBox, _toolTip);
                acToolTip.SetToolTip(acLabel, _toolTip);
            }
        }

        #endregion Propridades Ativas

        #region Metodos Publicos

        public void AddItem(string valor, int reg = -1, int orderBy = 1)
        {
            _load = true;
            if (_dataTable is null)
            {
                CriaDataTable();
            }

            if (reg == -1)
            {
                _dataTable.Columns[0].AutoIncrement = true;
                _dataTable.Columns[0].AutoIncrementStep = 1L;
            }

            _dataRow = _dataTable.NewRow();
            if (reg > -1)
            {
                _dataRow[0] = reg;
            }

            if (acTextBox.CharacterCasing == CharacterCasing.Upper)
            {
                valor = valor.ToUpper();
            }
            else if (acTextBox.CharacterCasing == CharacterCasing.Lower)
            {
                valor = valor.ToLower();
            }

            _dataRow[1] = valor;
            _dataTable.Rows.Add(_dataRow);
            if (string.IsNullOrEmpty(DisplayMember.Trim()))
            {
                acComboBox.DisplayMember = _dataTable.Columns[1].ColumnName;
            }

            if (string.IsNullOrEmpty(acComboBox.ValueMember.Trim()))
            {
                acComboBox.ValueMember = _dataTable.Columns[0].ColumnName;
            }

            if (orderBy > -1)
            {
                _dataTable.DefaultView.Sort = _dataTable.Columns[orderBy].ColumnName;
            }

            acComboBox.DataSource = _dataTable;
            acComboBox.SelectedIndex = -1;
            _load = false;
        }

        public void Clear()
        {
            if (_dataTable is object)
            {
                _dataTable.Clear();
                _dataTable = null;
            }

            acComboBox.DataSource = null;
            Codigo = -1;
            TextTag = "";
        }

        public void DataSource(DataTable dataTable = null, int orderBy = -1, string displayMember = "", string valueMember = "", string formatDisplayMenber = "")
        {
            _load = true;
            if (dataTable is null)
            {
                return;
            }

            acComboBox.DataSource = null;
            _dataTable = dataTable;
            if (!string.IsNullOrEmpty(displayMember.Trim()))
            {
                acComboBox.DisplayMember = displayMember;
            }
            else
            {
                acComboBox.DisplayMember = _dataTable.Columns[_dataTable.Columns.Count - 1].ColumnName;
            }

            if (!string.IsNullOrEmpty(valueMember.Trim()))
            {
                acComboBox.ValueMember = valueMember;
            }
            else
            {
                acComboBox.ValueMember = _dataTable.Columns[0].ColumnName;
            }

            if (orderBy > -1)
            {
                _dataTable.DefaultView.Sort = _dataTable.Columns[orderBy].ColumnName;
            }

            if (!string.IsNullOrEmpty(formatDisplayMenber.Trim()))
            {
                acComboBox.FormatString = formatDisplayMenber;
            }

            acComboBox.DataSource = _dataTable;
            acComboBox.SelectedIndex = -1;
            _load = false;
        }

        public string GetColumnText(int column)
        {
            DataRowView rowView = (DataRowView)acComboBox.SelectedItem;
            return Found() ? rowView.Row[column].ToString() : string.Empty;
        }

        public int GetIndex()
        {
            return int.TryParse(new Regex(@"\d+").Match(Name).Value, out int _index) ? _index : -1;
        }

        public void Refresh_Control()
        {
            if (IsCombobox && ShowButton)
            {
                acTextBox.IsComboBox = true;
                acTextBox.Dock = DockStyle.None;
                acTextBox.Width = acComboBox.Width - 16;
      
                acComboBox.Dock = DockStyle.Bottom;
                acComboBox.Visible = true;

                if (acTextBox.Height != acComboBox.Height)
                {
                    acTextBox.MinimumSize = new Size(0, acComboBox.Height);
                    acTextBox.Height = acComboBox.Height;
                }
            }
            else
            {
                acComboBox.Visible = false;                
                acTextBox.IsComboBox = false;
                acComboBox.Dock = DockStyle.None;
                acTextBox.Dock = DockStyle.Bottom;
                acComboBox.Width = acTextBox.Width;
                acComboBox.DropDownWidth = acTextBox.Width;
                
                if (_multiline)
                {
                    int alt = Height - (!_hideTitle ? acLabel.Height : 0);
                    if (alt > acComboBox.Height)
                    {
                        acTextBox.MinimumSize = new Size(0, alt);
                        acTextBox.Height = alt;
                    }
                    else if (acTextBox.Height != acComboBox.Height)
                    {
                        acTextBox.MinimumSize = new Size(0, acComboBox.Height);
                        acTextBox.Height = acComboBox.Height;
                    }
                }
                else if (acTextBox.Height != acComboBox.Height)
                {
                    acTextBox.MinimumSize = new Size(0, acComboBox.Height);
                    acTextBox.Height = acComboBox.Height;
                }
            }

            if (!_hideTitle)
            {
                if (!acLabel.Visible)
                {
                    acLabel.Visible = true;
                }

                acLabel.Width = Width + (BorderStyleFixed ? 0 : 3);
                if (Height != acLabel.Height + acComboBox.Height)
                {
                    Height = acLabel.Height + acComboBox.Height;
                }
            }
            else
            {
                acLabel.Width = 1;
                if (acLabel.Visible)
                {
                    acLabel.Visible = false;
                }

                if (Height != acTextBox.Height)
                {
                    Height = acTextBox.Height;
                }
            }

            if (IsCombobox && ShowButton)
            {
                if (acTextBox.Location != acComboBox.Location)
                {
                    acTextBox.Location = acComboBox.Location;
                }
            }
            else if (acComboBox.Location != acTextBox.Location)
            {
                acComboBox.Location = acTextBox.Location;
            }
        }
        public void SelectAllText()
        {
            acTextBox.SelectAll();
        }

        private string Exists(string _text)
        {
            if (!string.IsNullOrEmpty(Format) && !EnabledMask)
            {
                switch (Field_TextType)
                {
                    case TipoDeCampo.Numerico:
                    case TipoDeCampo.SemEspecial:
                        {
                            foreach (char c in _text)
                            {
                                if (!Format.Contains(c))
                                {
                                    _text = _text.Replace(c, char.Parse(""));
                                }
                            }

                            break;
                        }
                }
            }

            return _text;
        }

        /// <summary>' Retorna o index/código de acordo com texto e coluna específicado ''' </summary>
        public int FindStringExact(string texto, int col, bool index = false)
        {
            int FindStringExactRet = -1;
            if (string.IsNullOrEmpty(texto)) return FindStringExactRet;
            for (int i = 0, loopTo = _dataTable.Rows.Count - 1; i <= loopTo; i++)
            {
                if (_dataTable.Rows[i][col].Equals(texto))
                {
                    if (index)
                    {
                        return i;
                    }
                    else
                    {
                        return int.Parse(_dataTable.Rows[i][0].ToString());
                    }
                }
            }
            return FindStringExactRet;
        }

        /// <summary>' Retorna o primeiro index/código da lista que começa com o texto e coluna específicado ''' </summary>
        public int FindString(string texto, bool index = false)
        {
            int FindStringRet = -1;
            if (string.IsNullOrEmpty(texto))
                return FindStringRet;
            if (index)
            {
                return acComboBox.FindString(texto);
            }
            else
            {
                acComboBox.SelectedIndex = acComboBox.FindString(texto);
                return int.Parse(acComboBox.SelectedValue.ToString());
            }
        }

        #endregion Metodos Publicos

        #region Metodos Privados

        private void CriaDataTable(string tbl = "ItensCombo", string column1 = "Codigo", string column2 = "Texto", ArrayList columns = null)
        {
            _dataTable = new DataTable(tbl);
            _dataColumn = new DataColumn()
            {
                DataType = Type.GetType("System.Int32"),
                ColumnName = column1,
                AutoIncrement = true,
                AutoIncrementSeed = 1L,
                AutoIncrementStep = 1L
            };
            _dataTable.Columns.Add(_dataColumn);
            _dataColumn = new DataColumn()
            {
                DataType = Type.GetType("System.String"),
                ColumnName = column2
            };
            _dataTable.Columns.Add(_dataColumn);
            if (columns is object)
            {
                foreach (string column in columns)
                {
                    _dataColumn = new DataColumn()
                    {
                        DataType = Type.GetType("System.String"),
                        ColumnName = column
                    };
                    _dataTable.Columns.Add(_dataColumn);
                }
            }
        }

        private void FieldFormat()
        {
            if (PasswordChar)
            {
                return;
            }

            switch (Field_TextType)
            {
                case TipoDeCampo.Data:
                    {
                        _xTexto = _xTexto.Replace("/", "");
                        if (Functions.IsNumeric(_xTexto))
                        {
                            if (_xTexto.Length == 8)
                            {
                                _xTexto = double.Parse(_xTexto).ToString("##/##/####");
                            }
                            else if (_xTexto.Length == 6)
                            {
                                _xTexto = double.Parse(_xTexto).ToString("##/##/##");
                            }

                            if (DateTime.TryParse(_xTexto, out _))
                            {
                                _xTexto = DateTime.Parse(_xTexto).ToString("dd/MM/yyyy");
                            }
                        }

                        break;
                    }

                case TipoDeCampo.DataHora:
                    {
                        if (_xTexto.Split(' ').Count() == 2)
                        {
                            string dt = _xTexto.Split(' ')[0].Replace("/", "");
                            string hr = _xTexto.Split(' ')[1].Replace(":", "");
                            if (Functions.IsNumeric(dt))
                            {
                                if (dt.Length == 8)
                                {
                                    dt = double.Parse(dt).ToString("##/##/####");
                                }
                                else if (dt.Length == 6)
                                {
                                    dt = double.Parse(dt).ToString("##/##/##");
                                }

                                if (DateTime.TryParse(dt, out _))
                                {
                                    dt = DateTime.Parse(dt).ToString("dd/MM/yyyy");
                                }
                            }

                            if (Functions.IsNumeric(hr))
                            {
                                if (hr.Length == 6)
                                {
                                    hr = double.Parse(hr).ToString("##:##:##");
                                }
                                else if (hr.Length == 4)
                                {
                                    hr = double.Parse(hr).ToString("##:##");
                                }

                                if (DateTime.TryParse(hr, out _))
                                {
                                    hr = DateTime.Parse(hr).ToString("HH:mm:ss");
                                }
                            }

                            _xTexto = dt + " " + hr;
                        }

                        break;
                    }

                case TipoDeCampo.Moeda:
                    {
                        // evita erro de loop ao tentar setar informação que não existe na combobox
                        if (!Functions.IsNumeric(_xTexto))
                        {
                            if (IsCombobox && BlockText)
                            {
                                _xTexto = "";
                            }
                            else
                            {
                                _xTexto = double.Parse("0").ToString(Format);
                            }
                        }
                        else if (Functions.IsNumeric(_xTexto))
                        {
                            if (IsCombobox && BlockText && !Found())
                            {
                                return;
                            }

                            _xTexto = double.Parse(_xTexto).ToString(Format);
                        }

                        break;
                    }

                case TipoDeCampo.Numerico:
                    {
                        if (!string.IsNullOrEmpty(Format) && EnabledMask && Functions.IsNumeric(_xTexto))
                        {
                            _xTexto = int.Parse(_xTexto).ToString(Format);
                        }

                        break;
                    }

                case TipoDeCampo.Telefone:
                    {
                        _xTexto = _xTexto.Replace("(", "").Replace(")", "").Replace("-", "");

                        if (Functions.IsNumeric(_xTexto) && _xTexto.Length == 10)
                        {
                            Format = "(##)####-####";
                        }

                        _xTexto = FormatString(Format, _xTexto);
                        Format = "(##)#####-####";
                        break;
                    }

                case TipoDeCampo.CPFCNPJ:
                    {
                        _xTexto = _xTexto.Replace(".", "").Replace("/", "").Replace("-", "");

                        if (Functions.IsNumeric(_xTexto) && _xTexto.Length == 11)
                        {
                            Format = "###.###.###-##";
                        }

                        _xTexto = FormatString(Format, _xTexto);
                        Format = "##.###.###/####-##";
                        break;
                    }

                default:
                    {
                        if (!string.IsNullOrEmpty(Format) && EnabledMask)
                        {
                            _xTexto = FormatString(Format, _xTexto);
                        }

                        break;
                    }
            }

            if (!acTextBox.Text.Equals(_xTexto))
            {
                acTextBox.Text = _xTexto;
            }
        }

        private string FormatString(string mascara, string valor)
        {
            StringBuilder novoValor = new StringBuilder();
            string _rgx = @"[^\d]";
            if (Field_TextType == TipoDeCampo.Texto)
            {
                _rgx = "[^0-9A-Za-z]";
            }

            var rgx = new Regex(_rgx);
            valor = rgx.Replace(valor, "");
            int posicao = 0;
            int i = 0;
            while (mascara.Length > i)
            {
                if (mascara[i].ToString() == "#")
                {
                    if (valor.Length > posicao)
                    {
                        novoValor.Append(valor[posicao]);
                        posicao += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (valor.Length > posicao)
                {
                    novoValor.Append(mascara[i].ToString());
                }
                else
                {
                    break;
                }

                i += 1;
            }
            return novoValor.ToString();
        }

        private bool Found(bool preenche = false)
        {
            if (string.IsNullOrEmpty(acTextBox.Text) || _dataTable is null)
            {
                return false;
            }

            string consulta = _dataTable.Columns[DisplayMember].ColumnName;
            if (_dataTable.Columns[DisplayMember].DataType.FullName == "System.String")
            {
                consulta += " Like '" + acTextBox.Text.Trim() + "%'";
            }
            else
            {
                consulta += " = " + TextSQL;
            }

            var dr = _dataTable.Select(consulta);
            if (dr.Length > 0)
            {
                if (preenche)
                {
                    int xLen = acTextBox.TextLength;
                    acTextBox.Text = dr[0][DisplayMember].ToString();
                    acTextBox.Select(xLen, acTextBox.TextLength);
                }

                return true;
            }
            else
            {
                if (BlockText && !string.IsNullOrEmpty(acTextBox.Text))
                {
                    acTextBox.Text = "";
                }

                return false;
            }
        }

        private bool LiberaTeclas(short KeyAscii)
        {
            switch (KeyAscii)
            {
                case 8:
                case 11:
                case 13:
                case 127: // libera teclas {backspace, tab, enter, delete}
                    {
                        return false;
                    }

                default:
                    {
                        return true;
                    }
            }
        }

        private string NumSQL(string Valor, int xCasas = 2)
        {
            string NumSQLRet;
            if (!Functions.IsNumeric(Valor) || string.IsNullOrEmpty(Valor))
            {
                NumSQLRet = "0.00";
            }
            else
            {
                NumSQLRet = double.Parse(Valor).ToString("###,###,###,##0." + new string('0', xCasas));
                NumSQLRet = NumSQLRet.Replace(".", "");
                NumSQLRet = NumSQLRet.Replace(",", ".");
            }

            return NumSQLRet;
        }

        #endregion Metodos Privados

        #region acComboBox

        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            if (acComboBox.DropDownWidth != acTextBox.Width)
            {
                acComboBox.DropDownWidth = acTextBox.Width;
            }

            if (!acComboBox.Visible)
            {
                acComboBox.Visible = true;
            }

            if (double.Parse(Count) > 0d)
            {
                acComboBox.Focus();
                if (acComboBox.SelectedIndex == -1)
                {
                    SelectIndex = 0;
                }
            }

            acTextBox.BorderColor = Color.DodgerBlue;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            acTextBox.BorderColor = BorderColor;
            if (!acTextBox.Focus())
            {
                acComboBox.BorderColor = BorderColor;
            }

            if (_mouseClicked)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
            {
            _mouseClicked = false;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Escape)
            {
                if (acComboBox.SelectedIndex == 0 || double.Parse(Count) == 0d) // fecha lista
                {
                    if (acComboBox.DroppedDown)
                    {
                        acComboBox.DroppedDown = false;
                        acTextBox.Focus();
                    }

                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                DataRowView rowView = (DataRowView)acComboBox.SelectedItem;
                if (rowView != null && acTextBox.Text.ToUpper() != rowView[DisplayMember].ToString().ToUpper())
                {
                    acTextBox.Text = rowView[DisplayMember].ToString();
                }
            }
            else if (e.KeyCode == Keys.End)
            {
                SelectIndex = (int)Math.Round(double.Parse(Count) - 1d);
            }
            else if (e.KeyCode == Keys.Home)
            {
                SelectIndex = 0;
            }

            KeyDown_Combo?.Invoke(this, e);
        }

        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            _mouseClicked = true;
            if (e.KeyChar == (int)Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            else
            {
                e.Handled = true;
                int index = acComboBox.FindString(e.KeyChar.ToString());
                if (_textoAnt is null)
                {
                    _textoAnt = e.KeyChar.ToString();
                }

                if (index > -1)
                {
                    if (_textoAnt == e.KeyChar.ToString())
                    {
                        _indexAnt += 1;
                        index = acComboBox.FindString(e.KeyChar.ToString(), _indexAnt - 1);
                        if (index > -1)
                        {
                            acComboBox.SelectedIndex = index;
                        }
                    }
                    else
                    {
                        _textoAnt = e.KeyChar.ToString();
                        _indexAnt = index;
                        acComboBox.SelectedIndex = index;
                    }
                }
            }
        }

        private void ComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            _mouseClicked = true;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_load)
            {
                if (Codigo != -1)
                    Codigo = -1;
                return;
            }

            if (acComboBox.SelectedIndex == -1 || acComboBox.SelectedValue == null)
            {
                Codigo = -1;
                SelectedIndex?.Invoke(this, e);
                return;
            }

            _indexAnt = acComboBox.SelectedIndex;
            if (acComboBox.SelectedValue is null || !Functions.IsNumeric(acComboBox.SelectedValue.ToString()))
            {
                DataRowView rowView = (DataRowView)acComboBox.SelectedItem;
                acTextBox.Text = rowView[DisplayMember].ToString();
            }
            else if (!Codigo.Equals(acComboBox.SelectedValue))
            {
                Codigo = int.Parse(acComboBox.SelectedValue.ToString());
            }

            SelectedIndex?.Invoke(this, e);
        }

        #endregion acComboBox

        #region acContextMenuStrip

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (acTextBox.CanUndo)
            {
                acContextMenuStrip.Items["Undo"].Enabled = true;
            }
            else
            {
                acContextMenuStrip.Items["Undo"].Enabled = false;
            }

            if (acTextBox.SelectedText.Length == 0)
            {
                acContextMenuStrip.Items["Cut"].Enabled = false;
                acContextMenuStrip.Items["Copy"].Enabled = false;
                acContextMenuStrip.Items["Delete"].Enabled = false;
            }
            else
            {
                acContextMenuStrip.Items["Cut"].Enabled = true;
                acContextMenuStrip.Items["Copy"].Enabled = true;
                acContextMenuStrip.Items["Delete"].Enabled = true;
            }

            if (Clipboard.ContainsText())
            {
                acContextMenuStrip.Items["Paste"].Enabled = true;
            }
            else
            {
                acContextMenuStrip.Items["Paste"].Enabled = false;
            }

            if (acTextBox.TextLength == 0)
            {
                acContextMenuStrip.Items["SelectAll"].Enabled = false;
            }
            else if (acTextBox.SelectionLength == acTextBox.TextLength)
            {
                acContextMenuStrip.Items["SelectAll"].Enabled = false;
            }
            else
            {
                acContextMenuStrip.Items["SelectAll"].Enabled = true;
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            acTextBox.Copy();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            int SelectionIndex = acTextBox.SelectionStart;
            int SelectionCount = acTextBox.SelectionLength;
            acTextBox.Text = acTextBox.Text.Remove(SelectionIndex, SelectionCount);
            acTextBox.SelectionStart = SelectionIndex;
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            _pasting = true;
            acTextBox.Paste();
            _pasting = false;
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            acTextBox.SelectAll();
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            acTextBox.Undo();
        }

        #endregion acContextMenuStrip

        #region acTextBox

        private void TextBox_Enter(object sender, EventArgs e)
        {
            acComboBox.BorderColor = BorderColorFocused;
            if (!EnabledText)
            {
                if (!TextReadOnly)
                {
                    acTextBox.BackColor = BackColorDisabled;
                }
            }
            else
            {
                acTextBox.BackColor = BackColorFocused;
            }

            if (string.IsNullOrEmpty(acTextBox.Text) || PasswordChar)
            {
                return;
            }

            _gotFocus = true;
            switch (Field_TextType)
            {
                case TipoDeCampo.Data:
                    {
                        acTextBox.Text = acTextBox.Text.Replace("/", "").Trim();
                        break;
                    }

                case TipoDeCampo.Moeda:
                    {
                        acTextBox.Text = acTextBox.Text.Replace(".", "").Trim();
                        break;
                    }

                case TipoDeCampo.DataHora: //não remove mascara
                    {
                        break;
                    }

                default:
                    {
                        if (!string.IsNullOrEmpty(Format) && EnabledMask) //remove mascara
                        {
                            string _rgx = @"[^\d]";
                            if (Field_TextType == TipoDeCampo.Texto)
                            {
                                _rgx = "[^0-9A-Za-z]";
                            }

                            var rgx = new Regex(_rgx);
                            acTextBox.Text = rgx.Replace(acTextBox.Text, "").Trim();
                        }
                        else
                        {
                            acTextBox.Text = acTextBox.Text.Trim();
                        }

                        break;
                    }
            }

            GotFocus?.Invoke(this, e);
        }

        private void TextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            PreviewKeyDown?.Invoke(sender, e);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
            _mouseClicked = false;
            _pasting = false;
            _isControl = false;
            if (e.Control && (int)e.KeyCode == 86)
            {
                _pasting = true;
                return;
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Escape)
            {
                if (double.Parse(Count) == 0d && acComboBox.DroppedDown)  // fecha lista
                {
                    acComboBox.DroppedDown = false;
                }
            }
            else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                _isControl = true;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
            short KeyAscii = (short)(e.KeyChar);
            _mouseClicked = KeyAscii == 40;
            if (char.IsControl(e.KeyChar))
            {
                if (KeyAscii == 13 && !MultiLine) // avança
                {
                    SendKeys.Send("{TAB}");
                    e.Handled = true;
                }

                return;
            }

            // campo bloqueado para digitação e lista em branca
            if (IsCombobox && BlockText && double.Parse(Count) == 0)
            {
                e.Handled = true;
                return;
            }

            // valida digitação
            switch (Field_TextType)
            {
                case TipoDeCampo.Data:
                case TipoDeCampo.Telefone:
                case TipoDeCampo.CPFCNPJ:
                    {
                        if (!char.IsNumber(e.KeyChar)) // senão for número testa teclas padrão
                        {
                            e.Handled = LiberaTeclas(KeyAscii);
                        }
                        else if (Functions.IsNumeric(acTextBox.Text))
                        {
                            int len = 0;
                            if (Field_TextType == TipoDeCampo.Data)
                            {
                                len = 8;
                            }
                            else if (Field_TextType == TipoDeCampo.Telefone)
                            {
                                len = 11;
                            }
                            else if (Field_TextType == TipoDeCampo.CPFCNPJ)
                            {
                                len = 14;
                            }

                            // bloqueia digitar quantidade de caracteres maior que maxlenght
                            if (acTextBox.TextLength >= len && acTextBox.SelectionLength == 0)
                            {
                                e.Handled = true;
                            }
                        }

                        break;
                    }

                case TipoDeCampo.Numerico:
                    {
                        if (EnabledMask)
                        {
                            e.Handled = false; // evita bloqueio de digitação quando estiver usando mascara
                        }
                        else if (Format.IndexOf(KeyAscii.ToString().ToUpper()) == 0) // libera somente números específicados
                        {
                            e.Handled = LiberaTeclas(KeyAscii);
                        }

                        break;
                    }

                case TipoDeCampo.Moeda:
                    {
                        if (!char.IsNumber(e.KeyChar))
                        {
                            if (e.KeyChar.ToString() == ",") // bloqueia mais de 1 virgula
                            {
                                if (_xTexto.Contains(",") && acTextBox.SelectedText.Length != acTextBox.TextLength)
                                {
                                    e.Handled = true;
                                }
                            }
                            else
                            {
                                e.Handled = LiberaTeclas(KeyAscii);
                            }
                        }

                        break;
                    }

                case TipoDeCampo.SemEspecial:
                    {
                        if (Format.IndexOf(KeyAscii.ToString().ToUpper()) == 0)
                        {
                            e.Handled = LiberaTeclas(KeyAscii);
                        }

                        break;
                    }

                case TipoDeCampo.Texto:
                    {
                        if (!string.IsNullOrEmpty(Format) && !EnabledMask && Format.IndexOf(e.KeyChar) == -1)
                        {
                            e.Handled = LiberaTeclas(KeyAscii);
                        }

                        break;
                    }
            }

            // impede aspa dupla e aspa simples e digitar espaço para password
            if (KeyAscii == 34 || PasswordChar && KeyAscii == 32) // OrElse KeyAscii = 39
            {
                e.Handled = true;
                return;
            }

            if (acTextBox.TextLength >= MaxLenght && acTextBox.SelectionLength == 0) // bloqueia digitar quantidade de caracteres maior que maxlenght
            {
                e.Handled = true;
            }

            if (!IsCombobox)
                return;

            // ================= autocomplete

            // guarda texto digitado + valor pressionado
            string Search = (acTextBox.Text.Substring(0, acTextBox.Text.Length - acTextBox.SelectionLength) + e.KeyChar).ToUpper();
            _position = Search.Length;
            int index = acComboBox.FindString(Search);
            if (index > -1)
            {
                acTextBox.Text = acComboBox.Items[index].ToString();
                acTextBox.Select(_position, acTextBox.Text.Length);
                if (Functions.IsNumeric(acComboBox.Items[index].ToString()))
                {
                    Codigo = int.Parse(acComboBox.Items[index].ToString());
                }

                acComboBox.SelectedIndex = index; // impede erro quando a combobox não possui codigo
                e.KeyChar = char.MinValue; // impede que texto seja duplicado
            }
            else // não achou
            {
                if (BlockText) // campo bloqueado
                {
                    e.Handled = LiberaTeclas(KeyAscii); // impede digitação
                }
                else
                {
                    e.Handled = false;
                }

                Codigo = -1;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsCombobox && e.KeyCode == Keys.Down)
            {
                if (!acComboBox.DroppedDown)
                {
                    acComboBox.DropDownWidth = acTextBox.Width;
                    acComboBox.DroppedDown = true; // abre lista
                }

                _mouseClicked = true;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            acComboBox.BorderColor = BorderColor;
            if (IsCombobox)
            {
                if (acComboBox.DroppedDown)
                {
                    acComboBox.DroppedDown = false;
                }

                if (BlockText && !Found())
                {
                    acTextBox.Text = "";
                }
                else
                {
                    Found(true);
                }

                if (string.IsNullOrEmpty(acTextBox.Text))
                {
                    SelectIndex = -1;
                }
            }
            // evita campo ficar com cor destacada ao perder o foco
            else if (EnabledText)
            {
                acTextBox.BackColor = Color.White;
            }
            else if (!TextReadOnly)
            {
                acTextBox.BackColor = BackColorDisabled;
            }

            // remove espaços somente se existir (evita entrar no evento sem necessidade)
            if (_xTexto.StartsWith(" ") || _xTexto.EndsWith(" "))
            {
                acTextBox.Text = _xTexto.ToString();
            }

            if (IsCombobox && !BlockText)
            {
                FieldFormat();  // Alterado para sempre formatar dados mesmo que seja combobox
            }
            else if (!IsCombobox)
            {
                FieldFormat();
            }

            LostFocus?.Invoke(this, e);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (IsCombobox)
            {
                if (BlockText && !Found() && !_isControl)
                {
                    if (!string.IsNullOrEmpty(acTextBox.Text))
                    {
                        acComboBox.SelectedIndex = -1;
                        acTextBox.Text = "";
                        return;
                    }
                }
                else if (_pasting)
                {
                    Found(true);
                }
            }

            switch (Field_TextType)
            {
                case TipoDeCampo.Numerico:
                case TipoDeCampo.CPFCNPJ:
                    {
                        // não formata o campo ao digitar pois irá formatar ao perder o foco
                        acTextBox.SelectionStart = acTextBox.TextLength;
                        break;
                    }

                default:
                    {
                        if (!(_gotFocus && _isControl))
                        {
                            if (IsCombobox && Field_TextType == TipoDeCampo.Moeda) // corrige posição do cursor do mouse
                            {
                                acTextBox.Select(_position, acTextBox.TextLength);
                            }
                            else if (IsCombobox)
                            {
                                acTextBox.SelectionStart = acTextBox.TextLength;
                            }
                            else if (!string.IsNullOrEmpty(Format) && EnabledMask)
                            {
                                int xPosicao = acTextBox.SelectionStart;
                                // evita erro de visualização da mascara na edição do campo
                                bool xForm = string.IsNullOrEmpty(acTextBox.Text.Substring(xPosicao));
                                if (xForm)
                                {
                                    string tmp = FormatString(Format, acTextBox.Text);
                                    if (!acTextBox.Text.Equals(tmp))
                                    {
                                        acTextBox.Text = tmp;
                                        acTextBox.SelectionStart = acTextBox.TextLength;
                                    }
                                }
                            }
                        }

                        break;
                    }
            }

            if (!_xTexto.Equals(acTextBox.Text))
            {
                _xTexto = acTextBox.Text;
            }

            // Correção combobox liberada com formato moeda
            if (IsCombobox && Field_TextType == TipoDeCampo.Moeda && !_isControl && !_gotFocus)
            {
                FieldFormat();
            }

            if (!acComboBox.Text.ToUpper().Equals(acTextBox.Text.ToUpper()))
            {
                acComboBox.Text = acTextBox.Text;
            }

            if (_pasting) // Evita colar caracter inválido
            {
                string oldText = acTextBox.Text;
                string newText = acTextBox.Text;
                if (!string.IsNullOrEmpty(Format) && !EnabledMask)
                {
                    for (int i = 0, loopTo = oldText.Length - 1; i <= loopTo; i++)
                    {
                        string str = oldText.Substring(i, 1);
                        if (str.Equals("#") || !Format.Contains(str))
                        {
                            newText = newText.Replace(str, "");
                        }
                    }

                    if (!acTextBox.Text.Equals(newText))
                    {
                        acTextBox.Text = newText.Substring(0, MaxLenght);
                        acTextBox.SelectionStart = acTextBox.TextLength;
                    }
                }
            }

            if (acTextBox.TextLength > MaxLenght && !acTextBox.Text.Equals(acTextBox.Text.Substring(0, MaxLenght)))
            {
                acTextBox.Text = acTextBox.Text.Substring(0, MaxLenght);
                acTextBox.SelectionStart = acTextBox.TextLength;
            }

            if (string.IsNullOrEmpty(acTextBox.Text))
            {
                acComboBox.SelectedIndex = -1;
                // Else 'AO APAGAR CARACTERES ESTAVA PREENCHENDO NOVAMENTE **TALVEZ NECESSITE VERIFICAR _ISCONTROL
                // acComboBox.SelectedIndex = acComboBox.FindString(acTextBox.Text)
            }

            _gotFocus = false;
            TextChanged?.Invoke(this, e);
        }

        private void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            acToolTip.BackColor = Color.White;
            acToolTip.ForeColor = Color.Black;
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
        }

        #endregion acTextBox
    }
}