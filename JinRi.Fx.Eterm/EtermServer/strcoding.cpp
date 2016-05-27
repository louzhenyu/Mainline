#include "StdAfx.h"
#include "strCoding.h"
#include "Global.h"

strCoding::strCoding(void)
{
}

strCoding::~strCoding(void)
{
}

void strCoding::Gb2312ToUnicode(WCHAR* pOut,char *gbBuffer)
{
    ::MultiByteToWideChar(CP_ACP,MB_PRECOMPOSED,gbBuffer,2,pOut,1);
    return;
}
void strCoding::UTF_8ToUnicode(WCHAR* pOut,char *pText)
{
    char* uchar = (char *)pOut;
     
    uchar[1] = ((pText[0] & 0x0F) << 4) + ((pText[1] >> 2) & 0x0F);
    uchar[0] = ((pText[1] & 0x03) << 6) + (pText[2] & 0x3F);

    return;
}

void strCoding::UnicodeToUTF_8(char* pOut,WCHAR* pText)
{
    // 注意 WCHAR高低字的顺序,低字节在前，高字节在后
    char* pchar = (char *)pText;

    pOut[0] = (0xE0 | ((pchar[1] & 0xF0) >> 4));
    pOut[1] = (0x80 | ((pchar[1] & 0x0F) << 2)) + ((pchar[0] & 0xC0) >> 6);
    pOut[2] = (0x80 | (pchar[0] & 0x3F));

    return;
}
void strCoding::UnicodeToGB2312(char* pOut,WCHAR uData)
{
    WideCharToMultiByte(CP_ACP,NULL,&uData,1,pOut,sizeof(WCHAR),NULL,NULL);
    return;
}

//做为解Url使用
char strCoding:: CharToInt(char ch){
        if(ch>='0' && ch<='9')return (char)(ch-'0');
        if(ch>='a' && ch<='f')return (char)(ch-'a'+10);
        if(ch>='A' && ch<='F')return (char)(ch-'A'+10);
        return -1;
}
char strCoding::StrToBin(char *str){
        char tempWord[2];
        char chn;

        tempWord[0] = CharToInt(str[0]);                         //make the B to 11 -- 140106011
        tempWord[1] = CharToInt(str[1]);                         //make the 0 to 0  -- 00000000

        chn = (tempWord[0] << 4) | tempWord[1];                //to change the BO to 10110000

        return chn;
}


//UTF_8 转gb2312
void strCoding::UTF_8ToGB2312(string &pOut, char *pText, int pLen)
{
     char buf[4];
     char* rst = new char[pLen + (pLen >> 2) + 2];
    memset(buf,0,4);
    memset(rst,0,pLen + (pLen >> 2) + 2);

    int i =0;
    int j = 0;
      
    while(i < pLen)
    {
        if(*(pText + i) >= 0)
        {
            
            rst[j++] = pText[i++];
        }
        else                 
        {
            WCHAR Wtemp;

            
            UTF_8ToUnicode(&Wtemp,pText + i);
              
            UnicodeToGB2312(buf,Wtemp);
            
            unsigned short int tmp = 0;
            tmp = rst[j] = buf[0];
            tmp = rst[j+1] = buf[1];
            tmp = rst[j+2] = buf[2];

            //newBuf[j] = Ctemp[0];
            //newBuf[j + 1] = Ctemp[1];

            i += 3;    
            j += 2;   
        }
        
  }
    rst[j]='\0';
   pOut = rst; 
    delete []rst;
}

//GB2312 转为 UTF-8
void strCoding::GB2312ToUTF_8(string& pOut,char *pText, int pLen)
{
    char buf[4];
    memset(buf,0,4);

    pOut.clear();

    int i = 0;
    while(i < pLen)
    {
        //如果是英文直接复制就可以
        if( pText[i] >= 0)
        {
            char asciistr[2]={0};
            asciistr[0] = (pText[i++]);
            pOut.append(asciistr);
        }
        else
        {
            WCHAR pbuffer;
            Gb2312ToUnicode(&pbuffer,pText+i);

            UnicodeToUTF_8(buf,&pbuffer);

            pOut.append(buf);

            i += 2;
        }
    }

    return;
}
//把str编码为网页中的 GB2312 url encode ,英文不变，汉字双字节  如%3D%AE%88
string strCoding::UrlGB2312(char * str)
{
    string dd;
    size_t len = strlen(str);
    for (size_t i=0;i<len;i++)
    {
        if(isalnum((BYTE)str[i]))
        {
            char tempbuff[2];
            sprintf_s(tempbuff,"%c",str[i]);
            dd.append(tempbuff);
        }
        else if (isspace((BYTE)str[i]))
        {
            dd.append("+");
        }
        else
        {
            char tempbuff[4];
			sprintf_s(tempbuff, "%%%X%X", ((BYTE*)str)[i] >> 4, ((BYTE*)str)[i] % 16);
            dd.append(tempbuff);
        }

    }
    return dd;
}

