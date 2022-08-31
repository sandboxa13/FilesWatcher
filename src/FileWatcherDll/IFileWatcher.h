#pragma once

#include "File.h"
#include <filesystem>
#include <Windows.h>

namespace filewatcherdll
{
	
	typedef void(*ui_callback)(const file* files, int count);

	class __declspec(dllexport) IFileWatcher 
	{
		
	};

	extern "C" __declspec(dllexport) IFileWatcher* create_file_watcher(ui_callback, wchar_t* path);
	extern "C" __declspec(dllexport) void dispose_file_watcher(IFileWatcher*);
}