using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SNPImageProcessor;

namespace FunctionTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Bitmap input = null;
            Bitmap output = null;
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                input= new Bitmap(openFile.FileName); ;
                output= IPTools.LinearHistogramScaling(IPTools.HistogramStretch(input),-10,260); 
            }
            InitializeComponent();
            Input.Image = input;
            Output.Image = output;
        }
        public Form1(Bitmap input)
        {
            Bitmap output = null;
            InitializeComponent();
            output = IPTools.LinearHistogramScaling(IPTools.HistogramStretch(input), -10, 260);
            Input.Image = input;
            Output.Image = output;
        }
    }
}
