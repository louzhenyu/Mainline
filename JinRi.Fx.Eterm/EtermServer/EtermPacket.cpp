#include "StdAfx.h"
#include "EtermPacket.h"
#include "Global.h"
#include "PrinterDecrypt.h"


CEtermPacket::CEtermPacket(void)
{
	SID=0x00;
	RID=0x00;
	nPacketLength=0;
	nPatcketTotalLength=0;
	m_nFirstLegnth=0;
	m_unlessLength=0;
	m_btye = NULL;
	m_big = 0;
}

CEtermPacket::~CEtermPacket(void)
{
	if (m_btye)
	{
		delete[] m_btye;
		m_btye = NULL;
	}
}

bool CEtermPacket::ValidatePakcet(byte* lpBuf,int nlen)
{

	//握手
	if (nlen > 9 && lpBuf[0] == 0x00 && (lpBuf[1] == 0x14 || lpBuf[1] == 0x19) && lpBuf[2] == 0x01)
	{
		SID = lpBuf[8];
		RID = lpBuf[9];
		m_pt = PACKET_TYPE::LOGIN;
		return true;
	}	
	else
	{
		//m_nFirstLegnth = 0;
		//m_unlessLength = 0;
		reset:
		
		if (nlen >20 && lpBuf[0]==0x01 && lpBuf[1]==0x00)
		{
			nPatcketTotalLength=MAKEWORD(lpBuf[3],lpBuf[2]);
			nPacketLength=nlen;
			if (nPatcketTotalLength < nPacketLength) nPatcketTotalLength = nPacketLength;
			m_pt = nPatcketTotalLength==nPacketLength?PACKET_TYPE::COMPLETE:PACKET_TYPE::RECIVE;
			if (m_btye != NULL)
			{
				delete[] m_btye;
				m_btye = NULL;
			}
			m_btye = new BYTE[nPatcketTotalLength];
			memset(m_btye, 0, sizeof(m_btye));
			memcpy(m_btye, lpBuf, nlen);
			if (m_cmdType == print) return m_pt == PACKET_TYPE::COMPLETE;

			BYTE btStart[] = {0x0F, 0x1B, 0x4D };
			BYTE btStart1[] = { 0x20, 0x0F, 0x02 };

			m_nFirstLegnth = 0;
			m_unlessLength = 0;
			for (int i = 0; i<nlen&&m_nFirstLegnth<30; i++)
			{
				if (memcmp(&lpBuf[i], btStart, 3) == 0 ||
					memcmp(&lpBuf[i], btStart1, 3) == 0)
				{
					m_nFirstLegnth += 3;
					break;
				}
				m_nFirstLegnth++;
			}

			if (m_pt==PACKET_TYPE::COMPLETE)
			{
				BYTE btEnd[] = { 0x1E, 0x1B, 0x62, 0x03 };
				BYTE btEnd1[] = { 0x21, 0x2A, 0x0F, 0x03 };

				if (nlen>4)
				for (int i = nlen - 4; i >= 0 && m_unlessLength<10; i--)
				{
					if (memcmp(&lpBuf[i], btEnd, 4) == 0||
						memcmp(&lpBuf[i], btEnd1, 4) == 0)
					{
						m_unlessLength += 4;
						break;
					}
					m_unlessLength++;
				}
			}
			else
			{
				m_unlessLength=0;
			}
			
			m_strResponse.Empty();
			m_vecRev.clear();
			if (m_cmdType == login) m_cmdType = normal;
			
			return m_pt == PACKET_TYPE::COMPLETE;
		}
		else if (m_pt == PACKET_TYPE::RECIVE || m_pt == PACKET_TYPE::HALF)
		{
			if (m_btye != NULL)
			{
				memcpy(&m_btye[nPacketLength], lpBuf, nlen);
			}

			nPacketLength+=nlen;

			m_pt = nPatcketTotalLength==nPacketLength?PACKET_TYPE::COMPLETE:PACKET_TYPE::HALF;
			
			if (m_pt==PACKET_TYPE::COMPLETE)
			{
				if (m_cmdType == print) return true;
				m_unlessLength = 0;
				BYTE btEnd[] = { 0x1E, 0x1B, 0x62, 0x03 };
				BYTE btEnd1[] = { 0x21, 0x2A, 0x0F, 0x03 };
				if (nlen>4)
				for (int i = nlen - 4; i >= 0 && m_unlessLength<10; i--)
				{
					if (memcmp(&lpBuf[i], btEnd, 4) == 0 ||
						memcmp(&lpBuf[i], btEnd1, 4) == 0)
					{
						m_unlessLength += 4;
						break;
					}
					m_unlessLength++;
				}
			}
			/*else
			{
				m_nFirstLegnth=0;
				m_unlessLength=0;
			}*/
			return m_pt == PACKET_TYPE::COMPLETE;
		}
		else
		{			
			if (nlen>10&&lpBuf[0]==0x01&&lpBuf[1]==0xFD)
			for (int i = 0; i < nlen-1; i++)
			{
				if (lpBuf[i] == 0x01 && lpBuf[i+1] == 0x00)
				{
					nlen -= i;
					memcpy(lpBuf, &lpBuf[i], nlen);
					goto reset;
				}
			}			

			if (nlen>2)
				m_big = lpBuf[0] == 0x01 && lpBuf[1] == 0xF6 ? 2 : 1;

			m_cmdType = login;		
			
			return true;
		}
	}

	return false;
}

