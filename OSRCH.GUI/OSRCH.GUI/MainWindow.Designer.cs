namespace OSRCH.GUI
{
    partial class MainWindow
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
            this.logsTextBox = new System.Windows.Forms.RichTextBox();
            this.pbProjectionX = new System.Windows.Forms.PictureBox();
            this.pbProjectionY = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelInstructionName = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbProjectionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProjectionY)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // logsTextBox
            // 
            this.logsTextBox.Location = new System.Drawing.Point(30, 293);
            this.logsTextBox.Name = "logsTextBox";
            this.logsTextBox.Size = new System.Drawing.Size(650, 235);
            this.logsTextBox.TabIndex = 2;
            this.logsTextBox.Text = "";
            // 
            // pbProjectionX
            // 
            this.pbProjectionX.Location = new System.Drawing.Point(33, 39);
            this.pbProjectionX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbProjectionX.Name = "pbProjectionX";
            this.pbProjectionX.Size = new System.Drawing.Size(306, 230);
            this.pbProjectionX.TabIndex = 0;
            this.pbProjectionX.TabStop = false;
            this.pbProjectionX.Paint += new System.Windows.Forms.PaintEventHandler(this.pbProjectionX_Paint);
            // 
            // pbProjectionY
            // 
            this.pbProjectionY.Location = new System.Drawing.Point(367, 39);
            this.pbProjectionY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbProjectionY.Name = "pbProjectionY";
            this.pbProjectionY.Size = new System.Drawing.Size(313, 230);
            this.pbProjectionY.TabIndex = 1;
            this.pbProjectionY.TabStop = false;
            this.pbProjectionY.Paint += new System.Windows.Forms.PaintEventHandler(this.pbProjectionY_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(706, 293);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 235);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Переривання";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelInstructionName);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.startButton);
            this.groupBox2.Controls.Add(this.importButton);
            this.groupBox2.Location = new System.Drawing.Point(706, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 257);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Дії";
            // 
            // labelInstructionName
            // 
            this.labelInstructionName.AutoSize = true;
            this.labelInstructionName.Location = new System.Drawing.Point(140, 27);
            this.labelInstructionName.Name = "labelInstructionName";
            this.labelInstructionName.Size = new System.Drawing.Size(38, 13);
            this.labelInstructionName.TabIndex = 3;
            this.labelInstructionName.Text = "text.txt";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(143, 91);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(98, 36);
            this.button3.TabIndex = 2;
            this.button3.Text = "Стоп";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(19, 91);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(98, 36);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Старт";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(19, 27);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(98, 36);
            this.importButton.TabIndex = 0;
            this.importButton.Text = "Імпорт";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Проекція X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(364, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Проекція Y";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 554);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.logsTextBox);
            this.Controls.Add(this.pbProjectionY);
            this.Controls.Add(this.pbProjectionX);
            this.Name = "MainWindow";
            this.Text = "ОСРЧ Лаба 1";
            ((System.ComponentModel.ISupportInitialize)(this.pbProjectionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProjectionY)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.RichTextBox logsTextBox;

        private System.Windows.Forms.PictureBox pbProjectionX;
        private System.Windows.Forms.PictureBox pbProjectionY;
   

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelInstructionName;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

