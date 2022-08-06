using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AC_Control
{
    public partial class AcButton : Button
    {
        #region Private Fields

        private string _toolTip;
        private readonly ToolTip toolTip = new ToolTip();

        #endregion Private Fields

        #region Public Constructors

        public AcButton()
        {
            BackColor = Color.White;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderColor = Color.DimGray;
            Font = new Font("Arial", 9f, FontStyle.Bold);
            FlatAppearance.BorderSize = 1;
            FlatAppearance.MouseDownBackColor = Color.WhiteSmoke;
            FlatAppearance.MouseOverBackColor = BackColor;
            UseVisualStyleBackColor = false;
            Size = new Size(85, 25);
        }

        #endregion Public Constructors

        #region Public Properties

        [Category("AC Control")]
        [Browsable(true)]
        public string ToolTip
        {
            get => _toolTip;
            set
            {
                _toolTip = value;
                toolTip.SetToolTip(this, _toolTip);
            }
        }

        #endregion Public Properties

    }
}
