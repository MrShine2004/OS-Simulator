using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using ProjectOS;
using static ProjectOS.CPU;

namespace ProjectOS
{
    public partial class MainWindow : Form
    {
        public OS myOS = new OS (0,0,0,0,0,0,0);
        public List<OSTask> tasksList = new List<OSTask>();
        public CPU myCpu = null;
        public int usageRAM = 0;
        public int currentTick = 0;
        public bool kvantStatus = false;
        public bool loadStatus = false;
        public OSTask executerTask = null;

        public bool System_Status = false;
        public Stopwatch stopwatch; // ������ Stopwatch ��� ������� �������
        public System.Windows.Forms.Timer uiTimer;
        public long timeStart;

        // Random ���������
        Random random = new Random();

        private int taskIdCounter = 0; // ������� ID �����, ������� � 0


        public MainWindow ()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeCommandsGridView();
            myCpu = new CPU(this);
        }
        private void InitializeDataGridView ()
        {
            // ��������� ������� � DataGridView
            dataGridViewTasks.Columns.Add("TaskId", "ID");
            dataGridViewTasks.Columns.Add("VTask", "������");
            dataGridViewTasks.Columns.Add("NCmnd", "����� ������");
            dataGridViewTasks.Columns.Add("DInOut", "������� ������ �����/������");
            dataGridViewTasks.Columns.Add("NInOut", "������������ ������ �����/������");
            dataGridViewTasks.Columns.Add("Prior", "���������");
            dataGridViewTasks.Columns.Add("Status", "������");
            dataGridViewTasks.Columns.Add("Takt", "����");

            // ��������� ������ ��� ������� ������� (� ��������)
            dataGridViewTasks.Columns[0].Width = 40;  // ������ ��� ������� � Task_Id
            dataGridViewTasks.Columns[1].Width = 100; // ������ ��� ������� � V_task
            dataGridViewTasks.Columns[2].Width = 80;  // ������ ��� ������� � N_cmnd
            dataGridViewTasks.Columns[3].Width = 100; // ������ ��� ������� � D_InOut
            dataGridViewTasks.Columns[4].Width = 100; // ������ ��� ������� � N_InOut
            dataGridViewTasks.Columns[5].Width = 80;  // ������ ��� ������� � Prior
            dataGridViewTasks.Columns[6].Width = 90;  // ������ ��� ������� � Status
            dataGridViewTasks.Columns[7].Width = 50;  // ������ ��� ������� � Takt
        }
        private void InitializeCommandsGridView ()
        {
            // ��������� ������� � DataGridView
            dataGridViewCommands.Columns.Add("Id", "�");
            dataGridViewCommands.Columns.Add("Type", "���");

            // ��������� ������ ��� ������� ������� (� ��������)
            dataGridViewCommands.Columns[0].Width = 40;  // ������ ��� ������� � Task_Id
            dataGridViewCommands.Columns[1].Width = 70; // ������ ��� ������� � V_task
        }
        private void KeyPress (object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        // ������ �������
        private void buttonStartOS_Click (object sender, EventArgs e)
        {
            taskIdCounter = 0;
            usageRAM = 0;
            currentTick = 0;
            myCpu.PC = 0;
            myOS.M_multi = 0;
            myOS.N_Proc = 0;
            myOS.T_obor = 0;
            RefreshInterface();


            if (!System_Status)
            {
                if ((textBoxRAM.Text != "" && IsInteger(textBoxRAM.Text)) &&
                (textBoxKvant.Text != "" && IsInteger(textBoxKvant.Text)) &&
                (textBoxTNext.Text != "" && IsInteger(textBoxTNext.Text)) &&
                (textBoxTInitIO.Text != "" && IsInteger(textBoxTInitIO.Text)) &&
                (textBoxTIntrIO.Text != "" && IsInteger(textBoxTIntrIO.Text)) &&
                (textBoxTLoad.Text != "" && IsInteger(textBoxTLoad.Text)) &&
                (textBoxSpeed.Text != "" && IsInteger(textBoxSpeed.Text)))
                {
                    // ��������� ���������� � ������ �������

                    myOS.V_ozu = int.Parse(textBoxRAM.Text);
                    myOS.Kvant = int.Parse(textBoxKvant.Text);
                    myOS.T_next = int.Parse(textBoxTNext.Text);
                    myOS.T_InitIO = int.Parse(textBoxTInitIO.Text);
                    myOS.T_IntrIO = int.Parse(textBoxTIntrIO.Text);
                    myOS.T_Load = int.Parse(textBoxTLoad.Text);
                    myOS.Speed = int.Parse(textBoxSpeed.Text);


                    System_Status = true;
                    stopwatch = Stopwatch.StartNew(); // ��������� Stopwatch

                    // ������ ��� ���������� ���������� ��� � 100 ��
                    myOS.T_multi = new System.Timers.Timer(100);
                    myOS.T_multi.Elapsed += UpdateTimeDisplay;
                    myOS.T_multi.Start();


                    Task.Run(() => TickSimulate());
                    Task.Run(() => SimulateOS());
                    // ������ ��� ���������� ���������� ��� � 100 ��
                    uiTimer = new System.Windows.Forms.Timer();
                    uiTimer.Interval = 1; // 100 ��
                    uiTimer.Tick += (sender, e) => RefreshInterface();
                    uiTimer.Start();
                }
                else
                {
                    MessageBox.Show("�� ����� ������������ ������!");
                    System_Status = false;
                }
                textBoxRAM.ReadOnly = true;
                textBoxKvant.ReadOnly = true;
                textBoxTNext.ReadOnly = true;
                textBoxTInitIO.ReadOnly = true;
                textBoxTIntrIO.ReadOnly = true;
                textBoxTLoad.ReadOnly = true;
                textBoxSpeed.ReadOnly = false;
            }
            else
            {
                MessageBox.Show("�� ��� ��������!");
            }

        }

        private async void TickSimulate ()
        {
            while (System_Status)
            {
                Thread.Sleep(myOS.Speed);
                currentTick++;
            }
        }

        // �������
        public void DelaySimulate (int temp, Process process, int k)
        {
            int tik = currentTick;
            process.AssociatedTask.currentCmd = process.AssociatedTask.CMDLen;
            while (currentTick - tik <= temp && process.AssociatedTask.currentCmd > 0)
            {
                process.AssociatedTask.currentCmd--;
                UpdateDataGridViewTasks(executerTask);
            }
        }

        // �������
        public void DelaySimulate (int temp)
        {
            int tik = currentTick;
            while (currentTick - tik <= temp)
            {

            }
        }

        // �������
        public void DelaySimulate (int temp, Process process)
        {
            int tik = currentTick;
            process.AssociatedTask.currentCmd = process.AssociatedTask.N_InOut;
            while (currentTick - tik <= temp && process.AssociatedTask.currentCmd > 0)
            {
                process.AssociatedTask.currentCmd--;
                UpdateDataGridViewTasks(executerTask);
            }
        }

        private async void SimulateOS ()
        {
            while (System_Status)
            {
                DelaySimulate(1);
                if (loadStatus)
                {
                    SortTask();
                    loadStatus = false;
                }
                //DelaySimulate(myOS.Speed);   // �������� ������ ���������
                if (tasksList.Count != 0)
                {
                    timeStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    // ������� �� ����� �������� ��� ����������
                    DelaySimulate(myOS.T_next);

                    // �������� ������ ������� ������
                    OSTask? currentTask = null;

                    if (tasksList.Count > 0)
                    {
                        int i = 0;
                        while (i < tasksList.Count && tasksList[i].Status != CMD.WAIT)
                        {
                            i++;
                        }
                        if (i < tasksList.Count && tasksList[i] != null)
                            currentTask = tasksList[i];
                    }
                    if (currentTask == null)
                    {
                        continue;
                    }
                    // ������������� � �������
                    Process currentProcess = new Process(currentTask.Task_Id, currentTask, currentTask.Prior);
                    executerTask = currentTask;
                    SelectTaskById(executerTask.Task_Id);

                    // ������� �� �������� ������ �������
                    DelaySimulate(myOS.T_Load);

                    for (int k = 0; k < myOS.Kvant; k++)
                    {
                        if (currentTask.N_cmnd > 0 &&
                                            (currentTask.Status == CMD.WAIT) &&
                                            (myCpu.CurProc == -1)
                                            )
                        {
                            myCpu.ExecuteCommand(currentProcess, k);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        private void UpdateTimeDisplay (Object source, System.Timers.ElapsedEventArgs e)
        {
            // ���������� Stopwatch ��� ��������� ���������� �������
            TimeSpan time = stopwatch.Elapsed;

            // ����������� �����: ����, ������, �������, ������������
            string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                time.Hours, time.Minutes, time.Seconds, time.Milliseconds);

            // ��������� ����������� ������� ������ ��
            if (labelTimeElapsed.InvokeRequired)
            {
                labelTimeElapsed.BeginInvoke(new Action(() => labelTimeElapsed.Text = "����� ������ ��: " + timeFormatted));
            }
            else
            {
                labelTimeElapsed.Text = "����� ������ ��: " + timeFormatted;
                labelTick.Text = "���: " + currentTick;
            }
        }

        private bool IsInteger (string lexeme)
        {
            // ����� ���������, �������� �� ������� ������
            return int.TryParse(lexeme, out int value);
        }
        void AddTask (OSTask task)
        {
            tasksList.Add(task);
            loadStatus = true;
        }
        public void SortTask ()
        {
            if (tasksList.Count() > 1)
                tasksList = tasksList
                .Where(t => t != null)  // ��������������� ��������, ������� �� �������� null
                .OrderByDescending(t => t.Prior)
                .ThenBy(t => t.Task_Id)
                .ToList();

        }

        // ������� ��� ���������� DataGridView
        public void UpdateDataGridViewTasks ()
        {
            if (dataGridViewTasks.InvokeRequired)
            {
                // ���� ����� ���� �� �� ��������� ������, ���������� Invoke
                dataGridViewTasks.Invoke(new Action(UpdateDataGridViewTasks));
            }
            else
            {
                // ������� ������� ������ � �������
                dataGridViewTasks.Rows.Clear();

                // �������� �� ������ ����� � ��������� �� � �������
                for (int i = 0; i < tasksList.Count; i++)
                {
                    OSTask task = tasksList[i];
                    dataGridViewTasks.Rows.Add(
                        task.Task_Id,
                        task.V_task,
                        task.N_cmnd,
                        task.D_InOut,
                        task.N_InOut,
                        task.Prior,
                        task.Status,
                        task.currentCmd
                    );
                }

            }
        }

        // ������� ��� ���������� DataGridView
        public void UpdateDataGridViewTasks (OSTask executerTaskUpdate)
        {
            if (dataGridViewTasks.InvokeRequired)
            {
                // ���� ����� ���� �� �� ��������� ������, ���������� Invoke
                dataGridViewTasks.Invoke(new Action(() => UpdateDataGridViewTasks(executerTaskUpdate)));
            }
            else
            {
                // ������� ������� ������ � �������
                dataGridViewTasks.Rows.Clear();

                // �������� �� ������ ����� � ��������� �� � �������
                for (int i = 0; i < tasksList.Count; i++)
                {
                    OSTask task = tasksList[i];
                    dataGridViewTasks.Rows.Add(
                        task.Task_Id,
                        task.V_task,
                        task.N_cmnd,
                        task.D_InOut,
                        task.N_InOut,
                        task.Prior,
                        task.Status,
                        task.currentCmd
                    );
                }
                SelectTaskById(executerTaskUpdate.Task_Id);
                UpdateCommandsTable(executerTaskUpdate);
            }
        }

        public void UpdateCommandsTable (OSTask task)
        {
            if (dataGridViewCommands.InvokeRequired)
            {
                // ���� ����� ���� �� �� ��������� ������, ���������� Invoke � ���������� process
                dataGridViewCommands.Invoke(new Action(() => UpdateCommandsTable(task)));
            }
            else
            {
                // ������� ������� ������ � �������
                dataGridViewCommands.Rows.Clear();

                // �������� �� ������ ����� � ��������� �� � �������
                for (int i = 0; i < task.Commands.Count; i++)
                {
                    dataGridViewCommands.Rows.Add(
                        i,
                        task.Commands[i]
                    );
                }
                SelectCmdById(task.currentCmd);
            }
        }

        public void RefreshInterface ()
        {
            if (InvokeRequired)
            {
                // ���� �� ��������� �� � �������� ������, �������� Invoke, ����� ������������� �� �������� �����
                Invoke(new Action(RefreshInterface));
            }
            else
            {
                // ����� �� ��� ��������� � �������� ������, ������� ����� ��������� ��������� ���������
                if (myCpu.Command)
                    panelCPUCommand.BackColor = Color.Green;
                else panelCPUCommand.BackColor = Color.Red;
                labelUsageRAM.Text = "������������ ������: " + usageRAM + " ��";
                if (myCpu.CurProc == -1)
                {
                    labelPerformedTask.Text = "����������� ������: ";
                }
                else
                {
                    labelPerformedTask.Text = "����������� ������: " + myCpu.CurProc;
                }
                labelNProc.Text = "����� ����������� �������: " + myOS.N_Proc;
                labelMmulty.Text = "����������� �������: " + myOS.M_multi;
                labelDsys.Text = "��������� ������� �� (������): " + myOS.D_sys + "%";
                labelTobor.Text = "����� ����� ��������: " + myOS.T_obor + " ��";
                labelPC.Text = "������� �������: " + myCpu.PC;
                labelTick.Text = "���: " + currentTick;
            }
        }



        private void buttonEndOS_Click (object sender, EventArgs e)
        {
            if (System_Status == false)
            {
                return;
            }
            System_Status = false;
            myCpu.CurProc = -1; 
            myCpu.Command = false;
            dataGridViewTasks.Rows.Clear();
            RefreshInterface();
            tasksList.Clear();
            myOS.T_multi.Stop();

            textBoxRAM.ReadOnly = false;
            textBoxKvant.ReadOnly = false;
            textBoxTNext.ReadOnly = false;
            textBoxTInitIO.ReadOnly = false;
            textBoxTIntrIO.ReadOnly = false;
            textBoxTLoad.ReadOnly = false;
            textBoxSpeed.ReadOnly = false;
        }

        private void buttonAddTask_Click (object sender, EventArgs e)
        {
            if (System_Status)
            {
                // ���������, ��� ��� ����������� ���� ���������, ����� ID, ������� ������������ �������������
                if (!string.IsNullOrWhiteSpace(textBoxVTask.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxNCmnd.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxNInOut.Text))
                {
                    // ���������� ���������� ID ������
                    string taskId = taskIdCounter.ToString();

                    OSTask newTask = new OSTask(int.Parse(taskId),
                        int.Parse(textBoxVTask.Text),
                        int.Parse(textBoxNCmnd.Text),
                        trackBarDInOut.Value,
                        int.Parse(textBoxNInOut.Text),
                        trackBarPriority.Value);

                    if (usageRAM + newTask.V_task <= myOS.V_ozu)
                    {
                        AddTask(newTask);

                        // ��������� ������ � DataGridView
                        dataGridViewTasks.Rows.Add(
                            newTask.Task_Id,
                            newTask.V_task,
                            newTask.N_cmnd,
                            newTask.D_InOut,
                            newTask.N_InOut,
                            newTask.Prior,
                            newTask.Status
                        );

                        // ����������� ������� ID ��� ��������� ������
                        taskIdCounter++;

                        // ������� ��������� ���� ����� ����������
                        //textBoxVTask.Clear();
                        //textBoxNCmnd.Clear();
                        //textBoxDInOut.Clear();
                        //textBoxNInOut.Clear();
                        //textBoxPrior.Clear();

                        if (radioButtonAuto.Checked) // ���� ������� �������������� ���������
                        {
                            // ���������� ��������� ������
                            textBoxVTask.Text = random.Next(1, 100).ToString(); // �������� ������
                            textBoxNCmnd.Text = random.Next(2, 50).ToString(); // ���������� ������
                            trackBarDInOut.Value = random.Next(0, 100); // ������� �������� ��������
                            textBoxNInOut.Text = random.Next(myOS.Kvant * 2, myOS.Kvant * 4).ToString(); // ���������� �����/������
                            labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
                            trackBarPriority.Value = random.Next(1, 10); // ��������� ������
                            labelPriority.Text = "" + trackBarPriority.Value;
                        }
                        usageRAM += newTask.V_task;
                        if (myOS.V_ozu > 0)
                            myOS.D_sys = Math.Round((100.0 / myOS.V_ozu) * usageRAM, 2);
                        else
                            myOS.D_sys = 0;
                        myOS.N_Proc++;
                        RefreshInterface();
                    }
                    else
                    {
                        MessageBox.Show("������������ ������!");
                    }


                }
                else
                {
                    MessageBox.Show("��������� ��� ���� ��� ���������� ������!");
                }
            }
            else
            {
                MessageBox.Show("�� �� ��������!");
            }
        }


        private void trackBarDInOut_Scroll (object sender, EventArgs e)
        {
            labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
        }

        private void trackBarPrior_Scroll (object sender, EventArgs e)
        {
            labelPriority.Text = "" + trackBarPriority.Value;
        }

        private void radioButtonAuto_Click (object sender, EventArgs e)
        {
            radioButtonManual.Checked = !radioButtonAuto.Checked;
            // ���������� ��������� ������
            textBoxVTask.Text = random.Next(1, 100).ToString(); // �������� ������
            textBoxNCmnd.Text = random.Next(2, 50).ToString(); // ���������� ������
            trackBarDInOut.Value = random.Next(0, 100); // ������� �������� ��������
            textBoxNInOut.Text = random.Next(myOS.Kvant * 2, myOS.Kvant * 4).ToString(); // ���������� �����/������
            labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
            trackBarPriority.Value = random.Next(1, 10); // ��������� ������
            labelPriority.Text = "" + trackBarPriority.Value;
        }

        private void radioButtonManual_Click (object sender, EventArgs e)
        {
            radioButtonAuto.Checked = !radioButtonManual.Checked;
        }

        private void buttonApplyNewParam_Click (object sender, EventArgs e)
        {
            if (System_Status)
            {
                if ((textBoxRAM.Text != "" && IsInteger(textBoxRAM.Text)) &&
                (textBoxKvant.Text != "" && IsInteger(textBoxKvant.Text)) &&
                (textBoxTNext.Text != "" && IsInteger(textBoxTNext.Text)) &&
                (textBoxTInitIO.Text != "" && IsInteger(textBoxTInitIO.Text)) &&
                (textBoxTIntrIO.Text != "" && IsInteger(textBoxTIntrIO.Text)) &&
                (textBoxTLoad.Text != "" && IsInteger(textBoxTLoad.Text)) &&
                (textBoxSpeed.Text != "" && IsInteger(textBoxSpeed.Text)))
                {
                    // ��������� ���������� � ������ �������

                    myOS.V_ozu = int.Parse(textBoxRAM.Text);
                    myOS.Kvant = int.Parse(textBoxKvant.Text);
                    myOS.T_next = int.Parse(textBoxTNext.Text);
                    myOS.T_InitIO = int.Parse(textBoxTInitIO.Text);
                    myOS.T_IntrIO = int.Parse(textBoxTIntrIO.Text);
                    myOS.T_Load = int.Parse(textBoxTLoad.Text);
                    myOS.Speed = int.Parse(textBoxSpeed.Text);
                }
                else
                {
                    MessageBox.Show("�� ����� ������������ ������!");
                }
            }
            else
            {
                MessageBox.Show("�� �� ��������!");
            }
        }

        // ����� ��� ��������� �����/������
        private void SimulateIOCommand (Process process, CPU myCpu, int N_InOut)
        {
            process.D_ready += myOS.T_IntrIO; // ����������� ����� ��������
            process.AssociatedTask.IO_cmnd--;
            DelaySimulate(process.AssociatedTask.N_InOut, process);
            //UpdateDataGridViewTasks();
        }

        // ����������� ��������� ������� �����/������
        public async Task ProcessIOCommand (Process currentProcess)
        {
            OSTask currentTask = currentProcess.AssociatedTask;
            await Task.Run(() =>
            {
                // ������������ ������� �����/������
                OSTask currentTask = currentProcess.AssociatedTask;
                OSTask task = currentProcess.AssociatedTask;

                // ���������� ������� �����/������
                currentTask.Status = CMD.IO;
                UpdateDataGridViewTasks(executerTask);
                SimulateIOCommand(currentProcess, myCpu, task.N_InOut);

                currentTask.Status = CMD.IO_END;
                UpdateDataGridViewTasks(executerTask);
                // ������� �� ������������ ���������� �����/������
                DelaySimulate(myOS.T_IntrIO);
                currentTask.IO_cmnd--;
                currentTask.N_cmnd--;


                currentProcess.AssociatedTask.Commands.RemoveAt(0);
                // ������� �� �� ��������� ��������� ��������
                DelaySimulate(myOS.T_InitIO);
                currentTask.Status = CMD.WAIT;
                UpdateDataGridViewTasks(executerTask);
                // ������� �� �� ������������ ����������
                DelaySimulate(myOS.T_IntrIO);
            });
        }

        // ���������� ������� CellClick
        private void dataGridViewTasks_CellClick (object sender, DataGridViewCellEventArgs e)
        {
            // ���������, ��� �������� �� ������, � �� �� ���������
            if (e.RowIndex >= 0)
            {
                // �������� ������, �� ������� ��������
                DataGridViewRow row = dataGridViewTasks.Rows[e.RowIndex];

                // ���������, ���������� �� ������� � ������ "TaskId" � �� ����� �� ��������
                if (row.Cells["TaskId"] != null && row.Cells["TaskId"].Value != null)
                {
                    // ������� �������� ID ������ �� ������
                    int taskId;
                    bool isValidId = int.TryParse(row.Cells["TaskId"].Value.ToString(), out taskId);

                    if (isValidId)
                    {
                        // ���� ������ �� ID � ������ �����
                        OSTask selectedTask = tasksList.FirstOrDefault(task => task.Task_Id == taskId);

                        if (selectedTask != null)
                        {
                            labelCmds.Text = "ID ������: " + taskId;
                            // ���� ������ �������, ��������� ������� ������ ��� ���� ������
                            UpdateCommandsTable(selectedTask);
                        }
                        else
                        {
                            labelCmds.Text = "������ �� �������.";
                        }
                    }
                    else
                    {
                        labelCmds.Text = "�������� ID ������.";
                    }
                }
                else
                {
                    labelCmds.Text = "ID ������ �����������.";
                }
            }
        }

        private void SelectTaskById (int taskId)
        {
            if (dataGridViewTasks.InvokeRequired)
            {
                // ���� ����� ���� �� ������� ������, ���������� Invoke
                dataGridViewTasks.Invoke(new Action(() => SelectTaskById(taskId)));
            }
            else
            {
                // ������� ��� ���������� ���������
                dataGridViewTasks.ClearSelection();

                // �������� �� ������� dataGridViewTasks
                foreach (DataGridViewRow row in dataGridViewTasks.Rows)
                {
                    // ���������, ��� ������ �������� �������� TaskId � ��� �� null
                    if (row.Cells["TaskId"].Value != null)
                    {
                        // ������� �������� ID ������ �� ������
                        int currentTaskId;
                        bool isValidId = int.TryParse(row.Cells["TaskId"].Value.ToString(), out currentTaskId);

                        if (isValidId && currentTaskId == taskId)
                        {
                            // ���� ID ���������, �������� ������
                            row.Selected = true;
                            // ������������ DataGridView, ����� ��������� ������ ���� �����
                            dataGridViewTasks.FirstDisplayedScrollingRowIndex = row.Index;
                            break; // ����� ����� �� �����, ��� ��� ������ �������
                        }
                        // ���� ������ �� ID � ������ �����
                        OSTask selectedTask = tasksList.FirstOrDefault(task => task.Task_Id == taskId);

                        if (selectedTask != null)
                        {
                            labelCmds.Text = "ID ������: " + taskId;
                            // ���� ������ �������, ��������� ������� ������ ��� ���� ������
                            UpdateCommandsTable(selectedTask);
                        }
                        else
                        {
                            labelCmds.Text = "������ �� �������.";
                        }
                    }
                }
            }
        }


        public void SelectCmdById (int cmdId)
        {
            if (dataGridViewCommands.InvokeRequired)
            {
                // ���� ����� ���� �� ������� ������, ���������� Invoke
                dataGridViewTasks.Invoke(new Action(() => SelectTaskById(cmdId)));
            }
            else
            {
                // ������� ��� ���������� ���������
                dataGridViewCommands.ClearSelection();

                // �������� �� ������� dataGridViewTasks
                foreach (DataGridViewRow row in dataGridViewCommands.Rows)
                {
                    // ���������, ��� ������ �������� �������� TaskId � ��� �� null
                    if (row.Cells["Id"].Value != null)
                    {
                        // ������� �������� ID ������ �� ������
                        int currentCmdId;
                        bool isValidId = int.TryParse(row.Cells["Id"].Value.ToString(), out currentCmdId);

                        if (isValidId && currentCmdId == cmdId)
                        {
                            // ���� ID ���������, �������� ������
                            row.Selected = true;
                            // ������������ DataGridView, ����� ��������� ������ ���� �����
                            dataGridViewCommands.FirstDisplayedScrollingRowIndex = row.Index;
                            break; // ����� ����� �� �����, ��� ��� ������ �������
                        }
                    }
                }
            }
        }
    }
}
