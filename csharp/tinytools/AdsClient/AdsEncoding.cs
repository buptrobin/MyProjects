using System;
using System.Text;
namespace AdsClient
{
	internal class AdsEncoding
	{
		public static string codeIt(string raw, string key)
		{
			return AdsEncoding.xorString(raw, key);
		}
		public static string codeIt(byte[] buf, string key)
		{
			return AdsEncoding.xorBytes(buf, key);
		}
        public static string codeIt2(byte[] buf, string key)
        {
            return AdsEncoding.xorBytesDefault(buf, key);
        }
		private static string xorString(string raw, string key)
		{
			byte[] buf = Encoding.GetEncoding("GBK").GetBytes(raw);
			return AdsEncoding.xorBytes(buf, key);
		}
		private static string xorBytes(byte[] buf, string key)
		{
			string result2;
			if (buf.Length == 0 || key.Length == 0)
			{
				result2 = Encoding.GetEncoding("GBK").GetString(buf);
			}
			else
			{
				byte[] bufk = Encoding.ASCII.GetBytes(key);
				int len_buf = buf.Length;
				int len_k = bufk.Length;
				if (len_k == 0 || len_buf == 0)
				{
					result2 = Encoding.GetEncoding("GBK").GetString(buf);
				}
				else
				{
					for (int i = 0; i < len_buf; i++)
					{
						byte c = buf[i];
						byte ck = bufk[i % len_k];
						if (c != 0 && c != ck)
						{
							c ^= ck;
						}
						buf[i] = c;
					}
					string result = Encoding.GetEncoding("GBK").GetString(buf);
					result2 = result;
				}
			}
			return result2;
		}

        private static string xorBytesDefault(byte[] buf, string key)
        {
            string result2;
            if (buf.Length == 0 || key.Length == 0)
            {
                result2 = Encoding.Default.GetString(buf);
            }
            else
            {
                byte[] bufk = Encoding.ASCII.GetBytes(key);
                int len_buf = buf.Length;
                int len_k = bufk.Length;
                if (len_k == 0 || len_buf == 0)
                {
                    result2 = Encoding.Default.GetString(buf);
                }
                else
                {
                    for (int i = 0; i < len_buf; i++)
                    {
                        byte c = buf[i];
                        byte ck = bufk[i % len_k];
                        if (c != 0 && c != ck)
                        {
                            c ^= ck;
                        }
                        buf[i] = c;
                    }
                    string result = Encoding.Default.GetString(buf);
                    result2 = result;
                }
            }
            return result2;
        }
		public static byte[] QueryEncoding(byte[] buf, string key)
		{
			byte[] result;
			if (buf.Length == 0 || key.Length == 0)
			{
				result = buf;
			}
			else
			{
				byte[] bufk = Encoding.ASCII.GetBytes(key);
				int len_buf = buf.Length;
				int len_k = bufk.Length;
				if (len_k == 0 || len_buf == 0)
				{
					result = buf;
				}
				else
				{
					for (int i = 0; i < len_buf; i++)
					{
						byte c = buf[i];
						byte ck = bufk[i % len_k];
						if (c != 0 && c != ck)
						{
							c ^= ck;
						}
						buf[i] = c;
					}
					result = buf;
				}
			}
			return result;
		}
	}
}
