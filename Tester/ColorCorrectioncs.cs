using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tester
{
    public partial class ColorCorrectioncs : Form
    {
        public Button Rup = new Button();
        public Button Rdown = new Button();
        public Button Gup = new Button();
        public Button Gdown = new Button();
        public Button Bup = new Button();
        public Button Bdown = new Button();
        public ColorCorrectioncs()
        {
            InitializeComponent();
            resize();
            Rup.Text = "+";
            Rup.Font = new Font("Ariel", (int)(Width*.1));
            Rdown.Text = "-";
            Rdown.Font = new Font("Ariel", (int)(Width * .1));
            Gup.Text = "+";
            Gup.Font = new Font("Ariel", (int)(Width * .1));
            Gdown.Text = "-";
            Gdown.Font = new Font("Ariel", (int)(Width * .1));
            Bup.Text = "+";
            Bup.Font = new Font("Ariel", (int)(Width * .1));
            Bdown.Text = "-";
            Bdown.Font = new Font("Ariel", (int)(Width * .1));
            this.Controls.Add(Rup);
            this.Controls.Add(Gup);
            this.Controls.Add(Bup);
            this.Controls.Add(Rdown);
            this.Controls.Add(Gdown);
            this.Controls.Add(Bdown);
        }

        private void ColorCorrectioncs_MouseHover(object sender, EventArgs e)
        {
            this.Opacity = 100;
        }

        private void ColorCorrectioncs_MouseLeave(object sender, EventArgs e)
        {
            this.Opacity = 75;
        }

        private void resize()
        {
            Rup.Location = new Point((int)(10), (int)(10));
            Rup.Width = (int)(Width * .45);
            Rup.Height = (int)(Height * .30);
            Gup.Location = new Point((int)10, (int)(Height * .30));
            Gup.Width = (int)(Width * .45);
            Gup.Height = (int)(Height * .30);
            Bup.Location = new Point((int)10, (int)(Height * .60));
            Bup.Width = (int)(Width * .45);
            Bup.Height = (int)(Height * .30);
            Rdown.Location = new Point((int)(Width * .45), (int)(10));
            Rdown.Width = (int)(Width * .45);
            Rdown.Height = (int)(Height * .30);
            Gdown.Location = new Point((int)(Width * .45), (int)(Height * .30));
            Gdown.Width = (int)(Width * .45);
            Gdown.Height = (int)(Height * .30); Rdown.Location = new Point((int)(0), (int)(10));
            Bdown.Location = new Point((int)(Width * .45), (int)(Height * .60));
            Bdown.Width = (int)(Width * .45);
            Bdown.Height = (int)(Height * .30);
        }

        private void Tresholdcs_Resize(object sender, EventArgs e)
        {
            resize();
        }
    }
}
