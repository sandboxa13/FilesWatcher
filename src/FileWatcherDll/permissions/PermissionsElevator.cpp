#include "PermissionsElevator.h"

namespace filewatcherdll
{
	void __stdcall restart_as_admin(wchar_t* path)
	{
		LPWSTR szPath = nullptr;

		SHELLEXECUTEINFO sei = { sizeof(sei) };

		sei.lpVerb = L"runas";
		sei.lpFile = path;
		sei.hwnd = NULL;
		sei.nShow = SW_NORMAL;

		if (!ShellExecuteEx(&sei))
		{
			DWORD dwError = GetLastError();
			if (dwError == ERROR_CANCELLED)
				CreateThread(0, 0, (LPTHREAD_START_ROUTINE)restart_as_admin, 0, 0, 0);
		}
	}
}