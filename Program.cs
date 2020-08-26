using System;
using System.IO;
using System.Collections.Generic;

namespace MonitorFileSize{
    class MainClass{
        static void Main(string[] args){
            Initialize();

            string path;
            Prompt("Enter the directory : ");
            path=Console.ReadLine();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists) {
                Prompt("The " + dirInfo.Name + " doesn't EXists!\n");
                Prompt("Enter a valid path.\n");
                return;
            }

            Prompt("Printing the Directory Structure of : " + dirInfo.FullName + " \n\n");
            PrintStructure(1, dirInfo);
            Prompt("\n");

            int choice=1;
            Monitor monitor = new Monitor(path);
            do{
                Prompt("1   :   Initialize \n");
                Prompt("2   :   Refresh \n");
                Prompt("3   :   Display\n");
                Prompt("4   :   Clear Screen\n");
                Prompt("0   :   Exit\n");
                try {
                    choice = int.Parse(Console.ReadLine());
                    monitor.Process(choice);
                }
                catch (Exception ex) {
                    Prompt("Error ?*-*? "+"\n\n");
                }
            } while (choice != 0);
        }
        static void Prompt(Object str) {
            Console.Write(str.ToString());
        }
        static void Prompt(char ch,int times) {
            for (int i=0;i<times;i++) {
                Console.Write(ch);
            }
        }
        /**
         * @PrintStructure : Prints the Structure of a Directory Recursively.
         */
        static void PrintStructure(int level,DirectoryInfo dirInfo) {
            FileInfo []fileInfo = dirInfo.GetFiles();
            DirectoryInfo[] _dirInfo = dirInfo.GetDirectories();
            foreach (FileInfo f in fileInfo) {
                Prompt(' ',level-1);
                Prompt("|-"+f.Name);
                Prompt('\n');
            }
            foreach (DirectoryInfo dir in _dirInfo) { 
                Prompt(' ',level-1);
                Prompt("|+"+dir.Name);
                Prompt('\n');
                PrintStructure(level+1,dir);
            }
        }
        static void Initialize() {
            Console.SetBufferSize(600,10000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
    /**
     * @Monitor : Class for detecting the change in file at Runtime.
     * 
     */
    class Monitor {
        public DirectoryInfo dirInfo;
        public DateTime dt;
        String s;
        Dictionary<FileInfo, List<long>> dic = new Dictionary<FileInfo, List<long>>();
        /**
         * str is a string variable denotes the directory for which the Monitor will be monitoring.
         */
        public Monitor(string str) {
            s = new string(str);
            dirInfo = new DirectoryInfo(s);
            foreach (FileInfo f in dirInfo.GetFiles()) {
                dic.Add(f,new List<long>());
                dic[f].Add(f.Length);
            }
        }
        public void ReInitialize() { 
            dirInfo = new DirectoryInfo(s);
            foreach (FileInfo f in dirInfo.GetFiles()) {
                dic.Add(f,new List<long>());
                dic[f].Add(f.Length);
            }
        }
        public void Refresh() {

            List<FileInfo> list = new List<FileInfo>(dic.Count);
            foreach (KeyValuePair<FileInfo,List<long>> v in dic) {
                list.Add(v.Key);
            }
            for (int i=0;i<list.Count;i++) {
                FileInfo f = list[i];
                List<long> size = dic[f];
                if (size[size.Count - 1] != (new FileInfo(f.FullName)).Length) {
                    size.Add((new FileInfo(f.FullName)).Length);
                }
            }
        }
        public void Display() {
            Console.WriteLine();
            Console.WriteLine("****************************************************************");
            foreach (KeyValuePair<FileInfo,List<long>> v in dic) {
                FileInfo f = v.Key;
                List<long> list = v.Value;
                Console.Write("Name : " + f.Name + " ");
                Console.Write("[ ");
                for (int i=0;i<list.Count;i++) {
                    Console.Write((list[i]/(1024*1024))+"mb , ");
                }
                Console.WriteLine(" ]");
            }
            Console.WriteLine("****************************************************************");
            Console.WriteLine();
        }
        public void Process(int choice) {
            if (choice == 1) {
                ReInitialize();
            }
            if (choice == 2) {
                Refresh();
            }
            if (choice==3) {
                Display();
            }
            if (choice == 4) {
                Console.Clear();
            }
        }
    }
}
