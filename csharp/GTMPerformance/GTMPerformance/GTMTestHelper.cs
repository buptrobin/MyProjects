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
using Msn.Ads.AdExpert.Management;
using Msn.Ads.AdExpert.RegistryAccess;
using Msn.Ads.AdExpert.Authentication;
using Msn.Ads.AdExpert.Utilities;
using SSTestLibrary;


namespace Microsoft.Advertising.BehaviorTargeting.Test.GTMTest.Performance
{
    public class GTMTestHelper
    {
        public string SSDBConn = "SSDBConn";
        public string SRDBConn = "BTSRDBConn";
        private TestContext testContextInstance;
        private IStreamliningDeployment ssdSvc = PseudoChannelCenter.GetChannel<IStreamliningDeployment>();
        int totalCapacity = 0;
        int test4ReuseCapacity = 0;
        int live4ReuseCapacity = 0;
        int noUsedCapacity = 0;
        public Dictionary<int, int> marketIdDistributionChannelIdMap = new Dictionary<int, int>();
        public Dictionary<int, CapacityInfo> currentCapacityMap = new Dictionary<int, CapacityInfo>();
        int[] marketids = { 9, 14, 20, 32, 53, 66, 93, 96, 119, 129, 139, 170, 175, 176, 189, 191 };
        SdDeploymentTask[] currentLiveSegmentTasks;
        Dictionary<int, SdDeploymentTask> currentLiveSegmentTaskMap;

        public string userNameEd = "Ed";
        public string userIdEd = "";
        public string projectName = "BT-CA";

        //fetch the current live segment tasks
        public void fetchLiveSegmentTasks()
        {
            currentLiveSegmentTaskMap = new Dictionary<int, SdDeploymentTask>();

            List<SdDeploymentTask> list = new List<SdDeploymentTask>();
            SdDeploymentTask[] tasks = ssdSvc.GetDeploymentTasksOfLiveSegments(DeliveryChannel.MSN, null);
            //Console.WriteLine("MSN, num={0}",  tasks.Length);
            foreach (SdDeploymentTask t in tasks)
            {
                currentLiveSegmentTaskMap[t.AuthoringId.Value] = t;
            }
            for (int i = 0; i < marketids.Length; i++)
            {
                tasks = ssdSvc.GetDeploymentTasksOfLiveSegments(DeliveryChannel.DAP, marketids[i]);
                //Console.WriteLine("market{0}, DAP, num={1}", marketids[i], tasks.Length);
                foreach (SdDeploymentTask t in tasks)
                {
                    currentLiveSegmentTaskMap[t.AuthoringId.Value] = t;
                }
            }
        }

        public SdDeploymentTask getLiveSegmentTaskBySegmentId(string segmentId)
        {
            string sqlTxt = string.Format("select [AuthoringSegmentId] from Segment where SegmentId='{0}'", segmentId);
            //Console.WriteLine(sqlTxt);
            int authId = GetIntFromDB(sqlTxt, "SSDBConn");
            //Console.WriteLine("authId=" + authId);

            if (currentLiveSegmentTaskMap != null)
            {
                return currentLiveSegmentTaskMap[authId];
            }
            return null;
        }



        public int Test4ReuseCapacity
        {
            get { return test4ReuseCapacity; }
            set { test4ReuseCapacity = value; }
        }

        public int Live4ReuseCapacity
        {
            get { return live4ReuseCapacity; }
            set { live4ReuseCapacity = value; }
        }

        public int NoUsedCapacity
        {
            get { return noUsedCapacity; }
            set { noUsedCapacity = value; }
        }

        public int TotalCapacity
        {
            get { return totalCapacity; }
            set { totalCapacity = value; }
        }



        public GTMTestHelper(TestContext context)
        {
            testContextInstance = context;

            this.marketIdDistributionChannelIdMap[191] = 1; // US
            this.marketIdDistributionChannelIdMap[96] = 2;   // JP
            this.marketIdDistributionChannelIdMap[189] = 3; // UK
            this.marketIdDistributionChannelIdMap[9] = 4; // AU
            this.marketIdDistributionChannelIdMap[72] = 5; // DE
            this.marketIdDistributionChannelIdMap[66] = 6; // FR
            this.marketIdDistributionChannelIdMap[32] = 7; // CA - EN
            // this.marketIdDistributionChannelIdMap[32] = 8; // CA - FR
            this.marketIdDistributionChannelIdMap[100] = 9; // KR
            this.marketIdDistributionChannelIdMap[175] = 10; // SE
            this.marketIdDistributionChannelIdMap[134] = 11; // NZ
            this.marketIdDistributionChannelIdMap[164] = 12; // SG
            this.marketIdDistributionChannelIdMap[112] = 13; // MY
            this.marketIdDistributionChannelIdMap[10] = 14; // AT
            this.marketIdDistributionChannelIdMap[170] = 15; // ES
            // this.marketIdDistributionChannelIdMap[16] =  
            this.marketIdDistributionChannelIdMap[119] = 17; // MX
            this.marketIdDistributionChannelIdMap[40] = 18; // TW
            this.marketIdDistributionChannelIdMap[39] = 19; // CN
            this.marketIdDistributionChannelIdMap[201] = 20; // HK
            this.marketIdDistributionChannelIdMap[129] = 21; // NL
            this.marketIdDistributionChannelIdMap[20] = 22; // BR
            this.marketIdDistributionChannelIdMap[93] = 23; // IT
            this.marketIdDistributionChannelIdMap[176] = 24; //  CH
            // this.marketIdDistributionChannelIdMap[176] = 25;// CH
            this.marketIdDistributionChannelIdMap[14] = 26; // BE
            // this.marketIdDistributionChannelIdMap[14] = 27; // BE
            this.marketIdDistributionChannelIdMap[65] = 28; // FI
            this.marketIdDistributionChannelIdMap[53] = 29; // DK
            this.marketIdDistributionChannelIdMap[139] = 30; // NO
            this.marketIdDistributionChannelIdMap[168] = 31; // ZA
            this.marketIdDistributionChannelIdMap[90] = 32; // IN
            this.marketIdDistributionChannelIdMap[156] = 33; // RU
            // this.marketIdDistributionChannelIdMap[191] = 34; // US

            userIdEd = TestRoutines.GetUserIdByName(userNameEd);
        }

        public TaxonomySegment createTaxSegment(string userId, string projectName, string segmentName)
        {
            //string testkw = "bmw";
            string taxSegmentId = string.Empty;
            string projectId = TestRoutines.GetProjectIdByName(userId, projectName);
            int languageId = 1033;
            int taxId = 1;
            ICategory categorySvc = PseudoChannelCenter.GetChannel<ICategory>();

            var allNodes = categorySvc.GetAllTaxonomy(taxId, languageId);
            Assert.IsTrue(allNodes.Count > 128);

            string[] nodeIds = new string[3];
            for (int i = 0; i < 3; ++i)
            {
                nodeIds[i] = allNodes[i].Id.ToString() + ":3";
            }

            var taxSeg = TestRoutines.CreateTaxonomySegment(
                    segmentName,
                    DateTime.UtcNow,
                    projectId,
                    "13 , 100",
                    3,
                    28,
                    10,
                    40,
                    "A",
                    "Australia",
                    nodeIds,
                    "No description",
                    ref taxSegmentId
                 );
            List<SdDeploymentTask> listNewTask = new List<SdDeploymentTask>();


            return taxSeg;
        }

        public TaxonomySegment createTaxSegment(string userId, string projectName, string projectId, string segmentName)
        {
            //string testkw = "bmw";
            string taxSegmentId = "";
            //string projectId = TestRoutines.GetProjectIdByName(userId, projectName);
            int languageId = 1033;
            int taxId = 1;
            ICategory categorySvc = PseudoChannelCenter.GetChannel<ICategory>();

            var allNodes = categorySvc.GetAllTaxonomy(taxId, languageId);
            //Assert.IsTrue(allNodes.Count > 128);

            string[] nodeIds = new string[3];
            for (int i = 0; i < 3; ++i)
            {
                nodeIds[i] = allNodes[i].Id.ToString() + ":3";
            }

            var taxSeg = TestRoutines.CreateTaxonomySegment(
                    segmentName,
                    DateTime.UtcNow,
                    projectId,
                    "13 , 100",
                    3,
                    28,
                    10,
                    40,
                    "A",
                    "Australia",
                    nodeIds,
                    "No description",
                    ref taxSegmentId
                 );

            return taxSeg;
        }

        /// <summary>
        /// get the request  revision id
        /// </summary>
        /// <param name="reqId">ABTesting request id</param>
        /// <param name="isPrimary">1: primary, 0: not primary</param>
        /// <returns>segment model revision id</returns>
        public int GetABSegRevisionId(string reqId, int isPrimary)
        {
            //string sqlTxt = string.Format("SELECT las.ModelRevisionId FROM LiveABTestingSegment las JOIN LiveABTestingRequest lar ON las.RequestRevisionId=lar.RevisionId WHERE lar.RequestId='{0}' AND las.IsPrimary={1}"
            //    , reqId, isPrimary);

            string sqlTxt = string.Format("select [RevisionId] from [LiveABTestingRequest] where RequestId='{0}'", reqId);
            return GetIntFromDB(sqlTxt, "BTSRDBConn");
        }

        /// <summary>
        /// get the request segment authoring id
        /// </summary>
        /// <param name="reqId">ABTesting request id</param>
        /// <param name="isPrimary">1: primary, 0: not primary</param>
        /// <returns>segment authoring id</returns>
        public int GetABSegAuthoringId(string reqId, int isPrimary)
        {
            string sqlTxt = string.Format("SELECT las.AuthoringId FROM LiveABTestingSegment las JOIN LiveABTestingRequest lar ON las.RequestRevisionId=lar.RevisionId WHERE lar.RequestId='{0}' AND las.IsPrimary={1}"
                , reqId, isPrimary);
            return GetIntFromDB(sqlTxt, "BTSRDBConn");
        }

        public int GetLatestSegModelRevisionId(int authId)
        {
            string sqlTxt = string.Format("SELECT RevisionId FROM SegmentModel WHERE AuthoringId={0} ORDER BY RevisionId DESC", authId);
            return GetIntFromDB(sqlTxt, "BTSRDBConn");
        }



        public Dictionary<int, CapacityInfo> GetCapacitySnap()
        {
            Dictionary<int, CapacityInfo> map = new Dictionary<int, CapacityInfo>();
            totalCapacity = 0;
            //get capacity
            string sqlTxt = "SELECT dcsvm.[MarketId] ,dcsvm.[Capacity],m.CountryCode FROM [BTSRDB].[dbo].[DeliveryChannelSettingValueMarketCapacity] dcsvm JOIN Market m on dcsvm.MarketId=m.Id";
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting("BTSRDBConn")))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        CapacityInfo cap = new CapacityInfo();
                        cap.marketId = sdr.GetInt32(0);
                        cap.capacity = sdr.GetInt32(1);
                        cap.countryCode = sdr.GetString(2);

                        map.Add(cap.marketId, cap);