//把str编码为网页中的 UTF-8 url encode ,英文不变，汉字三字节  如%3D%AE%88

string strCoding::UrlUTF8(char * str)
{
    string tt;
    string dd;
    GB2312ToUTF_8(tt,str,(int)strlen(str));

    size_t len=tt.length();
    for (size_t i=0;i<len;i++)
    {
        if(isalnum((BYTE)tt.at(i)))
        {
            char tempbuff[2]={0};
			sprintf_s(tempbuff, "%c", (BYTE)tt.at(i));
            dd.append(tempbuff);
        }
       /* else if (isspace((BYTE)tt.at(i)))
        {
            dd.append("+");
        }*/
        else
        {
            char tempbuff[4];
			sprintf_s(tempbuff, "%%%X%X", ((BYTE)tt.at(i)) >> 4, ((BYTE)tt.at(i)) % 16);
            dd.append(tempbuff);
        }

    }
    return dd;
}
//把url GB2312解码
string strCoding::UrlGB2312Decode(string str)
{
   string output="";
        char tmp[2];
        int i=0,idx=0,ndx,len=str.length();
        
        while(i<len){
                if(str[i]=='%'){
                        tmp[0]=str[i+1];
                        tmp[1]=str[i+2];
                        output += StrToBin(tmp);
                        i=i+3;
                }
                /*else if(str[i]=='+'){
                        output+=' ';
                        i++;
                }*/
                else{
                        output+=str[i];
                        i++;
                }
        }
        
        return output;
}

//把url utf8解码
string strCoding::UrlUTF8Decode(string str)
{
     string output="";

    string temp =UrlGB2312Decode(str);//

    //UTF_8ToGB2312(output,(char *)temp.data(),strlen(temp.data()));

	return temp;

}

bool strCoding::URL_GBK_Decode(const char *encd, char* decd)
{
    if( encd == 0 )
        return false;

    int i, j=0;
    const char *cd = encd;
    char p[2];
    unsigned int num;
    for( i = 0; i < (int)strlen( cd ); i++ )
    {
        memset( p, '\0', 2 );
        if( cd[i] != '%' )
        {
            decd[j++] = cd[i];
            continue;
        }else if(cd[i] == '%'
            && ( (cd[i+1]>='0'&&cd[i+1]<='9') ||
            (cd[i+1]>='A'&&cd[i+1]<='F') ||
            (cd[i+1]>='a'&&cd[i+1]<='f') ) )
        {
            p[0] = cd[++i];
            p[1] = cd[++i];
           
            sscanf_s( p, "%x", &num );
			sprintf_s(p, "%c", num);
            decd[j++] = p[0];
        }else
        {
            decd[j++] = cd[i];
        }
    }
    decd[j] = '\0';

    return true;
}


bool strCoding::URL_IS_UTF8(const char* url)
{
    char buffer[4096];
    memset(buffer,0,4096);

    URL_GBK_Decode(url,buffer);

    char* pGBKStr = buffer;
    int   strLen = strlen(pGBKStr);
   
    int nWideByte = 0;
    for( int i=0; i<strLen; i++)
    {
        if( pGBKStr[i] < 0 )
        {
            nWideByte++;
        }
    }

    if( nWideByte == 0 )
    {
        return false;
    }

    int nUtfLen =
        MultiByteToWideChar(CP_UTF8,MB_ERR_INVALID_CHARS,pGBKStr,-1,NULL, 0);
    if( nUtfLen >0 && (nWideByte%3==0) &&
        (strLen - (2*nWideByte/3)) == (nUtfLen-1) )
    {
        return true;
    }

    return false;
}


