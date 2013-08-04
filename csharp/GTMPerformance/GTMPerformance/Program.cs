using System;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Advertising.BehaviorTargeting.SegmentStudio.Contract;
using Microsoft.Advertising.BehaviorTargeting.SegmentStudio.DAL;
using Microsoft.Advertising.BehaviorTargeting.SegmentStudio.Model;
using Microsoft.AdCenter.BehaviorTargeting.SegmentStudio.BLL;
using Microsoft.Advertising.BehaviorTargeting.SegmentStudio.StreamlineDeployment;
using Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment;
using SrTaskStatus = Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment.TaskStatus;

using SSTestLibrary;

namespace Microsoft.Advertising.BehaviorTargeting.Test.GTMTest.Performance
{
    class GTMPerformance
    {
        private TestContext testContextInstance;
        private GTMTestHelper ta;
        

        GTMPerformance()
        {
            ta = new GTMTestHelper(testContextInstance);
        }
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("start GTMPerformance "+args.Length);
                GTMPerformance gpf = new GTMPerformance();
                if (args.Length == 2)
                {
                    string command = args[0];
                    string filename = args[1];

                    if (string.Compare(command, "all", true) == 0)
                    {
                        Console.WriteLine("get all live segment...");
                        IStreamliningDeployment ssdSvc = PseudoChannelCenter.GetChannel<IStreamliningDeployment>();
                        Console.WriteLine("ssdSvc=" + ssdSvc);

                        SdDeploymentTask[] tasks = ssdSvc.GetDeploymentTasksOfLiveSegments(null, null);
                        Console.WriteLine("task sum="+tasks.Length);
                        Console.WriteLine("get all live segmgents pass.");
                    }

                    if(string.Compare(command,"try",true)==0){
                        Console.WriteLine("trying...");
                        IStreamliningDeployment ssdSvc = PseudoChannelCenter.GetChannel<IStreamliningDeployment>();
                        Console.WriteLine("ssdSvc=" + ssdSvc);

                        bool flag = false;
                        Object dc = null;
                        if (string.Compare(filename, "DAP", true) == 0) dc = DeliveryChannel.DAP;
                        if (string.Compare(filename, "MSN", true) == 0) dc = DeliveryChannel.MSN;
                        SdDeploymentTask[] tasks;
                        if (dc == null)
                        {
                             tasks = ssdSvc.GetDeploymentTasksOfLiveSegments(null, null);
                        }
                        else
                        {
                             tasks = ssdSvc.GetDeploymentTasksOfLiveSegments((DeliveryChannel)dc, null);

                        }

                        Console.WriteLine("tasks = " + tasks.Count());

                    }

                    if (command.ToLower() == "deploy")
                    {
                        gpf.PerfLog("BEGING TO DEPLOY SEGMENTS");
                        gpf.DeploySegment(filename);
                        gpf.PerfLog("END DEPLOY SEGMENTS");
                    }

                    if (command.ToLower() == "flush")
                    {
                        gpf.PerfLog("BEGING TO FLUSH SEGMENTS");
                        gpf.FlushSegment(filename);
                        gpf.PerfLog("END FLUSH SEGMENTS");
                    }

                    if (command.ToLower() == "publish")
                    {
                        gpf.PerfLog("BEGING TO PUBLISH SEGMENTS");
                        gpf.PublishSegment(filename);
                        gpf.PerfLog("END PUBLISH SEGMENTS");
                    }
                }
                else
                {
                    Console.WriteLine("no arguments");
                    //string filename = @"e:\list.txt";
                    //if (args.Length > 0) filename = args[0];
                    //gpf.DeploySegment(filename);
                }

                if (args.Length == 1)
                {
                    ArrayList al = gpf.GetSegmentIds(args[0]);
                }
                //Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void PublishSegment(string filename)
        {
            ArrayList segmentIds = GetSegmentIds(filename);

            TimeSpan ts0 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan tsD = ts0;

            foreach(Guid segmentid in segmentIds)
            {
                TestRoutines.PublishSegment(segmentid.ToString());
            }

            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            
            PerfLog(DateTime.Now.ToString()+" PUBLISH="+tsD.Seconds);
        }

        public void DeploySegment(string filename)
        {

            ArrayList segmentIds = GetSegmentIds(filename);
            Dictionary<string, SdDeploymentTask> mapSegmentId2task = new Dictionary<string, SdDeploymentTask>();
            Dictionary<string, string> mapTaskName2SegmentId = new Dictionary<string, string>();
            Dictionary<string, string> mapSegmentId2TaskName = new Dictionary<string, string>();

            List<SdDeploymentTask> listTask = new List<SdDeploymentTask>();

            TimeSpan ts0 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan tsD = ts0;

            ta.currentCapacityMap = ta.GetCapacitySnap();

            string sqltxt = "";
            string segName = "";
            foreach (Guid segmentid in segmentIds)
            {
                sqltxt = string.Format("SELECT SegmentName from Segment WHERE SegmentId='{0}'", segmentid.ToString());
                segName = ta.GetStringFromDB(sqltxt, ta.SSDBConn);
                mapSegmentId2TaskName[segmentid.ToString()] = segName;

                SdDeploymentTask newTask = ta.CreateQuickNewTaskMSN(segName);
                listTask.Add(newTask);
                mapSegmentId2task[segmentid.ToString()] = newTask;
            }

            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " CREATETASKS=" + tsD.Seconds);

