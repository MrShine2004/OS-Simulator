using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ProjectOS
{
    public class OS
    {
        // Входные параметры ОС
        public int V_ozu;      // Размер памяти модели ОС
        public int Kvant;      // Квант времени (такты моделирования) (выделенное время для процесса)
        public int T_next;     // Затраты ОС на выбор процесса для выполнения
        public int T_InitIO;   // Затраты ОС на изменение состояния процесса
        public int T_IntrIO;   // Затраты ОС на обслуживание прерывания
        public int T_Load;     // Время на загрузку нового задания
        public int Speed;      // Скорость работы модели

        // Выходные параметры ОС
        public int N_Proc;     // Число загруженных заданий
        public double D_sys;   // Системные затраты ОС (в процентах)
        public System.Timers.Timer T_multi;    // Время работы модели с момента запуска
        public int M_multi;    // Число выполненных заданий с начала моделирования
        public long T_obor;  // Оборотное время (время между задачами)
        public double T_mono_all; // Время выполнения M_multi заданий в однопрограммной системе
        public int M_mono;     // Число заданий, которые могли бы быть выполнены в однопрограммной ОС
        public double D_multi; // Производительность модели ОС по сравнению с однопрограммной системой

        // Конструктор для инициализации параметров ОС
        public OS (int v_ozu, int kvant, int t_next, int t_initIO, int t_intrIO, int t_load, int speed)
        {
            V_ozu = v_ozu;
            Kvant = kvant;
            T_next = t_next;
            T_InitIO = t_initIO;
            T_IntrIO = t_intrIO;
            T_Load = t_load;
            Speed = speed;

            N_Proc = 0;
            D_sys = 0;
            M_multi = 0;
            T_obor = 0;
            T_mono_all = 0;
            M_mono = 0;
            D_multi = 0;
        }
        // Методы управления задачами, процессами и ресурсами можно добавить сюда
    }
}
