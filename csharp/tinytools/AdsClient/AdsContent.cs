using System;
namespace AdsClient
{
	internal class AdsContent : ICloneable
	{
		private string title;
		private string query;
		private string uri;
		private string shorturl;
		private string subject;
		private string seqnum;
		private string sizekb;
		private string adsmodify;
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
			}
		}
		public string Query
		{
			get
			{
				return this.query;
			}
			set
			{
				this.query = value;
			}
		}
		public string URI
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}
		public string ShortUrl
		{
			get
			{
				return this.shorturl;
			}
			set
			{
				this.shorturl = value;
			}
		}
		public string Subject
		{
			get
			{
				return this.subject;
			}
			set
			{
				this.subject = value;
			}
		}
		public string Seqnum
		{
			get
			{
				return this.seqnum;
			}
			set
			{
				this.seqnum = value;
			}
		}
		public string Sizekb
		{
			get
			{
				return this.sizekb;
			}
			set
			{
				this.sizekb = value;
			}
		}
		public string Adsmodify
		{
			get
			{
				return this.adsmodify;
			}
			set
			{
				this.adsmodify = value;
			}
		}
		public void Clear()
		{
			this.title = string.Empty;
			this.query = string.Empty;
			this.uri = string.Empty;
			this.shorturl = string.Empty;
			this.subject = string.Empty;
			this.seqnum = string.Empty;
			this.sizekb = string.Empty;
			this.adsmodify = string.Empty;
		}
		public object Clone()
		{
			return new AdsContent
			{
				Query = this.Query,
				Title = this.Title,
				URI = this.URI,
				ShortUrl = this.ShortUrl,
				Subject = this.Subject,
				Seqnum = this.Seqnum,
				Sizekb = this.Sizekb,
				Adsmodify = this.Adsmodify
			};
		}
	}
}
