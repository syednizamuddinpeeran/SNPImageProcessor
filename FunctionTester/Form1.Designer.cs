namespace FunctionTester
{
    partial class Form1
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
            this.Input = new System.Windows.Forms.PictureBox();
            this.Output = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Output)).BeginInit();
            this.SuspendLayout();
            // 
            // Input
            // 
            this.Input.Location = new System.Drawing.Point(23, 12);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(434, 463);
            this.Input.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Input.TabIndex = 0;
            this.Input.TabStop = false;
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(463, 12);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(434, 463);
            this.Output.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Output.TabIndex = 1;
            this.Output.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 487);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Input);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Output)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Input;
        private System.Windows.Forms.PictureBox Output;
    }
}

