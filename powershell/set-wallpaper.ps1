$Assem = (
	"System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
	"System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    )

$source = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Win32;

using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;

namespace ngwallpaper
{
    public class Wallpaper
    {
        public static string STORE_DIR = "D:/lg_wallpaper/pod/";
        Wallpaper( ) { }

  		const int SPI_SETDESKWALLPAPER = 20  ;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        static  extern int SystemParametersInfo (int uAction , int uParam , string lpvParam , int fuWinIni) ;

        public enum Style : int  
        {
            Tiled,
            Centered,
            Stretched
        }

        public static void set(Uri uri, Style style)
        {
            set(uri, style, STORE_DIR);


        }
        public static void set(Uri uri, Style style, string storeDir)
        {
			System.IO.Stream s = new WebClient( ).OpenRead( uri.ToString( ) );

			System.Drawing.Image img = System.Drawing.Image.FromStream( s );
			string filename = uri.ToString();
            //Console.WriteLine("filename=" + filename);
            int pos = filename.LastIndexOf("/");
            filename = filename.Substring(pos+1);
            //Console.WriteLine("filename=" + filename);

            string tempPath = Path.Combine( Path.GetTempPath( ), "wallpaper.bmp"  ) ;
            string storePath = storeDir + filename;
            img.Save(storePath);
			img.Save( tempPath ,  System.Drawing.Imaging.ImageFormat.Bmp ) ;

			RegistryKey key = Registry.CurrentUser.OpenSubKey( @"Control Panel\Desktop", true ) ;
			if ( style == Style.Stretched )
			{
				key.SetValue(@"WallpaperStyle", 2.ToString( ) ) ;
				key.SetValue(@"TileWallpaper", 0.ToString( ) ) ;
			}

			if ( style == Style.Centered )
			{
				key.SetValue(@"WallpaperStyle", 1.ToString( ) ) ;
				key.SetValue(@"TileWallpaper", 0.ToString( ) ) ;
			}

			if ( style == Style.Tiled )
			{
				key.SetValue(@"WallpaperStyle", 1.ToString( ) ) ;
				key.SetValue(@"TileWallpaper", 1.ToString( ) ) ;
			}

			SystemParametersInfo( SPI_SETDESKWALLPAPER, 
				0, 
				tempPath,  
				SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE );
            
        }

        public static string GetTitle(string content)
        {            
            int p1 = content.IndexOf("Photo of the Day");
            int p2 = content.IndexOf("<h1>", p1);
            int p3 = content.IndexOf("</h1>", p2);
            int len = p3 - (p2 + 4);
            string title = content.Substring(p2 + 4, len);
            return title;
        }
		
		public static string GetDescription(string content)
		{
            int p1 = content.IndexOf("This Month in Photo of the Day");
            int p2 = content.IndexOf("<p>", p1);
            int p3 = content.IndexOf("</p>", p2);
            int len = p3 - (p2 + 3);
            string desc = content.Substring(p2 + 3, len);

			return desc;
		}

        
        public static string GetImgURL(string content)
        {
            int pos1;
            int pos2;
            int len;
            string fileurl = "";
            //get picture url
            pos1 = content.IndexOf("download_link");

            if (pos1 >= 0)
            {
                pos2 = content.IndexOf("<a href=", pos1);
                int pos3 = content.IndexOf("Download Wallpaper (1600 x 1200 pixels)", pos2);
                len = pos3 - 2 - (pos2 + 9);
                fileurl = content.Substring(pos2 + 9, len);
            }
            else
            {
                pos1 = content.IndexOf("primary_photo");
                pos2 = content.IndexOf("<img src=", pos1);
                int pos3 = content.IndexOf("width=", pos2);
                len = pos3 - 3 - (pos2 + 10);
                fileurl = content.Substring(pos2 + 10, len);
            }
            return fileurl;
        }

        public static string GetPageContent()
        {
            string beginUrl = "http://photography.nationalgeographic.com/photography/photo-of-the-day/";

            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            Stream data = client.OpenRead(beginUrl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();

            return s;
        }
		
		public static void SetWallpaper(){
		
			string content = GetPageContent();
                    
            string fileurl = GetImgURL(content);
            Console.WriteLine("Impage: "+fileurl);

            string title = GetTitle(content);
            Console.WriteLine("Title: " + title);
			
			string description = GetDescription(content);
			Console.WriteLine("Description: " + description);

            set(new Uri(fileurl), Wallpaper.Style.Stretched);	
		}
    }
}
"@
Add-Type -ReferencedAssemblies $Assem -TypeDefinition $source -Language CSharpVersion3
[ngwallpaper.Wallpaper]::SetWallpaper( )