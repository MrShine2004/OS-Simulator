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
        public int Task_Id;    // Идентификатор задания, уникальный для каждого задания
        public int V_task;     // Размер задания в памяти (например, в мегабайтах)
        public int N_cmnd;     // Чистая длительность выполнения задания, в количестве команд, которые нужно выполнить
        public double D_InOut; // Процент времени, затрачиваемого на команды ввода/вывода, от общего времени выполнения задания
        public int N_InOut;    // Длительность команд ввода/вывода в тактах или единицах времени
        public int Prior;      // Приоритет задания, определяет его важность для планировщика
        public int IO_cmnd;    // Количество команд ввода-вывода, которые должны быть выполнены в рамках задания
        public CMD Status;     // Статус задания (например, выполняется, ожидает, завершено)
        public int currentCmd; // Индекс текущей команды, которая выполняется в данный момент в рамках задания
        public int CMDLen;     // Общее количество команд, которые нужно выполнить в рамках задания
        public int executedCmd; // Количество уже выполненных команд в рамках задания
        public List<CMD> Commands = new List<CMD>(); // Список команд, которые нужно выполнить в рамках задания
        public long executedTime; // Время, затраченное на выполнение задания, в тактах или миллисекундах
        public long startTime;    // Время начала выполнения задания (в тактах или миллисекундах)


        // Конструктор для задания
        public OSTask (int taskId, int vTask, int nCmnd, double dInOut, int nInOut, int prior, long timeTaskCreate)
        {
            Task_Id = taskId;
            V_task = vTask;
            N_cmnd = nCmnd;
            D_InOut = dInOut;
            N_InOut = nInOut;
            Prior = prior;
            IO_cmnd = (int)(N_cmnd * D_InOut / 100);
            Status = CMD.WAIT;
            CMDLen = 40;
            currentCmd = 0;
            executedCmd = 0;
            int tIO = IO_cmnd;
            int tCMND = N_cmnd;
            executedTime = 0;
            startTime = timeTaskCreate;
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

