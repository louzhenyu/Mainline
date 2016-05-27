#include "stdafx.h"
#include "RedisHelper.h"
#include "Global.h"

namespace EtermServer{


	// AP Hash Function
	unsigned int APHash(const char *str) {
		unsigned int hash = 0;
		int i;

		for (i = 0; *str; i++)
		{
			if ((i & 1) == 0)
			{
				hash ^= ((hash << 7) ^ (*str++) ^ (hash >> 3));
			}
			else
			{
				hash ^= (~((hash << 11) ^ (*str++) ^ (hash >> 5)));
			}
		}

		return (hash & 0x7FFFFFFF);
	}


	RedisHelper::RedisHelper()
	{
	}


	RedisHelper::~RedisHelper()
	{
	}


	string RedisHelper::get(const char* key)
	{
		if (!Global::RedisList) return "";

		xRedisClient xRedis;
		bool bret = xRedis.Init();
		if (!bret) return "";
		bret = xRedis.ConnectRedisCache(Global::RedisList, 1, 1);
		if (!bret) return "";
		RedisDBIdx dbi(&xRedis);
		dbi.CreateDBIndex(key, APHash, 1);
		string str;
		xRedis.get(dbi, key, str);
		xRedis.release();
		return str;
	}

	bool RedisHelper::set(const char* key, string value, unsigned int sec)
	{
		if (!Global::RedisList) return false;

		xRedisClient xRedis;
		bool bret = xRedis.Init();
		if (!bret) return false;
		bret = xRedis.ConnectRedisCache(Global::RedisList, 1, 1);
		if (!bret) return false;
		RedisDBIdx dbi(&xRedis);
		dbi.CreateDBIndex(key, APHash, 1);
		bret = xRedis.setex(dbi, key, sec, value);
		xRedis.release();
		return bret;
	}

	bool RedisHelper::del(const char* key)
	{
		if (!Global::RedisList) return false;

		xRedisClient xRedis;
		bool bret = xRedis.Init();
		if (!bret) return false;
		bret = xRedis.ConnectRedisCache(Global::RedisList, 1, 1);
		if (!bret) return false;
		RedisDBIdx dbi(&xRedis);
		if (dbi.CreateDBIndex(key, APHash, 1))
		{
			return xRedis.del(dbi, key);
		}
		return false;
	}
}