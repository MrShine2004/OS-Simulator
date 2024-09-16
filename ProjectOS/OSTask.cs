using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOS
{
    internal class OSTask
    {
        // Входные параметры задания
        public int Task_Id;    // Идентификатор задания
        public int V_task;     // Размер задания (память)
        public int N_cmnd;     // Чистая длительность выполнения задания (число команд)
        public double D_InOut; // Количество команд ввода/вывода (в процентах)
        public int N_InOut;    // Длительность команд ввода/вывода
        public int Prior;      // Приоритет задания

        // Конструктор для задания
        public OSTask(int taskId, int vTask, int nCmnd, double dInOut, int nInOut, int prior)
        {
            Task_Id = taskId;
            V_task = vTask;
            N_cmnd = nCmnd;
            D_InOut = dInOut;
            N_InOut = nInOut;
            Prior = prior;
        }
    }
}

