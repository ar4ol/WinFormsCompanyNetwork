namespace CompanyNetwork.Forms
{
    partial class NewMessage
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
            this.backButton = new System.Windows.Forms.Button();
            this.regDepButton = new System.Windows.Forms.Button();
            this.depNameBox = new System.Windows.Forms.TextBox();
            this.loginLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.backButton.FlatAppearance.BorderSize = 0;
            this.backButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.backButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backButton.Font = new System.Drawing.Font("MingLiU-ExtB", 40.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(88)))), ((int)(((byte)(101)))));
            this.backButton.Location = new System.Drawing.Point(2, -15);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(72, 64);
            this.backButton.TabIndex = 43;
            this.backButton.Text = "←";
            this.backButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            this.backButton.MouseLeave += new System.EventHandler(this.backButton_MouseLeave);
            this.backButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backButton_MouseMove);
            // 
            // regDepButton
            // 
            this.regDepButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(110)))), ((int)(((byte)(165)))));
            this.regDepButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.regDepButton.FlatAppearance.BorderSize = 0;
            this.regDepButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.regDepButton.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.regDepButton.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.regDepButton.Location = new System.Drawing.Point(208, 246);
            this.regDepButton.Name = "regDepButton";
            this.regDepButton.Size = new System.Drawing.Size(190, 55);
            this.regDepButton.TabIndex = 39;
            this.regDepButton.Text = "Отправить";
            this.regDepButton.UseVisualStyleBackColor = false;
            this.regDepButton.Click += new System.EventHandler(this.regDepButton_Click);
            // 
            // depNameBox
            // 
            this.depNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.depNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.depNameBox.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.depNameBox.ForeColor = System.Drawing.SystemColors.Window;
            this.depNameBox.Location = new System.Drawing.Point(12, 104);
            this.depNameBox.Multiline = true;
            this.depNameBox.Name = "depNameBox";
            this.depNameBox.Size = new System.Drawing.Size(386, 123);
            this.depNameBox.TabIndex = 38;
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Font = new System.Drawing.Font("Century Gothic", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.loginLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.loginLabel.Location = new System.Drawing.Point(121, 47);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(267, 34);
            this.loginLabel.TabIndex = 37;
            this.loginLabel.Text = "Текст сообщения:";
            // 
            // NewMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.ClientSize = new System.Drawing.Size(425, 327);
            this.Controls.Add(this.regDepButton);
            this.Controls.Add(this.depNameBox);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.backButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewMessage";
            this.Text = "Company Network ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button regDepButton;
        private System.Windows.Forms.TextBox depNameBox;
        private System.Windows.Forms.Label loginLabel;
    }
}