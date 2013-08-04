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
using Microsoft.Advertising.BehaviorTargeting.SegmentStudio.StreamlineDeployment;
using Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment;
using SrTaskStatus = Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment.TaskStatus;
namespace Microsoft.Advertising.BehaviorTargeting.Test.GTMTest.Performance
{
    class GTMExplorate
    {
        private TestContext testContextInstance;
        private GTMTestHelper ta;
        public int state = 0;
        public State currState = State.None;
        public Dictionary<string, string> taskRoute = new Dictionary<string, string>();
        int seed = 1;
        public Vertical[] verticals;
        public Market[] markets;
        public User[] users;

        GTMExplorate()
        {
            ta = new GTMTestHelper(testContextInstance);
            verticals = ta.GetVerticals();
            markets = ta.GetMarkets();
            users = ta.GetUsers();
        }

        static void Main(string[] args)
        {
            int n = 80;
            if (args.Length == 1) n = int.Parse(args[0]);

            Console.WriteLine("Round to run "+n);
            GTMExplorate gtme = new GTMExplorate();
            try
            {
                gtme.Go(n);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("=========================================================================");
                foreach (var pair in gtme.taskRoute)
                {
                    gtme.Log(pair.Value);
                }
            }
            Console.ReadKey();
        }

        public void Go(int round)
        {
            for (int i = 0; i < round; i++)
            {
                
                int r = GetRnd();
                Log("state rnd=" + r);
                if (r < 30) State_None();
                if (r >= 30 && r < 50) State_Created();
                if (r >= 50 && r < 65) State_Assigned();
                if (r >= 65 && r < 80) State_Completed();
                if (r >= 80) State_AllocatedTag();
                Thread.Sleep(1000);
            }

            foreach (var pair in taskRoute)
            {
                Log(pair.Value);
            }
        }

        public void State_None()
        {
            Log(" *State_None*");
            List<SdDeploymentTask> listTask = new List<SdDeploymentTask>();


            SdDeploymentTask newTask = CreateTask();

            listTask.Add(newTask);
            SdDeploymentTask[] latestTasks = listTask.ToArray();
            latestTasks = ta.ToSaveNewTasks(latestTasks);
            SdDeploymentTask task = latestTasks[0];
            
            string sAction = "New";
            if (task.Action == SegmentAction.Update) sAction = "Update";
            if (task.Action == SegmentAction.Flush) sAction = "Flush";

            string sDC = "DAP";
            if (task.DeliveryChannelConfiguration.DeliveryChannel == DeliveryChannel.MSN) sDC = "MSN";

            string sPS = "";
            if(sDC=="DAP"){
                if(task.DapChannelCfg.ProductionStatus == ProductionStatus.Live) sPS="Live";
                else sPS = "Test";
            }
            string s = string.Format("[Action:{0},Name:{1},DC:{2},PS:{3},Market:{4}]", 
                sAction, task.BusinessSegmentName,sDC, sPS, task.MarketName);

            if (!taskRoute.ContainsKey(task.Id.ToString()))
                taskRoute[task.Id.ToString()] = "";
            TraceLog(task, s);
            Log(string.Format("Create task for new segment, task={0}", latestTasks[0].BusinessSegmentName));
        }

