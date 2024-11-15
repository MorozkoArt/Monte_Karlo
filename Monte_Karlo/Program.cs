using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_method
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            List<List<string>> binary_encodings = new List<List<string>>();
            int max_znach  = Convert.ToInt32(Math.Pow(2, 15));           
            while (true)
            {
                int Var_Menu = -1;
                int Adaptation_option = -1;
                Console.WriteLine(" МЕНЮ");
                Console.WriteLine("1 - Сгенерировать список бинарных кодировок");
                Console.WriteLine("2 - Выполнить алгоритм ");
                Console.WriteLine("3 - Закрыть программу");
                Var_Menu = int.Parse(Console.ReadLine());
                switch (Var_Menu)
                {
                    // Генерация списка кодировок
                    case 1:
                        binary_encodings.Clear();
                        Console.WriteLine("Выберете вариант генерации приспособленности");
                        Console.WriteLine("В случии некорректного ввода - приспособленность будет генерироваться случайно");
                        Console.WriteLine("1 - Сгенерировать случайно");
                        Console.WriteLine("2 - Приспособленность соответсвует натуральному значению бинарного кода");
                        Console.WriteLine("3 - Приспособленность вычисляется как квадратичная функция");
                        Adaptation_option = int.Parse(Console.ReadLine());                      
                        HashSet<string> uniqueEncodings = new HashSet<string>();
                        while (uniqueEncodings.Count < max_znach)
                        {
                            string ns = null;

                            for (int j = 0; j < 15; ++j)
                            {
                                ns += rnd.Next(0, 2).ToString();
                            }
                            if (uniqueEncodings.Add(ns))
                            {
                                List<string> encodingData = new List<string>();
                                encodingData.Add(ns);
                                if (Adaptation_option == 1) encodingData.Add(rnd.Next(1, max_znach).ToString());
                                else if (Adaptation_option == 2) encodingData.Add(BinaryToDecimal(ns).ToString());
                                else if (Adaptation_option == 3) encodingData.Add(QuadraticAdaptationFunction(ns).ToString());
                                else if (Adaptation_option != 1 && Adaptation_option != 2 && Adaptation_option != 3) encodingData.Add(rnd.Next(1, max_znach).ToString());                          
                                binary_encodings.Add(encodingData);
                            }

                        }
                        Console.WriteLine("Список успешно сгенерирован!");
                        break;
                    // Алгоритм
                    case 2:
                        if (binary_encodings.Count > 0)
                        {
                            Console.WriteLine("Вывод Ланшафта");
                            List<int> indices_encodings = new List<int>(32);
                            // Вывод ланшафта без повторяющихся кодировок
                            for (int i = 0; i < 32; ++i)
                            {
                                bool flag = true;
                                while (flag == true) // генерация случайных кодировок без повторов
                                {
                                    bool chec = false;
                                    indices_encodings.Add(rnd.Next(0, max_znach));
                                    if (indices_encodings.Count > 1)
                                    {
                                        for (int j = 0; j < indices_encodings.Count-1; ++j)
                                        {
                                            if(indices_encodings[j] == indices_encodings[indices_encodings.Count-1])
                                            {
                                                indices_encodings.RemoveAt(indices_encodings.Count-1);
                                                flag = true;
                                                chec = true;
                                            }
                                        }
                                        if(chec == false) flag = false;                                       
                                    }                                                 
                                    else flag = false;                                                                     
                                }
                                Console.WriteLine("Индекс кодировки - " + indices_encodings[i] +  " кодировка - " + binary_encodings[indices_encodings[i]][0] + 
                                    " Приспособленность - " + binary_encodings[indices_encodings[i]][1]);
                            }
                            // Алгоритм
                            int max = 0;
                            string maxS = "Empty";
                            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------");
                            for (int i = 0; i < 32; ++i)
                            {

                                Console.Write("Номер шага - " + (i + 1) + " Max Приспособленность - " + max + " Max кодировка - " + maxS 
                                    + " Кодировка - " + binary_encodings[indices_encodings[i]][0] + " Приспособленность - " + binary_encodings[indices_encodings[i]][1]);
                                if (max < int.Parse(binary_encodings[indices_encodings[i]][1]))
                                {
                                    max = int.Parse(binary_encodings[indices_encodings[i]][1]);
                                    maxS = binary_encodings[indices_encodings[i]][0];
                                    Console.Write("  <--The maximum has changed!");
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine(maxS + " " + max);
                        }
                        else
                        {
                            Console.WriteLine("Список пустой!!!");
                        }
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Неккоректный ввод");
                        break;
                }
            }
        }
        //перевод из двоичной в десятичную сс. (Вариант - 2)
        public static int BinaryToDecimal(string binary)
        {
            int _decimal = 0;
            for (int i = 0; i < 15; ++i)
            {
                _decimal += int.Parse(binary[i].ToString()) * Convert.ToInt32(Math.Pow(2, 14 - i));
            }
            return _decimal;
        }
        //вычисление приспособленности по функции u = (xi - 2^(L-1))^2. (Вариант - 3)
        public static int QuadraticAdaptationFunction(string binary)
        {
            int xi = BinaryToDecimal(binary);
            return Convert.ToInt32(Math.Pow((xi - Convert.ToInt32(Math.Pow(2, 15 - 1))), 2));
        }
    }
}
