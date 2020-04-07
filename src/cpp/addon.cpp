#include "addon.h"

/*
* @function Init
*
* Function that performs all the necessary tasks when loading the module
*
* @return   {void}
*/
void Init() 
{
    // Assign the dispatchEventFromCS function to the emitEventToCpp of the Recognizer class in C#
    CS::recognizer->emitEventToCpp = gcnew System::Func<System::String^, System::String^, System::String^>(dispatchEventFromCs);
}

/*
* @function	triggerEventFromCs
*
* Function that receives the events of the recognition engine in CS.
*
* @returns	{nullptr}
*/
System::String^ dispatchEventFromCs(System::String^ data, System::String^ evName)
{
    napi_status status;
    napi_value result;
    napi_value params[2];

    // Convertimos los System::String en std::string
    string stringName = Utils::convert_from_cs_string(evName);
    string stringData = Utils::convert_from_cs_string(data);

    // Convertimos los std::string en javascript Strings
    napi_create_string_latin1(FuncResult.env, stringName.c_str(), NAPI_AUTO_LENGTH, &params[0]);
    napi_create_string_latin1(FuncResult.env, stringData.c_str(), NAPI_AUTO_LENGTH, &params[1]);

    // Obtenemos la función de la referencia
    napi_value func;
    napi_get_reference_value(FuncResult.env, FuncResult.func, &func);

    // Llamamos a la función javascript
    status = napi_call_function(FuncResult.env, FuncResult.global, func, 2, params, &result);
    assert(status == napi_ok);

    return nullptr;
}



#pragma unmanaged

/*
* @function Constructor
*
* Initialize the recognition engine with a given culture.
*
* @return   {boolean}       TRUE if is initialized, FALS if not.
*/
napi_value Constructor(napi_env env, napi_callback_info info)
{
    napi_status status;

    // Get the past culture
    size_t argc = 1;
    napi_value args[1];
    status = napi_get_cb_info(env, info, &argc, args, nullptr, nullptr);
    assert(status == napi_ok);

    string culture = Utils::get_javascript_string(env, args[0]);

    // Check if culture is installed
    if (culture != "") {
        if (!ManagedRecognizer::_isInstalledCulture(culture)) {
            return Utils::create_napi_boolean(env,false);
        }
    }

    // Instantiate the recognition engine
    ManagedRecognizer::_constructor(culture);

    return Utils::create_napi_boolean(env, true);
}

/*
* @function SetResultFunction
*
* Set the event broadcast to javascript
*
* @return   {napi_string}
*/
napi_value SetResultFunction(napi_env env, napi_callback_info info)
{
    napi_status status;
    napi_value result;

    // Get javascript result function
    size_t argc = 1;
    napi_value args[1];
    status = napi_get_cb_info(env, info, &argc, args, nullptr, nullptr);
    assert(status == napi_ok);

    if (argc < 1) {
        napi_throw_type_error(env, nullptr, "You must provide one argument");
        return nullptr;
    }

    // Persisnten function
    FuncResult.env = env;
    status = napi_get_global(env, &FuncResult.global);
    assert(status == napi_ok);

    napi_create_reference(env, args[0], 1, &FuncResult.func);

    return nullptr;
}

/*
* @function GetInstalledCultures
*
* Devuelve las culturas instaladas en el motor de reconocimiento
*
* @return   {napi_string} 
*/
napi_value GetInstalledCultures(napi_env env, napi_callback_info info)
{
    napi_status status;

    string cultures = ManagedRecognizer::_getInstalledCultures();

    napi_value str;
    napi_create_string_utf8(env, cultures.c_str(), NAPI_AUTO_LENGTH, &str);
    assert(status == napi_ok);

    return str;
}

/*
* @function GetEngineCulture
*
* Returns the culture of the recognition engine
*
* @return   {napi_string}
*/
napi_value GetEngineCulture(napi_env env, napi_callback_info info)
{
    napi_status status;

    string culture = ManagedRecognizer::_getEngineCulture();

    napi_value str;
    napi_create_string_utf8(env, culture.c_str(), NAPI_AUTO_LENGTH, &str);
    assert(status == napi_ok);

    return str;
}

/*
* @function AddGrammarXML
*
* Add a grammar from an XML file
*
* @return   {napi_string}
*/
napi_value AddGrammarXML(napi_env env, napi_callback_info info)
{
    napi_status status;

    size_t argc = 2;
    napi_value args[2];
    status = napi_get_cb_info(env, info, &argc, args, nullptr, nullptr);
    assert(status == napi_ok);

    if (argc < 1) {
        napi_throw_type_error(env, nullptr, "You must provide an XML grammar file");
        return nullptr;
    }

    string path = Utils::get_javascript_string(env, args[0]);
    string name = Utils::get_javascript_string(env, args[1]);

    ManagedRecognizer::_addGrammarXML(path, name);

    return nullptr;
};

/*
* @function Listen
*
* Listen to the recognition engine
*
* @return   {napi_string}
*/
napi_value Listen(napi_env env, napi_callback_info info)
{
    ManagedRecognizer::_listen();
    return nullptr;
};


#define DECLARE_NAPI_METHOD(name, func) { name, 0, func, 0, 0, 0, napi_default, 0 }


/*
* @function InitNode
*
* Function that creates the addon
*
* @return   {napi_value}
*/
napi_value InitNode(napi_env env, napi_value exports)
{
    napi_status status;

    // Managed assemblies load
    // . . . . . . . . . . . . . . . . . 

    Utils::assemblerLoads();

    // Invoke Init function
    // . . . . . . . . . . . . . . . . . 

    Init();

    // Create the addon methods
    // . . . . . . . . . . . . . . . . . 

    // Constructor
    napi_property_descriptor constructorJS = DECLARE_NAPI_METHOD("constructorJS", Constructor);
    status = napi_define_properties(env, exports, 1, &constructorJS);
    assert(status == napi_ok);

    // Result function emitter to javascript
    napi_property_descriptor result_function = DECLARE_NAPI_METHOD("result_function", SetResultFunction);
    status = napi_define_properties(env, exports, 1, &result_function);
    assert(status == napi_ok);

    // Get installed cultures
    napi_property_descriptor get_installed_cultures = DECLARE_NAPI_METHOD("get_installed_cultures", GetInstalledCultures);
    status = napi_define_properties(env, exports, 1, &get_installed_cultures);
    assert(status == napi_ok);

    // Get engine culture
    napi_property_descriptor get_engine_culture = DECLARE_NAPI_METHOD("get_engine_culture", GetEngineCulture);
    status = napi_define_properties(env, exports, 1, &get_engine_culture);
    assert(status == napi_ok);

    // Add a grammar from an XML file
    napi_property_descriptor add_grammar_xml = DECLARE_NAPI_METHOD("add_grammar_XML", AddGrammarXML);
    status = napi_define_properties(env, exports, 1, &add_grammar_xml);
    assert(status == napi_ok);

    napi_property_descriptor listen = DECLARE_NAPI_METHOD("listen", Listen);
    status = napi_define_properties(env, exports, 1, &listen);
    assert(status == napi_ok);

    return exports;
}

NAPI_MODULE(NODE_GYP_MODULE_NAME, InitNode)

