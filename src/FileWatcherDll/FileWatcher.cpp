#include "FileWatcher.h"

namespace filewatcherdll
{
	IFileWatcher* create_file_watcher(ui_callback callback)
	{
		return new file_watcher(callback);
	}

	void observe(IFileWatcher* ptr, wchar_t* path)
	{
		ptr->observe(path);
	}

	void stop(IFileWatcher* ptr)
	{
		ptr->stop_observe();
	}

	void dispose_file_watcher(IFileWatcher* ptr)
	{
		ptr->stop_observe();

		delete ptr;
	}

	template <typename TP>
	std::time_t to_time_t(TP tp)
	{
		using namespace std::chrono;
		auto sctp = time_point_cast<system_clock::duration>(tp - TP::clock::now()
			+ system_clock::now());
		return system_clock::to_time_t(sctp);
	}

	void file_watcher::stop_observe()
	{
		m_running = false;
	}

	void file_watcher::observe(wchar_t* path)
	{
		int pathLen = std::wcslen(path);
		m_path = new wchar_t[pathLen + 1];
		wcsncpy_s(m_path, pathLen + 1, path, pathLen);
	
		if (m_timer_thread.joinable())
		{
			m_timer_thread.join();
		}

		m_running = true;

		m_timer_thread = std::thread(&file_watcher::threadFunction, this);
	}

	void file_watcher::threadFunction()
	{
		while (m_running)
		{
			auto x = std::chrono::steady_clock::now() + std::chrono::milliseconds(250);
			check_files();
			std::this_thread::sleep_until(x);
		}
	}

	void file_watcher::check_files()
	{
		std::vector<file> files{};

		if (m_path == nullptr)
			return;

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

			std::time_t tt = to_time_t(entry.last_write_time());
			std::tm* gmt = std::gmtime(&tt);
			std::stringstream buffer{};
			buffer << std::put_time(gmt, "%d %B %Y %H:%M");
			std::string formattedFileTime = buffer.str();

			int lastWriteLen = std::strlen(formattedFileTime.c_str());
			file.last_write = new char[lastWriteLen + 1];
			strncpy_s(file.last_write, lastWriteLen + 1, formattedFileTime.c_str(), lastWriteLen);

			files.push_back(file);
		}

		if (files.empty()) 
		{
			m_ui_callback(nullptr, 0);
			return;
		}

		m_ui_callback(&files.front(), files.size());

		files.clear();
	}
}