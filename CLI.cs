using System;
using System.IO;
using System.Runtime.InteropServices;

namespace lab2
{
  
    class CLI
    {
        private string saveFile = "SavedSortedSet.csv";
        LabSortedSet LabList = new LabSortedSet();
        public void Run()
        {
            if(Load())
            {
                Console.WriteLine("Данные загружены с файла " + saveFile);
            }
            while (true)
            {
                ReadCommand();
            }
        }
        private bool Load()
        {
            if (File.Exists(saveFile))
            {
                string[] data = File.ReadAllLines(saveFile);
                foreach (string line in data)
                {
                    string[] lineData = line.Trim(' ').Split(',');
                    Lamp t = new Lamp(new User(), lineData[1], lineData[2]);
                    t.Id = int.Parse(lineData[0]);
                    t.Power = int.Parse(lineData[3]);
                    t.Resistance = int.Parse(lineData[4]);
                    if (lineData[5] != "")
                    {
                        t.RefreshRate = int.Parse(lineData[5]);
                    }
                    t.Configure(t.Owner, (LightType)Enum.Parse(typeof(LightType),lineData[6]), (Brightness)Enum.Parse(typeof(Brightness), lineData[7]));
                    LabList.Lamps.Add(t);
                }
                return true;
            }
            return false;
        }
        private void DoCommand(string[] args)
        {
            string key = args[0];
            switch (key)
            {
                case "help":
                    Help();
                    break;

                case "info":
                    Info();
                    break;

                case "show":
                    Show();
                    break;

                case "insert":
                    if (args.Length > 1) Insert(args[1]);
                    else InsertInfo();
                    break;

                case "update":
                    if (args.Length > 1) Update(args[1]);
                    else UpdateInfo();
                    break;

                case "remove_key":
                    if (args.Length > 1) Remove(args[1]);
                    else RemoveInfo();
                    break;

                case "clear":
                    Clear();
                    break;

                case "save":
                    Save();
                    break;

                case "execute_script":
                    if (args.Length > 1) ExecuteScript(args[1]);
                    else Console.WriteLine("} Введите execute_script {путь к файлу}");
                    break;

                case "print_ascending":
                    PrintAscending();
                    break;

                case "print_descending":
                    PrintDescending();
                    break;

                case "exit":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("} Такой команды не существует, напишите help чтобы вывести список команд");
                    break;
            }
        }
        private void ReadCommand()
        {
            Console.Write("->");
            string command = Console.ReadLine().TrimStart().ToLower();
            string[] args = command.Split(' ');
            DoCommand(args);
        }
        private void Help()
        {
            Console.WriteLine("} Список доступных команд:\n" +
                "} \thelp - вывести справку по доступным командам\n" +                                          // y
                "} \tinfo - вывести информацию о коллекции (тип, дата инициализации, количество элементов)\n" + // y
                "} \tshow - вывести все элементы коллекции в строковом представлении\n" +                       // y
                "} \tinsert {id} - добавить новый элемент в коллекцию\n" +                                      // y
                "} \tupdate {id} - обновить элемент коллекции по идентификатору\n" +                            // y
                "} \tremove_key {id} - удалить элемент по ключу (для словарей) или по идентификатору\n" +       // y
                "} \tclear - очистить коллекцию\n" +                                                            // y
                "} \tsave - сохранить коллекцию в файл\n" +                                                     // y
                "} \texecute_script file_name - выполнить набор команд из файла\n" +
                "} \texit - завершить программу\n" +                                                            // y
                "} \tprint_ascending - выводит элементы в порядке возрастания id\n" +                           // y
                "} \tprint_descending - выводит элементы в порядке убывания id");                               // y
        }
        private void Info()
        {
            Console.WriteLine($"}} Тип коллекции: {LabList.Type}\n" +
                $"}} Дата инициализации: {LabList.InitDate}\n" +
                $"}} Количество элементов: {LabList.Length}");
        }
        private void PrintLampInfo(Lamp lamp)
        {
            Console.WriteLine("} ----------------------------");
            Console.WriteLine("} ID: " + lamp.Id);
            Console.WriteLine("} Brand - " + lamp.Brand);
            Console.WriteLine("} Model - " + lamp.Model);
            Console.WriteLine("} Power - " + lamp.Power);
            Console.WriteLine("} Resistance - " + lamp.Resistance);
            if (lamp.RefreshRate != null) Console.WriteLine("} RefreshRate - " + lamp.RefreshRate);
            Console.WriteLine("} LightType - " + lamp.LightType);
            Console.WriteLine("} Brightness - " + lamp.Brightness);
        }
        private void Show()
        {
            var lamps = LabList.Lamps;
            
            if(lamps.Count == 0)
            {
                Console.WriteLine("} Коллекция пуста");
            } else
            {
                foreach (Lamp lamp in LabList.Lamps)
                {
                    PrintLampInfo(lamp);
                }
            }
        }
        private int? GetID(string id)
        {
            int? tid = null;
            try
            {
                tid = int.Parse(id);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! " + e.Message);
                Console.ResetColor();
                return null;
            }
            if (tid < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! id не может быть отрицательным");
                Console.ResetColor();
                return null;
            }
            return tid;
        }
        private void Insert(string id)
        {
            var temp = Objects.objects;
            int? tid = GetID(id);
            if (tid == null)
            {
                InsertInfo();
                return;
            }
            if (tid > temp.Length - 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! Не существует объекта с этим id");
                Console.ResetColor();
                return;
            }
            foreach (var t in temp)
            {
                if(t.Id == tid)
                {
                    if(LabList.Lamps.Add(t))
                    {
                        Console.WriteLine("} Элемент добавлен в список!");
                    } else
                    {
                        Console.WriteLine("! Ошибка добавления элемента в список");
                    }
                    return;
                }
            }
        }
        private void InsertInfo()
        {
            var temp = Objects.objects;
            Console.WriteLine("} Введите insert {id}, где id - целое число");
            Console.WriteLine("} Существующие объекты: ");
            foreach(var t in temp)
            {
                Console.WriteLine("} id:" + t.Id + " Brand: " + t.Brand + " Model: " + t.Model);
            }
        }
        private void Update(string id) 
        {
            int? tid = GetID(id);
            if(tid == null)
            {
                UpdateInfo();
                return;
            }
            Lamp obj = null;
            try
            {
                foreach (Lamp l in LabList.Lamps)
                {
                    if (l.Id == tid)
                    {
                        obj = l;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (obj == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! Элемента с таким id не существует или он не добавлен в список");
                Console.ResetColor();
                UpdateInfo();
                return;
            }
            Console.WriteLine("} Данные объекта: ");
            PrintLampInfo(obj);
            

            Console.WriteLine("} Введите новые значения для всех атрибутов или нажмите Enter чтобы оставить прежнее: ");
            Console.Write($"Brand(строковое значение) {obj.Brand} < ");
            string t = Console.ReadLine();
            if (t != " " && t != null && t != "") obj.Brand = t;

            Console.Write($"Model(строковое значение) {obj.Model} < ");
            string t1 = Console.ReadLine();
            if (t != " " && t != null && t != "") obj.Model = t1;

            Console.Write($"Power(вещественное число) {obj.Power} < ");
            while(true)
            {
                string temp = Console.ReadLine();
                float p;
                if(temp == "")
                {
                    break;
                }
                try
                {
                    p = float.Parse(temp);
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message + " Введите вещественное число.");
                    Console.Write($"Power(вещественное число) {obj.Power} < ");
                    continue;
                }
                if (p < 0)
                {
                    Console.WriteLine("Число не может быть отрицательным. Введите вещественное положительное число.");
                    Console.Write($"Power(вещественное число) {obj.Power} < ");
                    continue;
                }
                obj.Power = p;
                break;
            }
            Console.Write($"Resistance(вещественное число) {obj.Resistance} < ");
            while (true)
            {
                string temp = Console.ReadLine();
                float p;
                if (temp == "")
                {
                    break;
                }
                try
                {
                    p = float.Parse(temp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + " Введите вещественное число.");
                    Console.Write($"Resistance(вещественное число) {obj.Resistance} < ");
                    continue;
                }
                if (p < 0)
                {
                    Console.WriteLine("Число не может быть отрицательным. Введите вещественное положительное число.");
                    Console.Write($"Resistance(вещественное число) {obj.Resistance} < ");
                    continue;
                }
                obj.Resistance = p;
                break;
            }
            Console.Write($"RefreshRate(целое число, может быть пустым) {obj.RefreshRate} < ");
            while (true)
            {
                string temp = Console.ReadLine();
                int p;
                if (temp == "")
                {
                    break;
                }
                try
                {
                    p = int.Parse(temp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + " Введите целое число.");
                    Console.Write($"RefreshRate(целое число, может быть пустым)) {obj.RefreshRate} < ");
                    continue;
                }
                if (p < 0)
                {
                    Console.WriteLine("Число не может быть отрицательным. Введите целое положительное число.");
                    Console.Write($"RefreshRate(целое число, может быть пустым)) {obj.RefreshRate} < ");
                    continue;
                }
                obj.RefreshRate = p;
                break;
            }
            Console.Write($"LightType(один вариант из трех: WARM, WHITE, COLD) {obj.LightType} < ");
            while (true)
            {
                string temp = Console.ReadLine().Trim(' ').ToUpper();
                if(temp == "")
                {
                    break;
                }
                try
                {
                    LightType lt = (LightType)Enum.Parse(typeof(LightType), temp);
                    obj.Configure(obj.Owner, lt, obj.Brightness);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write($"LightType(один вариант из трех: WARM, WHITE, COLD) {obj.LightType} < ");
                    continue;
                }
                break;
            }
            Console.Write($"Brightness(один вариант из трех: LOW, NORMAL, BRIGHT) {obj.Brightness} < ");
            while (true)
            {

                string temp = Console.ReadLine().Trim(' ').ToUpper();
                if (temp == "")
                {
                    break;
                }
                try
                {
                    Brightness b = (Brightness)Enum.Parse(typeof(Brightness), temp);
                    obj.Configure(obj.Owner, obj.LightType, b);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write($"Brightness(один вариант из трех: LOW, NORMAL, BRIGHT) {obj.Brightness} < ");
                    continue;
                }
                break;
            }
        }
        private void UpdateInfo()
        {
            Console.WriteLine("} Введите update {id}, где id - целое число, идентификатор существующего объекта\n} id элемента можно узнать командой show");
        }
        private void Remove(string id) 
        {
            int? tid = GetID(id);
            if (tid == null) {
                RemoveInfo();
                return;
            }
            foreach (var l in LabList.Lamps)
            {
                if (tid == l.Id)
                {
                    if (LabList.Lamps.Remove(l))
                    {
                        Console.WriteLine("} Элемент удален!");
                    } else
                    {
                        Console.WriteLine("! Не удалось удалить элемент");
                    }
                    return;
                }   
            }
            Console.WriteLine("! Элемента с таким ID не существует");
        }
        private void RemoveInfo()
        {
            Console.WriteLine("} Введите remove_key {id}, id элемента можно узнать командой show (если коллекция не пуста)");
        }
        private void Clear() 
        {
            if (LabList.Length == 0)
            {
                Console.WriteLine("} Коллекция пуста, нечего очищать");
            }
            else
            {
                LabList.Lamps.Clear();
                Console.WriteLine("} Коллекция очищена!");
            }
        }
        private void Save()
        {
            File.Delete(saveFile);
            foreach (var l in LabList.Lamps)
            {
                try
                {
                    string data = $"{l.Id},{l.Brand},{l.Model},{l.Power},{l.Resistance},{l.RefreshRate},{l.LightType},{l.Brightness}\n";
                    File.AppendAllText(saveFile, data);
                } catch (Exception e)
                {
                    Console.WriteLine($"! Error: {e}");
                    return;
                }
            }
            Console.WriteLine("} Данные успешно записаны в файл " + saveFile);
        }
        private void ExecuteScript(string filepath)
        {
            try
            {
                string[] commands = File.ReadAllLines(filepath);
                foreach (string command in commands)
                {
                    string[] args = command.Split(' ');
                    DoCommand(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
        private void PrintAscending()
        {
            Show();
        }
        private void PrintDescending()
        {
            var temp = LabList.Lamps.Reverse();
            foreach(Lamp lamp in temp)
            {
                PrintLampInfo(lamp);
            }
        }
    }
}
