using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace ProjectOS
{
    public partial class MainWindow : Form
    {
        private bool System_Status = false;
        private Stopwatch stopwatch; // ������ Stopwatch ��� ������� �������
        private System.Timers.Timer displayTimer; // ������ ��� ���������� ����������


        // Random ���������
        Random random = new Random();

        private int taskIdCounter = 0; // ������� ID �����, ������� � 0

        public MainWindow ()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView ()
        {
            // ��������� ������� � DataGridView
            dataGridViewTasks.Columns.Add("TaskId", "ID ������");
            dataGridViewTasks.Columns.Add("VTask", "��� ������");
            dataGridViewTasks.Columns.Add("NCmnd", "����� ������");
            dataGridViewTasks.Columns.Add("DInOut", "�������� ��������");
            dataGridViewTasks.Columns.Add("NInOut", "����� �������� �����/������");
            dataGridViewTasks.Columns.Add("Prior", "���������");
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
            // �������� ��������� �������
            int NProcc = 0;     // ����� ����������� �������

            int DSys = 0;       // ��������� ������� �� (� ���������)

            int TMulti = 0;     // ����� ������ ������ �� � ������� �������

            int MMulti = 0;     // ����� ����������� ������� � �������
                                // ������ �������������

            int TObor = 0;      // ��������� �����

            int TMonoAll = 0;   // ����� ���������� M_multi ������� �
                                // ��������������� �������

            int MMono = 0;      // ����� �������, ������� ����� �� �����������
                                // �� ����� T_multi � ��������������� ��

            int DMulti = 0;     // ������������������ ������ �� �� ���������
                                // � ��������������� �� � ���������



            // ������� ��������� �������
            int OSRAM = 0;      // ������ ������ ������ ��

            int OSKvant = 0;    // ����� �������
                                // (����� ������ �������������,	Kvant
                                // ��������� �������� � ��������� ��������)

            int OSTNext = 0;    // ������� �� �� ����� �������� ��� ����������
                                // �� ����������(������ ������������� �������)

            int OSTInitIO = 0;  // ������� �� �� ��������� ��������� ��������
                                // �� ��������� �� �����(������) (� ����� ������)

            int OSTIntrIO = 0;  // ������� �� �� ������������ ������� 
                                // ���������(����������) �����(������)(� ����� ������)

            int OSTLoad = 0;    // ����� ������ �� �������� ������ ������� 

            int OSSpeed = 0;    // �������� ������ ������

            int OSTGlobl = 0;   // ������� �� �� ������� � ������ �������

            if (!System_Status)
            {
                if ((textBoxRAM.Text != "" && IsInteger(textBoxRAM.Text)) &&
                (textBoxKvant.Text != "" && IsInteger(textBoxKvant.Text)) &&
                (textBoxTNext.Text != "" && IsInteger(textBoxTNext.Text)) &&
                (textBoxTInitIO.Text != "" && IsInteger(textBoxTInitIO.Text)) &&
                (textBoxTIntrIO.Text != "" && IsInteger(textBoxTIntrIO.Text)) &&
                (textBoxTLoad.Text != "" && IsInteger(textBoxTLoad.Text)) &&
                (textBoxSpeed.Text != "" && IsInteger(textBoxSpeed.Text)) &&
                (textBoxTGlobl.Text != "" && IsInteger(textBoxTGlobl.Text)))
                {
                    // ��������� ���������� � ������ �������

                    OSRAM = int.Parse(textBoxRAM.Text);
                    OSKvant = int.Parse(textBoxKvant.Text);
                    OSTNext = int.Parse(textBoxTNext.Text);
                    OSTInitIO = int.Parse(textBoxTInitIO.Text);
                    OSTIntrIO = int.Parse(textBoxTIntrIO.Text);
                    OSTLoad = int.Parse(textBoxTLoad.Text);
                    OSSpeed = int.Parse(textBoxSpeed.Text);
                    OSTGlobl = int.Parse(textBoxTGlobl.Text);


                    System_Status = true;
                    stopwatch = Stopwatch.StartNew(); // ��������� Stopwatch

                    // ������ ��� ���������� ���������� ��� � 100 ��
                    displayTimer = new System.Timers.Timer(1);
                    displayTimer.Elapsed += UpdateTimeDisplay;
                    displayTimer.Start();


                    Task.Run(() => SimulateOS());
                }
                else
                {
                    MessageBox.Show("�� ����� ������������ ������!");
                    System_Status = false;
                }
            }
            else
            {
                MessageBox.Show("�� ��� ��������!");
            }

        }

        private void SimulateOS ()
        {
            while (System_Status)
            {

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
            }
        }

        private bool IsInteger (string lexeme)
        {
            // ����� ���������, �������� �� ������� ������
            return int.TryParse(lexeme, out int value);
        }

        private void buttonEndOS_Click (object sender, EventArgs e)
        {
            if (System_Status == false)
            {
                return;
            }
            System_Status = false;
            dataGridViewTasks.Rows.Clear();
            displayTimer.Stop();
        }

        private void buttonAddTask_Click (object sender, EventArgs e)
        {
            if (System_Status)
            {
                // ���������, ��� ��� ����������� ���� ���������, ����� ID, ������� ������������ �������������
                if (!string.IsNullOrWhiteSpace(textBoxVTask.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxNCmnd.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxNInOut.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxPrior.Text))
                {
                    // ���������� ���������� ID ������
                    string taskId = taskIdCounter.ToString();

                    // ��������� ������ � DataGridView
                    dataGridViewTasks.Rows.Add(
                        taskId,
                        textBoxVTask.Text,
                        textBoxNCmnd.Text,
                        trackBarDInOut.Value,
                        textBoxNInOut.Text,
                        textBoxPrior.Text
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
                        textBoxNCmnd.Text = random.Next(1, 50).ToString(); // ���������� ������
                        trackBarDInOut.Value = random.Next(0, 100); // ������� �������� ��������
                        textBoxNInOut.Text = random.Next(1, 10).ToString(); // ���������� �����/������
                        labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
                        textBoxPrior.Text = random.Next(1, 10).ToString(); // ��������� ������
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

        private void radioButtonAuto_Click (object sender, EventArgs e)
        {
            radioButtonManual.Checked = !radioButtonAuto.Checked;
            // ���������� ��������� ������
            textBoxVTask.Text = random.Next(1, 100).ToString(); // �������� ������
            textBoxNCmnd.Text = random.Next(1, 50).ToString(); // ���������� ������
            trackBarDInOut.Value = random.Next(0, 100); // ������� �������� ��������
            labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
            textBoxNInOut.Text = random.Next(1, 10).ToString(); // ���������� �����/������
            textBoxPrior.Text = random.Next(1, 10).ToString(); // ��������� ������
        }

        private void radioButtonManual_Click (object sender, EventArgs e)
        {
             radioButtonAuto.Checked = !radioButtonManual.Checked;
        }

    }
}