void CEtermPacket::UnPacket(CString& buffer)
{
	if (m_btye == NULL) return;
	if (m_nFirstLegnth > 20) m_nFirstLegnth = 20;
	if (m_unlessLength > 5) m_unlessLength = 4;

	USES_CONVERSION;
	int nIndex=m_nFirstLegnth-1;
		
	int nColumnNumber=0;

	while (nIndex++<nPatcketTotalLength - m_unlessLength)
	{		
		if (nIndex<0 || nIndex >= nPatcketTotalLength - m_unlessLength) break;

		switch(m_btye[nIndex])
		{
		case 0x1C:			//红色标记
			nColumnNumber++;
			buffer.Append(_T("『"));
			break;
		case 0x1D:
			buffer.Append(_T("』"));
			nColumnNumber++;
			break;		
		case 0x03:		
		case 0x00:
		case 0x0E:
		case 0x0F:
			break;
		case 0x1E:
			//buffer.AppendFormat(_T("%c"), 0x0E);
			buffer.Append(_T("▶"));
			nColumnNumber++;
			break;
		case 0x0D:
			while(++nColumnNumber%80!=0)
			{
				buffer.AppendFormat(_T("%c"), 0x20);
				continue;
			}
			if (nColumnNumber % 80 == 0) { nColumnNumber = 0; buffer.AppendFormat(_T("%c"), 0x0D); }
			break;
		case 0x1B:{
					  BYTE btHZ = m_btye[++nIndex];
					  if (btHZ == 0x0E)
					  {

						  while (true)
						  {
							  if (nIndex > nPatcketTotalLength - 4) break;

							  byte ch[] = { m_btye[++nIndex], m_btye[++nIndex], 0x00, 0x00 };
							  if (ch[0] == 0x1B && ch[1] == 0x0F)
							  {
								  break;
							  }
							  if (ch[0] == 0x78)
							  {
								  char szHZ[3] = { 0 };
								  ch[2] = m_btye[++nIndex];
								  ch[3] = m_btye[++nIndex];
								  ParseHZ(ch, szHZ);
								  buffer.AppendFormat(_T("%s"), A2T(szHZ));
							  }
							  else
							  {
								  UsasToGb(ch[0], ch[1]);
								  buffer.AppendFormat(_T("%s"), A2T((char*)ch));
							  }
							  
							  nColumnNumber++;
							  
							  if (nColumnNumber % 80 == 0) { nColumnNumber = 0; buffer.AppendFormat(_T("%c"), 0x0D); }
						  }
					  }
					  else if (btHZ==0x09)
					  {
						  buffer.Append(_T("▪"));
						  nColumnNumber++; 
					  }
		}
			break;
		default:
			nColumnNumber++;
			buffer.AppendFormat(_T("%c"), m_btye[nIndex]);
			if (nColumnNumber % 80 == 0) { nColumnNumber = 0; buffer.AppendFormat(_T("%c"), 0x0D); }
			break;
		}
	}	
}

