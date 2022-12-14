using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AC_Control
{
    public partial class AcTextBox : TextBox
    {
        #region Private Fields

        private int _border = 1;
        private bool _isComboBox = false;
        private Color _borderColor = Color.DimGray;
        private Color _borderColorFocused = Color.DodgerBlue;

        #endregion Private Fields

        #region Public Constructors

        public AcTextBox()
        {
            Height = 23;
            MinimumSize = new Size(0, 23);
            Font = new Font("Arial", 9f);
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Properties

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(1)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Border
        {
            get => _border;
            set
            {
                if (_border != value)
                {
                    _border = value;
                    NativeMethods.SendMessage(Handle, (int)NativeMethods.WM_NCPAINT, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "DimGray")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    NativeMethods.SendMessage(Handle, (int)NativeMethods.WM_NCPAINT, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColorFocused
        {
            get =>_borderColorFocused; 
            set
            {
                if (_borderColorFocused != value)
                {
                    _borderColorFocused = value;
                }
            }
        }

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsComboBox
        {
            get => _isComboBox;
            set
            {
                if (_isComboBox != value)
                {
                    _isComboBox = value;
                }
            }
        }

        #endregion Public Properties

        #region Protected Methods

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_NCPAINT && BorderStyle == BorderStyle.Fixed3D && Parent is object)
            {
                NCPaint();
                m.WParam = GetHRegion();
                base.DefWndProc(ref m);
                NativeMethods.DeleteObject(m.WParam);
                m.Result = IntPtr.Zero;
            }

            base.WndProc(ref m);
        }

        #endregion Protected Methods

        #region Private Methods

        private IntPtr GetHRegion()
        {
            IntPtr hRgn;
            var winRect = Parent.RectangleToScreen(Bounds);
            var clientRect = RectangleToScreen(ClientRectangle);
            var updateRegion = new Region(winRect);
            updateRegion.Complement(clientRect);
            using (var g = CreateGraphics())
            {
                hRgn = updateRegion.GetHrgn(g);
            }

            updateRegion.Dispose();
            return hRgn;
        }

        private void NCPaint()
        {
            if ((Parent is null) || (Width <= 0 || Height <= 0) || (BorderStyle != BorderStyle.Fixed3D)) return;

            var windowDC = NativeMethods.GetDCEx(Handle, IntPtr.Zero, (int)(NativeMethods.DCX_CACHE | NativeMethods.RDW_INVALIDATE | NativeMethods.DCX_CLIPSIBLINGS | NativeMethods.RDW_FRAME));
            if (windowDC.Equals(IntPtr.Zero))
            {
                return;
            }

            using (var bm = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
            {
                using (var g = Graphics.FromImage(bm))
                {
                    var borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
                    using (var borderPen = new Pen(Color.White))
                    {
                        g.DrawRectangle(borderPen, borderRect);
                        borderRect.Inflate(-1, -1);
                        g.DrawRectangle(SystemPens.Window, borderRect);
                    }

                    if (Focused)
                    {
                        ControlPaint.DrawBorder(g, new Rectangle(0, 0, Size.Width, Size.Height), BorderColorFocused, Border, ButtonBorderStyle.Solid, BorderColorFocused, Border, ButtonBorderStyle.Solid, BorderColorFocused, Convert.ToInt32(IsComboBox ? 0 : Border), ButtonBorderStyle.Solid, BorderColorFocused, Border, ButtonBorderStyle.Solid);
                    }
                    else
                    {
                        ControlPaint.DrawBorder(g, new Rectangle(0, 0, Size.Width, Size.Height), BorderColor, Border, ButtonBorderStyle.Solid, BorderColor, Border, ButtonBorderStyle.Solid, BorderColor, Convert.ToInt32(IsComboBox ? 0 : Border), ButtonBorderStyle.Solid, BorderColor, Border, ButtonBorderStyle.Solid);
                    }

                    using (var Rgn = new Region(new Rectangle(0, 0, Width, Height)))
                    {
                        Rgn.Exclude(new Rectangle(2, 2, Width - 4, Height - 4));
                        var hRgn = Rgn.GetHrgn(g);
                        if (!hRgn.Equals(IntPtr.Zero))
                        {
                            NativeMethods.SelectClipRgn(windowDC, hRgn);
                        }

                        var bmDC = g.GetHdc();
                        var hBmp = bm.GetHbitmap();
                        var oldDC = NativeMethods.SelectObject(bmDC, hBmp);
                        NativeMethods.BitBlt(windowDC, 0, 0, bm.Width, bm.Height, bmDC, 0, 0, (int)NativeMethods.SRCCOPY);
                        NativeMethods.SelectClipRgn(windowDC, IntPtr.Zero);
                        NativeMethods.DeleteObject(hRgn);
                        g.ReleaseHdc(bmDC);
                        NativeMethods.SelectObject(oldDC, hBmp);
                        NativeMethods.DeleteObject(hBmp);
                    }
                }
            }

            NativeMethods.ReleaseDC(Handle, windowDC);
        }

        #endregion Private Methods
    }
}