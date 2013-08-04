using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;
using System.Threading;
using System.Runtime.Serialization.Json;
using Microsoft.AdCenter.BehaviorTargeting.SegmentStudio.BLL.SSD;
using Microsoft.BT.SeMS.SMS.BussinessObject;
using SSTestLibrary;

using Microsoft.Minesage.SegmentStudio.Contract;
using Microsoft.Minesage.SegmentStudio.Model;
using Microsoft.Minesage.SegmentStudio.Common;
using Microsoft.Minesage.SegmentStudio;
namespace SSDTool
{
    class SSDTool
    {
        private static string PLAN_NAME = "June Deployment 06_02";
        static string FILENAME = @"D:\myprojects\charp\AddTasks\General\AddTasks\tasks_June.xml";
        private const String PlanNamePrefix = "Test Plan ";

        static void Main(string[] args)
        {
//            SSDTool ssdtool = new SSDTool();
//            ssdtool.GenXMLfromCSV();
            if(args.Length<1)
            {
                Console.WriteLine("Usage: SSDTool STRESS <round>");
                Console.WriteLine("       SSDTool [CREATE|ASSIGN|MAP|DEPLOY] <filename>");
                return;
            }
            
            if("STRESS"==args[0] || "stress"==args[0])
            {
                doStressTest(int.Parse(args[1])); ;
            }

//            string filename = args[args.Length - 1];
            string filename = FILENAME;

            string operations = "";
            foreach (var s in args)
            {
                operations = operations + s.ToUpper() + " ";
            }

            SSDTool ssdtool = new SSDTool();
            if (operations.Contains("CREATE"))
            {
                if(args.Count()<2)
                {
                    Console.WriteLine("Argument number not correct!");
                }
                else filename = args[1];
                Console.WriteLine("Creating steamlining deployment plan by file {0}...",filename);
                ssdtool.createPlan(filename);
            }

            if (operations.Contains("APPEND"))
            {
                if (args.Count() < 2)
                {
                    Console.WriteLine("Argument number not correct!");
                }
                else filename = args[1];
                Console.WriteLine("Append tasks to steamlining deployment plan by file {0}...", filename);
                ssdtool.appendPlan(filename);
            }
        }

        private void createPlan(string filename)
        {
            this.genPlanByFile(filename);
        }

        private void appendPlan(string filename)
        {
            this.appendTaskToPlanByFile(filename);
        }

        private void assignTask(string filename)
        {
            //TODO
        }

        private void mapTask(string filename)
        {
            //TODO
        }

        private void deployPlan(string filename)
        {
            //TODO
        }

