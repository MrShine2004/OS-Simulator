using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOS
{
    internal class CPU
    {
        // Выходные параметры процессора
        public int PC;          // Счетчик команд
        public int CurProc;     // Номер выполняемого процесса
        public string Command;  // Выполняемая команда или ожидание

        // Конструктор для процессора
        public CPU()
        {
            PC = 0;
            CurProc = -1; // Нет текущего процесса
            Command = "Idle";
        }

        // Методы для выполнения команд и управления процессами
        public void ExecuteCommand(Process process)
        {
            // Логика выполнения команды процесса
            PC++;
            CurProc = process.Process_Id;
            Command = "Executing Process " + process.Process_Id;
        }
    }
}
