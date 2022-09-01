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

	void file_watcher::threadFunction()
	{
		while (futureObj.wait_for(std::chrono::milliseconds(1)) == std::future_status::timeout)
		{
			auto x = std::chrono::steady_clock::now() + std::chrono::milliseconds(1000);
			check_files();
			std::this_thread::sleep_until(x);
		}
	}

	void file_watcher::timer_start()
	{
		futureObj = exitSignal.get_future();
		m_timer_thread = std::thread(&file_watcher::threadFunction,this);
	}

	void file_watcher::check_files()
	{
		files.clear();

		for (const auto& entry : std::filesystem::directory_iterator(m_path))
		{
			file file{};

			auto fileName = entry.path().filename().string();
			auto path = entry.path().string();

			int fileNameLen = std::strlen(fileName.c_str());
			int pathLen = std::strlen(path.c_str());

			file.name = new char[fileNameLen + 1];
			file.path = new char[pathLen + 1];

			strncpy_s(file.name, fileNameLen + 1, fileName.c_str(), fileNameLen);
			strncpy_s(file.path, pathLen + 1, path.c_str(), pathLen);

			file.is_directory = entry.is_directory();
			file.size = entry.file_size();

			files.push_back(file);
		}

		m_ui_callback(&files.front(), files.size());
	}
}