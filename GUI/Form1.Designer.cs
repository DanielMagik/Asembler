namespace GUI
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.showFileButton = new System.Windows.Forms.Button();
            this.saveFileButton = new System.Windows.Forms.Button();
            this.plainTextBox = new System.Windows.Forms.TextBox();
            this.threadsLabel = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.radioButtonASM = new System.Windows.Forms.RadioButton();
            this.radioButtonCpp = new System.Windows.Forms.RadioButton();
            this.encryptButton = new System.Windows.Forms.Button();
            this.ChessButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.infoTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.cipherTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // showFileButton
            // 
            this.showFileButton.Location = new System.Drawing.Point(12, 465);
            this.showFileButton.Name = "showFileButton";
            this.showFileButton.Size = new System.Drawing.Size(94, 23);
            this.showFileButton.TabIndex = 0;
            this.showFileButton.Text = "Open text file";
            this.showFileButton.UseVisualStyleBackColor = true;
            this.showFileButton.Click += new System.EventHandler(this.showButtonClick);
            // 
            // saveFileButton
            // 
            this.saveFileButton.Location = new System.Drawing.Point(214, 465);
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(89, 23);
            this.saveFileButton.TabIndex = 1;
            this.saveFileButton.Text = "Save text file";
            this.saveFileButton.UseVisualStyleBackColor = true;
            this.saveFileButton.Click += new System.EventHandler(this.saveButtonClick);
            // 
            // plainTextBox
            // 
            this.plainTextBox.Location = new System.Drawing.Point(12, 180);
            this.plainTextBox.Multiline = true;
            this.plainTextBox.Name = "plainTextBox";
            this.plainTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.plainTextBox.Size = new System.Drawing.Size(241, 264);
            this.plainTextBox.TabIndex = 2;
            // 
            // threadsLabel
            // 
            this.threadsLabel.AutoSize = true;
            this.threadsLabel.Location = new System.Drawing.Point(21, 119);
            this.threadsLabel.Name = "threadsLabel";
            this.threadsLabel.Size = new System.Drawing.Size(49, 13);
            this.threadsLabel.TabIndex = 4;
            this.threadsLabel.Text = "Threads:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(24, 135);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(46, 20);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButtonASM
            // 
            this.radioButtonASM.AutoSize = true;
            this.radioButtonASM.Location = new System.Drawing.Point(24, 66);
            this.radioButtonASM.Name = "radioButtonASM";
            this.radioButtonASM.Size = new System.Drawing.Size(48, 17);
            this.radioButtonASM.TabIndex = 6;
            this.radioButtonASM.Text = "ASM";
            this.radioButtonASM.UseVisualStyleBackColor = true;
            // 
            // radioButtonCpp
            // 
            this.radioButtonCpp.AutoSize = true;
            this.radioButtonCpp.Checked = true;
            this.radioButtonCpp.Location = new System.Drawing.Point(24, 89);
            this.radioButtonCpp.Name = "radioButtonCpp";
            this.radioButtonCpp.Size = new System.Drawing.Size(44, 17);
            this.radioButtonCpp.TabIndex = 7;
            this.radioButtonCpp.TabStop = true;
            this.radioButtonCpp.Text = "C++";
            this.radioButtonCpp.UseVisualStyleBackColor = true;
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(78, 73);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(75, 33);
            this.encryptButton.TabIndex = 12;
            this.encryptButton.Text = "Encrypting";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // ChessButton
            // 
            this.ChessButton.Location = new System.Drawing.Point(112, 465);
            this.ChessButton.Name = "ChessButton";
            this.ChessButton.Size = new System.Drawing.Size(96, 23);
            this.ChessButton.TabIndex = 13;
            this.ChessButton.Text = "Open key file";
            this.ChessButton.UseVisualStyleBackColor = true;
            this.ChessButton.Click += new System.EventHandler(this.ChessButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(28, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 25);
            this.label3.TabIndex = 14;
            this.label3.Text = "ADFGX cipher project";
            // 
            // infoTextBox
            // 
            this.infoTextBox.Location = new System.Drawing.Point(175, 37);
            this.infoTextBox.Multiline = true;
            this.infoTextBox.Name = "infoTextBox";
            this.infoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.infoTextBox.Size = new System.Drawing.Size(179, 118);
            this.infoTextBox.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 19;
            this.button1.Text = "Set threads";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.threadsButtonClick);
            // 
            // keyTextBox
            // 
            this.keyTextBox.Location = new System.Drawing.Point(435, 37);
            this.keyTextBox.Multiline = true;
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.Size = new System.Drawing.Size(84, 118);
            this.keyTextBox.TabIndex = 20;
            // 
            // cipherTextBox
            // 
            this.cipherTextBox.Location = new System.Drawing.Point(265, 180);
            this.cipherTextBox.Multiline = true;
            this.cipherTextBox.Name = "cipherTextBox";
            this.cipherTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cipherTextBox.Size = new System.Drawing.Size(272, 264);
            this.cipherTextBox.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(431, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.TabIndex = 22;
            this.label1.Text = "ADFGX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(406, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 100);
            this.label2.TabIndex = 23;
            this.label2.Text = "A\r\nD\r\nF\r\nG\r\nX";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 502);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cipherTextBox);
            this.Controls.Add(this.keyTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.infoTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ChessButton);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.radioButtonCpp);
            this.Controls.Add(this.radioButtonASM);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.threadsLabel);
            this.Controls.Add(this.plainTextBox);
            this.Controls.Add(this.saveFileButton);
            this.Controls.Add(this.showFileButton);
            this.Name = "Form1";
            this.Text = "JA PROJECT";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button showFileButton;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.TextBox plainTextBox;
        private System.Windows.Forms.Label threadsLabel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.RadioButton radioButtonASM;
        private System.Windows.Forms.RadioButton radioButtonCpp;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Button ChessButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox infoTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox keyTextBox;
        private System.Windows.Forms.TextBox cipherTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

