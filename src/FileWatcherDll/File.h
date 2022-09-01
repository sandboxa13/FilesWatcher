#pragma once
#include <string>

namespace filewatcherdll
{
	struct file
	{
		char* name;
		char* path;
		char* last_write;
		int size;
		bool is_directory;
	};
}