        /// <summary>
        /// 70% to assign
        /// 20% to re-edit
        /// 10% to delete
        /// </summary>
        public void State_Created()
        {
            Log(" *State_Created*");
            SdDeploymentTask[] latestTasks = ta.GetTask_Created();
            List<SdDeploymentTask> taskToAssign = new List<SdDeploymentTask>();
            List<SdDeploymentTask> taskToEdit = new List<SdDeploymentTask>();
            List<SdDeploymentTask> taskToDelete = new List<SdDeploymentTask>();

            foreach (SdDeploymentTask t in latestTasks)
            {
                int r = GetRnd();
                if (r < 75) taskToAssign.Add(t);
                if (r >= 75 && r < 95) taskToEdit.Add(t);
                if (r >= 95) taskToDelete.Add(t);

                Log("   State_Created r=" + r);
            }

            //allocate tags
            if (taskToAssign.Count > 0)
            {
                latestTasks = ta.ToAssignTasks(taskToAssign.ToArray());
                foreach (SdDeploymentTask t in latestTasks)
                {
                    string log = string.Format("-><Assign:[owner:{0}]>", latestTasks[0].OwnerName);
                    TraceLog(t, log);
                    Log(string.Format("Assigned task for new segment, task={0}", t.BusinessSegmentName));
                }
            }
            //re-edit tasks
            if (taskToEdit.Count > 0)
            {
                foreach (SdDeploymentTask t in taskToEdit)
                {
                    if (t.DapChannelCfg.DeliveryChannel == DeliveryChannel.DAP)
                    {
                        //change market
                        string newMarket = ChooseMarket();
                        t.MarketId = ta.GetMarketIdByName(newMarket);
                        t.MarketName = newMarket;

                        //change name
                        t.BusinessSegmentName = t.BusinessSegmentName + "_1";

                        //change PS
                        t.DapChannelCfg.ProductionStatus = ChoosePS();

                        //change vertical
                        string newVertical = ChooseVertical();
                        t.VerticalId = ta.GetVerticalIdByName(newVertical);
                        t.VerticalName = newVertical;

                        string log = string.Format("-><Edit:[market:{0},SegmentName:{1},PS:{2},Vertical:{3}]>", 
                            newMarket,t.BusinessSegmentName,t.DapChannelCfg.ProductionStatus.ToString(),newVertical);
                        TraceLog(t, log);
                    }
                    else  //msn segment
                    {
                        //change market
                        string newMarket = ChooseMarket();
                        t.MarketId = ta.GetMarketIdByName(newMarket);
                        t.MarketName = newMarket;

                        //change name
                        t.BusinessSegmentName = t.BusinessSegmentName + "_2";

                        string log = string.Format("-><Edit:[market:{0},SegmentName:{1}]>",
                            newMarket, t.BusinessSegmentName);
                        TraceLog(t, log);
                    }
                }

                latestTasks = ta.ToSaveUpdateTasks(taskToEdit.ToArray());
            }

            //delete tasks
            if (taskToDelete.Count > 0)
            {
                ta.ToDeleteTasks(taskToDelete.ToArray());
                foreach (SdDeploymentTask t in taskToDelete)
                {
                    string log = string.Format("-><Delete>");
                    TraceLog(t, log);
                    Log(taskRoute[t.Id.ToString()]);
                    Log(string.Format("Delete task for new segment, task={0}", t.BusinessSegmentName));
                }
            }
        }

        /// <summary>
        /// 50% to complete
        /// 20% roll back
        /// 20% do nothing
        /// 10% delete
        /// </summary>
        public void State_Assigned()
        {
            Log(" *State_Assigned*");
            SdDeploymentTask[] latestTasks = ta.GetTask_Assigned();
            List<SdDeploymentTask> taskToComplete = new List<SdDeploymentTask>();
            List<SdDeploymentTask> taskToRollback = new List<SdDeploymentTask>();
            List<SdDeploymentTask> taskToDelete = new List<SdDeploymentTask>();

            foreach (SdDeploymentTask t in latestTasks)
            {
                int r = GetRnd();
                if (r < 60) taskToComplete.Add(t);
                if (r>=60 && r<75) taskToRollback.Add(t);
                if (r > 95) taskToDelete.Add(t);

                Log("   State_Assigned r=" + r);
            }

            //complete tasks, 70%segment, 30%abtesting
            if (taskToComplete.Count > 0)
            {
                List<SdDeploymentTask> completeWithSeg = new List<SdDeploymentTask>();
                List<SdDeploymentTask> completeWithAB = new List<SdDeploymentTask>();

                foreach (SdDeploymentTask t in taskToComplete)
                {
                    int r = GetRnd();
                    if (r < 70) completeWithSeg.Add(t);
                    if (r >= 70) completeWithAB.Add(t);

                }

                latestTasks = ta.ToCompleteTasks(completeWithSeg.ToArray());
                foreach (SdDeploymentTask t in latestTasks)
                {
                    string log = string.Format("-><Complete:Seg,[ModelName:{0},ModelId:{1}]>", t.ModelName, t.ModelRevisionId);
                    TraceLog(t, log);
                }

                latestTasks = ta.ToCompleteTasksWithAB(completeWithAB.ToArray());
                foreach (SdDeploymentTask t in latestTasks)
                {
                    string log = string.Format("-><Complete:AB,[ModelName:{0},ModelId:{1}]>", t.ModelName, t.ModelRevisionId);
                    TraceLog(t, log);
                }
            }

            //rollback tasks
            if (taskToRollback.Count > 0)
            {
                latestTasks = ta.ToRollbackTasks(taskToRollback.ToArray());
                foreach (SdDeploymentTask t in latestTasks)
                {
                    string log = string.Format("-><Rollback>");
                    TraceLog(t, log);
                    Log(string.Format("Rollback task for new segment, task={0}", t.BusinessSegmentName));
                }
            }

            //delete tasks
            if (taskToDelete.Count > 0)
            {
                ta.ToDeleteTasks(taskToDelete.ToArray());
                foreach (SdDeploymentTask t in taskToDelete)
                {
                    string log = string.Format("-><Delete>");
                    TraceLog(t, log);
                    Log(taskRoute[t.Id.ToString()]);
                    Log(string.Format("Delete task for new segment, task={0}", t.BusinessSegmentName));
                }
            }
        }

