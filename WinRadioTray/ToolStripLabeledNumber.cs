using System;
using System.Drawing;
using System.Windows.Forms;


namespace WinRadioTray.Controls
{
    [System.ComponentModel.DesignerCategory("")]
    internal class ToolStripLabeledNumber : ToolStripControlHost
    {
        public Label Label;
        public Label Label2;
        public NumericUpDown NumericUpDown;

        public ToolStripLabeledNumber() : base(new Panel())
        {
            Panel panel = (Panel)this.Control;

            Label = new Label();

            NumericUpDown = new NumericUpDown();
            NumericUpDown.Left = Label.Right;
            NumericUpDown.Width = 50;
            NumericUpDown.Maximum = decimal.MaxValue;

            Label2 = new Label();
            Label2.Text = "Minutes";
            Label2.Left = NumericUpDown.Right;

            panel.Controls.Add(Label);
            panel.Controls.Add(NumericUpDown);
            panel.Controls.Add(Label2);
        }
    }
}

