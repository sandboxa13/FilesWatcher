#pragma once

#include "File.h"
#include <filesystem>
#include <Windows.h>
#include <functional>
#include <thread>
#include <future>
#include <sstream> 

namespace filewatcherdll
{
	typedef void(*ui_callback)(const file* files, int count);

	class __declspec(dllexport) IFileWatcher 
	{
	public:
		virtual void stop_observe(void) = 0;
		virtual void observe(wchar_t* path) = 0;
	};

	extern "C" __declspec(dllexport) IFileWatcher* create_file_watcher(ui_callback);
	extern "C" __declspec(dllexport) void dispose_file_watcher(IFileWatcher*);
	extern "C" __declspec(dllexport) void stop(IFileWatcher*);
	extern "C" __declspec(dllexport) void observe(IFileWatcher*, wchar_t* path);
}