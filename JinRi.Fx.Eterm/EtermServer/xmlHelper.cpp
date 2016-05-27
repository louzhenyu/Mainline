#include "stdafx.h"
#include "xmlHelper.h"
#include <zlib.h>
#include "stringEx.h"
#include "Global.h"

xmlHelper::xmlHelper()
{
	
}


xmlHelper::~xmlHelper()
{
	//Cleanup of XPath data 
	xmlXPathFreeObject(xpathObj);
	xmlXPathFreeContext(xpathCtx);
	/* free the document */
	xmlFreeDoc(doc);
	xmlCleanupParser();
}

bool xmlHelper::LoadFile(const char* szFilePath)
{
	
	/* Load XML document */
	doc = xmlParseFile(szFilePath);
	if (doc == NULL)
	{
		return false;
	}

	/* Create xpath evaluation context */
	xpathCtx = xmlXPathNewContext(doc);
	if (xpathCtx == NULL)
	{
		xmlXPathFreeContext(xpathCtx);
		xmlFreeDoc(doc);
		return false;
	}
	
	return true;	
}

void xmlHelper::getnodes(string xpathExpr)
{	
	char*  val = NULL;

	/* Evaluate xpath expression */
	xpathObj = xmlXPathEvalExpression((const xmlChar*)(xpathExpr.c_str()), xpathCtx);
	if (xpathObj == NULL)
	{
		//cout << "Error: unable to evaluate xpath expression" << xpathExpr << endl;
		xmlXPathFreeContext(xpathCtx);
		xmlFreeDoc(doc);
		return;
	}

	/* get values of the selected nodes */
	nodeset = xpathObj->nodesetval;
	if (xmlXPathNodeSetIsEmpty(nodeset))
	{
		//cout << "No such nodes." << endl;
		xmlXPathFreeObject(xpathObj);
		xmlXPathFreeContext(xpathCtx);
		xmlFreeDoc(doc);
		return;
	}

	stringEx ex;
	//get the value    
	int size = (nodeset) ? nodeset->nodeNr : 0;
	for (int i = 0; i <size; i++)
	{
		val = (char*)xmlNodeListGetString(doc, nodeset->nodeTab[i]->xmlChildrenNode, 1);
		//cout << "the results are: " << val << endl;
		xmlNodePtr propNodePtr = nodeset->nodeTab[i]->parent;
		if (propNodePtr != NULL)
		{
			//²éÕÒÊôÐÔ
			xmlAttrPtr attrPtr = propNodePtr->properties;
			while (attrPtr != NULL)
			{
				if (!xmlStrcmp(attrPtr->name, BAD_CAST "name"))
				{
					enums type;

					xmlChar* szAttr = xmlGetProp(propNodePtr, BAD_CAST "name");
					string key = string((const char*)szAttr);
					int n = key.rfind(".");
					if (n != string::npos)
					{
						key = key.substr(n+1);
						string value = string(ConvertEnc("utf-8", "gb2312", val));
						value = ex.trim(value);
						type.index = i;
						type.name = CA2T(key.c_str());
						type.des = CA2T(value.c_str());
						Global::cmdtypes.push_back(type);
					}					
					xmlFree(szAttr);
				}
				attrPtr = attrPtr->next;
			}
		}

		xmlFree(val);
	}

}

char * xmlHelper::ConvertEnc(char *encFrom, char *encTo, const char * in)
{

	static char bufin[1024], bufout[1024], *sin, *sout;
	int mode, lenin, lenout, ret, nline;
	iconv_t c_pt;

	if ((c_pt = iconv_open(encTo, encFrom)) == (iconv_t)-1)
	{
		printf("iconv_open false: %s ==> %s\n", encFrom, encTo);
		return NULL;
	}
	iconv(c_pt, NULL, NULL, NULL, NULL);

	lenin = strlen(in) + 1;
	lenout = 1024;
	sin = (char *)in;
	sout = bufout;
	ret = iconv(c_pt,(const char**)&sin, (size_t *)&lenin, &sout, (size_t *)&lenout);

	if (ret == -1)
	{
		return NULL;

	}

	iconv_close(c_pt);

	return bufout;
}