                        totalCapacity += cap.capacity;
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }
            }

            //get the used for each market
            foreach (var pair in map)
            {
                sqlTxt = string.Format("select COUNT(*) FROM DAPDeliveryChannelSettingValue WHERE IsAvailable=0 and KeyId=1 and MarketId={0}", pair.Key);
                map[pair.Key].used = GetIntFromDB(sqlTxt, SRDBConn);
            }

            //get reusable
            sqlTxt = string.Format("select COUNT(*) FROM DAPDeliveryChannelSettingValue WHERE IsTest=1 and IsAvailable=1 and KeyId=1");
            test4ReuseCapacity = GetIntFromDB(sqlTxt, SRDBConn);

            sqlTxt = string.Format("select COUNT(*) FROM DAPDeliveryChannelSettingValue WHERE IsTest=0 and IsAvailable=1 and KeyId=1");
            live4ReuseCapacity = GetIntFromDB(sqlTxt, SRDBConn);

            sqlTxt = string.Format("select COUNT(*) FROM DAPDeliveryChannelSettingValue WHERE IsAvailable=0 and KeyId=1");
            int used = GetIntFromDB(sqlTxt, SRDBConn);

            noUsedCapacity = totalCapacity - used - test4ReuseCapacity - live4ReuseCapacity;

            return map;
        }

        /// <summary>
        /// Get market id of a new task
        /// no mater which market
        /// </summary>
        /// <param name="IsLive">true: for live segment, false: for test segment</param>
        /// <returns></returns>
        public int GetAvailableMarketId()
        {
            Dictionary<int, CapacityInfo> map = GetCapacitySnap();

            foreach (KeyValuePair<int, CapacityInfo> kvp in map)
            {
                int capacity = map[kvp.Key].capacity;
                int used = map[kvp.Key].used;

                if (capacity > used) return kvp.Key;
            }
            return 0;
        }
        public bool CheckCapacity(SdDeploymentTask[] tasks, Dictionary<int, CapacityInfo> mapCapacity)
        {
            Dictionary<int, CapacityInfo> mapCapacityNew = GetCapacitySnap();

            foreach (SdDeploymentTask t in tasks)
            {
                if (t.DeliveryChannelConfiguration.DeliveryChannel == DeliveryChannel.DAP)
                {
                    int upstagid = t.DapChannelCfg.UpsTag.Id;
                    int axtagid = t.DapChannelCfg.AdExpertTag.Id;
                    //only new task will impact the tag and capacity
                    if (t.Action == SegmentAction.New)
                    {
                        mapCapacity[t.MarketId].used++;
                    }

                    if (t.Action == SegmentAction.Flush)
                    {
                        if (t.TaskStatus == SrTaskStatus.Deployed)
                        {
                            mapCapacity[t.MarketId].used--;
                        }
                    }
                }
            }
            //check the capacity
            foreach (KeyValuePair<int, CapacityInfo> kvp in mapCapacity)
            {
                int mid = kvp.Key;
                if (mapCapacity[mid].used != mapCapacityNew[mid].used || mapCapacity[mid].reusable != mapCapacityNew[mid].reusable)
                {
                    Log("marketid=" + mid);
                    Log("CheckTagAndCapacity Capaicity mapCapacity[mid].used={0}, mapCapacityNew[mid].used={1}, mapCapacity[mid].reusable={2}, mapCapacityNew[mid].reusable={3}"
                        , mapCapacity[mid].used, mapCapacityNew[mid].used, mapCapacity[mid].reusable, mapCapacityNew[mid].reusable);
                    return false;
                }
            }
            return true;
        }

        public bool CheckTaskStatus(Guid taskId, SrTaskStatus status)
        {
            string sqlTxt = string.Format("SELECT * FROM DeploymentTask WHERE Id='{0}' AND TaskStatusId={1}", taskId, (int)status);
            return (CheckExistInSRDB(sqlTxt));
        }

        public bool CheckExistInSRDB(string sqlTxt)
        {
            return (CheckExistInDB(sqlTxt, "BTSRDBConn"));
        }

        public bool CheckExistInSSDB(string sqlTxt)
        {
            return (CheckExistInDB(sqlTxt, "SSDBConn"));
        }

        public bool CheckExistInDB(string sqlTxt, string dbConn)
        {
            Log(sqlTxt);

            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting(dbConn)))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    if (sdr.HasRows) return true;
                }
                finally
                {
                    scd.Dispose();
                }
                return false;
            }
        }

        public bool CheckTagAndCapacity(SdDeploymentTask[] tasks, Dictionary<int, DAPDeliveryChannelSettingValue> mapDAPOrig, Dictionary<int, CapacityInfo> mapCapacity)
        {
            Dictionary<int, DAPDeliveryChannelSettingValue> mapDAP = GetDAPSnap();
            Dictionary<int, CapacityInfo> mapCapacityNew = GetCapacitySnap();

            foreach (SdDeploymentTask t in tasks)
            {
                if (t.DeliveryChannelConfiguration.DeliveryChannel == DeliveryChannel.DAP)
                {
                    int upstagid = t.DapChannelCfg.UpsTag.Id;
                    int axtagid = t.DapChannelCfg.AdExpertTag.Id;
                    Log("upstagid={0}, axtagid={1}", upstagid, axtagid);
                    //only new task will impact the tag and capacity
                    if (t.Action == SegmentAction.New)
                    {
                        //check available
                        if (!(mapDAPOrig[upstagid].IsAvailable == true && mapDAPOrig[axtagid].IsAvailable == true)
                                && mapDAP[upstagid].IsAvailable == false && mapDAP[axtagid].IsAvailable == false)
                        {
                            Log("CheckTagAndCapacity UPSTag={0}, AXTAG={1}", upstagid, axtagid);
                            Log("Check Live Tag Fail mapDAPOrig.UPS.isavailabe={0} mapDAPOrig.AX.isavailabe={1} mapDAP.UPS.isavailabe={2} mapDAPOrig.AX.isavailabe={3}",
                                mapDAPOrig[upstagid].IsAvailable, mapDAPOrig[axtagid].IsAvailable,
                                mapDAP[upstagid].IsAvailable, mapDAP[axtagid].IsAvailable);
                            return false;
                        }
                        //live
                        if (t.DapChannelCfg.ProductionStatus == ProductionStatus.Live)
                        {
                            //live cannot use test tag
                            if (mapDAPOrig[upstagid].IsTest == true || mapDAPOrig[axtagid].IsTest == true)
                            {
                                Log("CheckTagAndCapacity mapDAPOrig.UPS.istest={0}, mapDAPOrig.AX.istest={1}", mapDAPOrig[upstagid].IsTest, mapDAPOrig[axtagid].IsTest);
                                return false;
                            }
                            mapCapacity[t.MarketId].used++;
                            Assert.IsTrue(mapCapacity[t.MarketId].capacity >= mapCapacity[t.MarketId].used, "capacity < used");
                        }
                        //test
                        if (t.DapChannelCfg.ProductionStatus == ProductionStatus.Test)
                        {
                            if (mapDAPOrig[upstagid].IsTest == false || mapDAPOrig[axtagid].IsTest == false)
                            {
                                Log("CheckTagAndCapacity mapDAPOrig.UPS.istest={0}, mapDAPOrig.AX.istest={1}", mapDAPOrig[upstagid].IsTest, mapDAPOrig[axtagid].IsTest);
                                return false;
                            }

                            mapCapacity[t.MarketId].used++;
                            Assert.IsTrue(mapCapacity[t.MarketId].capacity >= mapCapacity[t.MarketId].used, "capacity < used");
                            //if (mapDAPOrig[upstagid].IsTest == true) mapCapacity[t.MarketId].reusable--;
                        }
                    }
                    if (t.Action == SegmentAction.Flush)
                    {
                        //After flush test the AX tag should be reusable
                        if (t.DapChannelCfg.ProductionStatus == ProductionStatus.Test)
                        {
                            if (mapDAPOrig[upstagid].IsAvailable != false || mapDAPOrig[axtagid].IsAvailable != false
                                    || mapDAP[upstagid].IsAvailable != true || mapDAP[axtagid].IsAvailable != true)
                            {
                                Log("CheckTagAndCapacity FLUSH UPSTag={0}, AXTAG={1}", upstagid, axtagid);
                                Log("Check Test Tag Fail mapDAPOrig.UPS.isavailabe={0} mapDAPOrig.AX.isavailabe={1} mapDAP.UPS.isavailabe={2} mapDAPOrig.AX.isavailabe={3}",
                                    mapDAPOrig[upstagid].IsAvailable, mapDAPOrig[axtagid].IsAvailable,
                                    mapDAP[upstagid].IsAvailable, mapDAP[axtagid].IsAvailable);
                                return false;
                            }
                        }
                        //After flush live, the AX tag should not be reusable 
                        if (t.DapChannelCfg.ProductionStatus == ProductionStatus.Live)
                        {
                            if (mapDAPOrig[upstagid].IsAvailable != false || mapDAPOrig[axtagid].IsAvailable != false
                                    || mapDAP[upstagid].IsAvailable != true || mapDAP[axtagid].IsAvailable != false)
                            {
                                Log("CheckTagAndCapacity FLUSH UPSTag={0}, AXTAG={1}", upstagid, axtagid);
                                Log("Check Live Tag Fail mapDAPOrig.UPS.isavailabe={0} mapDAPOrig.AX.isavailabe={1} mapDAP.UPS.isavailabe={2} mapDAP.AX.isavailabe={3}",
                                    mapDAPOrig[upstagid].IsAvailable, mapDAPOrig[axtagid].IsAvailable,
                                    mapDAP[upstagid].IsAvailable, mapDAP[axtagid].IsAvailable);
                                return false;
                            }
                        }

                        mapCapacity[t.MarketId].used--;
                        Assert.IsTrue(0 <= mapCapacity[t.MarketId].used, "used < 0");
                        //if (mapDAPOrig[upstagid].IsTest == true) mapCapacity[t.MarketId].reusable++;
                    }

                    if (t.Action == SegmentAction.Update)
                    {
                        if (t.DapChannelCfg.ProductionStatus == ProductionStatus.Live)
                        {
                            if (mapDAP[upstagid].IsAvailable == true || mapDAP[axtagid].IsAvailable == true)
                            {
                                Log("CheckTagAndCapacity UPSTag={0}, AXTAG={1}", upstagid, axtagid);
                                Log("Check Live Tag Fail mapDAPOrig.UPS.isavailabe={0} mapDAPOrig.AX.isavailabe={1} mapDAP.UPS.isavailabe={2} mapDAP.AX.isavailabe={3}",
                                    mapDAPOrig[upstagid].IsAvailable, mapDAPOrig[axtagid].IsAvailable,
                                    mapDAP[upstagid].IsAvailable, mapDAP[axtagid].IsAvailable);
                                return false;
                            }

                            if (mapDAP[upstagid].IsTest != false || mapDAP[axtagid].IsTest != false)
                            {
                                Log("The mapDAP[upstagid].IsTest!=false || mapDAP[axtagid].IsTest!=false");
                                return false;
                            }
                        }

                    }
                }
            }
            //check the capacity
            foreach (KeyValuePair<int, CapacityInfo> kvp in mapCapacity)
            {
                int mid = kvp.Key;
                if (mapCapacity[mid].used != mapCapacityNew[mid].used)
                {
                    Log("market id={0}", mid);
                    Log("CheckTagAndCapacity Capaicity mapCapacity[mid].used={0}, mapCapacityNew[mid].used={1}, mapCapacity[mid].reusable={2}, mapCapacityNew[mid].reusable={3}"
                        , mapCapacity[mid].used, mapCapacityNew[mid].used, mapCapacity[mid].reusable, mapCapacityNew[mid].reusable);
                    return false;
                }
            }
            return true;
        }

        public bool CheckDAP(Dictionary<int, DAPDeliveryChannelSettingValue> map)
        {
            string sqlTxt = String.Format("select id, marketId, name, IsTest, IsAvailable from DAPDeliveryChannelSettingValue");

            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting("BTSRDBConn")))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    int id;
                    //int marketid;
                    //string name;
                    //int istest;
                    //int isavailable;
                    while (sdr.Read())
                    {
                        id = sdr.GetInt32(0);
                        DAPDeliveryChannelSettingValue d = map[id];
                        if ((d.MarketId != sdr.GetInt32(1))
                            || (d.Name != sdr.GetString(2))
                            || (d.IsTest != GetBoolFromInt(sdr.GetInt32(3)))
                            || (d.IsAvailable != GetBoolFromInt(sdr.GetInt32(4))))
                        {
                            Log("CheckDAP " + d.ToString());
                            return false;
                        }
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }
            }
            return true;
        }
        public Dictionary<int, DAPDeliveryChannelSettingValue> GetDAPSnap()
        {
            string sqlTxt = String.Format("select id, keyId, marketId, name, IsTest, IsAvailable from DAPDeliveryChannelSettingValue");
            Dictionary<int, DAPDeliveryChannelSettingValue> map = new Dictionary<int, DAPDeliveryChannelSettingValue>();

            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting("BTSRDBConn")))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        DAPDeliveryChannelSettingValue d = new DAPDeliveryChannelSettingValue();
                        d.Id = sdr.GetInt32(0);
                        d.Key = (DeliveryChannelSettingKey)sdr.GetInt32(1);
                        if (sdr.IsDBNull(2)) d.MarketId = null; else d.MarketId = sdr.GetInt32(2);
                        d.Name = sdr.GetString(3);
                        if (sdr.IsDBNull(4)) d.IsTest = null; else d.IsTest = sdr.GetBoolean(4);
                        d.IsAvailable = sdr.GetBoolean(5);
                        map.Add(d.Id, d);
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }
            }
            return map;
        }

        public int GetIntFromDB(string sqlTxt, string dbConn)
        {
            Log(sqlTxt);
            int ret = 0;
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting(dbConn)))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        ret = sdr.GetInt32(0);
                    }
                }
                finally { scd.Dispose(); }
            }
            return ret;
        }

        public string GetStringFromDB(string sqlTxt, string dbConn)
        {
            Log(sqlTxt);
            string ret = null;
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting(dbConn)))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        ret = sdr.GetValue(0).ToString();
                    }
                }
                finally { scd.Dispose(); }
            }
            return ret;
        }

        public string GetName(string preName)
        {
            string s = DateTime.Now.Ticks.ToString();

            return preName + s.Substring(s.Length - 6);

        }

        public string LibGetUserIdByName(string userName)
        {
            string sqlTxt = string.Format("SELECT UserId from [User] WHERE Username='{0}'", userName);
            string userId = GetStringFromDB(sqlTxt, SSDBConn);
            return userId;
        }

        public string LibGetProjectIdByName(string projectName, string userId)
        {
            string sqlTxt = string.Format("select ProjectId from Project where ProjectName='{0}' and OwnerId='{1}'"
                    , projectName, userId);
            string projectId = GetStringFromDB(sqlTxt, SSDBConn);

            return projectId;
        }

        public SdDeploymentTask CreateTaskOfNewMSNSegment(string ownerName, string name, int marketId, string segmentName)
        {
            return CreateTaskOfNewMSNSegment(ownerName, marketId, segmentName);
        }

        public SdDeploymentTask CreateTaskOfNewMSNSegment(string ownerName, string marketName, string segmentName, Accessibility accessbility)
        {
            int marketId = GetMarketIdByName(marketName);
            SdDeploymentTask task = CreateTaskOfNewMSNSegment(ownerName, marketId, segmentName);
            task.Accessibility = accessbility;

            return task;
        }

        public SdDeploymentTask CreateTaskOfNewMSNSegment(string ownerName, int marketId, string segmentName)
        {
            string creatorName = "Admin";

            SdDeploymentTask task = new SdDeploymentTask();
            task.Id = System.Guid.NewGuid();
            task.Action = SegmentAction.New;
            task.BusinessSegmentName = segmentName;
            task.CreatedTime = DateTime.UtcNow;
            task.LastUpdatedTime = task.CreatedTime;
            task.OwnerId = new Guid(LibGetUserIdByName(ownerName));//ew Guid(TestRoutines.GetUserIdByName(ownerName));
            task.OwnerName = ownerName;
            task.CreatorName = creatorName;
            task.CreatorId = new Guid(LibGetUserIdByName(creatorName));//new Guid(TestRoutines.GetUserIdByName(creatorName));
            task.MarketId = marketId;
            task.MarketName = GetMarketNameById(marketId);
            task.TaskStatus = SrTaskStatus.Approved;
            task.DeliveryChannelConfiguration = new DeliveryChannelConfiguration() { DeliveryChannel = DeliveryChannel.MSN };
            task.VerticalId = 0;
            task.DapChannelCfg = null;
            task.Accessibility = Accessibility.Public;

            return task;

        }

        public SdDeploymentTask CreateTaskOfNewDAPSegment(string segmentName, string marketName, string ownerName, ProductionStatus ps, string verticalName, Accessibility accessbility)
        {
            string creatorName = "Admin";
            int marketId = GetMarketIdByName(marketName);

            SdDeploymentTask task = new SdDeploymentTask();
            task.Id = System.Guid.NewGuid();
            task.Action = SegmentAction.New;
            task.BusinessSegmentName = segmentName;
            task.CreatedTime = DateTime.UtcNow;
            task.LastUpdatedTime = task.CreatedTime;
            task.OwnerId = new Guid(LibGetUserIdByName(ownerName));//ew Guid(TestRoutines.GetUserIdByName(ownerName));
            task.OwnerName = ownerName;
            task.CreatorName = creatorName;
            task.CreatorId = new Guid(LibGetUserIdByName(creatorName));//new Guid(TestRoutines.GetUserIdByName(creatorName));
            task.VerticalId = GetVerticalIdByName(verticalName);//AUTO
            task.MarketId = marketId;
            task.MarketName = GetMarketNameById(marketId);
            task.TaskStatus = SrTaskStatus.Approved;
            task.DapChannelCfg = new DAPDeliveryChannelConfiguration()
            {
                DeliveryChannel = DeliveryChannel.DAP,
                ProductionStatus = ps,
                AdExpertFriendlyName = string.Empty,
                AdExpertTagDescription = string.Empty,
                NewAdExpertFriendlyName = string.Empty,
            };
            task.DeliveryChannelConfiguration = task.DapChannelCfg;
            task.Accessibility = accessbility;

            if (ps == ProductionStatus.Test) task.DapChannelCfg.AdExpertFriendlyName = "";

            return task;
        }

        public SdDeploymentTask CreateTaskOfNewSegment(string ownerName, string creatorName, int marketId, ProductionStatus ps, string segmentName, string AXFN)
        {
            SdDeploymentTask task = new SdDeploymentTask();
            task.Id = System.Guid.NewGuid();
            task.Action = SegmentAction.New;
            task.BusinessSegmentName = segmentName;
            task.CreatedTime = DateTime.UtcNow;
            task.LastUpdatedTime = task.CreatedTime;
            task.OwnerId = new Guid(LibGetUserIdByName(ownerName));//ew Guid(TestRoutines.GetUserIdByName(ownerName));
            task.OwnerName = ownerName;
            task.CreatorName = creatorName;
            task.CreatorId = new Guid(LibGetUserIdByName(creatorName));//new Guid(TestRoutines.GetUserIdByName(creatorName));
            task.VerticalId = 1;//AUTO
            task.MarketId = marketId;
            task.MarketName = GetMarketNameById(marketId);
            task.TaskStatus = SrTaskStatus.Approved;
            task.DapChannelCfg = new DAPDeliveryChannelConfiguration()
            {
                DeliveryChannel = DeliveryChannel.DAP,
                ProductionStatus = ps,
                AdExpertFriendlyName = string.Empty,
                AdExpertTagDescription = string.Empty,
                NewAdExpertFriendlyName = string.Empty,
            };
            task.DeliveryChannelConfiguration = task.DapChannelCfg;
            task.Accessibility = Accessibility.Public;

            if (ps == ProductionStatus.Test) task.DapChannelCfg.AdExpertFriendlyName = "";

            return task;

        }

        public SdDeploymentTask CreateTaskOfLiveSegment(SdDeploymentTask t)
        {
            SdDeploymentTask task = new SdDeploymentTask();
            task.Id = Guid.NewGuid();
            task.SegmentId = t.SegmentId;
            //task.Action = SegmentAction.Flush;
            task.OwnerId = t.OwnerId;
            task.OwnerName = t.OwnerName;
            task.CreatorId = new Guid(LibGetUserIdByName("Admin"));
            task.VerticalId = t.VerticalId;
            task.MarketId = t.MarketId;
            task.MarketName = t.MarketName;
            task.DapChannelCfg = t.DapChannelCfg;
            task.DeliveryChannelConfiguration = t.DeliveryChannelConfiguration;
            task.BusinessSegmentName = t.BusinessSegmentName;
            task.CreatedTime = DateTime.UtcNow;
            task.DeployTime = t.DeployTime;
            task.TaskStatus = SrTaskStatus.Approved;
            task.ModelRevisionId = t.ModelRevisionId;
            task.AuthoringId = t.AuthoringId;

            return task;
        }

        public string GetMarketNameById(int marketId)
        {
            //IStreamliningDeployment ssdSvc = PseudoChannelCenter.GetChannel<IStreamliningDeployment>();
            Market[] markets = ssdSvc.GetMarkets();
            foreach (Market m in markets)
            {
                if (m.Id == marketId) return m.Name;
            }
            throw new Exception("cannot find market with id=" + marketId);
        }

        public int GetMarketIdByName(string marketName)
        {
            //IStreamliningDeployment ssdSvc = PseudoChannelCenter.GetChannel<IStreamliningDeployment>();
            Market[] markets = ssdSvc.GetMarkets();
            foreach (Market m in markets)
            {
                if (m.Name == marketName) return m.Id;
            }
            throw new Exception("cannot find market with name=" + marketName);
        }

        public int GetVerticalIdByName(string verticalName)
        {
            Vertical[] verticals = ssdSvc.GetVerticals();
            foreach (Vertical v in verticals)
            {
                if (v.Name == verticalName) return v.Id;
            }
            throw new Exception("cannot find vertical with name=" + verticalName);
        }

        public Vertical[] GetVerticals()
        {
            return ssdSvc.GetVerticals();
        }

        public Market[] GetMarkets()
        {
            return ssdSvc.GetMarkets();
        }

        public User[] GetUsers()
        {
            return ssdSvc.GetUsers();
        }

        public bool GetBoolFromInt(int value)
        {
            //if (value == null) return null;
            if (value == 0) return false;

            return true;
        }

        #region Task Related

        public SdDeploymentTask[] ToSaveNewTasks(SdDeploymentTask[] tasks)
        {
            Log("================TO SAVE NEW TASKS");
            ssdSvc.UpdateDeploymentTasks(new SdDeploymentTask[] { }, new SdDeploymentTask[] { }, tasks.ToArray());
            Assert.IsTrue(CheckBatchTaskStatus(tasks.ToArray(), SrTaskStatus.Approved));
            SdDeploymentTask[] taskBack = GetBackDeploymentTasks(tasks.ToArray());
            Log("taskBack.Count=" + taskBack.Count());
            Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Approved));

            return taskBack;
        }

        public SdDeploymentTask[] ToSaveUpdateTasks(SdDeploymentTask[] tasks)
        {
            Log("================TO SAVE UPDATE TASKS");
            ssdSvc.UpdateDeploymentTasks(new SdDeploymentTask[] { }, tasks.ToArray(), new SdDeploymentTask[] { });
            Assert.IsTrue(CheckBatchTaskStatus(tasks.ToArray(), SrTaskStatus.Approved));
            SdDeploymentTask[] taskBack = GetBackDeploymentTasks(tasks.ToArray());
            Log("taskBack.Count=" + taskBack.Count());
            Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Approved));

            return taskBack;
        }

        public SdDeploymentTask[] ToAssignTasks(SdDeploymentTask[] tasks)
        {
            Log("================TO ASSIGN TASKS");
            List<Guid> taskToAssign = new List<Guid>();
            foreach (SdDeploymentTask t in tasks)
            {
                taskToAssign.Add(t.Id);
                if (t.Action == SegmentAction.Flush)
                {
                    t.TaskStatus = SrTaskStatus.Completed;
                }
                else
                {
                    t.TaskStatus = SrTaskStatus.Assigned;
                }
            }
            ssdSvc.UpdateDeploymentTasks(new SdDeploymentTask[] { }, tasks, new SdDeploymentTask[] { });
            SdDeploymentTask[] taskBack = ssdSvc.GetDeploymentTasks(taskToAssign.ToArray());

            Log("after assign latestTasks.Count=" + tasks.Count());
            //Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Assigned));

            return taskBack;
        }

        public SdDeploymentTask ToCompleteOneTaskWithSegment(SdDeploymentTask task, string segmentId, int authorintId)
        {
            Log("================TO COMPLETE TASK WITH SEGMENT");

            List<Guid> listTaskGuids = new List<Guid>();

            LibCompleteTask(task, userIdEd, authorintId);
            listTaskGuids.Add(task.Id);
            //LibCompleteTask(t, userIdEd);//map task with segment
            SdDeploymentTask taskBack = ssdSvc.GetDeploymentTasks(listTaskGuids.ToArray())[0];
            Assert.IsTrue(CheckBatchTaskStatus(new SdDeploymentTask[] { taskBack }, SrTaskStatus.Completed));

            return taskBack;
        }
        public SdDeploymentTask[] ToCompleteTasks(SdDeploymentTask[] tasks)
        {
            Log("================TO COMPLETE TASKS");
            userIdEd = TestRoutines.GetUserIdByName("Ed");
            List<Guid> listTaskGuids = new List<Guid>();
            foreach (SdDeploymentTask t in tasks)
            {
                listTaskGuids.Add(t.Id);
                LibCompleteTask(t, userIdEd);//map task with segment
            }

            SdDeploymentTask[] taskBack = ssdSvc.GetDeploymentTasks(listTaskGuids.ToArray());
            Log("after complete latestTasks.Count=" + taskBack.Count());
            Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Completed));

            return taskBack;
        }

        public SdDeploymentTask ToCompleteTaskWithSegment(SdDeploymentTask task, int authoringId)
        {
            Log("================TO COMPLETE TASK WITH SPECIFIED SEGMENT");
            List<Guid> listTaskGuids = new List<Guid>();
            listTaskGuids.Add(task.Id);
            LibCompleteTask(task, userIdEd, authoringId);//map task with segment

            SdDeploymentTask[] taskBack = ssdSvc.GetDeploymentTasks(listTaskGuids.ToArray());
            Log("after complete latestTasks.Count=" + taskBack.Count());
            Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Completed));

            return taskBack[0];
        }

        public SdDeploymentTask[] ToCompleteTasksWithAB(SdDeploymentTask[] tasks)
        {
            Log("================TO COMPLETE TASKS WITH ABTESTING REQUEST");
            List<Guid> listTaskGuids = new List<Guid>();
            foreach (SdDeploymentTask t in tasks)
            {
                listTaskGuids.Add(t.Id);
                LibCompleteTaskWithABTesting(t, userIdEd);
            }
            SdDeploymentTask[] taskBack = ssdSvc.GetDeploymentTasks(listTaskGuids.ToArray());
            Log("after complete latestTasks.Count=" + taskBack.Count());
            Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Completed));

            return taskBack;
        }

        public SdDeploymentTask[] ToAllocateTags(SdDeploymentTask[] tasks)
        {
            Log("================TO ALLOCATE TAGS");
            List<Guid> listTaskGuids = new List<Guid>();

            foreach (SdDeploymentTask t in tasks)
            {
                listTaskGuids.Add(t.Id);
            }

            SdDeploymentTask[] testBack = ssdSvc.AllocateTags(listTaskGuids.ToArray());
            //            Assert.IsTrue(CheckBatchTaskStatus(testBack, SrTaskStatus.Deployed));

            return testBack;
        }
        public SdDeploymentTask[] ToDeployTasks(SdDeploymentTask[] tasks)
        {
            Log("================TO DEPLOY TASKS");
            List<Guid> listTaskGuids = new List<Guid>();
            foreach (SdDeploymentTask t in tasks)
            {
                listTaskGuids.Add(t.Id);
            }
            LibDeployTasks(tasks);

            SdDeploymentTask[] testBack = ssdSvc.GetDeploymentTasks(listTaskGuids.ToArray());
            //Log("after deploy latestTasks.Count=" + testBack.Count());
            //Assert.IsTrue(CheckBatchTaskStatus(testBack, SrTaskStatus.Deployed));
            List<SdDeploymentTask> tasksToCheck = testBack.ToList<SdDeploymentTask>();
            int count = testBack.Count();
            for (int i = 0; i < count; i++)
            {
                if (tasksToCheck.Count() < 1) break; //all tasks are deployed successfully
                Thread.Sleep(2000);
                List<SdDeploymentTask> deployedTask = new List<SdDeploymentTask>();
                foreach (SdDeploymentTask t in tasksToCheck)
                {

                    if (CheckTaskStatus(t.Id, SrTaskStatus.Deploying)) continue;

                    if (CheckTaskStatus(t.Id, SrTaskStatus.Deployed))
                    {
                        deployedTask.Add(t);
                        continue;
                    }

                    //status incorrect
                    string s = string.Format("Task {0} status not expected", t.BusinessSegmentName);
                    Log(s);
                    Assert.Fail(s);
                }

                foreach (SdDeploymentTask t in deployedTask)
                {
                    tasksToCheck.Remove(t);
                    Console.WriteLine("Task " + t.BusinessSegmentName + " deployed");
                    tasksToCheck.Remove(t);
                }
            }

            testBack = ssdSvc.GetDeploymentTasks(listTaskGuids.ToArray());
            foreach(SdDeploymentTask t in testBack){
                CheckMDS(t);
            }


            return testBack;
        }

        public void CheckMDS(SdDeploymentTask task)
        {
            MDSSegment mdsseg = GetMDSSegById(task.SegmentId.Value);
            SRSegment srseg = GetSRSegById(task.SegmentId.Value);
            SRModel srmodel = GetSRModelById(task.ModelRevisionId.Value);

            Assert.IsTrue(mdsseg.IntId == srseg.AuthoringId, "task "+task.BusinessSegmentName+" mdsseg.IntId=" + mdsseg.IntId + " srseg.AuthoringId=" + srseg.AuthoringId);
            Assert.IsTrue((int)mdsseg.Type == srmodel.TypeId, "task "+task.BusinessSegmentName+" mdsseg.Type=" + mdsseg.Type + " srmodel.TypeId=" + srmodel.TypeId);
            Assert.IsTrue(mdsseg.AuthoringFriendlyName==task.BusinessSegmentName, "task "+task.BusinessSegmentName+" mdsseg.AuthoringFriendlyName=" + mdsseg.AuthoringFriendlyName + " task.BusinessSegmentName=" + task.BusinessSegmentName);
            Assert.IsTrue(srseg.BusinessSegmentName==task.BusinessSegmentName, "task "+task.BusinessSegmentName+" srseg.BusinessSegmentName=" + srseg.BusinessSegmentName + " task.BusinessSegmentName=" + task.BusinessSegmentName);
            Assert.IsTrue(mdsseg.AuthoringDescription.Contains(task.OwnerName), "task "+task.BusinessSegmentName+" mdsseg.AuthoringDescription=" + mdsseg.AuthoringDescription + " task.OwnerName=" + task.OwnerName);
            Assert.IsTrue(marketIdDistributionChannelIdMap[srseg.MarketId]==mdsseg.DistributionChannel,
                "task "+task.BusinessSegmentName+" srseg.MarketId=" + srseg.MarketId + " mdsseg.DistributionChannel=" + mdsseg.DistributionChannel);
            Assert.IsTrue(mdsseg.AdvertiserID==30131, "task "+task.BusinessSegmentName+" mdsseg.AdvertiserID=" + mdsseg.AdvertiserID);
            Assert.IsTrue(mdsseg.AdvertiserType==1, "task "+task.BusinessSegmentName+" mdsseg.AdvertiserType=" + mdsseg.AdvertiserType);
            Assert.IsTrue(mdsseg.Accessibility==(byte)task.Accessibility);
            Assert.IsTrue(mdsseg.ApplicationId == 1, "task " + task.BusinessSegmentName + " mdsseg.ApplicationId=" + mdsseg.ApplicationId);
            Assert.IsTrue(mdsseg.ApplicationId == 1, "task " + task.BusinessSegmentName + " mdsseg.ApplicationId=" + mdsseg.ApplicationId);

            Assert.IsTrue(mdsseg.MembershipExpirationSec == int.Parse(srmodel.ProcessEngingValue),
                "task " + task.BusinessSegmentName + " mdsseg.MembershipExpirationSec=" + mdsseg.MembershipExpirationSec + " srmodel.ProcessEngingValue=" + srmodel.ProcessEngingValue);
            Assert.IsTrue(mdsseg.AdExpertFriendlyName == task.BusinessSegmentName, "task " + task.BusinessSegmentName + " mdsseg.AdExpertFriendlyName=" + mdsseg.AdExpertFriendlyName);

            if (srmodel.ProcessEngineId == 2)//LUX
            {
                Assert.IsTrue(mdsseg.ProcessingEngineId == 2, "task " + task.BusinessSegmentName + " mdsseg.ProcessingEngineId=" + mdsseg.ProcessingEngineId);
                Assert.IsTrue(mdsseg.MembershipExpirationSec == int.Parse(srmodel.ProcessEngingValue), "task " + task.BusinessSegmentName + " mdsseg.MembershipExpirationSec=" + mdsseg.MembershipExpirationSec + " srmodel.ProcessEngingValue=" + srmodel.ProcessEngingValue);
                
            }
            else //cosmos
            {
                Assert.IsTrue(mdsseg.ProcessingEngineId == 1, "task " + task.BusinessSegmentName + " mdsseg.ProcessingEngineId=" + mdsseg.ProcessingEngineId);
            }


            //AttributeDefinitionId
            //ABTestConfiguration
            //CreatedTime
            //UpdatedTime
            //AuthoringUpdatedTime
            //Version
            //MembershipExpirationSec       only LUX have this field, others are 0
            //dap
            if (task.DapChannelCfg.DeliveryChannel == DeliveryChannel.DAP)
            {
                Assert.IsTrue(mdsseg.AdExpertVerticalId == task.VerticalId, "task " + task.BusinessSegmentName + " mdsseg.AdExpertVerticalId=" + mdsseg.AdExpertVerticalId + " task.VerticalId=" + task.VerticalId);
                Assert.IsTrue(mdsseg.DeliveryChannelId == 1, "task " + task.BusinessSegmentName + " mdsseg.DeliveryChannelId=" + mdsseg.DeliveryChannelId);
            }
            else //msn
            {
                Assert.IsTrue(mdsseg.AdExpertVerticalId == null, "task " + task.BusinessSegmentName + " mdsseg.AdExpertVerticalId=" + mdsseg.AdExpertVerticalId);
                Assert.IsTrue(mdsseg.DeliveryChannelId == 3, "task " + task.BusinessSegmentName + " mdsseg.DeliveryChannelId=" + mdsseg.DeliveryChannelId);
            }

            //new
            if (task.Action == SegmentAction.New)
            {
                Assert.IsTrue(srseg.SegmentLifeCycleStatusId == (byte)SegmentLifecycleStatus.Active, "task " + task.BusinessSegmentName + " srseg.SegmentLifeCycleStatusId=" + srseg.SegmentLifeCycleStatusId);
                Assert.IsTrue(mdsseg.SegmentLifeCycleStatus == 2, "task " + task.BusinessSegmentName + " mdsseg.SegmentLifeCycleStatus=" + mdsseg.SegmentLifeCycleStatus);
                Assert.IsTrue(mdsseg.AuthoringStatus == 1, "task " + task.BusinessSegmentName + " mdsseg.AuthoringStatus=" + mdsseg.AuthoringStatus);
                Assert.IsTrue(mdsseg.RuntimeStatus == 1, "task " + task.BusinessSegmentName + " mdsseg.RuntimeStatus=" + mdsseg.RuntimeStatus);
            }
            //update
            if (task.Action == SegmentAction.Update)
            {
                Assert.IsTrue(srseg.SegmentLifeCycleStatusId == (byte)SegmentLifecycleStatus.Active, "task " + task.BusinessSegmentName + " srseg.SegmentLifeCycleStatusId=" + srseg.SegmentLifeCycleStatusId);
                Assert.IsTrue(mdsseg.SegmentLifeCycleStatus == 2, "task " + task.BusinessSegmentName + " mdsseg.SegmentLifeCycleStatus=" + mdsseg.SegmentLifeCycleStatus);
                Assert.IsTrue(mdsseg.AuthoringStatus == 1, "task " + task.BusinessSegmentName + " mdsseg.AuthoringStatus=" + mdsseg.AuthoringStatus);
                Assert.IsTrue(mdsseg.RuntimeStatus == 1, "task " + task.BusinessSegmentName + " mdsseg.RuntimeStatus=" + mdsseg.RuntimeStatus);
            }
            //flush
            if (task.Action == SegmentAction.Flush)
            {
                Assert.IsTrue(srseg.SegmentLifeCycleStatusId == (byte)SegmentLifecycleStatus.Deleted, "task " + task.BusinessSegmentName + " srseg.SegmentLifeCycleStatusId=" + srseg.SegmentLifeCycleStatusId);
                Assert.IsTrue(mdsseg.SegmentLifeCycleStatus == 4, "task " + task.BusinessSegmentName + " mdsseg.SegmentLifeCycleStatus=" + mdsseg.SegmentLifeCycleStatus);
                Assert.IsTrue(mdsseg.AuthoringStatus == 2, "task " + task.BusinessSegmentName + " mdsseg.AuthoringStatus=" + mdsseg.AuthoringStatus);
                Assert.IsTrue(mdsseg.RuntimeStatus == 2, "task " + task.BusinessSegmentName + " mdsseg.RuntimeStatus=" + mdsseg.RuntimeStatus);
            }

        }

        public void ToDeleteTasks(SdDeploymentTask[] tasks)
        {
            Log("================TO DELETE TASKS");
            ssdSvc.UpdateDeploymentTasks(tasks.ToArray(), new SdDeploymentTask[] { }, new SdDeploymentTask[] { });
            SdDeploymentTask[] taskBack = GetBackDeploymentTasks(tasks.ToArray());
            Log("taskBack.Count=" + taskBack.Count());
            Assert.IsTrue(taskBack.Length == 0);
        }

        public SdDeploymentTask[] ToAddTaskOfLiveSegment(SdDeploymentTask[] tasks)
        {
            Log("================TO SAVE UPDATE TASKS");
            ssdSvc.UpdateDeploymentTasks(new SdDeploymentTask[] { }, new SdDeploymentTask[] { }, tasks.ToArray());
            Assert.IsTrue(CheckBatchTaskStatus(tasks.ToArray(), SrTaskStatus.Approved));
            SdDeploymentTask[] taskBack = GetBackDeploymentTasks(tasks.ToArray());
            Log("taskBack.Count=" + taskBack.Count());
            Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Approved));

            return taskBack;
        }

        public SdDeploymentTask[] ToRollbackTasks(SdDeploymentTask[] tasks)
        {
            Log("================TO ROLLBACK ASSIGNED TASKS");
            List<Guid> taskFromComplete = new List<Guid>();
            List<Guid> taskFromAssigned = new List<Guid>();
            foreach (SdDeploymentTask t in tasks)
            {
                if (t.TaskStatus == SrTaskStatus.Assigned)
                {
                    taskFromAssigned.Add(t.Id);
                    t.TaskStatus = SrTaskStatus.Approved;
                }
                if (t.TaskStatus == SrTaskStatus.Completed)
                {
                    taskFromComplete.Add(t.Id);
                    t.TaskStatus = SrTaskStatus.Assigned;
                }
            }

            ssdSvc.UpdateDeploymentTasks(new SdDeploymentTask[] { }, tasks, new SdDeploymentTask[] { });
            SdDeploymentTask[] taskBack = ssdSvc.GetDeploymentTasks(taskFromAssigned.ToArray());
            Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Approved));

            taskBack = ssdSvc.GetDeploymentTasks(taskFromComplete.ToArray());
            Assert.IsTrue(CheckBatchTaskStatus(taskBack, SrTaskStatus.Assigned));

            foreach (Guid g in taskFromAssigned)
                taskFromComplete.Add(g);
            taskBack = ssdSvc.GetDeploymentTasks(taskFromComplete.ToArray());
            return taskBack;
        }


        public int GetPrimaryModelRevisionIdByRequestId(string reqId)
        {
            string sqltxt = string.Format(@"select las.[ModelRevisionId] from [LiveABTestingSegment] las
                join [LiveABTestingRequest] lar on lar.RevisionId=las.RequestRevisionId
                where lar.RequestId='{0}' and las.IsPrimary=1", reqId);
            return GetIntFromDB(sqltxt, SRDBConn);

        }
        public int GetPrimaryModelAuthIdByRequestId(string reqId)
        {
            string sqltxt = string.Format(@" select sm.AuthoringId from dbo.SegmentModel sm 
                 join [LiveABTestingSegment] las on las.ModelRevisionId=sm.RevisionId
                 join [LiveABTestingRequest] lar on las.RequestRevisionId=lar.RevisionId
                 where las.IsPrimary=1 and lar.RequestId='{0}'", reqId);
            return GetIntFromDB(sqltxt, SRDBConn);
        }

        /// <summary>
        /// Create an ABTesting request quckly
        /// </summary>
        /// <returns>the request id</returns>
        public string CreateABRequestQuick()
        {
            GTMTestHelper ta = new GTMTestHelper(testContextInstance);

            //creaet 2 segment
            string userId = userIdEd;
            string segmentNameA = GetName("Publish1-");
            string segmentNameB = GetName("Publish2-");

            RuleSegment segA = CreateSimpleRuleSegment();
            RuleSegment segB = CreateSimpleRuleSegment();

            //TaxonomySegment segA = createTaxSegment(userId.ToString(), projectName, segmentNameA);
            //TaxonomySegment segB = createTaxSegment(userId.ToString(), projectName, segmentNameB);

            //publish the semgents
            TestRoutines.PublishSegment(segA.SegmentId);
            TestRoutines.PublishSegment(segB.SegmentId);
            //TestRoutines.PublishSegment(segA.SegmentId, "SSD Tax SegA", userId.ToString(), "AU");
            //TestRoutines.PublishSegment(segB.SegmentId, "SSD Tax SegB", userId.ToString(), "AU");

            segA.AuthoringSegmentId = GetAuthoringId(segA.SegmentId);
            segB.AuthoringSegmentId = GetAuthoringId(segB.SegmentId);

            //create ABTesting Request
            IUser userSvc = PseudoChannelCenter.GetChannel<IUser>();
            var invoker = userSvc.GetUserById(userId);

            var sm = new User();
            sm.UserType = UserType.SegmentManager;
            GlobalParameters gp = TestRoutines.GetGlobalParameter(sm);
            var listB = new List<string>();
            listB.Add(segB.AuthoringSegmentId.ToString());
            string requestName = GetName("ABRequest_");
            string reqId = ABTestingHelper.CreateLiveABTestingRequest(
                segA.AuthoringSegmentId.ToString(),
                listB,
                requestName,
                gp.UserEstimatedCTR,
                gp.CtrDelta,
                gp.SampleThreshold,
                invoker,
                2
                );
            Log("AB=" + reqId);

            return reqId;
        }


        public int GetAuthoringId(string segId)
        {
            string sqlTxt = string.Format("SELECT AuthoringSegmentId from Segment WHERE SegmentId='{0}'", segId);
            int ret = 0;
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting("SSDBConn")))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        ret = sdr.GetInt32(0);
                    }
                }
                finally { scd.Dispose(); }

            }

            return ret;
        }


        public SdDeploymentTask[] LibDeployTasks(SdDeploymentTask[] tasks)
        {
            List<Guid> taskGuid = new List<Guid>();

            foreach (SdDeploymentTask t in tasks)
            {
                taskGuid.Add(t.Id);
            }
            ssdSvc.DeployTasks(taskGuid.ToArray());
            Thread.Sleep(5000);
            //verify tas back
            SdDeploymentTask[] taskBack = ssdSvc.GetDeploymentTasks(taskGuid.ToArray());
            Log("taskBack count=" + taskBack.Count());
            foreach (SdDeploymentTask t in taskBack)
            {
                Assert.IsNotNull(t, "should not be null for task " + t.Id);
                //Assert.IsTrue(t.TaskStatus == SrTaskStatus.Deployed, "stats should be Deployed " + t.Id);
            }
            return taskBack;
        }

        public bool CheckBatchTaskStatus(SdDeploymentTask[] tasks, SrTaskStatus status)
        {
            foreach (SdDeploymentTask t in tasks)
            {
                if (!CheckTaskStatus(t.Id, status))
                {
                    Log("status incorrect " + t.Id + ", should be" + status);
                    return false;
                }
            }
            return true;
        }

        public SdDeploymentTask[] GetBackDeploymentTasks(SdDeploymentTask[] taskInput)
        {
            List<Guid> listTaskGuid = new List<Guid>();
            foreach (SdDeploymentTask t in taskInput)
            {
                listTaskGuid.Add(t.Id);
            }
            return ssdSvc.GetDeploymentTasks(listTaskGuid.ToArray());
        }

        public SdDeploymentTask LibCompleteTask(SdDeploymentTask task, string dataAnalystId)
        {
            //TaxonomySegment seg = CreateSegmentQuick();
            RuleSegment seg = CreateSimpleRuleSegment();
            TestRoutines.PublishSegment(seg.SegmentId);
            //TestRoutines.PublishSegment(seg.SegmentId, "SSDSeg", dataAnalystId, task.MarketName);
            seg.AuthoringSegmentId = GetAuthoringId(seg.SegmentId);

            return LibCompleteTask(task, dataAnalystId, seg.AuthoringSegmentId);
        }
        /*
        public SdDeploymentTask LibCompleteTask(SdDeploymentTask task, string dataAnalystId, SegmentBase seg)
        {
            //TestRoutines.PublishSegment(seg.SegmentId, "SSDSeg", dataAnalystId, task.MarketName);
            seg.AuthoringSegmentId = GetAuthoringId(seg.SegmentId);
            Log("authoring id=" + seg.AuthoringSegmentId);

            string sqlTxt = string.Format("select top 1 [RevisionId] from [SegmentModel] where [AuthoringId]='{0}' ORDER BY [LastUpdatedTime] DESC"
                , seg.AuthoringSegmentId);
            int revisionId = GetIntFromDB(sqlTxt, SRDBConn);
            sqlTxt = string.Format("select top 1 [AuthoringId] from [SegmentModel] where [AuthoringId]='{0}' ORDER BY [LastUpdatedTime] DESC"
                , seg.AuthoringSegmentId);
            int authId = GetIntFromDB(sqlTxt, SRDBConn);
            task.ModelRevisionId = revisionId;
            task.AuthoringId = authId;
            Log("revisionid=" + revisionId + " authid=" + authId);

            task.TaskStatus = SrTaskStatus.Completed;

            ssdSvc.UpdateDeploymentTasks(new SdDeploymentTask[] { }, new SdDeploymentTask[] { task }, new SdDeploymentTask[] { });
            return ssdSvc.GetDeploymentTasks(new Guid[] { task.Id })[0];
        }
        */
        public SdDeploymentTask LibCompleteTask(SdDeploymentTask task, string dataAnalystId, int authoringId)
        {
            //TestRoutines.PublishSegment(seg.SegmentId, "SSDSeg", dataAnalystId, task.MarketName);
            //seg.AuthoringSegmentId = GetAuthoringId(seg.SegmentId);
            Log("authoring id=" + authoringId);

            string sqlTxt = string.Format("select top 1 [RevisionId] from [SegmentModel] where [AuthoringId]='{0}' ORDER BY [LastUpdatedTime] DESC"
                , authoringId);
            int revisionId = GetIntFromDB(sqlTxt, SRDBConn);
            sqlTxt = string.Format("select top 1 [AuthoringId] from [SegmentModel] where [AuthoringId]='{0}' ORDER BY [LastUpdatedTime] DESC"
                , authoringId);
            int authId = GetIntFromDB(sqlTxt, SRDBConn);
            task.ModelRevisionId = revisionId;
            task.AuthoringId = authId;
            Log("revisionid=" + revisionId + " authid=" + authId);

            task.TaskStatus = SrTaskStatus.Completed;

            ssdSvc.UpdateDeploymentTasks(new SdDeploymentTask[] { }, new SdDeploymentTask[] { task }, new SdDeploymentTask[] { });
            return ssdSvc.GetDeploymentTasks(new Guid[] { task.Id })[0];
        }


        public SdDeploymentTask LibCompleteTaskWithABTesting(SdDeploymentTask task, string dataAnalystId)
        {
            string reqId = CreateABRequestQuick();
            //task.SegmentId = new Guid(reqId);
            //task.ModelRevisionId = GetABSegRevisionId(reqId, 1);
            task.ModelRevisionId = GetPrimaryModelRevisionIdByRequestId(reqId);
            task.AuthoringId = GetPrimaryModelAuthIdByRequestId(reqId);
            task.LiveABTestingRequestRevisionId = GetABSegRevisionId(reqId, 1);
            //task.

            //task.AuthoringId = GetABSegAuthoringId(reqId, 1);


            Log("revisionid=" + task.ModelRevisionId + " authid=" + task.AuthoringId);

            task.TaskStatus = SrTaskStatus.Completed;

            ssdSvc.UpdateDeploymentTasks(new SdDeploymentTask[] { }, new SdDeploymentTask[] { task }, new SdDeploymentTask[] { });
            return ssdSvc.GetDeploymentTasks(new Guid[] { task.Id })[0];

        }
        public TaxonomySegment CreateSegmentQuick()
        {
            string segmentName = GetName("Publish");

            //TaxonomySegment seg = createTaxSegment(userId.ToString(), projectName, segmentName);
            TaxonomySegment seg = createTaxSegment(userIdEd, projectName, LibGetProjectIdByName(projectName, userIdEd), segmentName);
            seg.AuthoringSegmentId = GetAuthoringId(seg.SegmentId);
            return seg;
        }

        public SdDeploymentTask CreateQuickNewTask(int marketId, ProductionStatus ps)
        {
            string segname = GetName("New");
            Log("create task name=" + segname);
            return CreateTaskOfNewSegment(userNameEd, "Admin", marketId, ps, segname, segname);
        }

        public SdDeploymentTask CreateQuickNewTask(string name, int marketId, ProductionStatus ps)
        {
            string segname = name;
            Log("create task name=" + segname);
            return CreateTaskOfNewSegment(userNameEd, "Admin", marketId, ps, segname, segname);
        }


        public SdDeploymentTask CreateQuickNewTaskMSN(int marketId)
        {
            string segname = GetName("MSN");
            SdDeploymentTask task = CreateTaskOfNewMSNSegment(userNameEd, "Admin", marketId, segname);

            return task;
        }

        public SdDeploymentTask CreateQuickNewTaskMSN(string segmentName)
        {
            return CreateTaskOfNewMSNSegment(userNameEd, "Admin", (int)MarketID.US, segmentName);
            /*
            var map = GetCapacitySnap();
            if (test4ReuseCapacity == 0 && live4ReuseCapacity == 0 && noUsedCapacity == 0) return null;
            foreach (var pair in map)
            {
                CapacityInfo ci = pair.Value;
                if (ci.used == ci.capacity) continue;
                if (test4ReuseCapacity > 0 || noUsedCapacity > 0)
                {
                    
                }

            }
            return null;*/
        }

        public SdDeploymentTask CreateQuickNewTaskMSN()
        {
            var map = GetCapacitySnap();
            if (test4ReuseCapacity == 0 && live4ReuseCapacity == 0 && noUsedCapacity == 0) return null;
            foreach (var pair in map)
            {
                CapacityInfo ci = pair.Value;
                if (ci.used == ci.capacity) continue;



                if (test4ReuseCapacity > 0 || noUsedCapacity > 0)
                {

                    return CreateQuickNewTaskMSN(ci.marketId);
                }

            }
            return null;
        }

        public SdDeploymentTask CreateQuickNewTask(string segmentName)
        {
            Console.WriteLine("test4ReuseCapacity = {0} && live4ReuseCapacity == {1} && noUsedCapacity == {2}",
                test4ReuseCapacity, live4ReuseCapacity, noUsedCapacity);
            if (test4ReuseCapacity == 0 && noUsedCapacity == 0)
                return CreateQuickNewTaskMSN(segmentName);

            if (test4ReuseCapacity > 0 || noUsedCapacity > 0)
            {
                return CreateQuickNewTaskTest(segmentName);
            }

            if (live4ReuseCapacity > 0 || noUsedCapacity > 0)
            {
                return CreateQuickNewTaskLive(segmentName);
            }
            return CreateQuickNewTaskTest(segmentName);
            //return null;
        }

        public SdDeploymentTask CreateQuickNewTaskTest()
        {
            var map = GetCapacitySnap();
            if (test4ReuseCapacity == 0 && live4ReuseCapacity == 0 && noUsedCapacity == 0) return null;
            foreach (var pair in map)
            {
                CapacityInfo ci = pair.Value;
                if (ci.used == ci.capacity) continue;
                if (test4ReuseCapacity > 0 || noUsedCapacity > 0)
                {
                    ci.used++;
                    if (test4ReuseCapacity > 0) test4ReuseCapacity--;
                    else noUsedCapacity--;

                    return CreateQuickNewTask(ci.marketId, ProductionStatus.Test);
                }

            }
            return null;
        }

        /// <summary>
        /// create the task with specified name
        /// </summary>
        /// <param name="segName"></param>
        /// <returns></returns>
        public SdDeploymentTask CreateQuickNewTaskTest(string segName)
        {
            var map = currentCapacityMap;
            if (test4ReuseCapacity == 0 && live4ReuseCapacity == 0 && noUsedCapacity == 0) return null;
            foreach (var pair in map)
            {
                CapacityInfo ci = pair.Value;
                if (ci.used == ci.capacity) continue;
                if (test4ReuseCapacity > 0 || noUsedCapacity > 0)
                {
                    ci.used++;
                    if (test4ReuseCapacity > 0) test4ReuseCapacity--;
                    else noUsedCapacity--;

                    return CreateQuickNewTask(segName, ci.marketId, ProductionStatus.Test);
                }

            }
            return null;
        }

        public SdDeploymentTask CreateQuickNewTaskLive()
        {
            var map = currentCapacityMap;
            if (test4ReuseCapacity == 0 && live4ReuseCapacity == 0 && noUsedCapacity == 0) return null;
            foreach (var pair in map)
            {
                CapacityInfo ci = pair.Value;
                if (ci.used == ci.capacity) continue;
                if (live4ReuseCapacity > 0 || noUsedCapacity > 0)
                {
                    ci.used++;
                    if (live4ReuseCapacity > 0) live4ReuseCapacity--;
                    else noUsedCapacity--;

                    return CreateQuickNewTask(ci.marketId, ProductionStatus.Live);
                }

            }
            return null;
        }

        public SdDeploymentTask CreateQuickNewTaskLive(string segName)
        {
            var map = currentCapacityMap;
            if (test4ReuseCapacity == 0 && live4ReuseCapacity == 0 && noUsedCapacity == 0) return null;
            foreach (var pair in map)
            {
                CapacityInfo ci = pair.Value;
                if (ci.used == ci.capacity) continue;
                if (live4ReuseCapacity > 0 || noUsedCapacity > 0)
                {
                    ci.used++;
                    if (live4ReuseCapacity > 0) live4ReuseCapacity--;
                    else noUsedCapacity--;

                    return CreateQuickNewTask(segName, ci.marketId, ProductionStatus.Live);
                    //    CreateQuickNewTask(ci.marketId, ProductionStatus.Live);
                }

            }
            return null;
        }


        public Guid GetLiveLiveSegment(int marketId)
        {
            return GetLiveSegmentByPS(marketId, ProductionStatus.Live);
        }

        public SdSegment GetLiveSegmentById(Guid segId, DeliveryChannel dc, int marketId)
        {
            if (segId == null) return null;
            //IStreamliningDeployment ssdSvc = PseudoChannelCenter.GetChannel<IStreamliningDeployment>();
            SdSegment[] segments = ssdSvc.GetLiveSegments(dc, marketId);
            SdDeploymentTask[] tasks = ssdSvc.GetDeploymentTasksOfLiveSegments(dc, marketId);
            //tasks[0].AuthoringId
            //

            foreach (SdSegment seg in segments)
            {
                if (seg.Id.Equals(segId)) return seg;
            }
            return null;
        }

        public SdDeploymentTask CreateTaskOfLiveSegment(Guid segmentId, DeliveryChannel dc, int marketId)
        {
            SdSegment seg = GetLiveSegmentById(segmentId, dc, marketId);
            SdDeploymentTask taskOfSegment = GetTaskOfLiveSegment(segmentId, dc, seg.MarketId);


            SdDeploymentTask task = new SdDeploymentTask();
            task.Id = Guid.NewGuid();
            task.SegmentId = segmentId; //US
            //task.Action = SegmentAction.Flush;
            task.OwnerId = seg.OwnerId;
            task.OwnerName = seg.SdTask.OwnerName;
            task.CreatorId = new Guid(LibGetUserIdByName("Admin"));
            task.VerticalId = seg.VerticalId;
            task.MarketId = seg.MarketId;
            task.MarketName = seg.SdTask.MarketName;
            task.DapChannelCfg = taskOfSegment.DapChannelCfg;
            task.DeliveryChannelConfiguration = taskOfSegment.DeliveryChannelConfiguration;
            task.BusinessSegmentName = taskOfSegment.BusinessSegmentName;
            task.CreatedTime = DateTime.UtcNow;
            task.DeployTime = taskOfSegment.DeployTime;
            task.TaskStatus = Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment.TaskStatus.Approved;
            task.ModelRevisionId = taskOfSegment.ModelRevisionId;
            task.AuthoringId = taskOfSegment.AuthoringId;
            return task;
        }

        public SdDeploymentTask CreateTaskOfLiveSegmentById(string segmentId)
        {
            string sqlTxt = string.Format("select [AuthoringSegmentId] from Segment where SegmentId='{0}'", segmentId);
            //Console.WriteLine(sqlTxt);
            int authId = GetIntFromDB(sqlTxt, "SSDBConn");
            //Console.WriteLine("authId=" + authId);

            //            sqlTxt = string.Format(@"select top 1 s.AuthoringId from Segment s join SegmentModel sm 
            //                        on s.ModelRevisionId=sm.RevisionId 
            //                        where sm.AuthoringId={0}
            //                        order by sm.RevisionNum desc", ssAuthId);
            //            Console.WriteLine(sqlTxt);
            //            int authId = GetIntFromDB(sqlTxt, "BTSRDBConn");
            //            Console.WriteLine("authId=" + authId);

            //SdDeploymentTask[] tasks = ssdSvc.GetDeploymentTasksOfLiveSegments(null, null);
            SdDeploymentTask taskOfSegment = getLiveSegmentTaskBySegmentId(segmentId);
            //foreach (SdDeploymentTask t in tasks)
            //{   

            //    if(t.AuthoringId==authId)
            //    {
            //        taskOfSegment = t;
            //        break;
            //    }
            //}
            if (taskOfSegment == null) return null;
            //Guid segmentId = new Guid();
            //if (taskOfSegment.SegmentId.HasValue)
            //    segmentId = taskOfSegment.SegmentId.Value;

            //SdSegment seg = GetLiveSegmentById(segmentId, dc, marketId);
            //SdDeploymentTask taskOfSegment = GetTaskOfLiveSegment(segmentId, dc, seg.MarketId);

            SdDeploymentTask task = new SdDeploymentTask();
            task.Id = Guid.NewGuid();
            task.SegmentId = taskOfSegment.SegmentId;
            //task.Action = SegmentAction.Flush;
            task.OwnerId = taskOfSegment.OwnerId;
            task.OwnerName = taskOfSegment.OwnerName;
            task.CreatorId = new Guid(LibGetUserIdByName("Admin"));
            task.VerticalId = taskOfSegment.VerticalId;
            task.MarketId = taskOfSegment.MarketId;
            task.MarketName = taskOfSegment.MarketName;
            task.DapChannelCfg = taskOfSegment.DapChannelCfg;
            task.DeliveryChannelConfiguration = taskOfSegment.DeliveryChannelConfiguration;
            task.BusinessSegmentName = taskOfSegment.BusinessSegmentName;
            task.CreatedTime = DateTime.UtcNow;
            task.DeployTime = taskOfSegment.DeployTime;
            task.TaskStatus = Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment.TaskStatus.Approved;
            task.ModelRevisionId = taskOfSegment.ModelRevisionId;
            task.AuthoringId = taskOfSegment.AuthoringId;
            task.Accessibility = taskOfSegment.Accessibility;

            return task;
        }

        public SdDeploymentTask CreateTaskOfLiveSegment(DeliveryChannel? dc, int? marketId)
        {
            SdDeploymentTask[] tasks = ssdSvc.GetDeploymentTasksOfLiveSegments(dc, marketId);
            if (tasks.Count() == 0) return null;

            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int rnd = ra.Next(0, tasks.Count());

            SdDeploymentTask taskOfSegment = tasks[rnd];

            if (taskOfSegment == null) return null;

            Guid segmentId = new Guid();
            if (taskOfSegment.SegmentId.HasValue)
                segmentId = taskOfSegment.SegmentId.Value;

            SdDeploymentTask task = new SdDeploymentTask();
            task.Id = Guid.NewGuid();
            task.SegmentId = segmentId;
            task.OwnerId = taskOfSegment.OwnerId;
            task.OwnerName = taskOfSegment.OwnerName;
            task.CreatorId = new Guid(LibGetUserIdByName("Admin"));
            task.VerticalId = taskOfSegment.VerticalId;
            task.MarketId = taskOfSegment.MarketId;
            task.MarketName = taskOfSegment.MarketName;
            task.DapChannelCfg = taskOfSegment.DapChannelCfg;
            task.DeliveryChannelConfiguration = taskOfSegment.DeliveryChannelConfiguration;
            task.BusinessSegmentName = taskOfSegment.BusinessSegmentName;
            task.CreatedTime = DateTime.UtcNow;
            task.DeployTime = taskOfSegment.DeployTime;
            task.TaskStatus = Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment.TaskStatus.Approved;
            task.ModelRevisionId = taskOfSegment.ModelRevisionId;
            task.AuthoringId = taskOfSegment.AuthoringId;
            return task;
        }


        public SdDeploymentTask CreateTaskOfLiveSegment(ProductionStatus ps, DeliveryChannel? dc, int? marketId)
        {

            SdDeploymentTask[] tasks = ssdSvc.GetDeploymentTasksOfLiveSegments(dc, marketId);
            SdDeploymentTask taskOfSegment = null;
            foreach (SdDeploymentTask t in tasks)
            {
                if (t.DapChannelCfg.ProductionStatus == ps)
                {
                    taskOfSegment = t;
                    break;
                }
            }
            if (taskOfSegment == null) return null;
            Guid segmentId = new Guid();
            if (taskOfSegment.SegmentId.HasValue)
                segmentId = taskOfSegment.SegmentId.Value;

            //SdSegment seg = GetLiveSegmentById(segmentId, dc, marketId);
            //SdDeploymentTask taskOfSegment = GetTaskOfLiveSegment(segmentId, dc, seg.MarketId);

            SdDeploymentTask task = new SdDeploymentTask();
            task.Id = Guid.NewGuid();
            task.SegmentId = segmentId;
            //task.Action = SegmentAction.Flush;
            task.OwnerId = taskOfSegment.OwnerId;
            task.OwnerName = taskOfSegment.OwnerName;
            task.CreatorId = new Guid(LibGetUserIdByName("Admin"));
            task.VerticalId = taskOfSegment.VerticalId;
            task.MarketId = taskOfSegment.MarketId;
            task.MarketName = taskOfSegment.MarketName;
            task.DapChannelCfg = taskOfSegment.DapChannelCfg;
            task.DeliveryChannelConfiguration = taskOfSegment.DeliveryChannelConfiguration;
            task.BusinessSegmentName = taskOfSegment.BusinessSegmentName;
            task.CreatedTime = DateTime.UtcNow;
            task.DeployTime = taskOfSegment.DeployTime;
            task.TaskStatus = Microsoft.Advertising.TargetingService.Authoring.StreamlineDeployment.TaskStatus.Approved;
            task.ModelRevisionId = taskOfSegment.ModelRevisionId;
            task.AuthoringId = taskOfSegment.AuthoringId;
            return task;
        }

        public SdDeploymentTask GetTaskOfLiveSegment(Guid segmentId, DeliveryChannel dc, int marketId)
        {
            if (segmentId == null) return null;
            IStreamliningDeployment ssdSvc = PseudoChannelCenter.GetChannel<IStreamliningDeployment>();

            SdDeploymentTask[] allLiveSegment = ssdSvc.GetDeploymentTasksOfLiveSegments(dc, marketId);
            foreach (SdDeploymentTask t in allLiveSegment)
            {
                if (t.SegmentId.Equals(segmentId)) return t;
            }
            return null;
        }

        public SdSegment GetSegmentByTask(SdDeploymentTask task)
        {
            SdSegment[] segments = ssdSvc.GetLiveSegments(task.DeliveryChannelConfiguration.DeliveryChannel, task.MarketId);

            foreach (SdSegment seg in segments)
            {
                if (task.SegmentId.Equals(seg.Id))
                    return seg;
            }

            return null;
        }

        public SdSegment GetSegmentById(Guid segmentId, int marketId)
        {
            SdSegment[] segments = ssdSvc.GetLiveSegments(DeliveryChannel.DAP, marketId);

            foreach (SdSegment seg in segments)
            {
                if (segmentId.Equals(seg.Id))
                    return seg;
            }

            return null;
        }

        /// <summary>
        /// return the segmentid of live test semgent of specified market
        /// </summary>
        /// <param name="marketId"></param>
        /// <returns></returns>
        public Guid GetLiveTestSegment(int marketId)
        {
            return GetLiveSegmentByPS(marketId, ProductionStatus.Test);
        }

        public Guid GetLiveSegmentByPS(int marketId, ProductionStatus ps)
        {
            SdDeploymentTask[] sdts = ssdSvc.GetDeploymentTasksOfLiveSegments(DeliveryChannel.DAP, marketId);
            foreach (SdDeploymentTask t in sdts)
            {
                if (t.DapChannelCfg.ProductionStatus == ps) return (Guid)t.SegmentId;
            }
            Log("marketId=" + marketId);
            throw new Exception("not find task.");
        }

        public Guid GetLiveSegmentById(int marketId, ProductionStatus ps, Guid gid)
        {
            SdDeploymentTask[] sdts = ssdSvc.GetDeploymentTasksOfLiveSegments(DeliveryChannel.DAP, marketId);
            foreach (SdDeploymentTask t in sdts)
            {
                if (t.Id == gid && t.DapChannelCfg.ProductionStatus == ps) return (Guid)t.SegmentId;
            }
            Log("gid=" + gid);
            throw new Exception("not find task.");
        }


        public RuleSegment CreateSimpleRuleSegment()
        {
            string userId = TestRoutines.GetUserIdByName(userNameEd);
            string projectId = TestRoutines.GetProjectIdByName(userId, projectName);
            string strFrequence = "5";
            //string attributeKW = "285cd54a-0d77-4a3a-bb8e-3be1ecccbf80";
            //string attributeAtlas = "174ad38d-cf5c-498e-acd7-645457b42d07";
            string simpleSegmentId = System.Guid.NewGuid().ToString().ToLower();
            RuleSegment segment = new RuleSegment();
            string countryName = string.Empty;
            IRuleAttribute attService = PseudoChannelCenter.GetChannel<IRuleAttribute>();
            string passportCountryAttribId = attService.GetFixedPassportCountryId();
            string passportAgeAttribId = attService.GetFixedPassportCountryId();
            segment.SegmentName = GetName("Rule");
            segment.Description = "No description.";
            segment.OwnerId = userId;
            segment.ProjectId = projectId;
            segment.CreateTime = DateTime.UtcNow;
            segment.SegmentId = simpleSegmentId;

            SegmentRule[] rules = new SegmentRule[1];
            rules[0] = new SegmentRule();
            rules[0].SegmentId = segment.SegmentId;
            rules[0].RuleId = System.Guid.NewGuid().ToString().ToLower();
            rules[0].IndexNo = 0;

            RuleConstraint[] constraints = new RuleConstraint[1];
            constraints[0] = new RuleConstraint();
            constraints[0].ConstraintId = System.Guid.NewGuid().ToString().ToLower();
            constraints[0].ConstraintValue = "United States";
            constraints[0].DataType = RuleAttributeDataType.Text;
            constraints[0].RuleId = rules[0].RuleId;
            constraints[0].IndexNo = 1;

            ConstraintExpression[] expression1 = new ConstraintExpression[1];
            expression1[0] = new ConstraintExpression();
            expression1[0].ExpressionId = System.Guid.NewGuid().ToString().ToLower();
            expression1[0].AttributeId = passportCountryAttribId;
            expression1[0].Operator = "=";
            expression1[0].IndexNo = 0;
            expression1[0].ConstraintId = constraints[0].ConstraintId;
            expression1[0].ExpressionValue = "United States";

            constraints[0].Expressions = expression1;

            rules[0].Constraints = constraints;
            segment.Rules = rules;
            segment.FullExpression = "";// string.Format("[{0}] + [{1}] > '{2}'", attributeKW, attributeAtlas, strFrequence);

            IRuleSegment irs = PseudoChannelCenter.GetChannel<IRuleSegment>();
            irs.Add(segment);

            return segment;
        }

        public SdDeploymentTask[] GetTask_Created()
        {
            return ssdSvc.GetLatestDeploymentTasks(DeliveryChannel.DAP, SdTaskStage.Created, null);
        }

        public SdDeploymentTask[] GetTask_Assigned()
        {
            return ssdSvc.GetLatestDeploymentTasks(DeliveryChannel.DAP, SdTaskStage.Assigned, null);
        }

        public SdDeploymentTask[] GetTask_Completed()
        {
            List<SdDeploymentTask> retTasks = new List<SdDeploymentTask>();
            SdDeploymentTask[] tasks = ssdSvc.GetLatestDeploymentTasks(DeliveryChannel.DAP, SdTaskStage.Completed, null);
            foreach (SdDeploymentTask t in tasks)
            {
                if (t.DapChannelCfg.AdExpertTag == null)
                    retTasks.Add(t);
            }

            return retTasks.ToArray<SdDeploymentTask>();

            //return ssdSvc.GetLatestDeploymentTasks(DeliveryChannel.DAP, SdTaskStage.Completed, null);
        }

        public SdDeploymentTask[] GetTask_AllocatedTag()
        {
            List<SdDeploymentTask> retTasks = new List<SdDeploymentTask>();
            SdDeploymentTask[] tasks = ssdSvc.GetLatestDeploymentTasks(DeliveryChannel.DAP, SdTaskStage.Completed, null);
            foreach (SdDeploymentTask t in tasks)
            {
                if (t.DapChannelCfg.AdExpertTag != null)
                    retTasks.Add(t);
            }

            return retTasks.ToArray<SdDeploymentTask>();

        }

        public MDSSegment GetMDSSegById(Guid id)
        {
            MDSSegment mdsseg = new MDSSegment(id);

            return mdsseg;
        }

        public SRSegment GetSRSegById(Guid id)
        {
            SRSegment srseg = null;
            string sqlTxt = string.Format("SELECT * from Segment where id='{0}'", id);
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting("BTSRDBConn")))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    //sdr.GetValues();
                    if (sdr.Read())
                    {
                        srseg = new SRSegment(sdr);
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }
            }

            return srseg;
        }

        public SRModel GetSRModelById(int modelrevisionid)
        {
            SRModel srmodel = new SRModel(modelrevisionid);
            
            return srmodel;
        }
        #endregion

        public void Log(string s)
        {
            //Log(s);
        }
        public void Log(string s, object o) { }
        public void Log(string s, object o, object o1) { }
        public void Log(string s, object o, object o1, object o2) { }
        public void Log(string s, object o, object o1, object o2, object o3) { }
        public void Log(string s, object o, object o1, object o2, object o3, object o4) { }


    }

    public class CapacityInfo
    {
        public string countryCode { get; set; }
        public int marketId { get; set; }
        public int capacity { get; set; }
        public int used { get; set; }
        public int reusable { get; set; }
    }

    public class MDSSegment
    {
        //0 Id
        //1 IntId
        //2 Type
        //3 AuthoringFriendlyName
        //4 AuthoringDescription
        //5 DistributionChannel
        //6 AttributeDefinitionId
        //7 ABTestConfiguration
        //8 AdvertiserID    ==30131, should be configurable
        //9 AdvertiserType  ==1
        //10 Accessibility
        //11 CreatedTime
        //12 UpdatedTime    
        //13 AuthoringUpdatedTime
        //14 Version 
        //15 ApplicationId  ==1
        //16 SegmentLifeCycleStatus
        //17 AuthoringStatus
        //18 RuntimeStatus
        //19 MembershipExpirationSec //Only LUX will set
        //20 AdCenterFriendlyName
        //21 AdCenterDescription
        //22 AdCenterVerticalId
        //23 AdCenterIsAvailableForSale
        //24 AdCenterCPCfloorPrice
        //25 AdCenterCPAfloorPrice
        //26 AdCenterCPMfloorPrice
        //27 AdCenterCurrencyId
        //28 AdExpertFriendlyName
        //29 AdExpertDescription
        //30 AdExpertVerticalId

        public string Id;
        public long IntId;
        public byte Type;
        public string AuthoringFriendlyName;
        public string AuthoringDescription;
        public int DistributionChannel;
        public string AttributeDefinitionId;
        public string ABTestConfiguration;
        public int AdvertiserID;
        public byte AdvertiserType;
        public byte Accessibility;
        public DateTime CreatedTime;
        public DateTime UpdatedTime;
        public DateTime AuthoringUpdatedTime;
        public int Version;
        public int ApplicationId;
        public byte SegmentLifeCycleStatus;
        public byte AuthoringStatus;
        public byte RuntimeStatus;
        public int MembershipExpirationSec;
        public string AdExpertFriendlyName;
        public string AdExpertDescription;
        public int? AdExpertVerticalId;

        public int DeliveryChannelId;
        public int ProcessingEngineId;

        public MDSSegment(SqlDataReader sdr)
        {
            InitMDSSeg(sdr);
        }

        public MDSSegment(Guid id)
        {
            string sqlTxt = string.Format("SELECT * from SegmentDefinition where id='{0}'", id);
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting("MDSDBConn")))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    //sdr.GetValues();
                    if (sdr.Read())
                    {
                        InitMDSSeg(sdr);
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }

                //get delivery channel
                sqlTxt = string.Format("select DeliveryChannelId from SegDefDeliveryChannelConfig where SegDefId='{0}'",id);
                scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    //sdr.GetValues();
                    if (sdr.Read())
                    {
                        DeliveryChannelId = sdr.GetInt32(0);
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }

                //get process engine
                sqlTxt = string.Format("select ProcessingEngineId from SegDefProcessingEngineConfig where SegDefId='{0}'", id);
                scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    //sdr.GetValues();
                    if (sdr.Read())
                    {
                        ProcessingEngineId = sdr.GetInt32(0);
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }
            }


        }
        public void InitMDSSeg(SqlDataReader sdr)
        {
            Object[] values = new Object[32];
            sdr.GetValues(values);

            Id = sdr.GetGuid(0).ToString();
            IntId = (long)values[1];
            Type = (byte)values[2];
            AuthoringFriendlyName = values[3].ToString();
            if (sdr.IsDBNull(4)) AuthoringDescription = null; else AuthoringDescription = values[4].ToString();
            DistributionChannel = (int)values[5];
            AttributeDefinitionId = values[6].ToString();
            if (sdr.IsDBNull(7)) ABTestConfiguration = null; else ABTestConfiguration = values[7].ToString();
            AdvertiserID = (int)values[8];
            AdvertiserType = (byte)values[9];
            Accessibility = (byte)values[10];
            CreatedTime = (DateTime)values[11];
            UpdatedTime = (DateTime)values[12];
            AuthoringUpdatedTime = (DateTime)values[13];
            Version = (int)values[14];
            ApplicationId = (int)values[15];
            SegmentLifeCycleStatus = (byte)values[16];
            AuthoringStatus = (byte)values[17];
            RuntimeStatus = (byte)values[18];
            MembershipExpirationSec = (int)values[19];
            if (sdr.IsDBNull(28)) AdExpertFriendlyName = null; else AdExpertFriendlyName = values[28].ToString();
            if (sdr.IsDBNull(29)) AdExpertDescription = null; else AdExpertDescription = values[29].ToString();
            if (sdr.IsDBNull(30)) AdExpertVerticalId = null; else AdExpertVerticalId = (int?)values[30];
        }
    }

    public class SRSegment
    {
        //0 Id
        //1 AuthoringId
        //2 BusinessSegmentName
        //3 ModelRevisionId
        //4 LiveABTestingRequestRevisionId
        //5 DeploymentTaskId
        //6 CreatedTime
        //7 LastUpdatedTime
        //8 LiveTime
        //9 FlushedTime
        //10 MarketId
        //11 VerticalId
        //12 SegmentLifeCycleStatusId
        //13 OwnerId
        //14 AccessibilityId

        public string Id;
        public int AuthoringId;
        public string BusinessSegmentName;
        public int? ModelRevisionId;
        public int? LiveABTestingRequestRevisionId;
        public string DeploymentTaskId;
        public DateTime CreatedTime;
        public DateTime LastUpdatedTime;
        public DateTime LiveTime;
        public DateTime? FlushedTime;
        public int MarketId;
        public int VerticalId;
        public int SegmentLifeCycleStatusId;
        public string OwnerId;
        public int AccessibilityId;

        public SRSegment(SqlDataReader sdr)
        {
            Object[] values = new Object[15];
            sdr.GetValues(values);

            Id = values[0].ToString();
            AuthoringId = (int)values[1];
            BusinessSegmentName = values[2].ToString();
            if (sdr.IsDBNull(3)) ModelRevisionId = null; else ModelRevisionId = (int?)values[3];
            if (sdr.IsDBNull(4)) LiveABTestingRequestRevisionId = null; else LiveABTestingRequestRevisionId = (int?)values[4];
            DeploymentTaskId = values[5].ToString();
            CreatedTime = (DateTime)values[6];
            LastUpdatedTime = (DateTime)values[7];
            LiveTime = (DateTime)values[8];
            if (sdr.IsDBNull(9)) FlushedTime = null; else FlushedTime = (DateTime)values[9];
            //FlushedTime = sdr.IsDBNull(9) ? DateTime.MinValue : (DateTime)values[9];
            MarketId = (int)values[10];
            VerticalId = (int)values[11];// sdr.GetInt32(11);
            SegmentLifeCycleStatusId = (int)values[12];// sdr.GetInt32(12);
            OwnerId = values[13].ToString();// sdr.GetGuid(13).ToString();
            if (!sdr.IsDBNull(14)) AccessibilityId = (byte)values[14];
        }
    }

    public class SRModel
    {
        public int RevisionId;
        public int AuthoringId;
        public int RevisionNum;
        public string ReferenceId;
        public string AuthoringName;
        public string Description;
        //Definition
        public DateTime CreatedTime;
        public DateTime LastUpdatedTime;
        public DateTime? StartTime;
        public DateTime? EndTime;
        public int SegmentSourceId;
        public int TypeId;
        public bool IsLive;
        public int SourceExtInfo;
        public string OwnerId;
        public bool IsDeleted;
        public bool IsDeployed;
        
        public int ProcessEngineId;
        public string ProcessEngingValue; 

        public SRModel(SqlDataReader sdr)
        {
            InitSRModel(sdr);
        }

        public SRModel(int revisionid)
        {
            string sqlTxt = string.Format("SELECT * from SegmentModel where RevisionId='{0}'", revisionid);
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting("BTSRDBConn")))
            {
                conn.Open();
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    //sdr.GetValues();
                    if (sdr.Read())
                    {
                        InitSRModel(sdr);
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }

                //get processengine
                sqlTxt = "select ProcessEngineId from SegmentModelProcessEngine where ModelRevisionId="+revisionid;
                scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    //sdr.GetValues();
                    if (sdr.Read())
                    {
                        ProcessEngineId = sdr.GetInt32(0);
                    }
                    sdr.Close();
                }
                finally
                { scd.Dispose(); }

                //get processenging value
                if (ProcessEngineId == 2)
                {
                    sqlTxt = "select Value from SegmentModelProcessEngineSetting where ModelRevisionId=" + revisionid;
                    scd = new SqlCommand(sqlTxt, conn);
                    try
                    {
                        SqlDataReader sdr = scd.ExecuteReader();
                        //sdr.GetValues();
                        if (sdr.Read())
                        {
                            ProcessEngingValue = sdr.GetString(0);
                        }
                        sdr.Close();
                    }
                    finally
                    { scd.Dispose(); }
                }
                else
                {
                    ProcessEngingValue = "0";
                }
            }
        }

        public void InitSRModel(SqlDataReader sdr)
        {
            Object[] values = new Object[18];
            sdr.GetValues(values);

            RevisionId = (int)values[0];
            AuthoringId = (int)values[1];
            RevisionNum = (int)values[2];
            ReferenceId = values[3].ToString();
            AuthoringName = values[4].ToString();
            Description = values[5].ToString();
            CreatedTime = (DateTime)values[7];
            LastUpdatedTime = (DateTime)values[8];
            if (sdr.IsDBNull(9)) StartTime = null; else StartTime = (DateTime)values[9];
            if (sdr.IsDBNull(10)) EndTime = null; else EndTime = (DateTime)values[10];
            SegmentSourceId = (int)values[11];//1=SegmentStudio
            TypeId = (int)values[12];
            IsLive = (bool)values[13];
            //SourceExtInfo = (int)values[14];
            OwnerId = values[15].ToString();
            IsDeleted = (bool)values[16];
            IsDeployed = (bool)values[17];
        }
    }
}
