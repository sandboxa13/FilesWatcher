#pragma once
#include <Windows.h>

namespace filewatcherdll
{
	extern "C" bool __declspec(dllexport) __stdcall is_user_admin();
}