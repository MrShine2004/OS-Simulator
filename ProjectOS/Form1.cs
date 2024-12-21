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
        public OSTask IOStatus = null;
        public int currentTaskId = 0;
        public OSTask executedTask = null;

        public Stopwatch stopwatch; // Объект Stopwatch для отсчёта времени
        public System.Windows.Forms.Timer uiTimer;
        public volatile bool System_Status = false;
        public long timeStart;
        public long timeExecutedTasks;
        private CancellationTokenSource _cancellationTokenSource;

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
            if (!System_Status)
            {
                taskIdCounter = 0;
                usageRAM = 0;
                currentTick = 0;
                myCpu.PC = 0;
                myOS.M_multi = 0;
                myOS.N_Proc = 0;
                myOS.T_obor = 0;
                myOS.T_multi_all = 0;
                myOS.T_mono_all = 0;
                myOS.D_multi = 0;
                myOS.M_mono = 0;
                myOS.T_multi = 0;
                RefreshInterface();

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


                    // Прекратить старую симуляцию (если она была)
                    if (_cancellationTokenSource != null)
                    {
                        _cancellationTokenSource.Cancel();
                        _cancellationTokenSource.Dispose();
                    }

                    _cancellationTokenSource = new CancellationTokenSource();

                    System_Status = true;
                    timeStart = myOS.T_multi; // Инициализация времени в тактах


                    Task.Run(() => TickSimulate());
                    Task.Run(() => SimulateOS(_cancellationTokenSource.Token));

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

        private void TickSimulate ()
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
                myOS.T_multi++;
                UpdateTimeDisplay();
            }
        }

        // Затраты
        public void DelaySimulate (int temp)
        {
            int tik = currentTick;
            while (currentTick - tik <= temp)
            {
                Thread.Sleep(myOS.Speed);
                myOS.T_multi++;
                UpdateTimeDisplay();
            }
        }
        
        // Затраты IO
        public int DelaySimulate (int temp, bool t, Process process)
        {
            int taktIO = 0;
            int tik = currentTick;
            while (currentTick - tik <= temp)
            {
                taktIO++;
                TaktUpIO(process.AssociatedTask);
                Thread.Sleep(myOS.Speed);
            }
            return taktIO;
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
                myOS.T_multi++;
                UpdateTimeDisplay();
            }
        }

        private void SimulateOS (CancellationToken token)
        {
            while (System_Status && !token.IsCancellationRequested)
            {
                // Если System_Status == false, выходим из цикла сразу
                if (!System_Status)
                {
                    _cancellationTokenSource.Cancel();
                    break;
                }


                if (loadStatus)
                {
                    SortTask();
                    loadStatus = false;
                }

                if (tasksList.Count > 0)
                {
                    // Затраты на выбор процесса для выполнения
                    DelaySimulate(myOS.T_next);

                    // Получаем первый элемент списка
                    OSTask? currentTask = null;

                    if (tasksList.Count > 0) // Проверка наличия задач в списке
                    {
                        int i = 0;
                        // Цикл для поиска первой задачи со статусом WAIT
                        while (i < tasksList.Count && tasksList[i].Status != CMD.WAIT)
                        {
                            i++; // Увеличение индекса для проверки следующей задачи
                        }
                        // Проверка, что индекс i не превышает количество задач и задача не равна null
                        if (i < tasksList.Count && tasksList[i] != null)
                            currentTask = tasksList[i]; // Присваивание текущей задачи
                    }

                    // Проверка, если текущая задача не установлена (null), пропустить текущую итерацию
                    if (currentTask == null)
                    {
                        continue;
                    }

                    // Инкапсулируем в процесс
                    Process currentProcess = new Process(currentTask.Task_Id, currentTask, currentTask.Prior);
                    currentTaskId = currentProcess.AssociatedTask.Task_Id;
                    executedTask = currentProcess.AssociatedTask;

                    // Затраты на загрузку нового задания
                    DelaySimulate(myOS.T_Load); // Имитация затрат времени на загрузку нового задания

                    // Цикл выполнения команд в пределах кванта времени
                    for (int k = 0; k < myOS.Kvant; k++)
                    {
                        // Проверка: если у текущего процесса есть команды для выполнения,
                        // процесс находится в состоянии ожидания (WAIT),
                        // и центральный процессор не занят другим процессом
                        if (
                            currentProcess.AssociatedTask.N_cmnd > 0 && // Проверка наличия оставшихся команд у текущего процесса
                            (currentProcess.AssociatedTask.Status == CMD.WAIT) && // Проверка, что процесс находится в состоянии WAIT
                            (myCpu.CurProc == -1) // Проверка, что центральный процессор свободен
                           )
                        {
                            // Выполнение команды процессора для текущего процесса
                            myCpu.ExecuteCommand(currentProcess, k);
                        }
                        else
                        {
                            // Прерывание цикла, если условия для выполнения команды не выполняются
                            break;
                        }
                    }

                    //myOS.T_multi_all += myOS.T_multi - timeStart; // Изменение на использование тактов

                    if (!System_Status)
                        break;
                }
                else
                {

                    if (token.IsCancellationRequested)
                    {
                        // Выполняем необходимые действия при отмене
                        Console.WriteLine("Симуляция завершена.");
                        break;
                    }
                    if (System_Status)
                    {
                        Thread.Sleep(myOS.Speed);
                        myOS.T_multi++;
                        UpdateTimeDisplay(); 
                        if (!System_Status)
                        {
                            _cancellationTokenSource.Cancel();
                            break;
                        }
                    }
                    if (!System_Status)
                    {
                        _cancellationTokenSource.Cancel();
                        break;
                    }
                }
            }
            _cancellationTokenSource.Cancel();
            return;
        }

        private DateTime lastUpdate = DateTime.MinValue;

        private void UpdateTimeDisplay ()
        {
            if (this.IsDisposed)
            {
                return;
            }

            // Ограничение частоты обновления интерфейса (например, не чаще раза в 100 мс)
            if ((DateTime.Now - lastUpdate).TotalMilliseconds < 100)
            {
                return;
            }

            lastUpdate = DateTime.Now;

            if (labelTimeElapsed.InvokeRequired)
            {
                labelTimeElapsed.BeginInvoke(new Action(() => labelTimeElapsed.Text = "Время работы ОС: " + myOS.T_multi + " тактов"));
            }
            else
            {
                labelTimeElapsed.Text = "Время работы ОС: " + myOS.T_multi + " тактов";
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
            // Проверяем, что в списке задач больше одного элемента
            if (tasksList.Count() > 1)
            {
                // Сортируем список задач:
                // 1. Убираем все элементы, которые равны null
                // 2. Сортируем по убыванию приоритета задачи (чем больше Prior, тем выше приоритет)
                // 3. Если приоритеты равны, сортируем по возрастанию Task_Id (чем меньше ID, тем раньше в списке)
                tasksList = tasksList
                    .Where(t => t != null)  // Отфильтровываем элементы, которые не являются null
                    .OrderByDescending(t => t.Prior)  // Сортировка по убыванию приоритета
                    .ThenBy(t => t.Task_Id)  // Сортировка по возрастанию Task_Id при равных приоритетах
                    .ToList();  // Преобразование в список
            }
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

                // Создаем временный список для хранения новых задач
                var newTasks = new List<OSTask>();

                // Обновляем или добавляем задачи в таблицу
                foreach (OSTask task in tasksList.ToList())
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

                    // Если задачи не было, добавляем её во временный список
                    if (!taskFound)
                    {
                        newTasks.Add(task);
                    }
                }

                // Добавляем новые задачи в DataGridView после завершения итерации
                foreach (var task in newTasks)
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
                labelDmulti.Text = "Производительность \nпо сравнению: " + myOS.D_multi.ToString("F2") + "%" + " = " + myOS.M_multi + "/" + myOS.M_mono;
                labelTobor.Text = "Время между задачами: " + myOS.T_obor + " тактов";
                labelPC.Text = "Счётчик комманд: " + myCpu.PC;
                UpdateTimeDisplay();
            }
        }



        private void buttonEndOS_Click (object sender, EventArgs e)
        {
            if (System_Status)
            {
                System_Status = false;
                myCpu.CurProc = -1;
                myCpu.Command = false;
                dataGridViewTasks.Rows.Clear();
                RefreshInterface();
                tasksList.Clear();

                // Отменяем текущий асинхронный процесс
                _cancellationTokenSource?.Cancel();
                System_Status = false;
                // Остановить таймер обновления интерфейса
                if (uiTimer != null)
                {
                    uiTimer.Stop();
                }

                textBoxRAM.ReadOnly = false;
                textBoxKvant.ReadOnly = false;
                textBoxTNext.ReadOnly = false;
                textBoxTInitIO.ReadOnly = false;
                textBoxTIntrIO.ReadOnly = false;
                textBoxTLoad.ReadOnly = false;
                textBoxSpeed.ReadOnly = false;

                taskIdCounter = 0;
                usageRAM = 0;
                currentTick = 0;
                myCpu.PC = 0;
                myOS.M_multi = 0;
                myOS.N_Proc = 0;
                myOS.T_obor = 0;
                myOS.T_multi_all = 0;
                myOS.T_mono_all = 0;
                myOS.D_multi = 0;
                myOS.M_mono = 0;
                myOS.T_multi = 0;
                return;
            }
        }

        private void buttonAddTask_Click (object sender, EventArgs e)
        {
            if (System_Status)
            {
                if (tasksList.Count != 0 && timeExecutedTasks == 0)
                {
                    Console.WriteLine("" + tasksList.Count + " 1 " + timeExecutedTasks);
                    timeExecutedTasks = myOS.T_multi;
                }

                if (tasksList.Count == 0 && timeExecutedTasks != 0)
                {
                    Console.WriteLine("" + tasksList.Count + " 2 " + timeExecutedTasks);
                    timeExecutedTasks = 0;
                }
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
                        trackBarPriority.Value,
                        myOS.T_multi);

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

        // Метод для симуляции выполнения команды ввода/вывода
        // Принимает процесс, процессор и количество операций ввода/вывода
        private int SimulateIOCommand (Process process, CPU myCpu, int N_InOut)
        {
            int taktIO = 0; // Счетчик тактов ввода/вывода
            process.AssociatedTask.currentCmd = process.AssociatedTask.N_InOut; // Устанавливаем текущую команду как число операций ввода/вывода для задачи

            // Цикл выполняется до тех пор, пока не пройдет заданное количество тактов
            while (taktIO < process.AssociatedTask.N_InOut)
            {
                taktIO++; // Увеличиваем счетчик тактов
                TaktUpIO(process.AssociatedTask); // Выполняем шаг симуляции ввода/вывода для текущей задачи
                Thread.Sleep(myOS.Speed); // Задержка для имитации временной паузы в работе ОС
                process.AssociatedTask.currentCmd--; // Уменьшаем количество оставшихся команд для выполнения
                UpdateDataGridViewTasks(); // Обновляем отображение данных в таблице задач
            }
            return taktIO; // Возвращаем общее количество тактов, затраченных на симуляцию
        }

        // Метод для обработки команды ввода/вывода
        // Принимает текущий процесс для выполнения команды
        public async Task ProcessIOCommand (Process currentProcess)
        {
            int executedTime = 0; // Инициализация времени выполнения
            OSTask currentTask = currentProcess.AssociatedTask; // Получение связанной задачи из текущего процесса

            // Выполняем асинхронную обработку команды ввода/вывода
            await Task.Run(() =>
            {
                OSTask currentTask = currentProcess.AssociatedTask; // Получаем текущую задачу
                OSTask task = currentProcess.AssociatedTask; // Дублируем ссылку на задачу

                // Проверяем, не занята ли операция ввода/вывода и устанавливаем статус
                if (IOStatus == null)
                    IOStatus = currentTask;

                currentTask.Status = CMD.IO; // Устанавливаем статус задачи на выполнение команды ввода/вывода
                UpdateDataGridViewTasks(); // Обновляем интерфейс для отображения изменений

                // Выполняем симуляцию команды ввода/вывода и обновляем время выполнения
                executedTime += SimulateIOCommand(currentProcess, myCpu, task.N_InOut);

                currentTask.Status = CMD.IO_END; // Устанавливаем статус окончания выполнения команды
                UpdateDataGridViewTasks(); // Обновляем интерфейс

                currentTask.IO_cmnd--; // Уменьшаем счетчик команд ввода/вывода
                currentTask.N_cmnd--; // Уменьшаем общее количество оставшихся команд

                currentProcess.AssociatedTask.executedCmd++; // Увеличиваем счетчик выполненных команд

                // Устанавливаем статус ожидания для текущей задачи после завершения команды
                currentTask.Status = CMD.WAIT;
                UpdateDataGridViewTasks(); // Обновляем интерфейс

                //currentTask.executedTime += executedTime; // Добавляем время выполнения к общему времени задачи
                myOS.T_mono_all += executedTime; // Увеличиваем общее время выполнения моноопераций

                // Сбрасываем статус ввода/вывода
                IOStatus = null;
            });
        }

        // Метод для обновления статуса выполнения ввода/вывода
        // Принимает текущую задачу, чтобы обновить её состояние
        public void TaktUpIO (OSTask curTask)
        {
            // Проверка, является ли текущая задача активной для выполнения ввода/вывода
            if (IOStatus == curTask && IOStatus != null)

            {
                myOS.T_multi_all++;
                Console.WriteLine(myOS.T_multi_all + " " + curTask.Task_Id);
            }
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
