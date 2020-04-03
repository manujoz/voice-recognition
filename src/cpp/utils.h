#pragma once

#include "includes.h"
#include "loadAssemblies.h"

using namespace std;

namespace Utils
{
	napi_value		create_napi_boolean(napi_env env,bool value);
	string			get_javascript_string(napi_env env, napi_value arg);
	System::String^	convert_to_cs_string(string str);
	string			convert_from_cs_string(System::String^ string);
	void			assemblerLoads();
}
