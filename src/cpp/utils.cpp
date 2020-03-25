#include "utils.h"

namespace Utils
{
	string get_javascript_string(napi_env env, napi_value arg)
	{
        napi_status status;

        napi_valuetype type;
        status = napi_typeof(env, arg, &type);
        assert(status == napi_ok);

        if (type != napi_string) {
            napi_throw_type_error(env, nullptr, "You mus provide a string param");
            return NULL;
        }

        char str[512];
        size_t result;
        status = napi_get_value_string_utf8(env, arg, str, 512, &result);
        assert(status == napi_ok);

        string s(str);
        return s;
	}

    System::String^ convert_to_cs_string(string str)
    {
        System::String^ cadenaConvert = gcnew System::String(str.c_str());
        return cadenaConvert;
    }
}

