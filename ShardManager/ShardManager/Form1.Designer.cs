namespace ShardManager
{
    partial class ShardMapManagerForm
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
            this.setupButton = new System.Windows.Forms.Button();
            this.outputText = new System.Windows.Forms.RichTextBox();
            this.loginText = new System.Windows.Forms.TextBox();
            this.passwordText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // setupButton
            // 
            this.setupButton.Location = new System.Drawing.Point(297, 15);
            this.setupButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(96, 26);
            this.setupButton.TabIndex = 0;
            this.setupButton.Text = "Setup sharding";
            this.setupButton.UseVisualStyleBackColor = true;
            this.setupButton.Click += new System.EventHandler(this.setupButton_Click);
            // 
            // outputText
            // 
            this.outputText.Location = new System.Drawing.Point(16, 48);
            this.outputText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.outputText.Name = "outputText";
            this.outputText.Size = new System.Drawing.Size(623, 387);
            this.outputText.TabIndex = 2;
            this.outputText.Text = "";
            // 
            // loginText
            // 
            this.loginText.Location = new System.Drawing.Point(16, 15);
            this.loginText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loginText.Name = "loginText";
            this.loginText.Size = new System.Drawing.Size(132, 22);
            this.loginText.TabIndex = 3;
            // 
            // passwordText
            // 
            this.passwordText.Location = new System.Drawing.Point(157, 15);
            this.passwordText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.passwordText.Name = "passwordText";
            this.passwordText.PasswordChar = '*';
            this.passwordText.Size = new System.Drawing.Size(132, 22);
            this.passwordText.TabIndex = 4;
            this.passwordText.UseSystemPasswordChar = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 450);
            this.Controls.Add(this.passwordText);
            this.Controls.Add(this.loginText);
            this.Controls.Add(this.outputText);
            this.Controls.Add(this.setupButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "ShardMap Manager";
            this.Load += new System.EventHandler(this.ShardMapManagerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.RichTextBox outputText;
        private System.Windows.Forms.TextBox loginText;
        private System.Windows.Forms.TextBox passwordText;
    }
}

