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
    class GTMMigration
    {
        private TestContext testContextInstance;
        private GTMTestHelper ta;
        private string SSDBConn = "SSDBConn";
        private string SRDBConn = "BTSRDBConn";
        private string MDSDBConn = "MDSDBConn";

        Dictionary<string,MDSSegment> MDSSegmentsMap = new Dictionary<string,MDSSegment>();
        Dictionary<string,SRSegment> SRSegmentsMap = new Dictionary<string,SRSegment>();
        Dictionary<int, SRModel> SRModelsMap = new Dictionary<int, SRModel>();
        Dictionary<int, int> mapDC2Market = new Dictionary<int, int>() {
            {0,0},{1,191},{2,96},{3,189},{4,9},{5,72},{6,66},{7,32},{9,100},{10,175},{11,134},{12,164},
            {13,112},{14,10},{15,170},{17,119},{18,40},{19,39},{20,201},{21,129},{22,20},{23,93},{24,176},
            {26,14},{28,65},{29,53},{30,139},{31,168},{32,90},{33,156}
        };

        GTMMigration()
        {
            ta = new GTMTestHelper(testContextInstance);
        }
        static void Main(string[] args)
        {
            GTMMigration g = new GTMMigration();
            g.CheckPublishedSegmentModel();
            g.CheckVertical();
            //g.initData();
            //g.CheckMDS();

            g.Log("=====END=====");
            Console.ReadKey();
        }

        public void initData()
        {
            Log("----Start init the data");
            //init the segments
            GetSRSegments();
            GetMDSSegments();
            GetSRSegmentModels();
        }

        public void CheckMDS()
        {
            Log("---Check consitency---");
            foreach (var pair in MDSSegmentsMap)
            {
                CheckConsistent(pair.Value);
            }
            Log("---Check segment type---");
            foreach (var pair in MDSSegmentsMap)
            {
                CheckSegmentType(pair.Value);
            }
            Log("---Check Distributeion Channel---");
            foreach (var pair in MDSSegmentsMap)
            {
                CheckDistributionChannel(pair.Value);
            }

            CheckAccessbility();
        }

        #region Check MDS DATA
        public void CheckConsistent(MDSSegment mdsseg)
        {
            string segmentid = mdsseg.Id;
            if (!SRSegmentsMap.ContainsKey(segmentid))
            {
                Log("Segment " + segmentid + " in MDS but not in BTSR, SegmentLifeCycleStatus=" + mdsseg.SegmentLifeCycleStatus);
                return;
            }
            SRSegment srseg = SRSegmentsMap[segmentid];
            int? revid = srseg.ModelRevisionId;
            if (revid == null)
            {
                Log("Segment " + segmentid + " in BTSR has no ModelRevisionId");
                return;
            } 

        }

        /// <summary>
        ///	1: RuleBased
        ///	2: Taxonomy
        ///	3: Mixed
        ///	4: PM Segment
        ///	5: SDR
        /// </summary>
        /// <param name="mdsseg"></param>
        public void CheckSegmentType(MDSSegment mdsseg)
        {
            string segmentid = mdsseg.Id;
            if (!SRSegmentsMap.ContainsKey(segmentid))
            {
                return;
            }
            SRSegment srseg = SRSegmentsMap[segmentid];
            int? revid = srseg.ModelRevisionId;
            if (revid == null)
            {
                return;
            } 
            SRModel srmodel = SRModelsMap[revid.Value];

            if (srmodel.TypeId != mdsseg.Type)
            {
                Log(string.Format("Type inconsistent, segmentid={1}, mds type={2}, model type={3}",
                    segmentid, mdsseg.Type, srmodel.TypeId));
            }
        }

        public void CheckDistributionChannel(MDSSegment mdsseg)
        {
            string segmentid = mdsseg.Id;
            if (!SRSegmentsMap.ContainsKey(segmentid))
            {
                return;
            }
            SRSegment srseg = SRSegmentsMap[segmentid];
            int? revid = srseg.ModelRevisionId;
            if (revid == null)
            {
                return;
            }
            SRModel srmodel = SRModelsMap[revid.Value];

            if (srmodel.TypeId != mdsseg.Type)
            {
                Log(string.Format("Type inconsistent, segmentid={1}, mds type={2}, model type={3}",
                    segmentid, mdsseg.Type, srmodel.TypeId));
            }
        }

        /// <summary>
        /// MDS Segment AttributeDefinitionId == Model Id
        /// </summary>
        /// <param name="mdsseg"></param>
        public void CheckAuthoringDefinitionId(MDSSegment mdsseg)
        {
            string segmentid = mdsseg.Id;
            if (!SRSegmentsMap.ContainsKey(segmentid)) return;

            SRSegment srseg = SRSegmentsMap[segmentid];
            
            int? revid = srseg.ModelRevisionId;
            if (revid == null) return;

            SRModel model = SRModelsMap[revid.Value];
            if(!(model.ReferenceId != mdsseg.AttributeDefinitionId)){
                Log(string.Format("MDS AttributeDefinitionId {1} not equal to model id {2}",
                    mdsseg.AttributeDefinitionId,model.ReferenceId));
            };
        }

        /// <summary>
        /// 1. MSN should be private
        /// 2. Custom in list  should be private
        /// </summary>
        /// <param name="mdsseg"></param>
        public void CheckAccessbility()
        {
            //get the segment id delivery channel = MSN
            List<string> msnSegmentIds = new List<string>();
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting(MDSDBConn)))
            {
                conn.Open();
                string sqlTxt = "SELECT * FROM SegDefDeliveryChannelConfig WHERE DeliveryChannelId=3";
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        msnSegmentIds.Add(sdr.GetString(0));
                    }
                }
                finally { scd.Dispose(); }
            }

            Log("Get MSN segment count=" + msnSegmentIds.Count);

            foreach(string segId in msnSegmentIds){
                MDSSegment mdsseg = MDSSegmentsMap[segId];
                if (mdsseg.Accessibility != (byte)Accessibility.Private)
                {
                    Log(string.Format("MDS MSN segment {1} accessbility is not private",
                        segId));
                };
            }

        }

        /// <summary>
        /// The segment in SSDB of publishstatus=1 should have segment model in SRDB
        /// </summary>
        public void CheckPublishedSegmentModel()
        {
            Log("---start to verify the published segment");
            int ret=0;
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting(SRDBConn)))
            {
                
                conn.Open();
                string sqlTxt = @"SELECT count(*)
                        FROM [SegmentStudioDB].[dbo].[Segment] ss 
                        where PublishStatus=1 and ss.AuthoringSegmentId not in (select sm.AuthoringId from [BTSRDB].dbo.SegmentModel sm)";
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
            if (ret == 0) Log("---PASSED");
            else Log(string.Format("---there are {0} segments published and not deployed missed in segmentmodel table",ret));
        }

        public void CheckVertical()
        {
            Log("---start to verify the vertical setting after migration");
            string sql1 = "select COUNT(*) from SegmentDefinition where ApplicationId=1 and AdExpertVerticalId is null";
            int count1 = ta.GetIntFromDB(sql1, MDSDBConn);
            if (count1 > 0)
            {
                Log(string.Format("There are {0} segment of AdExpertVerticalId is null", count1));
            }

            string sql2 = @"select COUNT(*) from SegmentDefinition where ApplicationId=1 and AdCenterVerticalId is null";
            int count2 = ta.GetIntFromDB(sql2, MDSDBConn);
            if (count2 > 0)
            {
                Log(string.Format("There are {0} segment which AdCenterVerticalId is null", count2));
            }
            Log("---end verify the vertical setting after migration");
        }
        #endregion

        #region Utility
        //utils
        public void GetSRSegments()
        {
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting(SRDBConn)))
            {
                conn.Open();
                string sqlTxt = "select * from segment";
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        SRSegment seg = new SRSegment(sdr);
                        SRSegmentsMap.Add(seg.Id,seg);
                    }
                }
                finally { scd.Dispose(); }
            }
            Log("Get SR segment count=" + SRSegmentsMap.Count);
        }

        public void GetMDSSegments()
        {
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting(MDSDBConn)))
            {
                conn.Open();
                string sqlTxt = "SELECT * FROM SegmentDefinition WHERE ApplicationId=1";
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        MDSSegment seg = new MDSSegment(sdr);
                        MDSSegmentsMap.Add(seg.Id, seg);
                    }
                }
                finally { scd.Dispose(); }
            }
            Log("Get MDS segment count=" + MDSSegmentsMap.Count);
            
        }

        public void GetSRSegmentModels()
        {
            using (SqlConnection conn = new SqlConnection(TestRoutines.GetAppSetting(SRDBConn)))
            {
                conn.Open();
                string sqlTxt = @"SELECT [RevisionId]
                                      ,[AuthoringId]
                                      ,[RevisionNum]
                                      ,[ReferenceId]
                                      ,[AuthoringName]
                                      ,[Description]
                                      ,[CreatedTime]
                                      ,[LastUpdatedTime]
                                      ,[StartTime]
                                      ,[EndTime]
                                      ,[SegmentSourceId]
                                      ,[TypeId]
                                      ,[IsLive]
                                      ,[OwnerId]
                                      ,[IsDeleted]
                                      ,[IsDeployed]
                                  FROM [BTSRDB].[dbo].[SegmentModel]";
                SqlCommand scd = new SqlCommand(sqlTxt, conn);
                try
                {
                    SqlDataReader sdr = scd.ExecuteReader();
                    while (sdr.Read())
                    {
                        SRModel seg = new SRModel(sdr);
                        SRModelsMap.Add(seg.RevisionId, seg);
                    }
                }
                finally { scd.Dispose(); }
            }
            Log("Get SR segment model count=" + SRModelsMap.Count);
            
        }

        public void Log(string s)
        {
            Console.WriteLine(s);
        }
        #endregion
    }


    public class MDSSegment
    {
        //0 Id
        //1 IntId
        //3 Type
        //4 AuthoringFriendlyName
        //5 AuthoringDescription
        //6 DistributionChannel
        //7 AttributeDefinitionId
        //8 ABTestConfiguration
        //9 AdvertiserID
        //11Accessibility
        //15Vrsion
        //16ApplicationId
        //17SegmentLifeCycleStatus
        //18AuthoringStatus
        //19RuntimeStatus
        //20MembershipExpirationSec
        //21AdCenterFriendlyName
        //22AdCenterDescription
        //23AdCenterVerticalId
        //24AdCenterIsAvailableForSale
        //25AdCenterCPCfloorPrice
        //26AdCenterCPAfloorPrice
        //27AdCenterCPMfloorPrice
        //28AdCenterCurrencyId
        //29AdExpertFriendlyName
        //30AdExpertDescription
        //31AdExpertVerticalId

        public string Id;
        public long IntId;
        public byte Type;
        public string AuthoringFriendlyName;
        public string AuthoringDescription;
        public int DistributionChannel;
        public string AttributeDefinitionId;
        public string ABTestConfiguration;
        public byte Accessibility;
        public DateTime CreatedTime;
        public DateTime UpdatedTime;
        public DateTime AuthoringUpdatedTime;
        public int ApplicationId;
        public byte SegmentLifeCycleStatus;
        public byte AuthoringStatus;
        public byte RuntimeStatus;
        public int? AdExpertVerticalId;

        public MDSSegment(SqlDataReader sdr)
            {
                Object[] values = new Object[32];
                sdr.GetValues(values);

                Id = sdr.GetGuid(0).ToString();
                IntId = (long)values[1];
                Type = (byte)values[2];
                AuthoringFriendlyName = values[3].ToString();
                AuthoringDescription = values[4].ToString();
                DistributionChannel = (int)values[5];
                AttributeDefinitionId = values[6].ToString();
                ABTestConfiguration = values[7].ToString();
                Accessibility = (byte)values[10];
                CreatedTime = (DateTime)values[11];
                UpdatedTime = (DateTime)values[12];
                AuthoringUpdatedTime = (DateTime)values[13];
                ApplicationId = (int)values[15];
                SegmentLifeCycleStatus = (byte)values[16];
                AuthoringStatus = (byte)values[17];
                RuntimeStatus = (byte)values[18];
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

        public SRSegment(SqlDataReader sdr){
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

        public SRModel(SqlDataReader sdr){
            Object[] values = new Object[16];
            sdr.GetValues(values);
            
            RevisionId = (int)values[0];
            AuthoringId = (int)values[1];
            RevisionNum = (int)values[2];
            ReferenceId = values[3].ToString();
            AuthoringName = values[4].ToString();
            Description = values[5].ToString();
            CreatedTime = (DateTime)values[6];
            LastUpdatedTime = (DateTime)values[7];
            if (sdr.IsDBNull(8)) StartTime = null; else StartTime = (DateTime)values[8];
            if (sdr.IsDBNull(9)) EndTime = null; else EndTime = (DateTime)values[9];
            SegmentSourceId = (int)values[10];//1=SegmentStudio
            TypeId = (int)values[11];
            IsLive = (bool)values[12];
            //SourceExtInfo = (int)values[13];
            OwnerId = values[13].ToString();
            IsDeleted = (bool)values[14];
            IsDeployed = (bool)values[15];
        }
    }
}
