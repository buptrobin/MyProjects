using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoricalCTR
{
    using System.IO;

    class Program
    {
        public const string TagListingCTR = "ListingCTR";
        public const string TagLocationCTR = "LocationCTR";
        public const string TagQueryListingLocationCTR = "QueryListingLocationCTR";

        //Listing CTR index files
        private static string AdUserDataDirPath = @"\\rowa05\ctr";
//        private static string ListingCTRFile = "ListingCTR.txt";
//        private static string ListingCTRIndexDir = "ListingCTRIndex";
//        private static string ListingLocationCTRFile = "ListingLocationCTR.txt";
//        private static string ListingLocationCTRIndexDir = "ListingLocationCTRIndex";
//        private static string QueryListingLocationCTRFile = "QueryListingLocationCTR.txt";
//        private static string QueryListingLocationCTRIndexDir = "QueryListingLocationCTRIndex";

        //Historical CTR information for pClick debugging
        private static string AdAdjustedCTRFile = "AdAdjustedCTR.txt";
        private static string AdAdjustedCTRIndexDir = "AdAdjustedCTRIndex";
        private static string ListingAdAdjustedCTRFile = "ListingAdAdjustedCTR.txt";
        private static string ListingAdAdjustedCTRIndexDir = "ListingAdAdjustedCTRIndex";
        private static string QueryAdML1CTRFile = "QueryAdML1CTR.txt";
        private static string QueryAdML1CTRIndexDir = "QueryAdML1CTRIndex";
        private static string QueryAdAdjustedCTRFile = "QueryAdAdjustedCTR.txt";
        private static string QueryAdAdjustedCTRIndexDir = "QueryAdAdjustedCTRIndex";
        private static string QueryListingAdAdjustedCTRFile = "QueryListingAdAdjustedCTR.txt";
        private static string QueryListingAdAdjustedCTRIndexDir = "QueryListingAdAdjustedCTRIndex";

        private static readonly string LogCategory = "ListingCTRDataProvider";
        private static readonly string AdAdjustedCTRLogCategory = "AdAdjustedCTR";
        private static readonly string QueryAdML1CTRLogCategory = "QueryAdML1CTR";
        private static readonly string QueryAdAdjustedCTRLogCategory = "QueryAdAdjustedCTR";
        private static readonly string ListingAdAdjustedCTRLogCategory = "ListingAdAdjustedCTR";
        private static readonly string QueryListingAdAdjustedCTRLogCategory = "QueryListingAdAdjustedCTR";

        static void Main(string[] args)
        {
            string query = "car";
            long listingid = 5240561251;
            long adid = 244116492;
            if (args.Length > 0)
            {
                AdUserDataDirPath = args[0];
                query = args[1];
                listingid = long.Parse(args[2]);
                adid = long.Parse(args[3]);
            }

            Console.WriteLine("AdUserDataDirPath={0}, Query={1}, ListingId={2}, AdId={3}", AdUserDataDirPath, query, listingid, adid);

            GetCTR(query, listingid, adid);
            Console.ReadKey();
        }


        public static void GetCTR(string query, long listingid, long adid)
        {
            CTRResult info = GenerateAdAdjustedCTRInfo(adid);
            CTRResultForUI AdAdjustedCTR = CTRResultForUI.ParseCTRResult(info, true);

            info = GenerateListingAdAdjustedCTRInfo(adid, listingid);
            CTRResultForUI ListingAdAdjustedCTR = CTRResultForUI.ParseCTRResult(info, true);

            info = GenerateQueryAdML1CTRInfo(query, adid);
            CTRResultForUI QueryAdML1CTR = CTRResultForUI.ParseCTRResult(info, false);

            info = GenerateQueryAdAdjustedCTRInfo(query, adid);
            CTRResultForUI QueryAdAdjustedCTR = CTRResultForUI.ParseCTRResult(info, true);

            info = GenerateQueryListingAdAdjustedCTRInfo(query, listingid, adid);
            CTRResultForUI QueryListingAdAdjustedCTR = CTRResultForUI.ParseCTRResult(info, true);

            Console.WriteLine("CleanKeyword*Ad ML-1 CTR={0} CTRTip={1}", QueryAdML1CTR.CTR, QueryAdML1CTR.CTRTip);
            Console.WriteLine("Listing*Ad Adjusted CTR={0} CTRTip={1}", ListingAdAdjustedCTR.CTR, ListingAdAdjustedCTR.CTRTip);
            Console.WriteLine("Ad Adjusted CTR={0} CTRTip={1}", AdAdjustedCTR.CTR, AdAdjustedCTR.CTRTip);
            Console.WriteLine("CleanKeyword*Ad Adjusted CTR ={0} CTRTip={1}", QueryAdAdjustedCTR.CTR, QueryAdAdjustedCTR.CTRTip);
            Console.WriteLine("CleanKeyword*Listing*Ad Adjusted CTR={0} CTRTip={1}", QueryListingAdAdjustedCTR.CTR, QueryListingAdAdjustedCTR.CTRTip);
        }

        public static CTRResult GenerateQueryAdML1CTRInfo(string cleanKeyword, long adId)
        {
            try
            {
                CTRResult result = null;
                UserDataLogFile userDataLog = new UserDataLogFile(AdUserDataDirPath, QueryAdML1CTRFile, QueryAdML1CTRIndexDir);
                string[] matchedCTRInfo = userDataLog.GetMatchedLines(cleanKeyword, adId.ToString()); // 1 line for each query.

                if (matchedCTRInfo != null && matchedCTRInfo.Length > 0)
                {
                    if (matchedCTRInfo.Length == 1)
                    {
                        //No adjusted impression for Query*Ad ML1 CTR
                        result = Parse(matchedCTRInfo[0]);
                    }
                    else
                    {
                        Console.WriteLine("[Data Error]: There are {0} lines data associated with cleanKeyword = {1} and AdId = {2}",
                            matchedCTRInfo.Length, cleanKeyword, adId);
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("[Data Warning]: There is no data associated with cleanKeyword = {0}, AdId = {1}",
                            cleanKeyword, adId);
                }

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static CTRResult GenerateQueryAdAdjustedCTRInfo(string cleanKeyword, long adId)
        {
            try
            {
                CTRResult result = null;
                UserDataLogFile userDataLog = new UserDataLogFile(AdUserDataDirPath, QueryAdAdjustedCTRFile, QueryAdAdjustedCTRIndexDir);
                string[] matchedCTRInfo = userDataLog.GetMatchedLines(cleanKeyword, adId.ToString()); // 1 line for each query.

                if (matchedCTRInfo != null && matchedCTRInfo.Length > 0)
                {
                    if (matchedCTRInfo.Length == 1)
                    {
                        result = ParseLineWithAdjustedImpression(matchedCTRInfo[0]);
                    }
                    else
                    {
                        Console.WriteLine(
                            "[Data Error]: There are {0} lines data associated with cleanKeyword = {1} and AdId = {2}",
                            matchedCTRInfo.Length, cleanKeyword, adId);
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine(
                            "[Data Warning]: There is no data associated with cleanKeyword = {0}, AdId = {1}",
                            cleanKeyword, adId);
                }

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static CTRResult GenerateQueryListingAdAdjustedCTRInfo(string cleanKeyword, long listingId, long adId)
        {
            try
            {
                CTRResult result = null;
                UserDataLogFile userDataLog = new UserDataLogFile(AdUserDataDirPath, QueryListingAdAdjustedCTRFile, QueryListingAdAdjustedCTRIndexDir);

                string listingAdPairKey = listingId.ToString() + "+" + adId.ToString();
                string[] matchedCTRInfo = userDataLog.GetMatchedLines(cleanKeyword, listingAdPairKey); // 1 line for each query.

                if (matchedCTRInfo != null && matchedCTRInfo.Length > 0)
                {
                    if (matchedCTRInfo.Length == 1)
                    {
                        result = ParseLineWithAdjustedImpression(matchedCTRInfo[0]);
                    }
                    else
                    {
                        Console.WriteLine("[Data Error]: There are {0} lines data associated with cleanKeyword = {1}, ListingId = {2}, AdId = {3}",
                            matchedCTRInfo.Length, cleanKeyword, listingId, adId);
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("[Data Warning]: There is no data associated with cleanKeyword = {0}, ListingId = {1}, AdId = {2}",
                            cleanKeyword, listingId, adId);
                }

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static CTRResult GenerateAdAdjustedCTRInfo(long adId)
        {
            try
            {
                CTRResult result = null;
                UserDataLogFile userDataLog = new UserDataLogFile(AdUserDataDirPath, AdAdjustedCTRFile, AdAdjustedCTRIndexDir);

                //The IndexGenerator needs two keys, so we add a virtual key "0" for it. 
                string[] matchedCTRInfo = userDataLog.GetMatchedLines(adId.ToString(), "0"); // 1 line for each query.

                if (matchedCTRInfo != null && matchedCTRInfo.Length > 0)
                {
                    if (matchedCTRInfo.Length == 1)
                    {
                        result = ParseLineWithAdjustedImpression(matchedCTRInfo[0]);
                    }
                    else
                    {
                        Console.WriteLine("[Data Error]: There are {0} lines data associated with AdId = {1}", matchedCTRInfo.Length, adId);
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("[Data Warning]: There is no data associated with AdId = {0}", adId);
                }

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static CTRResult GenerateListingAdAdjustedCTRInfo(long adId, long listingId)
        {
            try
            {
                CTRResult result = null;
                UserDataLogFile userDataLog = new UserDataLogFile(AdUserDataDirPath, ListingAdAdjustedCTRFile, ListingAdAdjustedCTRIndexDir);
                string[] matchedCTRInfo = userDataLog.GetMatchedLines(adId.ToString(), listingId.ToString()); // 1 line for each query.

                if (matchedCTRInfo != null && matchedCTRInfo.Length > 0)
                {
                    if (matchedCTRInfo.Length == 1)
                    {
                        result = ParseLineWithAdjustedImpression(matchedCTRInfo[0]);
                    }
                    else
                    {
                        Console.WriteLine("[Data Error]: There are {0} lines data associated with ListingId = {1} and AdId = {2}",
                            matchedCTRInfo.Length, listingId, adId);
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("[Data Warning]: There is no data associated with ListingId = {0} and  AdId = {1}", listingId, adId);
                }

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static CTRResult Parse(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] lineInfo = line.Split('\t');

                if (lineInfo.Length == 4)
                {
                    long impressions;
                    long clicks;
                    if (long.TryParse(lineInfo[2], out impressions) && long.TryParse(lineInfo[3], out clicks))
                    {
                        if (impressions >= 0 && clicks >= 0)
                        {
                            return new CTRResult() { Impressions = impressions, Clicks = clicks };
                        }
                    }
                }
            }

            return null;
        }

        private static CTRResult ParseLineWithAdjustedImpression(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] lineInfo = line.Split('\t');

                //The line contains both raw impression and adjusted impression
                if (lineInfo.Length == 5)
                {
                    long impressions;
                    double adjustedImpressions;
                    long clicks;
                    if (long.TryParse(lineInfo[2], out impressions) && double.TryParse(lineInfo[3],out adjustedImpressions) && long.TryParse(lineInfo[4], out clicks))
                    {
                        if (impressions >= 0 && adjustedImpressions >= 0 && clicks >= 0)
                        {
                            return new CTRResult() { Impressions = impressions, AdjustedImpressions = adjustedImpressions, Clicks = clicks };
                        }
                    }
                }

                //The line only contains adjusted impression, no raw impression
                if (lineInfo.Length == 4)
                {
                    double adjustedImpressions;
                    long clicks;
                    if (double.TryParse(lineInfo[2], out adjustedImpressions) && long.TryParse(lineInfo[3], out clicks))
                    {
                        if (adjustedImpressions >= 0 && clicks >= 0)
                        {
                            return new CTRResult() {AdjustedImpressions = adjustedImpressions, Clicks = clicks };
                        }
                    }
                }
            }

            return null;
        }

        private static AdCTRResult GetAdCTRResultForUI(Dictionary<CTRType, CTRResult> dicCTR)
        {
            AdCTRResult adCTRResult = new AdCTRResult();

            foreach (var ctrInfo in dicCTR)
            {
                switch (ctrInfo.Key)
                {
                    case CTRType.AdAdjustedCTR:
                        adCTRResult.AdAdjustedCTR = CTRResultForUI.ParseCTRResult(ctrInfo.Value, true);
                        break;
                    case CTRType.ListingAdAdjustedCTR:
                        adCTRResult.ListingAdAdjustedCTR = CTRResultForUI.ParseCTRResult(ctrInfo.Value, true);
                        break;
                    case CTRType.QueryAdML1CTR:
                        adCTRResult.QueryAdML1CTR = CTRResultForUI.ParseCTRResult(ctrInfo.Value, false);
                        break;
                    case CTRType.QueryAdAdjustedCTR:
                        adCTRResult.QueryAdAdjustedCTR = CTRResultForUI.ParseCTRResult(ctrInfo.Value, true);
                        break;
                    case CTRType.QueryListingAdAdjustedCTR:
                        adCTRResult.QueryListingAdAdjustedCTR = CTRResultForUI.ParseCTRResult(ctrInfo.Value, true);
                        break;

                    default:
                        {
                            throw new ArgumentException("Unsupported CTR type: " + ctrInfo.Key.ToString());
                        }
                }
            }

            return adCTRResult;
        }
    }

    public enum CTRType : uint
    {
        None = 0,
        AggrCTR = 1,
        PositionCTR = 2,
        QueryPositionCTR = 4,

        AdAdjustedCTR = 8,
        ListingAdAdjustedCTR = 16,
        QueryAdAdjustedCTR = 32,
        QueryAdML1CTR = 64,
        QueryListingAdAdjustedCTR = 128,
    }

    public class AdCTRResult
    {
        public CTRResultForUI ListingAdAdjustedCTR { get; set; }
        public CTRResultForUI AdAdjustedCTR { get; set; }
        public CTRResultForUI QueryAdML1CTR { get; set; }
        public CTRResultForUI QueryAdAdjustedCTR { get; set; }
        public CTRResultForUI QueryListingAdAdjustedCTR { get; set; }
    }

    public class CTRResultForUI
    {
        public string CTR { get; set; }
        public string CTRTip { get; set; }

        public static CTRResultForUI ParseCTRResult(CTRResult ctrResult, bool isAdjusted)
        {
            if (ctrResult == null)
            {
                return CTRResultForUI.NoneCTR;
            }

            CTRResultForUI ctrForUI = new CTRResultForUI();

            //The adjusted impression is double type, and may be less than 1. Adding 1 to avoid the impression is 0.
            long adjustedImpression = (long)ctrResult.AdjustedImpressions + 1;
            long impression = (isAdjusted ? adjustedImpression : ctrResult.Impressions);

            if (ctrResult.Clicks == 0 || impression == 0)
            {
                ctrForUI.CTR = "0.00%";
            }
            else
            {
                double ctr = Math.Min(100.0f, ctrResult.Clicks * 100.0f / impression);
                ctrForUI.CTR = ctr.ToString("F2") + "%";
            }

            ctrForUI.CTRTip = ctrResult.Clicks.ToString("n0") + "/" + impression.ToString("n0");

            return ctrForUI;
        }

        public static readonly CTRResultForUI NoneCTR = new CTRResultForUI { CTR = "N/A", CTRTip = "N/A" };
    }

    public class CTRResult
    {
        public long Impressions { get; set; }

        public double AdjustedImpressions { get; set; }

        public long Clicks { get; set; }
    }

    public class UserDataLogFile
    {
        private static string FirstLevelIndexFileName = "FirstLevelIndexFile.txt";
        private static string SecondLevelIndexFileName = "SecondLevelIndexFile.txt";

        private string _dataRootDir;
        private string _baseFile;
        private string _indexDir;
        private Encoding _fileEncoding;
        private long _fileStartPos;

        private static readonly string LogCategory = "ListingCTRDataProvider";

        public UserDataLogFile(string dataRootDir, string baseFile, string indexDir)
        {
            _dataRootDir = dataRootDir;
            _baseFile = baseFile;
            _indexDir = indexDir;
            _fileEncoding = Encoding.UTF8;
            _fileStartPos = _fileEncoding.GetPreamble().Length;
        }

        /// <summary>
        /// The method is used to find out all the lines started with <paramref name="query"/>
        /// in the file specified by <code>_basefile</code>
        /// </summary>
        /// <param name="query">the query string</param>
        /// <returns>all the lines started with the specified query string</returns>
        public string[] GetMatchedLines(string query, string market)
        {
            if (string.IsNullOrEmpty(query))
            {
                Console.WriteLine("The query words used to search is null or empty.");
                return null;
            }

            if (_dataRootDir == null || string.IsNullOrEmpty(_baseFile) || string.IsNullOrEmpty(_indexDir) || _fileEncoding == null)
            {
                Console.WriteLine("dataRootDir, baseFile, indexDir or fileEncoding is null.");
                return null;
            }

            FileStream baseFileFS = null;
            try
            {
                string baseFilePath = Path.Combine(_dataRootDir, _baseFile);
                baseFileFS = new FileStream(baseFilePath, FileMode.Open, FileAccess.Read);

                long[] offsetPair = FindOffsetPair(query, market);

                if (offsetPair == null || offsetPair.Length != 2)
                {
                    Console.WriteLine("Can not find Query: {0} in index file.", query);
                    return null;
                }

                long startPos = offsetPair[0];
                long endPos = offsetPair[1];

                byte[] buffer = new byte[endPos - startPos + 1];
                baseFileFS.Seek(startPos, SeekOrigin.Begin);
                baseFileFS.Read(buffer, 0, buffer.Length);
                return _fileEncoding.GetString(buffer).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("GetMatchedLines Failed: File Path is not valid.");
                throw ex;
            }
            catch (IOException ex)
            {
                Console.WriteLine("GetMatchedLines Failed: Reading base file failed.");
                throw ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("GetMatchedLines Failed: Getting access to base file denied.");
                throw ex;
            }
            finally
            {
                if (baseFileFS != null)
                {
                    baseFileFS.Close();
                }
            }
        }

        /// <summary>
        /// The method is used to fetch the start file offset and end file offset of data related to 
        /// <paramref name="query"/> in the file specified by <code>_basefile</code>
        /// </summary>
        /// <param name="query">the query string</param>
        /// <returns>if the query exists in the <code>_basefile</code>, return the start file offset and end file offset pair, or return null</returns>
        private long[] FindOffsetPair(string query, string market)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }

            string indexDirPath;
            try
            {
                indexDirPath = Path.Combine(_dataRootDir, _indexDir);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("FindOffsetPair Failed: Index directory path is not valid.");
                throw ex;
            }

            //Get the second level index dir's path.
            string secondLevelIndexDirPath = FindNextLevelIndexFile(query, market, indexDirPath, FirstLevelIndexFileName);
            if (string.IsNullOrEmpty(secondLevelIndexDirPath))
            {
                return null;
            }

            //Get the third level index file's path.
            string thirdLevelIndexFilePath = FindNextLevelIndexFile(query, market, secondLevelIndexDirPath, SecondLevelIndexFileName);
            if (string.IsNullOrEmpty(thirdLevelIndexFilePath))
            {
                return null;
            }

            return FindBaseFileOffset(query, market, thirdLevelIndexFilePath);
        }

        /// <summary>
        /// the method is used to get file name of the 2nd level index file which stores the start file offset and end file offset value in <code>_basefile</code>
        /// about <paramref name="query"/>
        /// </summary>
        /// <param name="query">the query string</param>
        /// <returns>2nd level index file name</returns>
        private string FindNextLevelIndexFile(string query, string market, string currentLevelIndexDir, string currentLevelIndexFile)
        {
            StreamReader currentLevelIndexSR = null;
            try
            {
                string currentLevelIndexFilePath = Path.Combine(currentLevelIndexDir, currentLevelIndexFile);
                currentLevelIndexSR = new StreamReader(currentLevelIndexFilePath);

                while (!currentLevelIndexSR.EndOfStream)
                {
                    string line = currentLevelIndexSR.ReadLine();
                    string[] lineInfo = line.Split('\t');
                    //line should have 3 columns: query, market, filename or dirname.
                    if (lineInfo == null || lineInfo.Length != 3)
                    {
                        Console.WriteLine("wrong line format in 1st level index file, it's not like 'queryword\tfilename'.");
                        return null;
                    }
                    int comp1 = string.Compare(query, lineInfo[0], StringComparison.OrdinalIgnoreCase);
                    int comp2 = string.Compare(market, lineInfo[1], StringComparison.OrdinalIgnoreCase);

                    if (comp1 < 0 || (comp1 == 0 && comp2 <= 0))
                    {
                        return Path.Combine(currentLevelIndexDir, lineInfo[2]);
                    }
                }
                return null;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("FindNextLevelIndexFile failed: Directory path is not valid.");
                throw ex;
            }
            catch (IOException ex)
            {
                Console.WriteLine("FindNextLevelIndexFile failed: Reading index file failed.");
                throw ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("FindNextLevelIndexFile failed: Getting access to index file denied.");
                throw ex;
            }
            finally
            {
                if (currentLevelIndexSR != null)
                {
                    currentLevelIndexSR.Close();
                }
            }
        }

        /// <summary>
        /// The method is used to get the start file offset and end file offset of <param name="query"/> in <code>_basefile</code>
        /// from the second level index file specified by <paramref name="secondClassIndexFileName"/>
        /// </summary>
        /// <param name="secondClassIndexFileName">second level index file name</param>
        /// <param name="query">the query string</param>
        /// <returns>if the query exists in the <code>_basefile</code>, return the start file offset and end file offset pair, or return null</returns>
        private long[] FindBaseFileOffset(string query, string market, string leafLevelIndexFilePath)
        {
            FileStream leafLevelIndexFS = null;
            try
            {
                FileInfo fileInfo = new FileInfo(leafLevelIndexFilePath);
                if (fileInfo.Length <= 0)
                {
                    Console.WriteLine("The index file: {0} has no content.", leafLevelIndexFilePath);
                    return null;
                }

                leafLevelIndexFS = new FileStream(leafLevelIndexFilePath, FileMode.Open, FileAccess.Read);

                // Verify whether the index file is UTF8 Encoding.
                byte[] encodingHead = _fileEncoding.GetPreamble();
                byte[] actualEncodingHead = new byte[encodingHead.Length];
                leafLevelIndexFS.Read(actualEncodingHead, 0, actualEncodingHead.Length);
                for (int i = 0; i < encodingHead.Length; i++)
                {
                    if (actualEncodingHead[i] != encodingHead[i])
                    {
                        Exception ex = new Exception("The indexfile: " + leafLevelIndexFilePath + " 's encoding is not " + _fileEncoding.ToString() + ".");
                        throw ex;
                    }
                }

                long low = _fileStartPos;
                long high = fileInfo.Length;
                long pos = 0;

                while (low <= high)
                {
                    long mid = (high + low) / 2;
                    string line = ReadLine(leafLevelIndexFS, mid, out pos);
                    string[] lineInfo = line.Split('\t');
                    //line should have 4 columns: query, market, startoffset, endoffset.
                    if (lineInfo == null || lineInfo.Length != 4)
                    {
                        Console.WriteLine("wrong line format in 2nd level index file, it's not like 'queryword\tstartOffset\tendOffset'.");
                        return null;
                    }
                    int comp1 = String.Compare(lineInfo[0], query, StringComparison.OrdinalIgnoreCase);
                    int comp2 = String.Compare(lineInfo[1], market, StringComparison.OrdinalIgnoreCase);

                    if (comp1 == 0 && comp2 == 0)
                    {
                        long startOffset;
                        long endOffset;
                        if (long.TryParse(lineInfo[2], out startOffset) && long.TryParse(lineInfo[3], out endOffset))
                        {
                            return new long[2] { startOffset, endOffset };
                        }
                        else
                        {
                            Console.WriteLine("{0} in {1} does not conform to its data format like 'queryword\tstartOffset\tendOffset'.");
                            return null;
                        }
                    }
                    else if (comp1 < 0 || (comp1 == 0 && comp2 < 0))
                    {
                        low = mid + 1;
                    }
                    else
                    {
                        high = pos - 1;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("FindBaseFileOffset failed: Leaf index file path is not valid.");
                throw ex;
            }
            catch (IOException ex)
            {
                Console.WriteLine("FindBaseFileOffset failed: Reading leaf index file failed.");
                throw ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("FindBaseFileOffset failed: Getting access to leaf index file denied.");
                throw ex;
            }
            finally
            {
                if (leafLevelIndexFS != null)
                {
                    leafLevelIndexFS.Close();
                }
            }

            return null;
        }

        /// <summary>
        /// The method is used to read the whole line that contains the position specified by <paramref name="fileOffset"/> 
        /// from the file stream specified by <paramref name="fs"/>
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="fileOffset"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private string ReadLine(FileStream fs, long fileOffset, out long pos)
        {
            long linePos = PosLineContaining(fs, fileOffset);
            pos = linePos;

            fs.Seek(linePos, SeekOrigin.Begin);
            byte[] buffer = new byte[1024];
            StringBuilder sb = new StringBuilder();

            int temp = fs.Read(buffer, 0, buffer.Length);
            int i = 0;
            while (temp > 0)
            {
                for (i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == '\r')
                    {
                        break;
                    }
                }
                sb.Append(_fileEncoding.GetString(buffer, 0, i));
                if (i < buffer.Length)
                {
                    break;
                }
                temp = fs.Read(buffer, 0, buffer.Length);
            }

            return sb.ToString();
        }

        /// <summary>
        /// The method is used to locate the start position of the line which contains 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private long PosLineContaining(FileStream stream, long pos)
        {
            int subtract = 1024;

            int lastBreak = -1;
            while (true)
            {
                long seekPt = pos - subtract;
                if (seekPt < _fileStartPos)
                {
                    seekPt = _fileStartPos;
                }
                stream.Seek(seekPt, SeekOrigin.Begin);
                for (int i = 0; i < pos - seekPt; ++i)
                {
                    int b = stream.ReadByte();
                    if (b == '\n')
                    {
                        lastBreak = (int)(pos - seekPt - (i + 1));
                    }
                }

                if (lastBreak >= 0)
                {
                    return pos - lastBreak;
                }
                else if (seekPt <= _fileStartPos)
                {
                    return _fileStartPos;
                }

                subtract += 1024;
            }
        }
    }
}
