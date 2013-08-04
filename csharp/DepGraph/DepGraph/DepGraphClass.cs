using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using EnvDTE;


namespace DepGraph
{
	/// <summary>
	/// Summary description for DepGraphMainClass.
	/// </summary>
	class DepGraphMainClass
	{

        StreamWriter dFile;

        // Exclusion file name.
        static string _excludeFileName = "depExcl.txt";
        // Array for store exclude project list.
        ArrayList _exclNames = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            DepGraphMainClass dg = new DepGraphMainClass();
            dg.GenerateGraph();
		}

        void GenerateGraph(){
            EnvDTE.DTE dte;

            try{
                dte = (EnvDTE.DTE)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.10.0"); 
            }
            catch(COMException){
                Console.WriteLine("Can't obtain instance of the currently running Visual Studio .NET IDE. ");
                return;
            }
			
            string slnName = Path.GetFileNameWithoutExtension(dte.Solution.FullName);
            string dir = Path.GetDirectoryName(dte.Solution.FullName) + "\\";
            string dFileName = dir + "dep.txt";
            Console.WriteLine("output "+dFileName);
            if(File.Exists(dFileName)){
                File.Delete(dFileName);
            }

            Console.WriteLine("Processing solution " + dte.Solution.FullName);
            dFile = File.CreateText(dFileName);
            dFile.WriteLine("digraph G {"); //the first line for the .dot file

            _exclNames = new ArrayList();
            // Reading of exclusion file
            string exclFullName = dir + _excludeFileName;
            if(File.Exists(exclFullName)){
                StreamReader exclFile = File.OpenText(exclFullName);

                string excl; 
                while ((excl = exclFile.ReadLine()) != null) 
                {
                    excl = excl.Trim().ToLower();
                    if (excl.Length > 0 && !excl.StartsWith("#")){
                        _exclNames.Add(excl);
                    }
                    
                }
            }

            int n = dte.Solution.SolutionBuild.BuildDependencies.Count;
            Console.WriteLine("Total number of Dependencies is " + n + ".");
            Console.WriteLine("Total number of excludes is " + _exclNames.Count + ".");

            Dictionary<string, int> dicProj = new Dictionary<string, int>();

            for(int i = 1; i <=n; i++)
            {

                EnvDTE.BuildDependency bd = dte.Solution.SolutionBuild.BuildDependencies.Item(i);
                string prjName = bd.Project.Name; 

                if(isExclude(prjName)){
                    dFile.WriteLine(prjName  + " [shape=box,style=filled,color=hotpink1];" ); // Excluded project 
                }else{
                    dFile.WriteLine(prjName + " [shape=box,style=filled,color=olivedrab1];" ); // Normal projects
                }

                if (dicProj.ContainsKey(prjName)) dicProj[prjName] = dicProj[prjName] + 1;
                else dicProj[prjName] = 0;
                
                // Information about dependencies of current project.
                IEnumerable depPrj = (IEnumerable)bd.RequiredProjects;
                
                foreach (Project item in depPrj){
                    if(!isExclude(item.Name )){ // Exclude project (for example, frequently used).
                        dFile.WriteLine(item.Name + " -> " + bd.Project.Name );
                        if (dicProj.ContainsKey(item.Name)) dicProj[item.Name] = dicProj[item.Name] + 1;
                        else dicProj[item.Name] = 0;
                    }
                    
                }     
            }
            dFile.WriteLine("}");
            dFile.Flush();
            dFile.Close();

            /*
            string cmdLine = " -Tpng " + dFileName + " -o " + dir  + slnName + "_dep.png";
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "dot.exe";
            proc.StartInfo.Arguments = cmdLine;
            proc.StartInfo.WindowStyle =  ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
            */
            foreach (string key in dicProj.Keys)
            {
                if (dicProj[key] == 0) Console.WriteLine(key + "==0");
            }
            Console.ReadKey();
            //File.Delete(dFileName);
        }

        bool isExclude(string a_prjName){
            a_prjName = a_prjName.ToLower();
            foreach (string s in _exclNames){
                if (s == a_prjName){
                    return true;
                }
            }
            return false;
        }
	}
}
