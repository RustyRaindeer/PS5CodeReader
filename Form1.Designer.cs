namespace PS5CodeReader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LogBox = new ReadOnlyRichTextBox();
            panel4 = new Panel();
            PanelRawCommand = new Panel();
            label4 = new Label();
            TextBoxRawCommand = new TextBox();
            PanelInterpretError = new Panel();
            label5 = new Label();
            TextBoxInterpretError = new TextBox();
            label3 = new Label();
            label1 = new Label();
            ComboBoxDevices = new ComboBox();
            ComboBoxOperationType = new ComboBox();
            ButtonRunOperation = new Button();
            panel3 = new Panel();
            ComboBoxDatabase = new ComboBox();
            ShowErrorLine = new CheckBox();
            label2 = new Label();
            panel4.SuspendLayout();
            PanelRawCommand.SuspendLayout();
            PanelInterpretError.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // LogBox
            // 
            LogBox.Dock = DockStyle.Fill;
            LogBox.Location = new Point(0, 0);
            LogBox.Margin = new Padding(3, 2, 3, 2);
            LogBox.Name = "LogBox";
            LogBox.ReadOnly = true;
            LogBox.Size = new Size(728, 329);
            LogBox.TabIndex = 5;
            LogBox.TabStop = false;
            LogBox.Text = "";
            // 
            // panel4
            // 
            panel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel4.Controls.Add(LogBox);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 223);
            panel4.Margin = new Padding(3, 2, 3, 2);
            panel4.Name = "panel4";
            panel4.Size = new Size(728, 329);
            panel4.TabIndex = 14;
            // 
            // PanelRawCommand
            // 
            PanelRawCommand.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PanelRawCommand.Controls.Add(label4);
            PanelRawCommand.Controls.Add(TextBoxRawCommand);
            PanelRawCommand.Dock = DockStyle.Top;
            PanelRawCommand.Location = new Point(0, 176);
            PanelRawCommand.Margin = new Padding(3, 2, 3, 2);
            PanelRawCommand.Name = "PanelRawCommand";
            PanelRawCommand.Size = new Size(728, 47);
            PanelRawCommand.TabIndex = 13;
            PanelRawCommand.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 2);
            label4.Name = "label4";
            label4.Size = new Size(89, 15);
            label4.TabIndex = 11;
            label4.Text = "Raw Command";
            // 
            // TextBoxRawCommand
            // 
            TextBoxRawCommand.Enabled = false;
            TextBoxRawCommand.Location = new Point(3, 20);
            TextBoxRawCommand.Margin = new Padding(3, 2, 3, 2);
            TextBoxRawCommand.Name = "TextBoxRawCommand";
            TextBoxRawCommand.Size = new Size(587, 23);
            TextBoxRawCommand.TabIndex = 12;
            TextBoxRawCommand.KeyPress += TextBoxRawCommand_KeyPress;
            // 
            // PanelInterpretError
            // 
            PanelInterpretError.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PanelInterpretError.Controls.Add(label5);
            PanelInterpretError.Controls.Add(TextBoxInterpretError);
            PanelInterpretError.Dock = DockStyle.Top;
            PanelInterpretError.Location = new Point(0, 129);
            PanelInterpretError.Margin = new Padding(3, 2, 3, 2);
            PanelInterpretError.Name = "PanelInterpretError";
            PanelInterpretError.Size = new Size(728, 47);
            PanelInterpretError.TabIndex = 13;
            PanelInterpretError.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(3, 2);
            label5.Name = "label5";
            label5.Size = new Size(133, 15);
            label5.TabIndex = 11;
            label5.Text = "Interpret Error Response";
            // 
            // TextBoxInterpretError
            // 
            TextBoxInterpretError.Enabled = false;
            TextBoxInterpretError.Location = new Point(3, 20);
            TextBoxInterpretError.Margin = new Padding(3, 2, 3, 2);
            TextBoxInterpretError.Name = "TextBoxInterpretError";
            TextBoxInterpretError.Size = new Size(587, 23);
            TextBoxInterpretError.TabIndex = 12;
            TextBoxInterpretError.Text = "OK 00000000 80800000 00000000 00000000 00000100 0000 0000 FFFF FFFF";
            TextBoxInterpretError.TextChanged += TextBoxInterpretError_TextChanged;
            TextBoxInterpretError.KeyPress += TextBoxInterpretError_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 4);
            label3.Name = "label3";
            label3.Size = new Size(121, 15);
            label3.TabIndex = 10;
            label3.Text = "Select Operation Type";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 45);
            label1.Name = "label1";
            label1.Size = new Size(117, 15);
            label1.TabIndex = 4;
            label1.Text = "Serial Devices (UART)";
            // 
            // ComboBoxDevices
            // 
            ComboBoxDevices.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxDevices.FormattingEnabled = true;
            ComboBoxDevices.Location = new Point(3, 62);
            ComboBoxDevices.Margin = new Padding(3, 2, 3, 2);
            ComboBoxDevices.Name = "ComboBoxDevices";
            ComboBoxDevices.Size = new Size(587, 23);
            ComboBoxDevices.TabIndex = 3;
            ComboBoxDevices.DropDown += ComboBoxDevices_DropDown;
            // 
            // ComboBoxOperationType
            // 
            ComboBoxOperationType.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxOperationType.FormattingEnabled = true;
            ComboBoxOperationType.Location = new Point(3, 22);
            ComboBoxOperationType.Margin = new Padding(3, 2, 3, 2);
            ComboBoxOperationType.Name = "ComboBoxOperationType";
            ComboBoxOperationType.Size = new Size(587, 23);
            ComboBoxOperationType.TabIndex = 9;
            ComboBoxOperationType.SelectedIndexChanged += ComboBoxOperationType_SelectedIndexChanged;
            // 
            // ButtonRunOperation
            // 
            ButtonRunOperation.Location = new Point(594, 23);
            ButtonRunOperation.Margin = new Padding(3, 2, 3, 2);
            ButtonRunOperation.Name = "ButtonRunOperation";
            ButtonRunOperation.Size = new Size(131, 22);
            ButtonRunOperation.TabIndex = 0;
            ButtonRunOperation.Text = "Run Operation";
            ButtonRunOperation.UseVisualStyleBackColor = true;
            ButtonRunOperation.Click += ButtonRunOperations_Click;
            // 
            // panel3
            // 
            panel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel3.Controls.Add(ButtonRunOperation);
            panel3.Controls.Add(ComboBoxDatabase);
            panel3.Controls.Add(ComboBoxOperationType);
            panel3.Controls.Add(ComboBoxDevices);
            panel3.Controls.Add(ShowErrorLine);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(label3);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(3, 2, 3, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(728, 129);
            panel3.TabIndex = 11;
            // 
            // ComboBoxDatabase
            // 
            ComboBoxDatabase.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxDatabase.FormattingEnabled = true;
            ComboBoxDatabase.Items.AddRange(new object[] { "RustyRaindeer/PS5CodeReader/master/ErrorCodes.json", "amoamare/Console-Service-Tool/master/Resources/ErrorCodes.json" });
            ComboBoxDatabase.Location = new Point(3, 104);
            ComboBoxDatabase.Margin = new Padding(3, 2, 3, 2);
            ComboBoxDatabase.Name = "ComboBoxDatabase";
            ComboBoxDatabase.Size = new Size(587, 23);
            ComboBoxDatabase.TabIndex = 9;
            // 
            // ShowErrorLine
            // 
            ShowErrorLine.AutoSize = true;
            ShowErrorLine.Location = new Point(594, 64);
            ShowErrorLine.Margin = new Padding(3, 2, 3, 2);
            ShowErrorLine.Name = "ShowErrorLine";
            ShowErrorLine.Size = new Size(133, 19);
            ShowErrorLine.TabIndex = 11;
            ShowErrorLine.Text = "Show Line Response";
            ShowErrorLine.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 87);
            label2.Name = "label2";
            label2.Size = new Size(116, 15);
            label2.TabIndex = 4;
            label2.Text = "Error codes database";
            label2.Click += label2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(728, 552);
            Controls.Add(panel4);
            Controls.Add(PanelRawCommand);
            Controls.Add(PanelInterpretError);
            Controls.Add(panel3);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PS5 Code Reader";
            Load += Form1_Load;
            panel4.ResumeLayout(false);
            PanelRawCommand.ResumeLayout(false);
            PanelRawCommand.PerformLayout();
            PanelInterpretError.ResumeLayout(false);
            PanelInterpretError.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private ReadOnlyRichTextBox LogBox;
        private Panel PanelRawCommand;
        private Panel PanelInterpretError;
        private Panel panel4;
        private TextBox TextBoxRawCommand;
        private TextBox TextBoxInterpretError;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label1;
        private ComboBox ComboBoxDevices;
        private ComboBox ComboBoxOperationType;
        private Button ButtonRunOperation;
        private Panel panel3;
        private CheckBox ShowErrorLine;
        private Label label2;
        private ComboBox ComboBoxDatabase;
    }
}