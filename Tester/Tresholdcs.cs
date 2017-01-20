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
    public partial class Treshold : Form
    {
        public TrackBar treshTrack=new TrackBar();
        public Treshold()
        {
            InitializeComponent();
            resize();
            this.Controls.Add(treshTrack);
        }

        private void Tresholdcs_MouseHover(object sender, EventArgs e)
        {
            this.Opacity=100;
        }

        private void Tresholdcs_MouseLeave(object sender, EventArgs e)
        {
            this.Opacity = 75;
        }
        private void resize() 
        {
            treshTrack.Maximum = 255;
            treshTrack.Minimum = 0;
            treshTrack.SmallChange = 5;
            treshTrack.Location = new Point((int)(0), (int)(0));
            treshTrack.Width = (int)(Width * .80);
            treshTrack.Height = (int)(Height * .80);
        }

        private void Tresholdcs_Resize(object sender, EventArgs e)
        {
            resize();
        }
    }
}
