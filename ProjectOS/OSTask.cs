using ProjectOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOS
{
    public enum CMD { ARIFM, IO_START, IO, IO_END, END, WAIT };
    public class OSTask
    {
        // Входные параметры задания
        public int Task_Id;    // Идентификатор задания
        public int V_task;     // Размер задания (память)
        public int N_cmnd;     // Чистая длительность выполнения задания (число команд)
        public double D_InOut; // Количество команд ввода/вывода (в процентах)
        public int N_InOut;    // Длительность команд ввода/вывода
        public int Prior;      // Приоритет задания
        public int IO_cmnd;    // Количество команд ввода-вывода
        public CMD Status;  // Статус задачи
        public int currentCmd;
        public int CMDLen;
        public int executedCmd;
        public List<CMD> Commands = new List<CMD>();

        // Конструктор для задания
        public OSTask(int taskId, int vTask, int nCmnd, double dInOut, int nInOut, int prior)
        {
            Task_Id = taskId;
            V_task = vTask;
            N_cmnd = nCmnd;
            D_InOut = dInOut;
            N_InOut = nInOut;
            Prior = prior;
            IO_cmnd = (int)(N_cmnd * D_InOut / 100);
            Status = CMD.WAIT;
            CMDLen = 50;
            currentCmd = 0;
            executedCmd = 0;
            int tIO = IO_cmnd;
            int tCMND = N_cmnd;
            Random rng = new Random();
            for (int i = 0; i< N_cmnd; i++)
            {
                //Если осталась команда, кроме заввершающей
                if (tCMND > 1)
                {
                    // Если есть ввод/вывод
                    if (tIO > 0)
                    {
                        //Если есть арифм.
                        if (tCMND - tIO > 0)
                        {
                            if (rng.Next(0, 2) == 0)
                            {
                                // Арифметическая команда
                                Commands.Add(CMD.ARIFM);
                                tCMND--;
                            }
                            else
                            {
                                // Команда ввода-вывода
                                Commands.Add(CMD.IO);
                                tCMND--;
                                tIO--;
                            }
                        }
                        else
                        {
                            // Команда ввода-вывода
                            Commands.Add(CMD.IO);
                            tCMND--;
                            tIO--;
                        }
                    }
                    else
                    {
                        if (tCMND - tIO > 0)
                        {

                            // Арифметическая команда
                            Commands.Add(CMD.ARIFM);
                            tCMND--;
                        }
                        else
                        {
                            // Комманд нет
                            break;
                        }
                    }
                }
            }
            Commands.Add(CMD.END);
        }
    }
}