        /// <summary>
        /// 50% to allocate
        /// 20% roll back
        /// 20% do nothing
        /// 5% delete
        /// </summary>
        public void State_Completed()
        {
            Log(" *State_Completed*");
            SdDeploymentTask[] latestTasks = ta.GetTask_Completed();
            List<SdDeploymentTask> taskToAllocate = new List<SdDeploymentTask>();
            List<SdDeploymentTask> taskToRollback = new List<SdDeploymentTask>();
            List<SdDeploymentTask> taskToDelete = new List<SdDeploymentTask>();

            foreach (SdDeploymentTask t in latestTasks)
            {
                int r = GetRnd();
                if (r < 75) taskToAllocate.Add(t);
                if (r >= 75 && r < 95) taskToRollback.Add(t);
                if (r > 95) taskToDelete.Add(t);

                Log("   State_Completed r=" + r);

            }

            //allocate tags
            if (taskToAllocate.Count > 0)
            {
                latestTasks = ta.ToAllocateTags(taskToAllocate.ToArray());
                foreach (SdDeploymentTask t in latestTasks)
                {
                    string log = string.Format("-><Allocate:[UPStag:{0}]>", t.DapChannelCfg.UpsTag.Name);
                    TraceLog(t, log);
                    //Log(string.Format("Allocate tags for task of new segment, taskid={0}", t.Id));
                }
            }
            //rollback tasks
            if (taskToRollback.Count > 0)
            {
                latestTasks = ta.ToRollbackTasks(taskToRollback.ToArray());
                foreach (SdDeploymentTask t in latestTasks)
                {
                    string log = string.Format("-><Rollback>");
                    TraceLog(t, log);
                    //Log(string.Format("Rollback task for new segment, taskid={0}", t.Id));
                }
            }

            //delete tasks
            if (taskToDelete.Count > 0)
            {
                ta.ToDeleteTasks(taskToDelete.ToArray());
                foreach (SdDeploymentTask t in taskToDelete)
                {
                    string log = string.Format("-><Delete>");
                    TraceLog(t, log);
                    Log(taskRoute[t.Id.ToString()]);
                    //Log(string.Format("Delete task for new segment, taskid={0}", t.Id));
                }
            }
        }

