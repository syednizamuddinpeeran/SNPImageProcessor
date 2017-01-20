namespace Tester
{
    partial class Treshold
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Treshold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 62);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Treshold";
            this.Opacity = 0.75D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Treshold";
            this.TopMost = true;
            this.MouseLeave += new System.EventHandler(this.Tresholdcs_MouseLeave);
            this.MouseHover += new System.EventHandler(this.Tresholdcs_MouseHover);
            this.Resize += new System.EventHandler(this.Tresholdcs_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}