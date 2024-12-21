using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectOS;
using static ProjectOS.MainWindow;

namespace ProjectOS
{
    public class CPU
    {
        // Выходные параметры процессора
        public int PC;          // Счетчик команд
        public int CurProc;     // Номер выполняемого процесса
        public bool Command;  // Выполняемая команда или ожидание
        public OS myOS;
        public MainWindow mw;

        // Конструктор для процессора
        public CPU (MainWindow mw)
        {
            PC = 0;
            CurProc = -1; // Нет текущего процесса
            Command = false;
            this.mw = mw;
            this.myOS = mw.myOS;
        }

        // Метод выполнения команд и управления процессами
        public void ExecuteCommand (Process process, int k)
        {
            // Записываем время начала выполнения в многозадачных тиках
            long startExecuted = myOS.T_multi;

            // Увеличиваем счетчик многозадачных тиков
            myOS.T_multi++;

            // Подсвечиваем выбранное задание в пользовательском интерфейсе по его ID
            mw.SelectTaskById(process.AssociatedTask.Task_Id);

            // Отмечаем, что команда сейчас выполняется
            this.Command = true;

            // Устанавливаем текущий ID процесса в процессоре
            this.CurProc = process.Process_Id;

            // Обновляем пользовательский интерфейс для отображения нового состояния процесса
            mw.RefreshInterface();

            // Обновляем таблицу команд в интерфейсе для связанного задания
            mw.UpdateCommandsTable(process.AssociatedTask);

            // Подсвечиваем выполняемую команду в интерфейсе
            mw.SelectCmdById(process.AssociatedTask.executedCmd);

            // Устанавливаем задачу для отслеживания статуса ввода/вывода
            mw.IOStatus = process.AssociatedTask;

            // Проверяем, есть ли еще команды для выполнения в текущем задании
            if (process.AssociatedTask.Commands.Count() > process.AssociatedTask.executedCmd)
            {
                // Увеличиваем счётчик программ для перехода к следующей команде
                PC++;

                // Создаём ссылки для удобства
                OSTask currentTask = process.AssociatedTask;
                Process currentProcess = process;

                // Логика обработки различных типов команд

                // Если команда связана с операцией ввода/вывода
                if (process.AssociatedTask.Commands[process.AssociatedTask.executedCmd] == CMD.IO)
                {
                    // Устанавливаем текущую длину команды для операции ввода/вывода
                    currentTask.currentCmd = currentTask.N_InOut;

                    // Увеличиваем счётчик программ для операций ввода/вывода
                    process.PC_IO++;

                    // Симулируем задержку ОС для изменения состояния процесса
                    mw.DelaySimulate(myOS.T_InitIO);

                    // Изменяем статус задачи, чтобы указать начало операции ввода/вывода
                    currentProcess.AssociatedTask.Status = CMD.IO_START;

                    // Обновляем интерфейс для отображения статуса задачи
                    mw.UpdateDataGridViewTasks();

                    // Сбрасываем текущий ID процесса и статус выполнения команды
                    this.CurProc = -1;
                    this.Command = false;

                    // Обновляем интерфейс для отображения изменений
                    mw.RefreshInterface();

                    // Запускаем асинхронную обработку команды ввода/вывода
                    mw.ProcessIOCommand(currentProcess);

                    // Завершаем метод, так как обработка ввода/вывода выполняется отдельно
                    return;
                }
                // Если команда связана с арифметическими операциями
                else if (process.AssociatedTask.Commands[process.AssociatedTask.executedCmd] == CMD.ARIFM)
                {
                    // Устанавливаем статус задачи как выполнение арифметической операции
                    currentTask.Status = CMD.ARIFM;

                    // Увеличиваем счётчик программ для процесса
                    process.PC++;

                    // Обновляем интерфейс для отображения нового статуса задачи
                    mw.UpdateDataGridViewTasks();

                    // Устанавливаем текущую длину команды
                    currentTask.currentCmd = currentTask.CMDLen;

                    // Назначаем процесс процессору для выполнения
                    this.CurProc = currentProcess.Process_Id;
                    this.Command = true;

                    // Обновляем интерфейс для отображения изменений
                    mw.RefreshInterface();

                    // Симулируем задержку ОС для изменения состояния процесса
                    mw.DelaySimulate(myOS.T_InitIO);

                    // Симулируем время, затраченное на выполнение команды
                    mw.DelaySimulate(process.AssociatedTask.currentCmd, process, k);
                }
                // Если команда указывает на завершение процесса
                else if (process.AssociatedTask.Commands[process.AssociatedTask.executedCmd] == CMD.END)
                {
                    // Симулируем задержку ОС для изменения состояния процесса
                    mw.DelaySimulate(myOS.T_InitIO);

                    // Помечаем статус задачи как завершённый
                    currentTask.Status = CMD.END;

                    // Переходим к следующей команде
                    process.AssociatedTask.executedCmd++;

                    // Обновляем интерфейс для отображения завершенного статуса
                    mw.UpdateDataGridViewTasks();

                    // Симулируем задержку ОС для обработки прерывания
                    mw.DelaySimulate(myOS.T_IntrIO);

                    // Ищем задачу в списке задач для удаления
                    var taskToRemove = mw.tasksList.FirstOrDefault(i => i.Task_Id == currentTask.Task_Id);
                    if (taskToRemove != null)
                    {
                        if (taskToRemove != null)
                        {
                            // Если задача найдена, удаляем её из списка
                            mw.tasksList.Remove(taskToRemove);
                        }
                        else
                        {
                            // Если задача не найдена, сбрасываем статус ввода/вывода и возвращаемся
                            mw.IOStatus = null;
                            return;
                        }

                        // Проверяем, что использование оперативной памяти больше или равно нулю
                        if (mw.usageRAM >= 0)
                        {
                            // Уменьшаем используемую оперативную память на объем текущей задачи
                            mw.usageRAM -= currentTask.V_task;
                        }

                        // Проверка состояния оперативной памяти
                        if (myOS.V_ozu != 0)
                        {
                            // Рассчитываем степень заполненности системы оперативной памяти в процентах
                            myOS.D_sys = Math.Round((100.0 / myOS.V_ozu) * mw.usageRAM, 2);
                        }
                        else
                        {
                            // Если объем оперативной памяти равен нулю, устанавливаем заполненность в ноль
                            myOS.D_sys = 0;
                        }

                        // Обновляем количество процессов и выполняем инкремент счётчика мультизадачности
                        myOS.N_Proc -= 1;
                        myOS.M_multi += 1;

                        // Обновляем интерфейс после изменения состояния
                        mw.RefreshInterface();
                        mw.SortTask();
                        mw.UpdateDataGridViewTasks();

                        // Добавляем время выполнения текущей задачи
                        currentTask.executedTime += myOS.T_multi - startExecuted;

                        // Сбрасываем текущий процесс и статус команды в центральном процессоре
                        mw.myCpu.CurProc = -1;
                        mw.myCpu.Command = false;

                        // Обновляем общее время оборота задачи
                        mw.myOS.T_obor = myOS.T_multi - currentTask.startTime;

                        // Увеличиваем общее время моно- и мультизадачности в тактах
                        mw.myOS.T_mono_all += currentTask.executedTime;
                        mw.myOS.T_multi_all += currentTask.executedTime;

                        // Проверяем, что время моно- и мультизадачности не равно нулю, чтобы избежать деления на ноль
                        if (myOS.T_mono_all != 0 && myOS.M_multi != 0)
                        {
                            // Рассчитываем производительность в режиме моно
                            myOS.M_mono = (int)((double)myOS.T_multi_all / ((double)myOS.T_mono_all / myOS.M_multi));
                        }
                        else
                        {
                            // Если данные некорректные, устанавливаем производительность моно в ноль
                            myOS.M_mono = 0;
                        }

                        // Рассчитываем производительность, если M_mono > 0
                        if (myOS.M_mono != 0)
                        {
                            myOS.D_multi = ((double)myOS.M_multi / myOS.M_mono) * 100.0;
                        }


                        Console.WriteLine("M_multi: " + mw.myOS.M_multi + " M_mono: " + mw.myOS.M_mono + " T_multi_all: " + mw.myOS.T_multi_all + " T_mono_all: " + mw.myOS.T_mono_all);
                        Console.WriteLine("D_multi: " + mw.myOS.D_multi);
                        mw.RefreshInterface();
                        mw.IOStatus = null;
                        return;
                    }
                    else
                    {
                        //MessageBox.Show("Некорректное удаление!");
                    }
                }
                //process.AssociatedTask.Commands.RemoveAt(0);
                process.AssociatedTask.executedCmd++;
                // Затраты ОС на изменение состояния процесса
                mw.DelaySimulate(myOS.T_InitIO);
                // Затраты ОС на обслуживание прерывания
                mw.DelaySimulate(myOS.T_IntrIO);
                currentTask.Status = CMD.WAIT;
                mw.UpdateDataGridViewTasks();
                mw.myCpu.CurProc = -1;
                mw.myCpu.Command = false;
                currentTask.executedTime += myOS.T_multi - startExecuted;
                mw.RefreshInterface();
                mw.IOStatus = null;
            }
        }
    }
}
