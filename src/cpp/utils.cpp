#include "utils.h"

namespace Utils
{
    /*
    * @function	create_napi_boolean
    *
    * Create a boolean capable of being sent to javascript
    *
    * @returns	{napi_boolean}
    */
    napi_value create_napi_boolean(napi_env env, bool value)
    {
        napi_value boolean;
        napi_value response;

        if (value) {
            napi_create_int32(env, 1, &boolean);
        } else {
            napi_create_int32(env, 0, &boolean);
        }

        napi_coerce_to_bool(env, boolean, &response);
        return response;
    }

    /*
    * @function	get_javascript_string
    *
    * Convert javascript string to std::string
    *
    * @returns	{std::string}
    */
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
        status = napi_get_value_string_latin1(env, arg, str, 512, &result);
        assert(status == napi_ok);

        string s(str);
        return s;
	}
       
    /*
    * @function	convert_from_cs_string
    *
    * Converts a System::String ^ to a std string
    *
    * @returns	{std::string}
    */
    string convert_from_cs_string(System::String^ str) {
        return msclr::interop::marshal_as<string>(str);
    }

    /*
    * @function	convert_to_cs_string
    *
    * Converts a std::string to a System::String^
    *
    * @returns	{System::String^}
    */
    System::String^ convert_to_cs_string(string str)
    {
        return gcnew System::String(str.c_str());
    }

    /*
    * @function AssemblerLoads
    *
    * Load assemblies into custom routes
    *
    * @return   {void}
    */
    void assemblerLoads()
    {
        Assembler::ListenLoadAssemblies();
    }

}

