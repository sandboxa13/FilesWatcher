#include "FileWatcher.h"

namespace filewatcherdll
{
	IFileWatcher* create_file_watcher(ui_callback callback, wchar_t* path)
	{
		return new file_watcher(callback, path);
	}

	void dispose_file_watcher(IFileWatcher* ptr)
	{
		delete ptr;
	}
}