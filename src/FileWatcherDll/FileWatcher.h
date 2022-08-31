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
			for (const auto& entry : std::filesystem::directory_iterator(path))
			{
				file file {};

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

			callback(&files.front(), files.size());
		}

		~file_watcher()
		{
			files.clear();
		}

	private:
		ui_callback m_ui_callback;
		std::vector<file> files {};
	};
}