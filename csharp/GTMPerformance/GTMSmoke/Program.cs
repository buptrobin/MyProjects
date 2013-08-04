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
//using Microsoft.AdCenter.BehaviorTargeting.SegmentStudio.BLL;
using Microsoft.Advertising.BehaviorTargeting.SegmentStudio.StreamlineDeployment;
using Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment;
using SrTaskStatus = Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment.TaskStatus;
using Microsoft.Advertising.BehaviorTargeting.Test.GTMTest.Performance;

namespace Microsoft.Advertising.BehaviorTargeting.Test.GTMTest.Performance
{
    class GTMSmoke
    {
        private TestContext testContextInstance;
        private GTMTestHelper ta;
        public static int TASKNUMBER = 1;

        GTMSmoke()
        {
            ta = new GTMTestHelper(testContextInstance);
        }
        static void Main(string[] args)
        {
            
            Console.WriteLine("GTM SMOKE TEST STARTING!");
            GTMSmoke smoke = new GTMSmoke();
            try
            {
                smoke.DeploySegment();
                Console.WriteLine("GTM SMOKE TEST PASS!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("GTM SMOKE TEST FAIL!");
            }

            
        }

        public void DeploySegment()
        {

            List<SdDeploymentTask> listTask = new List<SdDeploymentTask>();

            ta.currentCapacityMap = ta.GetCapacitySnap();

            for (int i = 0; i < TASKNUMBER; i++)
            {
                SdDeploymentTask newTask = ta.CreateQuickNewTaskLive();
                listTask.Add(newTask);
            }

            //assign tasks
            SdDeploymentTask[] latestTasks = listTask.ToArray();
            latestTasks = ta.ToSaveNewTasks(latestTasks);
            Console.WriteLine("...create new task done");
            latestTasks = ta.ToAssignTasks(latestTasks);
            Console.WriteLine("...assign task to user done");
            latestTasks = ta.ToCompleteTasks(latestTasks);
            Console.WriteLine("...complete task done");
            //allocate tags
            latestTasks = ta.ToAllocateTags(latestTasks);
            Console.WriteLine("...allocate AdExpert tag done");
            //deploy these tags
            latestTasks = ta.ToDeployTasks(latestTasks);
            Console.WriteLine("...deploy segment done");
        }

        public void Log(string s)
        {
            Console.WriteLine(s);
        }

    }
}