        public static void doStressTest(int rounds)
        {
            Console.WriteLine(@"
=====================================
     TO DO STRESS TEST {0} ROUND
=====================================", rounds);

            SSDTool ta = new SSDTool();
            Console.WriteLine(ta.getCurrentTime());

            SSDBusinessLogic bll = new SSDBusinessLogic();

            string planName = "SSDTest Stress " + ta.getCurrentTime();
            SSDeploymentPlan plan = ta.createPlan();
            plan.DeploymentTasks = new SSDeploymentTask[] {};
            bll.AssignResourceToNewTasks(plan);
            bll.SaveDeploymentPlan(plan);
            SSDBusinessLogic.FinalizePlan(plan.Id);

            SSDBusinessLogic.DeployPlan(plan.Id);

//            ta.checkPlanDeplyment(plan);
            
        }
        
        public void genPlanByFile(string filename)
        {
            /*IRuleSegment ruleSegmentSvc = PseudoChannelCenter.GetChannel<IRuleSegment>();
            var seg = ruleSegmentSvc.GetById("a60efc89-d97a-4183-8403-97600629636b");
            seg.SegmentName="copid_rule";
            seg.SegmentId = Guid.NewGuid().ToString();
            seg.AuthoringSegmentId = 288438753;
            ruleSegmentSvc.Add(seg);
            */
            SSDTool ta = new SSDTool();
            SSDBusinessLogic bll = new SSDBusinessLogic();

            PLAN_NAME = ta.parsePlanName(filename);
            Console.WriteLine(PLAN_NAME);

            SSDeploymentPlan plan = ta.createPlan();
            Console.WriteLine("create plan done.");
            SSDeploymentTask[] tasks = ta.ParseTasks(plan, filename);
            Console.WriteLine("parse tasks done.");
            plan.DeploymentTasks = ta.AppendTasks(plan.DeploymentTasks, tasks);
            Console.WriteLine("append tasks done.");
            //SSDeploymentTask[] gentasks = ta.generateTasks(plan, 2, 3);
            //SSDeploymentTask[] gentasks = ta.generateTasks(plan, "US", "Ed", 2, 3);
            //plan.DeploymentTasks = ta.AppendTasks(plan.DeploymentTasks, gentasks);

            //bll.AssignResourceToNewTasks(plan);
            bll.SaveDeploymentPlan(plan);
            Console.WriteLine("Plan saved.");
            Console.ReadKey();
            /*
            SSDBusinessLogic.FinalizePlan(plan.Id);
            Console.WriteLine("Saved plan {0}", plan.Id);
            Console.ReadKey();
            ta.deletePlan(plan.Id.ToString());
            Console.WriteLine("Deleted plan {0}", plan.Id);
            Console.ReadKey();
            */

        }

        public void appendTaskToPlanByFile(string filename)
        {
            SSDTool ta = new SSDTool();
            SSDBusinessLogic bll = new SSDBusinessLogic();

            PLAN_NAME = ta.parsePlanName(filename);
            Console.WriteLine(PLAN_NAME);

            SSDeploymentPlan plan = bll.GetLatestDeploymentPlan();              
            Console.WriteLine("get latest plan done.");
            SSDeploymentTask[] tasks = ta.ParseTasks(plan, filename);
            Console.WriteLine("parse tasks done.");
            plan.DeploymentTasks = ta.AppendTasks(plan.DeploymentTasks, tasks);
            Console.WriteLine("append tasks done.");
            bll.SaveDeploymentPlan(plan);
            Console.WriteLine("Plan saved.");
            Console.ReadKey();

        }

        private Dictionary<string, Guid> getUsers()
        {
            const String OWNER_COL_ID = "ID";
            const String OWNER_COL_NAME = "Name";
            SSDBusinessLogic bll = new SSDBusinessLogic();
            DataTable dt = bll.GetUsers();
            Dictionary<string, Guid> ret = new Dictionary<string, Guid>();
            foreach (DataRow row in dt.Rows)
            {
                ret.Add(row[OWNER_COL_NAME].ToString(), new Guid(row[OWNER_COL_ID].ToString()));
            }
            return ret;
        }

        private void deletePlan(string planId)
        {
            var _conStr =
                "Data Source=atc-adcr29v1;Initial Catalog=BT_SR;Persist Security Info=no;Integrated Security=SSPI";

            using (SqlConnection conn = new SqlConnection(_conStr))
            {   
                conn.Open();

                SqlCommand cmd = new SqlCommand("DeleteDeploymentPlan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@PlanID", planId));
                cmd.ExecuteReader();

                Console.ReadKey();
            }
        }

        
        private string getRandomFreeUPSTagMarket()
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            SSDBusinessLogic bll = new SSDBusinessLogic();
            DataTable upsUsage = bll.GetUPSTagUsage();
            int sum = 0;
            ArrayList allUPSMarkets = new ArrayList(50);
                
            foreach (DataRow dr in upsUsage.Rows)
            {
                string market = (string)dr["Market"];
                if (market.Equals("All")) continue;
                
                sum = (int) (dr["Free"]);
                for (int i = 0; i < sum; i++)
                    allUPSMarkets.Add(market);
            }
            return (string)allUPSMarkets[rd.Next(0, allUPSMarkets.Count)];
        }

        private SSDeploymentTask[] generateTasks(SSDeploymentPlan plan, string market, string owner, int liveTaskNum, int testTaskNum)
        {
            var taskList = new List<SSDeploymentTask>(10);
            Dictionary<string, Guid> userList = getUsers();
            for (var i = 0; i < liveTaskNum; i++)
            {
                string segmentname = "Live Task " + i + " "+DateTime.Now.Ticks;

                var oneTask = createTask(market, segmentname, owner, 
                    ProductionStatus.Live, SegmentAction.New, new SSDeployDeliveryEngineSetting());
                oneTask.OwnerId = userList[owner];
                taskList.Add(oneTask);
            }
            for (var i = 0; i < testTaskNum; i++)
            {
                string segmentname = "Test Task " + i +" " + DateTime.Now.Ticks;

                var oneTask = createTask(market, segmentname, owner, 
                    ProductionStatus.Test, SegmentAction.New, new SSDeployDeliveryEngineSetting());
                oneTask.OwnerId = userList[owner];
                taskList.Add(oneTask);
            }
            return taskList.ToArray();
        }

