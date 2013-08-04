using BrightIdeasSoftware;
using System;
using System.Drawing;
namespace LongkeyMusic
{
	public class AlbumDecoration : AbstractDecoration
	{
		private AlbumMeta meta;
		private int imageWidth = 100;
		public Font AlbumFont = new Font("Segoe UI", 9f, FontStyle.Bold);
		public Color AlbumColor = Color.FromArgb(255, 32, 32, 32);
		public Size CellPadding = new Size(2, 2);
		public AlbumDecoration(AlbumMeta meta)
		{
			this.meta = meta;
		}
		public override void Draw(ObjectListView olv, Graphics g, Rectangle r)
		{
			Rectangle cellBounds = base.CellBounds;
			cellBounds.Inflate(-this.CellPadding.Width, -this.CellPadding.Height);
			Rectangle r2 = cellBounds;
			r2.X += this.imageWidth;
			r2.Width -= this.imageWidth;
			if (this.meta.CoverThumnail != null)
			{
				g.DrawImage(this.meta.CoverThumnail, cellBounds.Location);
			}
			using (StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap))
			{
				stringFormat.Trimming = StringTrimming.EllipsisCharacter;
				stringFormat.Alignment = StringAlignment.Near;
				stringFormat.LineAlignment = StringAlignment.Near;
				using (SolidBrush solidBrush = new SolidBrush(this.AlbumColor))
				{
					string text = (this.meta.Album == null) ? "" : ("专辑： " + this.meta.Album);
					g.DrawString(text, this.AlbumFont, solidBrush, r2, stringFormat);
					SizeF sizeF = g.MeasureString(text, this.AlbumFont, r2.Width, stringFormat);
					r2.Y += 2 * (int)sizeF.Height;
					r2.Height -= 2 * (int)sizeF.Height;
					string text2 = (this.meta.Artist == null) ? "" : ("艺人： " + this.meta.Artist);
					g.DrawString(text2, this.AlbumFont, solidBrush, r2, stringFormat);
					sizeF = g.MeasureString(text2, this.AlbumFont, r2.Width, stringFormat);
					r2.Y += 2 * (int)sizeF.Height;
					r2.Height -= 2 * (int)sizeF.Height;
					string s = (this.meta.Year == 0) ? "" : ("发行： " + this.meta.Year.ToString());
					g.DrawString(s, this.AlbumFont, solidBrush, r2, stringFormat);
				}
			}
		}
	}
}
