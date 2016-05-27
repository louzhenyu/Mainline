
// CntrItem.h : CEtermServerCntrItem 类的接口
//

#pragma once

class CEtermServerDoc;
class CEtermServerView;

class CEtermServerCntrItem : public CRichEditCntrItem
{
	DECLARE_SERIAL(CEtermServerCntrItem)

// 构造函数
public:
	CEtermServerCntrItem(REOBJECT* preo = NULL, CEtermServerDoc* pContainer = NULL);
		// 注意:  允许 pContainer 为 NULL 以启用 IMPLEMENT_SERIALIZE
		//  IMPLEMENT_SERIALIZE 要求类具有带零
		//  参数的构造函数。  OLE 项通常是用
		//  非 NULL 文档指针构造的

// 特性
public:
	CEtermServerDoc* GetDocument()
		{ return reinterpret_cast<CEtermServerDoc*>(CRichEditCntrItem::GetDocument()); }
	CEtermServerView* GetActiveView()
		{ return reinterpret_cast<CEtermServerView*>(CRichEditCntrItem::GetActiveView()); }
	
// 实现
public:
	~CEtermServerCntrItem();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
};

