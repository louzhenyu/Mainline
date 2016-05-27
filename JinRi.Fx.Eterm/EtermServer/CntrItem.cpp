
// CntrItem.cpp : CEtermServerCntrItem 类的实现
//

#include "stdafx.h"
#include "EtermServer.h"

#include "EtermServerDoc.h"
#include "EtermServerView.h"
#include "CntrItem.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CEtermServerCntrItem 的实现

IMPLEMENT_SERIAL(CEtermServerCntrItem, CRichEditCntrItem, 0)

CEtermServerCntrItem::CEtermServerCntrItem(REOBJECT* preo, CEtermServerDoc* pContainer)
	: CRichEditCntrItem(preo, pContainer)
{
	// TODO:  在此添加一次性构造代码
}

CEtermServerCntrItem::~CEtermServerCntrItem()
{
	// TODO:  在此处添加清理代码
}


// CEtermServerCntrItem 诊断

#ifdef _DEBUG
void CEtermServerCntrItem::AssertValid() const
{
	CRichEditCntrItem::AssertValid();
}

void CEtermServerCntrItem::Dump(CDumpContext& dc) const
{
	CRichEditCntrItem::Dump(dc);
}
#endif