        private SSDeploymentTask[] generateTasks(SSDeploymentPlan plan, int liveTaskNum, int testTaskNum)
        {
            Dictionary<string, Guid> userList = getUsers();
            Random rd = new Random((int)DateTime.Now.Ticks);
            var taskList = new List<SSDeploymentTask>(10);
            for(var i=0;i<liveTaskNum;i++)
            {
                string market = getRandomFreeUPSTagMarket();
                Console.WriteLine(String.Format("Live {0} market {1}",i,market));
                string segmentname = "Live Task " + DateTime.Now.Ticks;
                string owner = "Ed";

                var oneTask = createTask(market, segmentname, owner, 
                    ProductionStatus.Live, SegmentAction.New, new SSDeployDeliveryEngineSetting());
                oneTask.OwnerId = userList[owner];
                taskList.Add(oneTask);
                
            }

            for(var i=0;i<testTaskNum;i++)
            {
                string market = getRandomFreeUPSTagMarket();
                Console.WriteLine(String.Format("Test {0} market {1}", i, market));
                string segmentname = "Test Task " + DateTime.Now.Ticks;
                string owner = "Ed";

                var oneTask = createTask(market, segmentname, owner,
                    ProductionStatus.Test, SegmentAction.New, new SSDeployDeliveryEngineSetting());
                oneTask.OwnerId = userList[owner];
                taskList.Add(oneTask);
            }
            return taskList.ToArray();
        }

        private string parsePlanName(string filename)
        {
            string planName = PLAN_NAME;

            /*
            XmlDocument doc = null;

            //load tasks
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(filename);
                doc = new XmlDocument();
                doc.Load(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            if (doc == null) throw new Exception("XmlDocument is null.");

            XmlElement root = doc.DocumentElement;
            var path = "/plan";

            if(root==null) return planName;

            XmlNodeList nodeList = root.SelectNodes(path);
            if (nodeList == null) throw new Exception("XmlNodeList is null. No task found in file.");
            var node = nodeList[0];
            planName = node.Attributes["name"] == null ? planName : node.Attributes["name"].Value;
            */
            return planName;
        }
        