            //assign tasks
            SdDeploymentTask[] latestTasks = listTask.ToArray();
            latestTasks = ta.ToSaveNewTasks(latestTasks);

            ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " SAVETASKS=" + tsD.Seconds);


            latestTasks = ta.ToAssignTasks(latestTasks);

            ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " ASSIGNTASKS=" + tsD.Seconds);

            List<SdDeploymentTask> list2 = new List<SdDeploymentTask>();
            int p = 0;
            foreach(Guid segmentid in segmentIds){
                //TestRoutines.PublishSegment(segmentid.ToString());
                //PerfLog("complete segmentId = " + segmentid);
                int authId = ta.GetAuthoringId(segmentid.ToString());
                Log("authId=" + authId);
                
                list2.Add(ta.ToCompleteTaskWithSegment(latestTasks[p], authId));
                p++;
            }
            ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " COMPLETETASKS=" + tsD.Seconds);
            
            //allocate tags
            latestTasks = ta.ToAllocateTags(list2.ToArray());

            ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " ALLOCATETAG=" + tsD.Seconds);
            
            //deploy these tags
            latestTasks = ta.ToDeployTasks(latestTasks);

            ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " DEPLOYTASKS=" + tsD.Seconds);
        }

        public void FlushSegment(string filename)
        {

            ArrayList segmentIds = GetSegmentIds(filename);

            
            //foreach (Guid segmentid in segmentIds)
            //{
            //    Console.WriteLine("segmentId = " + segmentid);
            //    LibFlushOneSegment(segmentid.ToString());
            //}

            List<SdDeploymentTask> listTask = new List<SdDeploymentTask>();
            ta.fetchLiveSegmentTasks();
            foreach(Guid segId in segmentIds)
            {
                Console.WriteLine("aaa segId = " + segId);
                SdDeploymentTask taskTest = ta.CreateTaskOfLiveSegmentById(segId.ToString());
                    //ta.getLiveSegmentTaskBySegmentId(segId.ToString());
                    
                Console.WriteLine("taskTest=" + taskTest);
                taskTest.Action = SegmentAction.Flush;
                listTask.Add(taskTest);
            }

            TimeSpan ts0 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan tsD = ts0; 
            
                        //assign tasks
            SdDeploymentTask[] latestTasks = listTask.ToArray();
            latestTasks = ta.ToSaveNewTasks(latestTasks);

            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " SAVETASKS=" + tsD.Seconds);


            latestTasks = ta.ToAssignTasks(latestTasks);

            ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " ASSIGNTASKS=" + tsD.Seconds);

            //deploy these tags
            latestTasks = ta.ToDeployTasks(latestTasks);

            ts1 = new TimeSpan(DateTime.Now.Ticks);
            tsD = ts1.Subtract(ts0).Duration();
            ts0 = ts1;
            PerfLog(DateTime.Now.ToString() + " DEPLOYTASKS=" + tsD.Seconds);

        }


        public ArrayList GetSegmentIds(string filename)
        {
            ArrayList segmentids = new ArrayList();

            //Console.WriteLine(filename);
            using(StreamReader sr = File.OpenText(filename)){
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    Log(str);
                    segmentids.Add(new Guid(str));
                    //"0048534c-5263-4588-875c-8afe6ffb95c8"
                }
            };
            
            return segmentids;
        }

        public void LibFlushOneSegment(string segmentId)
        {
            //add 1 flush tasks
            Console.WriteLine("segment id=" + segmentId);
            int authId = ta.GetAuthoringId(segmentId);
            //string sqlTxt = string.Format("SELECT MarketId FROM [BTSRDB].[dbo].[SegmentModel] where AuthoringId={0}", authId);
            //int marketId = ta.GetIntFromDB(sqlTxt, "BTSRDBConn");

            //SdDeploymentTask taskTest = ta.CreateTaskOfLiveSegment(new Guid(segmentId), DeliveryChannel.DAP, marketId);
            SdDeploymentTask taskTest = ta.CreateTaskOfLiveSegment(ProductionStatus.Test, DeliveryChannel.DAP, null);
            taskTest.Action = SegmentAction.Flush;

            //save the task
            taskTest = ta.ToSaveNewTasks(new SdDeploymentTask[] { taskTest })[0];

            //Assign tasks
            taskTest = ta.ToAssignTasks(new SdDeploymentTask[] { taskTest })[0];

            //deploy the task
            taskTest = ta.ToDeployTasks(new SdDeploymentTask[] { taskTest })[0];
        }

        public void Log(string s)
        {
            //Console.WriteLine(s);
        }

        public void PerfLog(string s)
        {
            Console.WriteLine(s);
        }
    }
}












































































































































































































































































































































































































































































































































































































































































