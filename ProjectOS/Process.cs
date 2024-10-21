using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOS
{
    public class Process
    {
        // Выходные параметры процесса
        public int Process_Id;  // Номер процесса
        public int PC;          // Счетчик команд процесса
        public OSTask AssociatedTask;  // Параметры задания, породившего процесс
        public int CurrentPriority; // Текущий приоритет
        public int PC_IO;       // Счетчик команд ввода/вывода

        public double T_mono_i; // Время выполнения процесса в однопрограммной ОС
        public double T_multi_i;// Время выполнения процесса в модели ОС
        public double D_exe;    // Процент увеличения времени выполнения
        public double D_ready;  // Время нахождения в списке готовности (в процентах)

        // Конструктор для процесса
        public Process(int processId, OSTask task, int priority)
        {
            Process_Id = processId;
            AssociatedTask = task;
            CurrentPriority = priority;
            PC = 0;  // Начальный счетчик команд
            PC_IO = 0; // Начальный счетчик ввода/вывода
        }

        // Методы для выполнения команд, обработки ввода/вывода и переключения состояния процесса
    }
}

