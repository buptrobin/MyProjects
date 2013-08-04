// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <strstream>
#include "tchar.h"
#include <windows.h>

// STL
#pragma warning( push )
#pragma warning( disable: 4100 4127 4389 4511 4512 4702 4995)

using namespace std;
#include <string>
#include <iostream>
#include <sstream>
#include <iomanip>
#include <fstream>
#include <deque>
#include <vector>
#include <list>
#include <map>
#include <cassert>
#include <cmath>
#include <limits>
#include <Algorithm>
#include <set>


//using namespace std;
void UTF_8ToUnicode(wchar_t* pOut,char *pText)
{
char* uchar = (char *)pOut;
uchar[1] = ((pText[0] & 0x0F) << 4) + ((pText[1] >> 2) & 0x0F);
uchar[0] = ((pText[1] & 0x03) << 6) + (pText[2] & 0x3F);
return;
}
void UnicodeToUTF_8(char* pOut,wchar_t* pText)
{
// 注意 WCHAR高低字的顺序,低字节在前，高字节在后
char* pchar = (char *)pText;
pOut[0] = (0xE0 | ((pchar[1] & 0xF0) >> 4));
pOut[1] = (0x80 | ((pchar[1] & 0x0F) << 2)) + ((pchar[0] & 0xC0) >> 6);
pOut[2] = (0x80 | (pchar[0] & 0x3F));
return;
}
void UnicodeToGB2312(char* pOut,wchar_t uData)
{
WideCharToMultiByte(CP_ACP,NULL,&uData,1,pOut,sizeof(wchar_t),NULL,NULL);
return;
}    
void Gb2312ToUnicode(wchar_t* pOut,char *gbBuffer)
{
::MultiByteToWideChar(CP_ACP,MB_PRECOMPOSED,gbBuffer,2,pOut,1);
return ;
}
void GB2312ToUTF_8(string& pOut,char *pText, int pLen)
{
char buf[4];
int nLength = pLen* 3;
char* rst = new char[nLength];
memset(buf,0,4);
memset(rst,0,nLength);
int i = 0;
int j = 0;      
while(i < pLen)
{
   //如果是英文直接复制就能
   if( *(pText + i) >= 0)
   {
    rst[j++] = pText[i++];
   }
   else
   {
    wchar_t pbuffer;
    Gb2312ToUnicode(&pbuffer,pText+i);
    UnicodeToUTF_8(buf,&pbuffer);
    unsigned short int tmp = 0;
    tmp = rst[j] = buf[0];
    tmp = rst[j+1] = buf[1];
    tmp = rst[j+2] = buf[2];   
    j += 3;
    i += 2;
   }
}
rst[j] = '\0';
//返回结果
pOut = rst;             
delete []rst;  
return;
}
void UTF_8ToGB2312(string &pOut, char *pText, int pLen)
{
char * newBuf = new char[pLen];
char Ctemp[4];
memset(Ctemp,0,4);
int i =0;
int j = 0;
while(i < pLen)
{
   if(pText > 0)
   {
    newBuf[j++] = pText[i++];                       
   }
   else                 
   {
    WCHAR Wtemp;
    UTF_8ToUnicode(&Wtemp,pText + i);
    UnicodeToGB2312(Ctemp,Wtemp);
    newBuf[j] = Ctemp[0];
    newBuf[j + 1] = Ctemp[1];
    i += 3;    
    j += 2;   
   }
}
newBuf[j] = '\0';
pOut = newBuf;
delete []newBuf;
return; 
}


int main()
{
    string query("减肥");
    string path = "C:/myproject/cpp/test/query.txt";
    set<string> myset;

    ifstream input( path.c_str(), ifstream::in );
    if ( input.good() )
    {
        input.seekg( 0, ifstream::beg );
        char buf[512];
		for(int i=0;i<512;i++) buf[i]='\0';
        myset.empty();
        while ( input.peek() != EOF )
        {
            input.getline(buf, 512);
			printf("%d\n",strlen(buf));
			string pout;
			string sBuf(buf);

			std::wstring wstr(sBuf.length(),L' ');
			std::copy(sBuf.begin(), sBuf.end(), wstr.begin());

			//int nLen = (int)sBuf.length();
			//wstr.resize(nLen, L' ');
			//MultiByteToWideChar(CP_ACP,0,(LPCSTR)sBuf.c_str(),nLen,(LPWSTR)wstr.c_str(),nLen);


			int n = WideCharToMultiByte( CP_UTF8, 0, wstr.c_str(), -1, 0, 0, 0, 0 );  

			string keyword;
			keyword.resize(n);  
			WideCharToMultiByte( CP_UTF8, 0, wstr.c_str(), -1, (char*)keyword.c_str(), keyword.length(), 0, 0 );  

			//WideCharToMultiByte(CP_ACP,0,(LPCWSTR)ws.c_str(),nLen,(LPSTR)keyword.c_str(),nLen,NULL,NULL);

			
	
			myset.insert( pout );
        }
    }
    input.close();
    printf("...............");
    if ( myset.find(query) != myset.end() )
    {
        printf("Match!!\n");
    }
}
