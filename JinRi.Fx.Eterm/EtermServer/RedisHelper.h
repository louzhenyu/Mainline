#pragma once

namespace EtermServer{
	class RedisHelper
	{
	public:
		RedisHelper();
		~RedisHelper();
		static string get(const char* key);
		static bool set(const char* key, string value, unsigned int sec = 24 * 3600);
		static bool del(const char* key);
	};
}