        /// <summary>
        /// 80% to deploy
        /// 20% to delete
        /// </summary>
        public void State_AllocatedTag()
        {
            Log(" *State_AllocatedTag*");
            SdDeploymentTask[] latestTasks = ta.GetTask_AllocatedTag();
            List<SdDeploymentTask> taskToDeploy = new List<SdDeploymentTask>();
            List<SdDeploymentTask> taskToDelete = new List<SdDeploymentTask>();

            foreach (SdDeploymentTask t in latestTasks)
            {
                int r = GetRnd();
                if (r < 95) taskToDeploy.Add(t);
                if (r >= 95) taskToDelete.Add(t);
                Log("   State_AllocatedTag r=" + r);
            }

            //deploy tasks
            if (taskToDeploy.Count > 0)
            {
                latestTasks = ta.ToDeployTasks(taskToDeploy.ToArray());
                foreach (SdDeploymentTask t in latestTasks)
                {
                    string log = string.Format("-><Deploy>");
                    TraceLog(t, log);
                    Log(taskRoute[t.Id.ToString()]);
                    //Log(string.Format("Deploy task of new segment, taskid={0}", t.Id));
                }
            }

            //delete tasks
            if (taskToDelete.Count > 0)
            {
                ta.ToDeleteTasks(taskToDelete.ToArray());
                foreach (SdDeploymentTask t in taskToDelete)
                {
                    string log = string.Format("-><Delete>");
                    TraceLog(t, log);
                    Log(taskRoute[t.Id.ToString()]);
                    //Log(string.Format("Delete task for new segment, taskid={0}", t.Id));
                }
            }
        }

        public void State_Deployed()
        {
            //?
        }

        #region

        /// <summary>
        /// 80% DAP
        /// 20% MSN
        /// </summary>
        /// <returns></returns>
        public SdDeploymentTask CreateTask()
        {
            SdDeploymentTask retTask = new SdDeploymentTask();

            int rnd = GetRnd();
            if (rnd < 80) retTask = CreateDAPTask();
            if (rnd>=80) retTask = CreateMSNTask();

            return retTask;
        }

        /// <summary>
        /// 40% new
        /// 30% update
        /// 30% flush
        /// </summary>
        /// <returns></returns>
        public SdDeploymentTask CreateDAPTask()
        {
            SdDeploymentTask retTask = new SdDeploymentTask();

            int rnd = GetRnd();
            if (rnd < 40) retTask = CreateNewDAPTask();
            if (rnd >= 40 && rnd < 70) retTask = CreateUpdateDAPTask();
            if (rnd > 70) retTask = CreateFlushDAPTask();

            return retTask;
        }

        /// <summary>
        /// 50% live
        /// 50% test
        /// </summary>
        /// <returns></returns>
        public SdDeploymentTask CreateUpdateDAPTask()
        {
            SdDeploymentTask task = null;
            int r = GetRnd();
            if(r<50) task = ta.CreateTaskOfLiveSegment(ProductionStatus.Live, DeliveryChannel.DAP, null);
            else task = ta.CreateTaskOfLiveSegment(ProductionStatus.Test, DeliveryChannel.DAP, null);

            task.Action = SegmentAction.Update;
            return task;
        }

        /// <summary>
        /// 50% live
        /// 50% test
        /// </summary>
        /// <returns></returns>
        public SdDeploymentTask CreateFlushDAPTask()
        {
            SdDeploymentTask task = null;
            int r = GetRnd();
            if (r < 50) task = ta.CreateTaskOfLiveSegment(ProductionStatus.Live, DeliveryChannel.DAP, null);
            else task = ta.CreateTaskOfLiveSegment(ProductionStatus.Test, DeliveryChannel.DAP, null);

            task.Action = SegmentAction.Flush;
            return task;
        }

        public SdDeploymentTask CreateNewDAPTask()
        {
            SdDeploymentTask task = ta.CreateTaskOfNewDAPSegment(
                ta.GetName("Explore"), ChooseMarket(), "Ed", ChoosePS(), ChooseVertical(), ChooseAccessbilty());
            return task;
        }

        /// <summary>
        /// 40% new
        /// 30% update
        /// 30% flush
        /// </summary>
        /// <returns></returns>
        public SdDeploymentTask CreateMSNTask()
        {
            SdDeploymentTask retTask = new SdDeploymentTask();

            int rnd = GetRnd();
            if (rnd < 40) retTask = CreateNewMSNTask();
            if (rnd >= 40 && rnd < 70) retTask = CreateUpdateMSNTask();
            if (rnd > 70) retTask = CreateFlushMSNTask();

            return retTask;
        }

        public SdDeploymentTask CreateNewMSNTask()
        {
            SdDeploymentTask task = ta.CreateTaskOfNewMSNSegment("Ed",ChooseMarket(),ta.GetName("Explore"),Accessibility.Private);

            return task;
        }

