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
            label3 = new Label();
            label1 = new Label();
            ComboBoxDevices = new ComboBox();
            ComboBoxOperationType = new ComboBox();
            ButtonRunOperation = new Button();
            panel3 = new Panel();
            ShowErrorLine = new CheckBox();
            panel4.SuspendLayout();
            PanelRawCommand.SuspendLayout();
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
            LogBox.Size = new Size(728, 356);
            LogBox.TabIndex = 5;
            LogBox.TabStop = false;
            LogBox.Text = "";
            // 
            // panel4
            // 
            panel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel4.Controls.Add(LogBox);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 134);
            panel4.Margin = new Padding(3, 2, 3, 2);
            panel4.Name = "panel4";
            panel4.Size = new Size(728, 356);
            panel4.TabIndex = 14;
            // 
            // PanelRawCommand
            // 
            PanelRawCommand.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PanelRawCommand.Controls.Add(label4);
            PanelRawCommand.Controls.Add(TextBoxRawCommand);
            PanelRawCommand.Dock = DockStyle.Top;
            PanelRawCommand.Location = new Point(0, 87);
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
            panel3.Controls.Add(ComboBoxOperationType);
            panel3.Controls.Add(ComboBoxDevices);
            panel3.Controls.Add(ShowErrorLine);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(label3);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(3, 2, 3, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(728, 87);
            panel3.TabIndex = 11;
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(728, 490);
            Controls.Add(panel4);
            Controls.Add(PanelRawCommand);
            Controls.Add(panel3);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PS5 Code Reader";
            Load += Form1_Load;
            panel4.ResumeLayout(false);
            PanelRawCommand.ResumeLayout(false);
            PanelRawCommand.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private ReadOnlyRichTextBox LogBox;
        private Panel PanelRawCommand;
        private Panel panel4;
        private TextBox TextBoxRawCommand;
        private Label label4;
        private Label label3;
        private Label label1;
        private ComboBox ComboBoxDevices;
        private ComboBox ComboBoxOperationType;
        private Button ButtonRunOperation;
        private Panel panel3;
        private CheckBox ShowErrorLine;
    }
}