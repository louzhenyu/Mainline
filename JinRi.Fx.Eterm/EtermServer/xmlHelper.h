#pragma once
#include <libxml/tree.h>
#include <libxml/parser.h>
#include <libxml/xpath.h>
#include <libxml/xpathInternals.h>
#include <map>

class xmlHelper
{
public:
	xmlHelper();
	~xmlHelper();
	bool LoadFile(const char* szFilePath);
	void getnodes(string xpathExpr);
private:

	xmlDocPtr doc;
	xmlXPathContextPtr xpathCtx;
	xmlXPathObjectPtr xpathObj;
	xmlNodeSetPtr  nodeset;
	string xpathExpr;

	char * ConvertEnc(char *encFrom, char *encTo, const char * in);
};

