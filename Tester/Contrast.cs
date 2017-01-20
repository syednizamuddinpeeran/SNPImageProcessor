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
    public partial class contrast : Form
    {
        public Button up = new Button();
        public Button down = new Button();
        public contrast()
        {
            InitializeComponent();
            resize();
            this.Controls.Add(up);
            this.Controls.Add(down);
            up.Text = "+";
            up.Font = new Font("Ariel", (int)(Width * .1));
            down.Text = "-";
            down.Font = new Font("Ariel", (int)(Width * .1));
        }
        private void Brightness_MouseHover(object sender, EventArgs e)
        {
            this.Opacity = 100;
        }

        private void Brightness_MouseLeave(object sender, EventArgs e)
        {
            this.Opacity = 75;
        }
        private void resize()
        {
            up.Location = new Point((int)(10), (int)(10));
            up.Width = (int)(Width * .45);
            up.Height = (int)(Height * .45);
            down.Location = new Point((int)((Width * .45)), (int)(10));
            down.Width = (int)(Width * .45);
            down.Height = (int)(Height * .45);
        }

        private void Brightness_Resize(object sender, EventArgs e)
        {
            resize();
        }
    }
}
