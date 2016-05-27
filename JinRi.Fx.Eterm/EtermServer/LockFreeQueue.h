#ifndef LOCK_FREE_QUEUE_H_
#define LOCK_FREE_QUEUE_H_

//不加锁队列，适合一个线程读取，一个线程写
#include <list>
template <typename T>
class LockFreeQueue
{
public:
	LockFreeQueue()
	{
		list.push_back(T());//分割节点
		iHead = list.begin();
		iTail = list.end();
	};
	//存消息
	void Produce(const T& t) 
	{
		list.push_back(t);
		iTail = list.end();
		list.erase(list.begin(), iHead);
	};
	//取消息
	bool Consume(T& t) 
	{
		typename TList::iterator iNext = iHead;
		++iNext;
		if (iNext != iTail)
		{
			iHead = iNext;
			t = *iHead;
			return true;
		}
		return false;
	};
	//查看消息不删除
	bool Peek(T& t) 
	{
		typename TList::iterator iNext = iHead;
		++iNext;
		if (iNext != iTail)
		{
			t = *iNext;
			return true;
		}
		return false;
	}

	bool IsEmpty()
	{
		typename TList::iterator iNext = iHead;
		++iNext;
		if (iNext != iTail)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	int GetSize()
	{
		return list.size()-2;
	};
	int GetMaxSize()
	{
		return list.max_size();
	};

private:
	typedef std::list<T> TList;
	TList list;
	typename TList::iterator iHead, iTail;
};
#endif