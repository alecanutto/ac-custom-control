using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AC_Control
{
    public partial class AcGroupBox : Panel
    {
        #region Private Fields

        private bool disabled = false;
        private string title = "GroupName";
        private readonly List<Control> listControl = new List<Control>();

        #endregion Private Fields

        #region Public Constructors

        public AcGroupBox()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Disabled Properties

        [Browsable(false)]
        public override bool AllowDrop { get; set; }
        [Browsable(false)]
        public override AnchorStyles Anchor { get; set; }
        [Browsable(false)]
        public override bool AutoScroll { get; set; }
        [Browsable(false)]
        public override bool AutoSize { get; set; }
        [Browsable(false)]
        public override Color BackColor { get; set; }
        [Browsable(false)]
        public override Image BackgroundImage { get; set; }
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout { get; set; }
        [Browsable(false)]
        public override ContextMenuStrip ContextMenuStrip { get; set; }
        [Browsable(false)]
        public override Cursor Cursor { get; set; }
        [Browsable(false)]
        public override DockStyle Dock { get; set; }
        [Browsable(false)]
        public override Size MaximumSize { get; set; }
        [Browsable(false)]
        public override Size MinimumSize { get; set; }
        [Browsable(false)]
        public override RightToLeft RightToLeft { get; set; }

        #endregion

        #region Public Properties

        [Category("AC Control")]
        [Browsable(false)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DisabledControls
        {
            get => disabled;
            set
            {
                disabled = value;
                foreach (Control c in Controls)
                {
                    c.Enabled = !disabled;
                }
                panel1.Enabled = true;
            }
        }


        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue("GroupName")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Title
        {
            get => title;
            set
            {
                title = value;
                label1.Text = title;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public List<T> GetAllControls<T>() where T : Control
        {           
            return Controls.OfType<T>().ToList();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnResize(EventArgs eventargs)
        {
            RefreshControl();
        }

        #endregion Protected Methods

        #region Private Methods

        private void ControlAdd(object sender, ControlEventArgs e)
        {
            listControl.Add(e.Control);
            RefreshControl();
        }

        private void ControlRemove(object sender, ControlEventArgs e)
        {
            listControl.Remove(e.Control);
        }

        private void RefreshControl()
        {
            panel1.SendToBack();
            panel2.SendToBack();
            label1.BringToFront();
            foreach (var item in listControl)
            {
                item.BringToFront();
            }
        }

        #endregion Private Methods

    }
}