namespace ProjectOS
{
    partial class MainWindow
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
        private void InitializeComponent ()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            label2 = new Label();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            label15 = new Label();
            textBoxTaskId = new TextBox();
            textBoxVTask = new TextBox();
            textBoxNCmnd = new TextBox();
            textBoxNInOut = new TextBox();
            textBoxPrior = new TextBox();
            trackBarDInOut = new TrackBar();
            labelTrackBarLocation = new Label();
            buttonAddTask = new Button();
            radioButtonManual = new RadioButton();
            radioButtonAuto = new RadioButton();
            tableLayoutPanel2 = new TableLayoutPanel();
            buttonStartOS = new Button();
            buttonEndOS = new Button();
            label4 = new Label();
            textBoxRAM = new TextBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label1 = new Label();
            label9 = new Label();
            label3 = new Label();
            textBoxSpeed = new TextBox();
            textBoxKvant = new TextBox();
            textBoxTNext = new TextBox();
            textBoxTInitIO = new TextBox();
            textBoxTIntrIO = new TextBox();
            textBoxTLoad = new TextBox();
            labelTimeElapsed = new Label();
            dataGridViewTasks = new DataGridView();
            label10 = new Label();
            textBoxTGlobl = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarDInOut).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = SystemColors.ActiveCaption;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.4375F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 51.5625F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 174F));
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(label11, 1, 0);
            tableLayoutPanel1.Controls.Add(label12, 2, 0);
            tableLayoutPanel1.Controls.Add(label13, 0, 2);
            tableLayoutPanel1.Controls.Add(label14, 1, 2);
            tableLayoutPanel1.Controls.Add(label15, 2, 2);
            tableLayoutPanel1.Controls.Add(textBoxTaskId, 0, 1);
            tableLayoutPanel1.Controls.Add(textBoxVTask, 1, 1);
            tableLayoutPanel1.Controls.Add(textBoxNCmnd, 2, 1);
            tableLayoutPanel1.Controls.Add(textBoxNInOut, 1, 3);
            tableLayoutPanel1.Controls.Add(textBoxPrior, 2, 3);
            tableLayoutPanel1.Controls.Add(trackBarDInOut, 0, 3);
            tableLayoutPanel1.Controls.Add(labelTrackBarLocation, 0, 4);
            tableLayoutPanel1.Controls.Add(buttonAddTask, 1, 5);
            tableLayoutPanel1.Controls.Add(radioButtonManual, 0, 5);
            tableLayoutPanel1.Controls.Add(radioButtonAuto, 2, 5);
            tableLayoutPanel1.Location = new Point(16, 252);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50.8474579F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 49.1525421F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 17F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 172F));
            tableLayoutPanel1.Size = new Size(559, 366);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(180, 45);
            label2.TabIndex = 0;
            label2.Text = "Идентификатор задания";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Location = new Point(189, 0);
            label11.Name = "label11";
            label11.Size = new Size(192, 45);
            label11.TabIndex = 1;
            label11.Text = "Размер задания (память)";
            label11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            label12.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label12.AutoSize = true;
            label12.Location = new Point(387, 0);
            label12.Name = "label12";
            label12.Size = new Size(169, 45);
            label12.TabIndex = 2;
            label12.Text = "Чистая длительность выполнения задания (число команд)";
            label12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            label13.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label13.AutoSize = true;
            label13.Location = new Point(3, 88);
            label13.Name = "label13";
            label13.Size = new Size(180, 55);
            label13.TabIndex = 3;
            label13.Text = "Количество команд ввода/вывода (в процентах)";
            label13.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            label14.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label14.AutoSize = true;
            label14.Location = new Point(189, 88);
            label14.Name = "label14";
            label14.Size = new Size(192, 55);
            label14.TabIndex = 4;
            label14.Text = "Длительность команд ввода/вывода";
            label14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            label15.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label15.AutoSize = true;
            label15.Location = new Point(387, 88);
            label15.Name = "label15";
            label15.Size = new Size(169, 55);
            label15.TabIndex = 5;
            label15.Text = "Приоритет задания";
            label15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBoxTaskId
            // 
            textBoxTaskId.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxTaskId.Location = new Point(3, 48);
            textBoxTaskId.Name = "textBoxTaskId";
            textBoxTaskId.PlaceholderText = "ID";
            textBoxTaskId.Size = new Size(180, 23);
            textBoxTaskId.TabIndex = 6;
            textBoxTaskId.KeyPress += KeyPress;
            // 
            // textBoxVTask
            // 
            textBoxVTask.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxVTask.Location = new Point(189, 48);
            textBoxVTask.Name = "textBoxVTask";
            textBoxVTask.PlaceholderText = "Kb";
            textBoxVTask.Size = new Size(192, 23);
            textBoxVTask.TabIndex = 7;
            textBoxVTask.KeyPress += KeyPress;
            // 
            // textBoxNCmnd
            // 
            textBoxNCmnd.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxNCmnd.Location = new Point(387, 48);
            textBoxNCmnd.Name = "textBoxNCmnd";
            textBoxNCmnd.PlaceholderText = "Num";
            textBoxNCmnd.Size = new Size(169, 23);
            textBoxNCmnd.TabIndex = 8;
            textBoxNCmnd.KeyPress += KeyPress;
            // 
            // textBoxNInOut
            // 
            textBoxNInOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxNInOut.Location = new Point(189, 146);
            textBoxNInOut.Name = "textBoxNInOut";
            textBoxNInOut.PlaceholderText = "Миллисек.";
            textBoxNInOut.Size = new Size(192, 23);
            textBoxNInOut.TabIndex = 10;
            textBoxNInOut.KeyPress += KeyPress;
            // 
            // textBoxPrior
            // 
            textBoxPrior.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxPrior.Location = new Point(387, 146);
            textBoxPrior.Name = "textBoxPrior";
            textBoxPrior.PlaceholderText = "Num";
            textBoxPrior.Size = new Size(169, 23);
            textBoxPrior.TabIndex = 0;
            textBoxPrior.KeyPress += KeyPress;
            // 
            // trackBarDInOut
            // 
            trackBarDInOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBarDInOut.LargeChange = 1;
            trackBarDInOut.Location = new Point(3, 146);
            trackBarDInOut.Maximum = 100;
            trackBarDInOut.Name = "trackBarDInOut";
            trackBarDInOut.Size = new Size(180, 27);
            trackBarDInOut.TabIndex = 12;
            trackBarDInOut.Scroll += trackBarDInOut_Scroll;
            // 
            // labelTrackBarLocation
            // 
            labelTrackBarLocation.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelTrackBarLocation.AutoSize = true;
            labelTrackBarLocation.Location = new Point(3, 176);
            labelTrackBarLocation.Name = "labelTrackBarLocation";
            labelTrackBarLocation.Size = new Size(180, 17);
            labelTrackBarLocation.TabIndex = 13;
            labelTrackBarLocation.Text = "0%";
            labelTrackBarLocation.TextAlign = ContentAlignment.TopCenter;
            // 
            // buttonAddTask
            // 
            buttonAddTask.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonAddTask.Location = new Point(189, 196);
            buttonAddTask.Name = "buttonAddTask";
            buttonAddTask.Size = new Size(192, 167);
            buttonAddTask.TabIndex = 11;
            buttonAddTask.Text = "Добавить задачу";
            buttonAddTask.UseVisualStyleBackColor = true;
            buttonAddTask.Click += buttonAddTask_Click;
            // 
            // radioButtonManual
            // 
            radioButtonManual.AutoSize = true;
            radioButtonManual.Checked = true;
            radioButtonManual.Location = new Point(3, 196);
            radioButtonManual.Name = "radioButtonManual";
            radioButtonManual.Size = new Size(124, 19);
            radioButtonManual.TabIndex = 14;
            radioButtonManual.TabStop = true;
            radioButtonManual.Text = "Ручная генерация";
            radioButtonManual.UseVisualStyleBackColor = true;
            radioButtonManual.Click += radioButtonManual_Click;
            // 
            // radioButtonAuto
            // 
            radioButtonAuto.AutoSize = true;
            radioButtonAuto.Location = new Point(387, 196);
            radioButtonAuto.Name = "radioButtonAuto";
            radioButtonAuto.Size = new Size(169, 19);
            radioButtonAuto.TabIndex = 15;
            radioButtonAuto.Text = "Автоматическая генерация";
            radioButtonAuto.UseVisualStyleBackColor = true;
            radioButtonAuto.Click += radioButtonAuto_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.LightGreen;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47.89916F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52.10084F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 201F));
            tableLayoutPanel2.Controls.Add(buttonStartOS, 2, 0);
            tableLayoutPanel2.Controls.Add(buttonEndOS, 0, 0);
            tableLayoutPanel2.Controls.Add(label4, 0, 1);
            tableLayoutPanel2.Controls.Add(textBoxRAM, 0, 2);
            tableLayoutPanel2.Controls.Add(label5, 1, 1);
            tableLayoutPanel2.Controls.Add(label6, 2, 1);
            tableLayoutPanel2.Controls.Add(label7, 0, 3);
            tableLayoutPanel2.Controls.Add(label8, 1, 3);
            tableLayoutPanel2.Controls.Add(label1, 1, 0);
            tableLayoutPanel2.Controls.Add(label9, 2, 3);
            tableLayoutPanel2.Controls.Add(label10, 2, 5);
            tableLayoutPanel2.Controls.Add(label3, 0, 5);
            tableLayoutPanel2.Controls.Add(textBoxSpeed, 0, 6);
            tableLayoutPanel2.Controls.Add(textBoxKvant, 1, 2);
            tableLayoutPanel2.Controls.Add(textBoxTNext, 2, 2);
            tableLayoutPanel2.Controls.Add(textBoxTInitIO, 0, 4);
            tableLayoutPanel2.Controls.Add(textBoxTIntrIO, 1, 4);
            tableLayoutPanel2.Controls.Add(textBoxTLoad, 2, 4);
            tableLayoutPanel2.Controls.Add(textBoxTGlobl, 2, 6);
            tableLayoutPanel2.Controls.Add(labelTimeElapsed, 1, 6);
            tableLayoutPanel2.Location = new Point(16, 12);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 53.3333321F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 46.6666679F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel2.Size = new Size(559, 234);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // buttonStartOS
            // 
            buttonStartOS.Location = new Point(360, 3);
            buttonStartOS.Name = "buttonStartOS";
            buttonStartOS.Size = new Size(196, 23);
            buttonStartOS.TabIndex = 3;
            buttonStartOS.Text = "Запустить ОС";
            buttonStartOS.UseVisualStyleBackColor = true;
            buttonStartOS.Click += buttonStartOS_Click;
            // 
            // buttonEndOS
            // 
            buttonEndOS.Location = new Point(3, 3);
            buttonEndOS.Name = "buttonEndOS";
            buttonEndOS.Size = new Size(165, 23);
            buttonEndOS.TabIndex = 4;
            buttonEndOS.Text = "Завершить ОС";
            buttonEndOS.UseVisualStyleBackColor = true;
            buttonEndOS.Click += buttonEndOS_Click;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(3, 32);
            label4.Name = "label4";
            label4.Size = new Size(165, 28);
            label4.TabIndex = 5;
            label4.Text = "RAM";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBoxRAM
            // 
            textBoxRAM.Location = new Point(3, 63);
            textBoxRAM.Name = "textBoxRAM";
            textBoxRAM.PlaceholderText = "Kb";
            textBoxRAM.Size = new Size(165, 23);
            textBoxRAM.TabIndex = 6;
            textBoxRAM.Text = "640";
            textBoxRAM.KeyPress += KeyPress;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(174, 32);
            label5.Name = "label5";
            label5.Size = new Size(180, 28);
            label5.TabIndex = 7;
            label5.Text = "Квант времени";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(360, 32);
            label6.Name = "label6";
            label6.Size = new Size(196, 28);
            label6.TabIndex = 8;
            label6.Text = "Затраты ОС на выбор процесса";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(3, 90);
            label7.Name = "label7";
            label7.Size = new Size(165, 39);
            label7.TabIndex = 9;
            label7.Text = "Затраты ОС на изменение состояния процесса";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(174, 90);
            label8.Name = "label8";
            label8.Size = new Size(180, 39);
            label8.TabIndex = 10;
            label8.Text = "Затраты ОС по обслуживанию сигнала окончания";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.BackColor = Color.GhostWhite;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(174, 0);
            label1.Name = "label1";
            label1.Size = new Size(180, 32);
            label1.TabIndex = 0;
            label1.Text = "Параметры ОС";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Location = new Point(360, 90);
            label9.Name = "label9";
            label9.Size = new Size(196, 39);
            label9.TabIndex = 11;
            label9.Text = "Число тактов на загрузку нового задания";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(3, 158);
            label3.Name = "label3";
            label3.Size = new Size(165, 42);
            label3.TabIndex = 2;
            label3.Text = "Скорость работы модели";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBoxSpeed
            // 
            textBoxSpeed.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxSpeed.Location = new Point(3, 203);
            textBoxSpeed.Name = "textBoxSpeed";
            textBoxSpeed.PlaceholderText = "Тактов в секунду";
            textBoxSpeed.Size = new Size(165, 23);
            textBoxSpeed.TabIndex = 1;
            textBoxSpeed.Text = "600";
            textBoxSpeed.KeyPress += KeyPress;
            // 
            // textBoxKvant
            // 
            textBoxKvant.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxKvant.Location = new Point(174, 63);
            textBoxKvant.Name = "textBoxKvant";
            textBoxKvant.PlaceholderText = "Тактов";
            textBoxKvant.Size = new Size(180, 23);
            textBoxKvant.TabIndex = 13;
            textBoxKvant.Text = "40";
            textBoxKvant.KeyPress += KeyPress;
            // 
            // textBoxTNext
            // 
            textBoxTNext.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxTNext.Location = new Point(360, 63);
            textBoxTNext.Name = "textBoxTNext";
            textBoxTNext.PlaceholderText = "Тактов";
            textBoxTNext.Size = new Size(196, 23);
            textBoxTNext.TabIndex = 14;
            textBoxTNext.Text = "4";
            textBoxTNext.KeyPress += KeyPress;
            // 
            // textBoxTInitIO
            // 
            textBoxTInitIO.Location = new Point(3, 132);
            textBoxTInitIO.Name = "textBoxTInitIO";
            textBoxTInitIO.PlaceholderText = "Тактов";
            textBoxTInitIO.Size = new Size(165, 23);
            textBoxTInitIO.TabIndex = 15;
            textBoxTInitIO.Text = "2";
            textBoxTInitIO.KeyPress += KeyPress;
            // 
            // textBoxTIntrIO
            // 
            textBoxTIntrIO.Location = new Point(174, 132);
            textBoxTIntrIO.Name = "textBoxTIntrIO";
            textBoxTIntrIO.PlaceholderText = "Тактов";
            textBoxTIntrIO.Size = new Size(180, 23);
            textBoxTIntrIO.TabIndex = 16;
            textBoxTIntrIO.Text = "1";
            textBoxTIntrIO.KeyPress += KeyPress;
            // 
            // textBoxTLoad
            // 
            textBoxTLoad.Location = new Point(360, 132);
            textBoxTLoad.Name = "textBoxTLoad";
            textBoxTLoad.PlaceholderText = "Тактов";
            textBoxTLoad.Size = new Size(196, 23);
            textBoxTLoad.TabIndex = 17;
            textBoxTLoad.Text = "4";
            textBoxTLoad.KeyPress += KeyPress;
            // 
            // labelTimeElapsed
            // 
            labelTimeElapsed.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelTimeElapsed.AutoSize = true;
            labelTimeElapsed.Location = new Point(174, 200);
            labelTimeElapsed.Name = "labelTimeElapsed";
            labelTimeElapsed.Size = new Size(180, 34);
            labelTimeElapsed.TabIndex = 19;
            labelTimeElapsed.Text = "Время работы ОС: 0 с";
            labelTimeElapsed.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dataGridViewTasks
            // 
            dataGridViewTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTasks.Location = new Point(598, 252);
            dataGridViewTasks.Name = "dataGridViewTasks";
            dataGridViewTasks.Size = new Size(665, 366);
            dataGridViewTasks.TabIndex = 3;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Location = new Point(360, 158);
            label10.Name = "label10";
            label10.Size = new Size(196, 42);
            label10.TabIndex = 12;
            label10.Text = "Затраты ОС на общение с общими данными";
            label10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBoxTGlobl
            // 
            textBoxTGlobl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxTGlobl.Location = new Point(360, 203);
            textBoxTGlobl.Name = "textBoxTGlobl";
            textBoxTGlobl.PlaceholderText = "Тактов";
            textBoxTGlobl.Size = new Size(196, 23);
            textBoxTGlobl.TabIndex = 18;
            textBoxTGlobl.KeyPress += KeyPress;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1276, 632);
            Controls.Add(dataGridViewTasks);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            Name = "MainWindow";
            Text = "MainWindow";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarDInOut).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private Label label2;
        private TextBox textBoxSpeed;
        private Label label3;
        private Button buttonStartOS;
        private Button buttonEndOS;
        private Label label4;
        private TextBox textBoxRAM;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox textBoxKvant;
        private TextBox textBoxTNext;
        private TextBox textBoxTInitIO;
        private TextBox textBoxTIntrIO;
        private TextBox textBoxTLoad;
        private Label labelTimeElapsed;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private TextBox textBoxTaskId;
        private TextBox textBoxVTask;
        private TextBox textBoxNCmnd;
        private TextBox textBoxNInOut;
        private TextBox textBoxPrior;
        private Button buttonAddTask;
        private DataGridView dataGridViewTasks;
        private TrackBar trackBarDInOut;
        private Label labelTrackBarLocation;
        private RadioButton radioButtonManual;
        private RadioButton radioButtonAuto;
        private Label label10;
        private TextBox textBoxTGlobl;
    }
}
