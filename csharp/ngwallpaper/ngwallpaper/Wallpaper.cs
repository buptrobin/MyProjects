using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Win32;

using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;

namespace ngwallpaper
{
    class Wallpaper
    {
        public static string STORE_DIR = "C:/lg_wallpaper/pod/";
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
            Console.WriteLine("filename=" + filename);
            int pos = filename.LastIndexOf("/");
            filename = filename.Substring(pos+1);
            Console.WriteLine("filename=" + filename);

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
    }
}
