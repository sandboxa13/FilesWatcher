#pragma once
#include <string>

namespace filewatcherdll
{
	struct file
	{
		char* name;
		char* path;
		int size;
		bool is_directory;
	};
}