        private SSDeploymentTask[] ParseTasks(SSDeploymentPlan plan, string filename)
        {
            var taskList = new List<SSDeploymentTask>(10);
            Dictionary<string, Guid> userList = getUsers();

            XmlDocument doc = null;

            //load tasks
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(filename);
                doc = new XmlDocument();
                doc.Load(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            if (doc == null) throw new Exception("XmlDocument is null.");

            //parse the tasks
            XmlElement root = doc.DocumentElement;
            var path = "/plan/tasks/task";
            if (root != null)
            {
                XmlNodeList nodeList = root.SelectNodes(path);
                if (nodeList == null) throw new Exception("XmlNodeList is null. No task found in file.");

                foreach (XmlNode node in nodeList)
                {
                    if (node == null) continue;

                    string market = node.Attributes["market"] == null ? "US" : node.Attributes["market"].Value;
                    string segmentname = node.Attributes["segmentname"].Value;
                    string owner = node.Attributes["owner"].Value;
                    string productionstatus = node.Attributes["ProductionStatus"].Value;
                    string segmentaction = node.Attributes["SegmentAction"].Value;

                    Console.WriteLine("");
                    Console.Write(
                        String.Format("market={0} segmentname={1} PruductionAction={2} SegmentAction={3}"
                                      , market, segmentname, productionstatus, segmentaction));
                    var ps = (ProductionStatus) Enum.Parse(typeof(ProductionStatus), productionstatus, true);
                    var sa = (SegmentAction)Enum.Parse(typeof(SegmentAction), segmentaction, true);

                    switch (sa)
                    {
                        case SegmentAction.New:
                            {
                                var oneTask = createTask(market, segmentname, owner, ps, sa, new SSDeployDeliveryEngineSetting());
                                oneTask.OwnerId = userList[owner];
                                taskList.Add(oneTask);
                                Console.Write("   --New");
                            }
                            break;
                        case SegmentAction.Update:
                            foreach(SSDeploymentTask task in plan.DeploymentTasks)
                            {
                                if (task.SegmentName==null || !task.SegmentName.Equals(segmentname)) continue;
                                task.SegmentActionId = SegmentAction.Update;
                                task.OwnerName = owner;
                                task.OwnerId = userList[owner];
                                if (node.Attributes["NewSegmentName"] != null)
                                    task.NewBusinessSegmentName = node.Attributes["NewSegmentName"].Value;
                                Console.Write("   --Update");
                            }
                            break;
                        case SegmentAction.Flush:
                            foreach (SSDeploymentTask task in plan.DeploymentTasks)
                            {
                                if (task.SegmentName == null || !task.SegmentName.Equals(segmentname))
                                {
                                    if (task.SegmentName == null) Console.Write("   --null");
                                    continue;
                                }
                                task.SegmentActionId = SegmentAction.Flush;
                                Console.Write("   --Flush");
                            }
                            break;
                    }
                }
            }
            Console.WriteLine();
            return taskList.ToArray();   
        }

        private SSDeploymentPlan createPlan()
        {
            var bll = new SSDBusinessLogic();
            TestRoutines.LoginAsAdmin();
            var id = TestRoutines.GetUserIdByName("Admin");
            Guid userId = new Guid(TestRoutines.GetUserIdByName("Admin"));
            string planName = PLAN_NAME;
            SSDeploymentPlan plan = bll.CreateDeploymentPlan(planName, userId);
            return plan;
        }

        SSDeploymentTask[] AppendTasks(SSDeploymentTask[] ssTasks1, SSDeploymentTask[] ssTasks2)
        {

            SSDeploymentTask[] ssTasks = new SSDeploymentTask[ssTasks1.Length + ssTasks2.Length];

            for (int idx = 0; idx < ssTasks1.Length; idx++)
            {
                ssTasks[idx] = ssTasks1[idx];
            }
            for (int idx = 0; idx < ssTasks2.Length; idx++)
            {
                ssTasks[ssTasks1.Length + idx] = ssTasks2[idx];
            }

            return ssTasks;
        }
        private SSDeploymentTask createTask(string marketName, string segmentName, string owner, ProductionStatus statusId, SegmentAction actionId, SSDeployDeliveryEngineSetting deliveryEngine)
        {
            string[] marketnameArray = { "", "US", "UK", "CA", "FR", "JP", "ES", "DE", "NL", "MX", "BR", "AU", "IT" };

            int marketId = 0;
            for (int i = 0; i < marketnameArray.Length; i++)
            {
                if (marketName.Equals(marketnameArray[i])) { marketId = i; }
            }

            SSDeploymentTask task = new SSDeploymentTask();
            task.Id = Guid.NewGuid();
            task.BusinessSegmentName = segmentName;            
            task.IsCompleted = false;
            task.LiveABTestingRequestId = null;
            task.LiveABTestingRequestName = null;
            task.MarketId = marketId;
            task.MarketName = marketName;
            task.OwnerId = new Guid(TestRoutines.GetUserIdByName(owner));
            task.ProductionStatusId = statusId;
            task.SegmentActionId = actionId;
            task.TaskDeliveryEngineSetting = deliveryEngine;
            task.TaskDeliveryEngineSetting.FriendlyNameValue = segmentName;
            task.TaskDeliveryEngineSetting.DeployDeliveryEngineId = 1;
            task.TaskDeliveryEngineSetting.IsTest = (statusId == ProductionStatus.Test);
            return task;
        }

        private string getCurrentTime()
        {
            return String.Format("{0}-{1}-{2} {3}:{4}:{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                 DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public void GenXMLfromCSV()
        {
            FileStream wFile = new FileStream(@"D:\tmp\output.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(wFile);

            try{
                FileStream aFile = new FileStream(@"D:\tmp\a.csv", FileMode.Open);
                StreamReader sr = new StreamReader(aFile);

                string s = sr.ReadLine();
                string[] strArray;
                string sOut = "";
                while(s!=null)
                {
                    strArray = s.Split(',');
                    sOut = String.Format(
                            "<task market=\"{0}\" owner=\"{1}\" segmentname=\"{2}\" ProductionStatus=\"{3}\" SegmentAction=\"{4}\"/>",
                            strArray[0],strArray[1],strArray[3],strArray[2],"Update");
                    sw.WriteLine(sOut);
                    Console.WriteLine(sOut);
                    s = sr.ReadLine();
                }
                
                sr.Close();
                aFile.Close();
                    
            }catch(Exception ex){}


            sw.Flush();
            sw.Close();

        }
    }


}
