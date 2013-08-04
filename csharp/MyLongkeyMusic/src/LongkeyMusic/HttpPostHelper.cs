using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
namespace LongkeyMusic
{
	public class HttpPostHelper
	{
		private HttpWebRequest _req;
		private string _params = string.Empty;
		private string _url;
		public HttpPostHelper(string url)
		{
			this._url = url;
		}
		public void Add(string key, string value)
		{
			if (this._params != string.Empty)
			{
				this._params += "&";
			}
			this._params = this._params + key + "=" + value;
		}
		public XmlDocument GetResult()
		{
			XmlDocument xmlDocument = new XmlDocument();
			this._req = (HttpWebRequest)WebRequest.Create(this._url);
			this._req.Method = "POST";
			this._req.ContentType = "application/x-www-form-urlencoded";
			Encoding uTF = Encoding.UTF8;
			byte[] bytes = uTF.GetBytes(this._params);
			this._req.ContentLength = (long)bytes.Length;
			using (Stream requestStream = this._req.GetRequestStream())
			{
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)this._req.GetResponse();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string xml = streamReader.ReadToEnd();
			xmlDocument.LoadXml(xml);
			streamReader.Close();
			return xmlDocument;
		}
	}
}