        public SdDeploymentTask CreateUpdateMSNTask()
        {
            SdDeploymentTask task = null;
            task = ta.CreateTaskOfLiveSegment(DeliveryChannel.MSN, null);

            task.Action = SegmentAction.Update;
            return task;
        }

        public SdDeploymentTask CreateFlushMSNTask()
        {
            SdDeploymentTask task = null;
            task = ta.CreateTaskOfLiveSegment(DeliveryChannel.MSN, null);

            task.Action = SegmentAction.Flush;
            return task;
        }


        public string ChooseMarket()
        {
            //get available market
            ArrayList availableMarket = new ArrayList();
            ta.currentCapacityMap = ta.GetCapacitySnap();
            foreach (var pair in ta.currentCapacityMap)
            {
                CapacityInfo cap = pair.Value;
                if (cap.capacity > 0)
                {
                    availableMarket.Add(cap.countryCode);
                }
            }
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks + seed));
            seed += ra.Next(1,5);

            int rd = ra.Next(0, availableMarket.Count);
            return (string)availableMarket[rd];
        }

        public ProductionStatus ChoosePS()
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks + seed));
            seed += ra.Next(1, 5);
            int rd = ra.Next(1, 100);
            if (rd < 40) return ProductionStatus.Live;
            else return ProductionStatus.Test;
        }

        public string ChooseVertical()
        {
            Vertical[] verticals = ta.GetVerticals();
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks + seed));
            seed += ra.Next(1, 5);
            int rd = ra.Next(0, verticals.Count());
            return verticals[rd].Name;
        }

        public Accessibility ChooseAccessbilty()
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks + seed));
            seed += ra.Next(1, 5);
            int rd = ra.Next(1, 100);
            if (rd < 20) return Accessibility.Private;
            else return Accessibility.Public;
        }

        public int GetRnd()
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks+seed));
            seed += ra.Next(1, 5);
            return ra.Next(0, 100);
        }
        
        public void TraceLog(SdDeploymentTask task, string trace)
        {
            if (taskRoute.ContainsKey(task.Id.ToString()))
            {
                TraceLog(task.Id.ToString(), trace);
            }
            else
            {
                TraceLog(task.Id.ToString(), "("+task.BusinessSegmentName+")");
            }
        }

        public void TraceLog(string taskid, string trace)
        {
            if (taskRoute.ContainsKey(taskid))
            {
                taskRoute[taskid] += trace;
            }
            else
            {
                taskRoute[taskid] = trace;
            }

            Log(taskRoute[taskid]);
        }

        public void Log(string s)
        {
            Console.WriteLine(s);
        }

        public void getTask_Created()
        {

        }
        #endregion

    }

    public enum State
    {
        None = 0,
        Created = 1,
        Approved = 2,
        Assigned = 3,
        Completed = 4,
        Allocated = 5,
        Deployed = 6
    }

    public enum Action
    {
        AddNew = 1,
        AddUpdate = 2,
        AddFlush = 3,
        Delete = 4,
        Save = 5,
        Assign = 6,
        RollBack = 7,
        Complete = 8,
        Allocate = 9,
        Deploy = 10
    }


    class Route
    {
        public State from;
        public State to;
        public Action action;
        public string parameter;

        public Route(string pFrom, string pTo, string pAction, string pParameter)
        {
            this.initMember(pFrom, pTo, pAction, pParameter);
        }
        public Route(string init)
        {
            string[] input = init.Split(':');
            this.initMember(input[0], input[1], input[2], input[3]);
        }

        public void initMember(string pFrom, string pTo, string pAction, string pParameter)
        {
            from = (State)Enum.Parse(typeof(State), pFrom);
            to = (State)Enum.Parse(typeof(State), pTo);
            action = (Action)Enum.Parse(typeof(Action), pAction);
            parameter = pParameter;
        }
    }

    class Task
    {
        public int marketId;
        public string marketName;
        public string segmentName;
        public ProductionStatus productionstatus;
        public string ownerid;
        public string ownername;
        public SegmentAction action;
        public int verticalId;
        public string verticalName;
        public Accessibility accessbility;

        public Task(string marketname, string segmentname, string statusname, string ownername, string actionname, string verticalname, string accessbilityname)
        {

        }

    }
}
