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

        // Методы для выполнения команд и управления процессами
        public void ExecuteCommand(Process process, int k)
        {
            this.Command = true;
            this.CurProc = process.Process_Id;
            CMD executeCmd = process.AssociatedTask.Commands[process.AssociatedTask.currentCmd];
            mw.SelectCmdById(process.AssociatedTask.currentCmd);
            mw.RefreshInterface();
            if(process.AssociatedTask.Commands.Count > process.AssociatedTask.currentCmd)
            {
                PC++;
                OSTask currentTask = process.AssociatedTask;
                Process currentProcess = process;
                // Логика выполнения команды процесса
                if (executeCmd == CMD.IO)
                {
                    currentTask.currentCmd = currentTask.N_InOut;
                    process.PC_IO++;
                    // Затраты ОС на изменение состояния процесса
                    mw.DelaySimulate(myOS.T_InitIO);
                    currentProcess.AssociatedTask.Status = CMD.IO_START;
                    mw.UpdateDataGridViewTasks(mw.executerTask);
                    Console.WriteLine("Here, " + process.AssociatedTask.N_cmnd);
                    Console.WriteLine("Here, 2 " + process.AssociatedTask.N_cmnd);
                    this.CurProc = -1;
                    this.Command = false;
                    mw.RefreshInterface();

                    // Асинхронная обработка команды ввода/вывода
                    mw.ProcessIOCommand(currentProcess);
                    return;
                }
                else if (executeCmd == CMD.ARIFM)
                {
                    currentTask.Status = CMD.ARIFM;
                    // Увеличиваем счетчик команд процесса
                    process.PC++;
                    mw.UpdateDataGridViewTasks(mw.executerTask);
                    currentTask.currentCmd = currentTask.CMDLen;
                    // Инициализация процессора процессом
                    this.CurProc = currentProcess.Process_Id;
                    Command = true;
                    mw.RefreshInterface();
                    // Затраты ОС на изменение состояния процесса
                    mw.DelaySimulate(myOS.T_InitIO);
                    mw.DelaySimulate(process.AssociatedTask.currentCmd, process, k);
                }
                else if (executeCmd == CMD.END)
                {
                    // Затраты ОС на изменение состояния процесса
                    mw.DelaySimulate(myOS.T_InitIO);
                    // Удаляем процесс из списка заданий, так как он завершён
                    currentTask.Status = CMD.END;

                    process.AssociatedTask.currentCmd++;

                    mw.UpdateDataGridViewTasks();
                    // Затраты ОС на обслуживание прерывания
                    mw.DelaySimulate(myOS.T_IntrIO);

                    var taskToRemove = mw.tasksList.FirstOrDefault(i => i.Task_Id == currentTask.Task_Id);
                    if (taskToRemove != null)
                    {
                        if (taskToRemove != null)
                            mw.tasksList.Remove(taskToRemove);
                        else return;
                        if (mw.usageRAM >= 0)
                        {
                            mw.usageRAM -= currentTask.V_task;
                        }
                        else
                        {
                        }
                        if (myOS.V_ozu != 0)
                            myOS.D_sys = Math.Round((100.0 / myOS.V_ozu) * mw.usageRAM, 2);
                        else
                            myOS.D_sys = 0;
                        myOS.N_Proc -= 1;
                        myOS.M_multi += 1;
                        mw.RefreshInterface();
                        mw.SortTask();
                        mw.UpdateDataGridViewTasks();
                        myOS.T_obor = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - mw.timeStart;

                        mw.myCpu.CurProc = -1;
                        mw.myCpu.Command = false;
                        mw.RefreshInterface();
                        return;
                    }
                    else
                    {
                        //MessageBox.Show("Некорректное удаление!");
                    }
                }
                process.AssociatedTask.currentCmd++;
                // Затраты ОС на изменение состояния процесса
                mw.DelaySimulate(myOS.T_InitIO);
                currentTask.Status = CMD.WAIT;
                mw.UpdateDataGridViewTasks(mw.executerTask);
                // Затраты ОС на обслуживание прерывания
                mw.DelaySimulate(myOS.T_IntrIO);
                mw.myCpu.CurProc = -1;
                mw.myCpu.Command = false;
                mw.RefreshInterface();
            }
        }
    }
}
