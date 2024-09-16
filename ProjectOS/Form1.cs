using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace ProjectOS
{
    public partial class MainWindow : Form
    {
        private bool System_Status = false;
        private Stopwatch stopwatch; // Объект Stopwatch для отсчёта времени
        private System.Timers.Timer displayTimer; // Таймер для обновления интерфейса


        // Random генератор
        Random random = new Random();

        private int taskIdCounter = 0; // Счётчик ID задач, начиная с 0

        public MainWindow ()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView ()
        {
            // Добавляем колонки в DataGridView
            dataGridViewTasks.Columns.Add("TaskId", "ID задачи");
            dataGridViewTasks.Columns.Add("VTask", "Тип задачи");
            dataGridViewTasks.Columns.Add("NCmnd", "Число команд");
            dataGridViewTasks.Columns.Add("DInOut", "Дисковые операции");
            dataGridViewTasks.Columns.Add("NInOut", "Число операций ввода/вывода");
            dataGridViewTasks.Columns.Add("Prior", "Приоритет");
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

        // Запуск СИСТЕМЫ
        private void buttonStartOS_Click (object sender, EventArgs e)
        {
            taskIdCounter = 0;
            // Выходные параметры СИСТЕМЫ
            int NProcc = 0;     // число загруженных заданий

            int DSys = 0;       // системные затраты ОС (в процентах)

            int TMulti = 0;     // время работы модели ОС с момента запуска

            int MMulti = 0;     // число выполненных заданий с момента
                                // начала моделирования

            int TObor = 0;      // оборотное время

            int TMonoAll = 0;   // время выполнения M_multi заданий в
                                // однопрограммной системе

            int MMono = 0;      // число заданий, которые могли бы выполниться
                                // за время T_multi в однопрограммной ОС

            int DMulti = 0;     // производительность модели ОС по сравнению
                                // с однопрограммной ОС в процентах



            // Входные параметры СИСТЕМЫ
            int OSRAM = 0;      // размер памяти модели ОС

            int OSKvant = 0;    // квант времени
                                // (число тактов моделирования,	Kvant
                                // доступных процессу в состоянии «Активен»)

            int OSTNext = 0;    // затраты ОС на выбор процесса для выполнения
                                // на процессоре(тактов моделируемого времени)

            int OSTInitIO = 0;  // затраты ОС на изменение состояния процесса
                                // по обращению ко вводу(выводу) (в числе тактов)

            int OSTIntrIO = 0;  // затраты ОС по обслуживанию сигнала 
                                // окончания(прерывания) ввода(вывода)(в числе тактов)

            int OSTLoad = 0;    // число тактов на загрузку нового задания 

            int OSSpeed = 0;    // скорость работы модели

            int OSTGlobl = 0;   // затраты ОС на общение с общими данными

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
                    // Получение параметров и запуск системы

                    OSRAM = int.Parse(textBoxRAM.Text);
                    OSKvant = int.Parse(textBoxKvant.Text);
                    OSTNext = int.Parse(textBoxTNext.Text);
                    OSTInitIO = int.Parse(textBoxTInitIO.Text);
                    OSTIntrIO = int.Parse(textBoxTIntrIO.Text);
                    OSTLoad = int.Parse(textBoxTLoad.Text);
                    OSSpeed = int.Parse(textBoxSpeed.Text);
                    OSTGlobl = int.Parse(textBoxTGlobl.Text);


                    System_Status = true;
                    stopwatch = Stopwatch.StartNew(); // Запускаем Stopwatch

                    // Таймер для обновления интерфейса раз в 100 мс
                    displayTimer = new System.Timers.Timer(1);
                    displayTimer.Elapsed += UpdateTimeDisplay;
                    displayTimer.Start();


                    Task.Run(() => SimulateOS());
                }
                else
                {
                    MessageBox.Show("Вы ввели некорректные данные!");
                    System_Status = false;
                }
            }
            else
            {
                MessageBox.Show("ОС уже запущена!");
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
            // Используем Stopwatch для получения прошедшего времени
            TimeSpan time = stopwatch.Elapsed;

            // Форматируем время: часы, минуты, секунды, миллисекунды
            string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                time.Hours, time.Minutes, time.Seconds, time.Milliseconds);

            // Обновляем отображение времени работы ОС
            if (labelTimeElapsed.InvokeRequired)
            {
                labelTimeElapsed.BeginInvoke(new Action(() => labelTimeElapsed.Text = "Время работы ОС: " + timeFormatted));
            }
            else
            {
                labelTimeElapsed.Text = "Время работы ОС: " + timeFormatted;
            }
        }

        private bool IsInteger (string lexeme)
        {
            // Метод проверяет, является ли лексема числом
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
                // Проверяем, что все необходимые поля заполнены, кроме ID, который генерируется автоматически
                if (!string.IsNullOrWhiteSpace(textBoxVTask.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxNCmnd.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxNInOut.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxPrior.Text))
                {
                    // Генерируем уникальный ID задачи
                    string taskId = taskIdCounter.ToString();

                    // Добавляем данные в DataGridView
                    dataGridViewTasks.Rows.Add(
                        taskId,
                        textBoxVTask.Text,
                        textBoxNCmnd.Text,
                        trackBarDInOut.Value,
                        textBoxNInOut.Text,
                        textBoxPrior.Text
                    );

                    // Увеличиваем счётчик ID для следующей задачи
                    taskIdCounter++;

                    // Очищаем текстовые поля после добавления
                    //textBoxVTask.Clear();
                    //textBoxNCmnd.Clear();
                    //textBoxDInOut.Clear();
                    //textBoxNInOut.Clear();
                    //textBoxPrior.Clear();

                    if (radioButtonAuto.Checked) // Если выбрана автоматическая генерация
                    {
                        // Генерируем случайные данные
                        textBoxVTask.Text = random.Next(1, 100).ToString(); // Величина задачи
                        textBoxNCmnd.Text = random.Next(1, 50).ToString(); // Количество команд
                        trackBarDInOut.Value = random.Next(0, 100); // Процент дисковых операций
                        textBoxNInOut.Text = random.Next(1, 10).ToString(); // Количество ввода/вывода
                        labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
                        textBoxPrior.Text = random.Next(1, 10).ToString(); // Приоритет задачи
                    }

                }
                else
                {
                    MessageBox.Show("Заполните все поля для добавления задачи!");
                }
            }
            else
            {
                MessageBox.Show("ОС не запущена!");
            }
        }


        private void trackBarDInOut_Scroll (object sender, EventArgs e)
        {
            labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
        }

        private void radioButtonAuto_Click (object sender, EventArgs e)
        {
            radioButtonManual.Checked = !radioButtonAuto.Checked;
            // Генерируем случайные данные
            textBoxVTask.Text = random.Next(1, 100).ToString(); // Величина задачи
            textBoxNCmnd.Text = random.Next(1, 50).ToString(); // Количество команд
            trackBarDInOut.Value = random.Next(0, 100); // Процент дисковых операций
            labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
            textBoxNInOut.Text = random.Next(1, 10).ToString(); // Количество ввода/вывода
            textBoxPrior.Text = random.Next(1, 10).ToString(); // Приоритет задачи
        }

        private void radioButtonManual_Click (object sender, EventArgs e)
        {
             radioButtonAuto.Checked = !radioButtonManual.Checked;
        }

    }
}
