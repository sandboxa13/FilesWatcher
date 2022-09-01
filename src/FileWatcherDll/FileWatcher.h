#pragma once

#include "IFileWatcher.h"

namespace filewatcherdll
{
	class file_watcher : public IFileWatcher
	{
	public:
		file_watcher(ui_callback callback, wchar_t* path)
			: m_ui_callback(callback)
		{
			int pathLen = std::wcslen(path);
			m_path = new wchar_t[pathLen + 1];
			wcsncpy_s(m_path, pathLen + 1, path, pathLen);

			timer_start();
		}

		~file_watcher()
		{
			files.clear();
		}

		void threadFunction();
		void timer_start();
		void check_files();
		void stop(void);

	private:
		ui_callback m_ui_callback;
		std::vector<file> files {};
		wchar_t* m_path;
		std::thread m_timer_thread;
		std::promise<void> exitSignal;
		std::future<void> futureObj;
		bool m_running;
	};
}