bool strCoding::URL_UTF8_Decode(const char* url,char* decd)
{
    char buffer[4096];
    memset(buffer,0,4096);
   
    URL_GBK_Decode(url,buffer);
    char* pGBKStr = buffer;

    int   nGbkLen = MultiByteToWideChar(CP_UTF8,MB_ERR_INVALID_CHARS,pGBKStr,-1,NULL, 0);

    if( nGbkLen >0 ) // UTF8 STR
    {
        LPWSTR wStr = new WCHAR[nGbkLen];
        MultiByteToWideChar(CP_UTF8,0,pGBKStr, -1, wStr, nGbkLen);
       
        int astrLen     = WideCharToMultiByte(CP_ACP, 0, wStr, -1, NULL, 0, NULL, NULL);
        char* converted = new char[astrLen];
        WideCharToMultiByte(CP_ACP, 0, wStr, -1, converted, astrLen, NULL, NULL);
		strcpy_s(decd, astrLen,converted);
       
        delete wStr;
        delete converted;
       
        return true;
    }

    return false;
}


bool strCoding::URL_Decode(const char* url,char* pStr)
{
    int   nWideByte = 0;
    char* pGBKStr   = NULL;
   
    char buffer[4096];
    memset(buffer,0,4096);
   
    URL_GBK_Decode(url,buffer);
    pGBKStr    = buffer;
    int strLen = strlen(pGBKStr);
   
    for( int i=0; i<strLen; i++)
    {
        if( pGBKStr[i] < 0 )
        {
            nWideByte++;
        }
    }
   
    int nGbkLen =
        MultiByteToWideChar(CP_ACP,MB_ERR_INVALID_CHARS,pGBKStr,-1,NULL, 0);
   
    if( nGbkLen >0 && (strLen - (nWideByte/2)) == (nGbkLen-1) ) // GBK STR
    {
		strcpy_s(pStr, nGbkLen,  pGBKStr);
    }
   
    nGbkLen = MultiByteToWideChar(CP_UTF8,MB_ERR_INVALID_CHARS,pGBKStr,-1,NULL, 0);
    if( nGbkLen >0 &&
        (nWideByte%3==0) &&
        (strLen - (2*nWideByte/3)) == (nGbkLen-1))              // UTF8 STR
    {
        LPWSTR wStr = new WCHAR[nGbkLen];
        MultiByteToWideChar(CP_UTF8,0,pGBKStr, -1, wStr, nGbkLen);
       
        int astrLen     = WideCharToMultiByte(CP_ACP, 0, wStr, -1, NULL, 0, NULL, NULL);
        char* converted = new char[astrLen];
        WideCharToMultiByte(CP_ACP, 0, wStr, -1, converted, astrLen, NULL, NULL);
		strcpy_s(pStr, astrLen, converted);
       
        delete wStr;
        delete converted;
       
        return true;
    }
   
	strcpy_s(pStr, nGbkLen,pGBKStr);

    return true;
}

//based on javascript encodeURIComponent()
string strCoding::urlencode(const string &c)
{
 
    string escaped="";
    int max = c.length();
    for(int i=0; i<max; i++)
    {
        if ( (48 <= c[i] && c[i] <= 57) ||//0-9
             (65 <= c[i] && c[i] <= 90) ||//abc...xyz
             (97 <= c[i] && c[i] <= 122) || //ABC...XYZ
             (c[i]=='~' || c[i]=='!' || c[i]=='*' || c[i]=='(' || c[i]==')' || c[i]=='\'')
        )
        {
            escaped.append( &c[i], 1);
        }
        else
        {
            escaped.append("%");
            escaped.append( char2hex(c[i]) );//converts char 255 to string "ff"
        }
    }
    return escaped;
}
 
string strCoding::char2hex( char dec )
{
    char dig1 = (dec&0xF0)>>4;
    char dig2 = (dec&0x0F);
    if ( 0<= dig1 && dig1<= 9) dig1+=48;    //0,48inascii
    if (10<= dig1 && dig1<=15) dig1+=97-10; //a,97inascii
    if ( 0<= dig2 && dig2<= 9) dig2+=48;
    if (10<= dig2 && dig2<=15) dig2+=97-10;
 
    string r;
    r.append( &dig1, 1);
    r.append( &dig2, 1);
    return r;
}