#pragma once

#include "includes.h"

using namespace std;

namespace Utils
{
	string get_javascript_string(napi_env env, napi_value arg);
	System::String^ convert_to_cs_string(string str);
}
