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
        public int currentTaskId = 0;
        public OSTask executedTask = null;

        public bool System_Status = false;
        public Stopwatch stopwatch; // Объект Stopwatch для отсчёта времени
        public System.Windows.Forms.Timer uiTimer;
        public long timeStart;

        // Random генератор
        Random random = new Random();

        private int taskIdCounter = 0; // Счётчик ID задач, начиная с 0


        public MainWindow ()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeCommandsGridView();
            myCpu = new CPU(this);
        }
        private void InitializeDataGridView ()
        {
            // Добавляем колонки в DataGridView
            dataGridViewTasks.Columns.Add("TaskId", "ID");
            dataGridViewTasks.Columns.Add("VTask", "Размер");
            dataGridViewTasks.Columns.Add("NCmnd", "Число команд");
            dataGridViewTasks.Columns.Add("DInOut", "Процент команд ввода/вывода");
            dataGridViewTasks.Columns.Add("NInOut", "Длительность команд ввода/вывода");
            dataGridViewTasks.Columns.Add("Prior", "Приоритет");
            dataGridViewTasks.Columns.Add("Status", "Статус");
            dataGridViewTasks.Columns.Add("Takt", "Такт");

            // Установка ширины для каждого столбца (в пикселях)
            dataGridViewTasks.Columns[0].Width = 40;  // Ширина для столбца с Task_Id
            dataGridViewTasks.Columns[1].Width = 100; // Ширина для столбца с V_task
            dataGridViewTasks.Columns[2].Width = 80;  // Ширина для столбца с N_cmnd
            dataGridViewTasks.Columns[3].Width = 100; // Ширина для столбца с D_InOut
            dataGridViewTasks.Columns[4].Width = 100; // Ширина для столбца с N_InOut
            dataGridViewTasks.Columns[5].Width = 80;  // Ширина для столбца с Prior
            dataGridViewTasks.Columns[6].Width = 90;  // Ширина для столбца с Status
            dataGridViewTasks.Columns[7].Width = 50;  // Ширина для столбца с Takt
        }
        private void InitializeCommandsGridView ()
        {
            // Добавляем колонки в DataGridView
            dataGridViewCommands.Columns.Add("Num", "№");
            dataGridViewCommands.Columns.Add("Type", "Тип");

            // Установка ширины для каждого столбца (в пикселях)
            dataGridViewCommands.Columns[0].Width = 40;  // Ширина для столбца с Task_Id
            dataGridViewCommands.Columns[1].Width = 70; // Ширина для столбца с V_task
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
                    // Получение параметров и запуск системы

                    myOS.V_ozu = int.Parse(textBoxRAM.Text);
                    myOS.Kvant = int.Parse(textBoxKvant.Text);
                    myOS.T_next = int.Parse(textBoxTNext.Text);
                    myOS.T_InitIO = int.Parse(textBoxTInitIO.Text);
                    myOS.T_IntrIO = int.Parse(textBoxTIntrIO.Text);
                    myOS.T_Load = int.Parse(textBoxTLoad.Text);
                    myOS.Speed = int.Parse(textBoxSpeed.Text);


                    System_Status = true;
                    stopwatch = Stopwatch.StartNew(); // Запускаем Stopwatch

                    // Таймер для обновления интерфейса раз в 100 мс
                    myOS.T_multi = new System.Timers.Timer(100);
                    myOS.T_multi.Elapsed += UpdateTimeDisplay;
                    myOS.T_multi.Start();


                    Task.Run(() => TickSimulate());
                    Task.Run(() => SimulateOS());
                    // Таймер для обновления интерфейса раз в 100 мс
                    uiTimer = new System.Windows.Forms.Timer();
                    uiTimer.Interval = 1; // 100 мс
                    uiTimer.Tick += (sender, e) => RefreshInterface();
                    uiTimer.Start();
                }
                else
                {
                    MessageBox.Show("Вы ввели некорректные данные!");
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
                MessageBox.Show("ОС уже запущена!");
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

        // Затраты
        public void DelaySimulate (int temp, Process process, int k)
        {
            int tik = currentTick;
            int temp2 = temp;
            process.AssociatedTask.currentCmd = process.AssociatedTask.CMDLen;
            while (currentTick - tik <= temp2 && process.AssociatedTask.currentCmd > 0)
            {
                Thread.Sleep(myOS.Speed);
                process.AssociatedTask.currentCmd--;
                UpdateDataGridViewTasks();
            }
        }

        // Затраты
        public void DelaySimulate (int temp)
        {
            int tik = currentTick;
            while (currentTick - tik <= temp)
            {
                Thread.Sleep(myOS.Speed);
            }
        }

        // Затраты
        public void DelaySimulate (int temp, Process process)
        {
            int tik = currentTick;
            process.AssociatedTask.currentCmd = process.AssociatedTask.N_InOut;
            while (currentTick - tik <= temp && process.AssociatedTask.currentCmd > 0)
            {
                Thread.Sleep(myOS.Speed);
                process.AssociatedTask.currentCmd--;
                UpdateDataGridViewTasks();
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
                //DelaySimulate(myOS.Speed);   // Скорость работы симуляции
                if (tasksList.Count != 0)
                {
                    timeStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    // Затраты на выбор процесса для выполнения
                    DelaySimulate(myOS.T_next);

                    // Получаем первый элемент списка
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
                    // Инкапсулируем в процесс
                    Process currentProcess = new Process(currentTask.Task_Id, currentTask, currentTask.Prior);
                    currentTaskId = currentTask.Task_Id;
                    executedTask = currentTask;

                    // Затраты на загрузку нового задания
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
                labelTick.Text = "Тик: " + currentTick;
            }
        }

        private bool IsInteger (string lexeme)
        {
            // Метод проверяет, является ли лексема числом
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
                .Where(t => t != null)  // Отфильтровываем элементы, которые не являются null
                .OrderByDescending(t => t.Prior)
                .ThenBy(t => t.Task_Id)
                .ToList();

        }

        // Функция для обновления DataGridView
        public void UpdateDataGridViewTasks ()
        {
            if (dataGridViewTasks.InvokeRequired)
            {
                // Если вызов идет не из основного потока, используем Invoke
                dataGridViewTasks.Invoke(new Action(UpdateDataGridViewTasks));
            }
            else
            {
                // Отключаем обновление интерфейса для оптимизации
                dataGridViewTasks.SuspendLayout();

                // Сначала удаляем строки, если задачи больше нет в списке tasksList
                for (int i = dataGridViewTasks.Rows.Count - 1; i >= 0; i--)
                {
                    var cellValue = dataGridViewTasks.Rows[i].Cells["TaskId"].Value;

                    // Проверяем, есть ли значение в ячейке TaskId
                    if (cellValue == null || !(cellValue is int taskId))
                    {
                        // Пропускаем строки с пустыми или некорректными значениями TaskId
                        continue;
                    }

                    // Удаляем строку, если задачи с таким TaskId нет в tasksList
                    if (!tasksList.Any(task => task.Task_Id == taskId))
                    {
                        dataGridViewTasks.Rows.RemoveAt(i);
                    }
                }

                // Обновляем или добавляем задачи в таблицу
                foreach (OSTask task in tasksList)
                {
                    bool taskFound = false;

                    // Проверяем, есть ли уже строка с таким TaskId
                    foreach (DataGridViewRow row in dataGridViewTasks.Rows)
                    {
                        if (row.Cells["TaskId"].Value != null && (int)row.Cells["TaskId"].Value == task.Task_Id)
                        {
                            // Обновляем данные существующей строки, проверяя значения на null
                            row.Cells["VTask"].Value = task.V_task;
                            row.Cells["NCmnd"].Value = task.N_cmnd;
                            row.Cells["DInOut"].Value = task.D_InOut;
                            row.Cells["NInOut"].Value = task.N_InOut;
                            row.Cells["Prior"].Value = task.Prior;
                            row.Cells["Status"].Value = task.Status;
                            row.Cells["Takt"].Value = task.currentCmd;

                            taskFound = true;
                            break;
                        }
                    }

                    // Если задачи не было, добавляем её как новую строку
                    if (!taskFound)
                    {
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

                // Включаем обновление интерфейса обратно
                dataGridViewTasks.ResumeLayout();
            }
        }




        // Функция для обновления DataGridView
        public void UpdateDataGridViewTasks (int ID)
        {
            if (dataGridViewTasks.InvokeRequired)
            {
                // Если вызов идет не из основного потока, используем Invoke
                dataGridViewTasks.Invoke(new Action(() => UpdateDataGridViewTasks(ID)));
            }
            else
            {
                // Очищаем текущие строки в таблице
                dataGridViewTasks.Rows.Clear();

                // Проходим по списку задач и добавляем их в таблицу
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

        public void UpdateCommandsTable (OSTask task)
        {
            if (dataGridViewCommands.InvokeRequired)
            {
                // Если вызов идет не из основного потока, используем Invoke с аргументом process
                dataGridViewCommands.Invoke(new Action(() => UpdateCommandsTable(task)));
            }
            else
            {
                // Очищаем текущие строки в таблице
                dataGridViewCommands.Rows.Clear();

                // Проходим по списку задач и добавляем их в таблицу
                for (int i = 0; i < task.Commands.Count; i++)
                {
                    dataGridViewCommands.Rows.Add(
                        i,
                        task.Commands[i]
                    );
                }
            }
        }

        public void RefreshInterface ()
        {
            if (InvokeRequired)
            {
                // Если мы находимся не в основном потоке, вызываем Invoke, чтобы переключиться на основной поток
                Invoke(new Action(RefreshInterface));
            }
            else
            {
                // Здесь мы уже находимся в основном потоке, поэтому можно безопасно обновлять интерфейс
                if (myCpu.Command)
                    panelCPUCommand.BackColor = Color.Green;
                else panelCPUCommand.BackColor = Color.Red;
                labelUsageRAM.Text = "Используемая память: " + usageRAM + " Кб";
                if (myCpu.CurProc == -1)
                {
                    labelPerformedTask.Text = "Выполняемая задача: ";
                }
                else
                {
                    labelPerformedTask.Text = "Выполняемая задача: " + myCpu.CurProc;
                }
                labelNProc.Text = "Число загруженных заданий: " + myOS.N_Proc;
                labelMmulty.Text = "Выполненных заданий: " + myOS.M_multi;
                labelDsys.Text = "Системные затраты ОС (память): " + myOS.D_sys + "%";
                labelTobor.Text = "Время между задачами: " + myOS.T_obor + " мс";
                labelPC.Text = "Счётчик комманд: " + myCpu.PC;
                labelTick.Text = "Тик: " + currentTick;
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
                // Проверяем, что все необходимые поля заполнены, кроме ID, который генерируется автоматически
                if (!string.IsNullOrWhiteSpace(textBoxVTask.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxNCmnd.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxNInOut.Text))
                {
                    // Генерируем уникальный ID задачи
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

                        // Добавляем данные в DataGridView
                        dataGridViewTasks.Rows.Add(
                            newTask.Task_Id,
                            newTask.V_task,
                            newTask.N_cmnd,
                            newTask.D_InOut,
                            newTask.N_InOut,
                            newTask.Prior,
                            newTask.Status
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
                            textBoxNCmnd.Text = random.Next(2, 50).ToString(); // Количество команд
                            trackBarDInOut.Value = random.Next(0, 100); // Процент дисковых операций
                            textBoxNInOut.Text = random.Next(150, 350).ToString(); // Количество ввода/вывода
                            labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
                            trackBarPriority.Value = random.Next(1, 10); // Приоритет задачи
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
                        MessageBox.Show("Недостаточно памяти!");
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

        private void trackBarPrior_Scroll (object sender, EventArgs e)
        {
            labelPriority.Text = "" + trackBarPriority.Value;
        }

        private void radioButtonAuto_Click (object sender, EventArgs e)
        {
            radioButtonManual.Checked = !radioButtonAuto.Checked;
            // Генерируем случайные данные
            textBoxVTask.Text = random.Next(1, 100).ToString(); // Величина задачи
            textBoxNCmnd.Text = random.Next(2, 50).ToString(); // Количество команд
            trackBarDInOut.Value = random.Next(0, 100); // Процент дисковых операций
            textBoxNInOut.Text = random.Next(150, 350).ToString(); // Количество ввода/вывода
            labelTrackBarLocation.Text = "" + trackBarDInOut.Value + "%";
            trackBarPriority.Value = random.Next(1, 10); // Приоритет задачи
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
                    // Получение параметров и запуск системы

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
                    MessageBox.Show("Вы ввели некорректные данные!");
                }
            }
            else
            {
                MessageBox.Show("ОС не запущена!");
            }
        }

        // Метод для симуляции ввода/вывода
        private void SimulateIOCommand (Process process, CPU myCpu, int N_InOut)
        {
            process.D_ready += myOS.T_IntrIO; // Увеличиваем время ожидания
            process.AssociatedTask.IO_cmnd--;
            DelaySimulate(process.AssociatedTask.N_InOut, process);
            //UpdateDataGridViewTasks();
        }

        // Асинхронная обработка команды ввода/вывода
        public async Task ProcessIOCommand (Process currentProcess)
        {
            OSTask currentTask = currentProcess.AssociatedTask;
            await Task.Run(() =>
            {
                // Обрабатываем команду ввода/вывода
                OSTask currentTask = currentProcess.AssociatedTask;
                OSTask task = currentProcess.AssociatedTask;

                // Выполнение команды ввода/вывода
                currentTask.Status = CMD.IO;
                UpdateDataGridViewTasks();
                SimulateIOCommand(currentProcess, myCpu, task.N_InOut);

                currentTask.Status = CMD.IO_END;
                UpdateDataGridViewTasks();
                // Затраты на обслуживание прерывания ввода/вывода
                DelaySimulate(myOS.T_IntrIO);
                currentTask.IO_cmnd--;
                currentTask.N_cmnd--;


                //currentProcess.AssociatedTask.Commands.RemoveAt(0);
                currentProcess.AssociatedTask.executedCmd++;
                // Затраты ОС на изменение состояния процесса
                DelaySimulate(myOS.T_InitIO);
                currentTask.Status = CMD.WAIT;
                UpdateDataGridViewTasks();
                // Затраты ОС на обслуживание прерывания
                DelaySimulate(myOS.T_IntrIO);
            });
        }

        // Обработчик события CellClick
        private void dataGridViewTasks_CellClick (object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что кликнули на строку, а не на заголовок
            if (e.RowIndex >= 0)
            {
                // Получаем строку, на которую кликнули
                DataGridViewRow row = dataGridViewTasks.Rows[e.RowIndex];

                // Проверяем, существует ли колонка с именем "TaskId" и не пусто ли значение
                if (row.Cells["TaskId"] != null && row.Cells["TaskId"].Value != null)
                {
                    // Пробуем получить ID задачи из ячейки
                    int taskId;
                    bool isValidId = int.TryParse(row.Cells["TaskId"].Value.ToString(), out taskId);

                    if (isValidId)
                    {
                        // Ищем задачу по ID в списке задач
                        OSTask selectedTask = tasksList.FirstOrDefault(task => task.Task_Id == taskId);

                        if (selectedTask != null)
                        {
                            labelCmds.Text = "ID задачи: " + taskId;
                            // Если задача найдена, обновляем таблицу команд для этой задачи
                            UpdateCommandsTable(selectedTask);
                        }
                        else
                        {
                            labelCmds.Text = "Задача не найдена.";
                        }
                    }
                    else
                    {
                        labelCmds.Text = "Неверный ID задачи.";
                    }
                }
                else
                {
                    labelCmds.Text = "ID задачи отсутствует.";
                }
            }
        }

        public void SelectTaskById (int taskId)
        {
            if (dataGridViewTasks.InvokeRequired)
            {
                // Если вызов идет из другого потока, используем Invoke
                dataGridViewTasks.Invoke(new Action(() => SelectTaskById(taskId)));
            }
            else
            {
                // Очищаем все предыдущие выделения
                dataGridViewTasks.ClearSelection();

                // Проходим по строкам dataGridViewTasks
                foreach (DataGridViewRow row in dataGridViewTasks.Rows)
                {
                    // Проверяем, что строка содержит значение TaskId и оно не null
                    if (row.Cells["TaskId"].Value != null)
                    {
                        // Пробуем получить ID задачи из ячейки
                        int currentTaskId;
                        bool isValidId = int.TryParse(row.Cells["TaskId"].Value.ToString(), out currentTaskId);

                        if (isValidId && currentTaskId == taskId)
                        {
                            // Если ID совпадает, выбираем строку
                            row.Selected = true;
                            // Прокручиваем DataGridView, чтобы выбранная строка была видна
                            dataGridViewTasks.FirstDisplayedScrollingRowIndex = row.Index;
                            break; // Можно выйти из цикла, так как задача найдена
                        }
                        // Ищем задачу по ID в списке задач
                        OSTask selectedTask = tasksList.FirstOrDefault(task => task.Task_Id == taskId);

                        if (selectedTask != null)
                        {
                            labelCmds.Text = "ID задачи: " + taskId;
                            // Если задача найдена, обновляем таблицу команд для этой задачи
                            UpdateCommandsTable(selectedTask);
                            SelectCmdById(selectedTask.executedCmd);
                        }
                        else
                        {
                            labelCmds.Text = "Задача не найдена.";
                        }
                    }
                }
            }
        }

        public void SelectCmdById (int cmdId)
        {
            if (dataGridViewCommands.InvokeRequired)
            {
                // Если вызов идет из другого потока, используем Invoke
                dataGridViewCommands.Invoke(new Action(() => SelectCmdById(cmdId)));
            }
            else
            {
                // Очищаем все предыдущие выделения
                dataGridViewCommands.ClearSelection();

                // Проходим по строкам dataGridViewCommands
                foreach (DataGridViewRow row in dataGridViewCommands.Rows)
                {
                    // Проверяем, что строка содержит значение Num и оно не null
                    if (row.Cells["Num"].Value != null)
                    {
                        // Пробуем получить ID задачи из ячейки
                        int currentCmdId;
                        bool isValidId = int.TryParse(row.Cells["Num"].Value.ToString(), out currentCmdId);

                        if (isValidId && currentCmdId == cmdId)
                        {
                            // Если ID совпадает, выбираем строку
                            row.Selected = true;
                            // Прокручиваем DataGridView, чтобы выбранная строка была видна
                            if (row.Index > 4)
                                dataGridViewCommands.FirstDisplayedScrollingRowIndex = row.Index - 5;
                            else
                                dataGridViewCommands.FirstDisplayedScrollingRowIndex = 0;
                            break; // Можно выйти из цикла, так как задача найдена
                        }
                    }
                }
            }
        }
    }
}
