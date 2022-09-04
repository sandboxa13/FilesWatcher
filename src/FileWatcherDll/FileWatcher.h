#pragma once

#include "IFileWatcher.h"

namespace filewatcherdll
{
	class file_watcher : public IFileWatcher
	{
	public:
		file_watcher(ui_callback callback)
			: m_ui_callback(callback), m_running(false), m_path(nullptr)
		{
		}

		~file_watcher()
		{
			files.clear();
		}

		void threadFunction();
		void check_files();
		void stop_observe(void);
		void observe(wchar_t* path);

	private:
		ui_callback m_ui_callback;
		std::vector<file> files {};
		wchar_t* m_path;
		std::thread m_timer_thread;
		bool m_running;
	};
}