void CEtermPacket::UsasToGb(byte& c1,byte& c2)
{
	if (c1>0x24 && c1<0x29)
	{
		byte temp=c1;
		c1=c2;
		c2=temp+10;
	}
	if (c1>0x24)
	{
		c1+=0x80;
	}
	else
	{
		c1+=0x8E;
	}

	c2+=0x80;
}

void CEtermPacket::ParseHZ(BYTE* pData, char* szHZ)
{
	int c1 = pData[0];
	int c2 = pData[1];
	int c3 = pData[2];
	int c4 = pData[3];

	int v4 = c1;
	int v8 = c3;
	if (c1 != 0x78) return;
	if (c2 != 0x38) //eTerm.00281C1C
	{
		c2 = c2 - 0x3F;			//c2=8
		v4 = c4;
		v8 = v8 - 0x3F;			//v8=1A

		c2 = c2 << 0xC;
		v8 = v8 << 0x6;
		c2 = c2 | v8;

		v4 = v4 - 0x3F;			//v4=0x34
		c2 = c2 | v4;
	}
	szHZ[0] = c2 >> 0x8;
	szHZ[1] = c2 & 0xfff;
	szHZ[2] = 0;
}

void CEtermPacket::ParseData(BYTE* pData,int nSize)
{			
	CString strResponse;
		
	if (m_cmdType == print&&m_btye != NULL)
	{
		for (int i = 0; i<nPatcketTotalLength; i++)
		{
			if (m_btye[i] == 0x00) continue;
			strResponse.AppendFormat(_T("%c"), m_btye[i]);
		}
	}
	else if (m_cmdType == login&&nSize>10)
	{	
		char* szbuf = new char[nSize];
		memset(szbuf, 0, sizeof(szbuf));
		memcpy(szbuf, &pData[4], nSize - 4);		
		m_strResponse.Format(_T("%s"),CA2W(szbuf));
		delete[] szbuf;
	}
	else
	{
		UnPacket(strResponse);
	}

	switch (m_pt)
	{
	case LOGIN:
		return;
	case COMPLETE:
		m_strResponse.Append(strResponse);		
		m_vecRev.push_back(m_strResponse);
		if (m_cmdType == print) PrintParse();
		else if (m_cmdType == xs) xsParse();
		break;
	case HALF:
	case RECIVE:
		m_strResponse.Append(strResponse);		
		break;
	default:
		return;
	}
}

void CEtermPacket::PrintParse()
{
	int nS = m_strResponse.Find(_T("<Response>"));
	int nE = m_strResponse.Find(_T("</Response>"));
	if (nS == -1 || nE == -1) return;
	m_strResponse = m_strResponse.Mid(nS, nE - nS + _tcslen(_T("</Response>")));

	CString strData = m_strResponse;	
	
	int nPos1 = strData.Find(_T("<ITINERARY>")) + _tcslen(_T("<ITINERARY>"));
	int nPos2 = strData.Find(_T("</ITINERARY>"));
	if (nPos1 > 0 && nPos2 > 0 && nPos2 > nPos1)
	{
		CString sTemp;
		CPrinterDecrypt decrypt;
		sTemp = strData.Mid(nPos1, nPos2 - nPos1);
		sTemp = decrypt.Decrypt(sTemp);
		sTemp = strData.Mid(0, nPos1) + sTemp + strData.Mid(nPos2, strData.GetLength() - nPos2);
		if (m_cmd.Find(_T("VV")) == -1)
		{
			decrypt.ConvertXmlToHtml(sTemp, Global::EtermXSLPath + (m_cmd.Find(_T("EN")) >= 0 ? _T("EN.xsl") : _T("CN.xsl")), m_strResponse);
		}
		else
		{
			m_strResponse = sTemp;
		}
		std::wstring sData = m_strResponse.GetBuffer();
		m_strResponse.ReleaseBuffer();
		decrypt.AppendInfo(sData);
		m_strResponse.Format(_T("%s"), sData.c_str());
	}
	else
	{
		m_strResponse = strData;
	}
}

void CEtermPacket::xsParse()
{
	if (m_strResponse.Find(_T("SESSION CURRENTLY LOCKED")) != -1)
	{
		m_pt = HALF;
	}
	else if (m_strResponse.Find(_T("SEE ")) != -1)
	{
		m_pt = COMPLETE;
	}
}