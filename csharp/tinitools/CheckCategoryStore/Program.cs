using System;
using System.Collections.Generic;
using System.Linq;


namespace CheckCategoryStore
{
    using System.IO;
    using System.Net;

    class CategoryStoreChecker
    {
        List<string> machinelist;

        private const string machinelistFile = @".\Machines.csv";

        private const string OUTPUTFOLDER = @".\";

        static void Main(string[] args)
        {

            var csc = new CategoryStoreChecker();
            csc.CheckCategoryStore();

            Console.ReadKey();
        }

        public void CheckCategoryStore()
        {
            InitCSMachineList(machinelist, machinelistFile);

            //PrintMachines(machinelist);

            CheckExpansionByURL(machinelist);
        }

        private void CheckExpansionByURL(List<string> mlist)
        {
            if (mlist.Count < 1) return;


            string urlTemplate =
                @"http://{0}:6438/AdCenterCategoryStore?func=GetExpansions&dc=0&lcid=2052&kwd=0+%E4%B8%89%E6%98%9F";

            //@"http://cockpit.autopilot.ch1d.osdinfra.net:81/http/{0}/6438/AdCenterCategoryStore?func=GetExpansions&dc=0&lcid=2052&kwd=0+%E4%B8%89%E6%98%9F";
            

            var wc = new WebClient();
            foreach (var machinename in mlist)
            {
                
                string url = string.Format(urlTemplate, machinename);
                Console.WriteLine("{0}:{1}", machinename, url);
                
                string content = wc.DownloadString(url);

                File.WriteAllText(OUTPUTFOLDER + machinename, content);
            }
        }

        private void InitCSMachineList(List<string> list, string s)
        {
            machinelist = new List<string>();
            try
            {
                var partitions = new HashSet<string>();
                string[] allMachines = File.ReadAllLines(s);
                var csMachines = allMachines.Where(machine => machine.IndexOf("Category_PS") > 0).ToList();

                foreach (var machineinfo in csMachines)
                {
                    int pos1 = machineinfo.IndexOf(',');
                    if (pos1 < 0) return;
                    string machinename = machineinfo.Substring(0, pos1);

                    pos1 = machineinfo.IndexOf('[');
                    int pos2 = machineinfo.IndexOf(']');

                    string partition = machineinfo.Substring(pos1, pos2 - pos1 + 1);
                    if (!partitions.Contains(partition))
                    {
                        partitions.Add(partition);
                        machinelist.Add(machinename);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Read machine list file fail. "+e.ToString());
                return;
            }
        }

        private void PrintMachines(List<string> mlist)
        {
            foreach (var m in mlist)
            {
                Console.WriteLine("{0}", m);
            }
        }
    }
}
