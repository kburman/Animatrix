namespace Tester.Demo
{
    partial class AnimatedImageDisplay
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
            this.animator1 = new Animatrix.Animator();
            this.SuspendLayout();
            // 
            // animator1
            // 
            this.animator1.FPS = 100;
            // 
            // AnimatedImageDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 258);
            this.Name = "AnimatedImageDisplay";
            this.Text = "AnimatedImageDisplay";
            this.ResumeLayout(false);

        }

        #endregion

        private Animatrix.Animator animator1;
    }
}