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
namespace Microsoft.Advertising.BehaviorTargeting.Test.GTMTest.Performance
{
    class GTMRoute
    {
        public int state = 0;
        public State currState = State.None;
        static void Main(string[] args)
        {
            GTMRoute gtmr = new GTMRoute();
            Route route = new Route("None:Created:AddNew:[US,live,amit,Health,Public],");

            gtmr.Go(route);

        }

        public void Go(Route route)
        {
            do
            {
                if (currState == State.None) State_None();
            } while (currState!=State.None && currState!=State.Deployed);
        }

        public void State_None()
        {
            
        }

        
    }

    public enum State
    {
        None = 0,
        Created = 1,
        Approved = 2 ,
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
        RollBack  = 7,
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
