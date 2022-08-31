#pragma once
#include <Windows.h>

namespace filewatcherdll
{
	extern "C" void __declspec(dllexport) __stdcall restart_as_admin(wchar_t* path);
}