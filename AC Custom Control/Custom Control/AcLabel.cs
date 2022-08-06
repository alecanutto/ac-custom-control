using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AC_Control
{
    public partial class AcLabel : Label
    {
        #region Private Fields

        private Color _borderColor = Color.DimGray;

        #endregion Private Fields

        #region Public Constructors

        public AcLabel()
        {
            Font = new Font("Arial", 9f, FontStyle.Bold);
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Properties

        [Category("AC Control")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "DimGray")]
        [RefreshProperties(RefreshProperties.Repaint)]
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

        #endregion Public Properties

        #region Protected Methods

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == NativeMethods.WM_NCPAINT && Convert.ToBoolean(BorderStyle.FixedSingle) && Parent is object)
            {
                m.Msg = 15;
                NCPaint();
                m.WParam = GetHRegion();
                base.DefWndProc(ref m);
                NativeMethods.DeleteObject(m.WParam);
                m.Result = IntPtr.Zero;
            }
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
            if ((Parent is null) || (Width <= 0 || Height <= 0) || (BorderStyle != BorderStyle.FixedSingle)) return;

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
                    using (var borderPen = new Pen(BackColor))
                    {
                        g.DrawRectangle(borderPen, borderRect);
                        borderRect.Inflate(-1, -1);
                        g.DrawRectangle(SystemPens.Window, borderRect);
                    }

                    switch (BackColor)
                    {
                        case var @case when @case == Color.Transparent:
                            {
                                ControlPaint.DrawBorder(g, new Rectangle(0, 0, Size.Width, Size.Height), BorderColor, 1, ButtonBorderStyle.Solid, BorderColor, 1, ButtonBorderStyle.Solid, BorderColor, 1, ButtonBorderStyle.Solid, BorderColor, 0, ButtonBorderStyle.Solid);
                                break;
                            }

                        default:
                            {
                                ControlPaint.DrawBorder(g, new Rectangle(0, 0, Size.Width, Size.Height + 4), BorderColor, 1, ButtonBorderStyle.Solid, BorderColor, 1, ButtonBorderStyle.Solid, BorderColor, 1, ButtonBorderStyle.Solid, BackColor, 1, ButtonBorderStyle.Solid);
                                break;
                            }
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
