#include "addon.h"

/*
* @function	triggerEventFromCs
*
* Función que recibe los eventos del motor de reconocimiento en CS. Esta función es asignada
* a la biblioteca CS en el constructor de la clase CppRecognizer.
*
* @returns	{System::String}	Devuelve un string porque es necesaria la devoluci�n para el funcionamiento
*/
System::String^ dispatchEventFromCs(System::String^ data, System::String^ evName)
{
    napi_status status;
    napi_value result;
    napi_value params[2];

    // Convertimos los System::String en std::string
    string stringName = msclr::interop::marshal_as<string>(evName);
    string stringData = msclr::interop::marshal_as<string>(data);

    // Convertimos los std::string en javascript Strings
    napi_create_string_utf8(FuncResult.env, stringName.c_str(), NAPI_AUTO_LENGTH, &params[0]);
    napi_create_string_utf8(FuncResult.env, stringData.c_str(), NAPI_AUTO_LENGTH, &params[1]);

    // Obtenemos la función de la referencia
    napi_value func;
    napi_get_reference_value(FuncResult.env, FuncResult.func, &func);

    // Llamamos a la función javascript
    status = napi_call_function(FuncResult.env, FuncResult.global, func, 2, params, &result);
    assert(status == napi_ok);

    return nullptr;
}

/*
* @function	SetTriggerEvent
*
* Se asigna el triggerEvent a la clase de reconocimiento de C#
*
* @returns	{void}
*/
void SetFunctionEventCSharp() {
    // Asignamos el TriggerEvent para escuchar los eventos enviados por el motor de reconocimiento

    CsRecognizer::Instance()->emitEventToCpp = gcnew System::Func<System::String^, System::String^, System::String^>(dispatchEventFromCs);
}

/*
* @function VcInitialize
*
* Función que realiza todas las tareas necesarias al cargar el módulo
*
* @return   {void}
*/
void VcInitialize()
{    
    // Ponemos a la eschca la carga dez los ensamblados para que se busquen en la carpeta BIN
    Assembler::ListenLoadAssemblies();

    // Asignamos el triggerEvent
    SetFunctionEventCSharp();
}



#pragma unmanaged

/*
* @function VcCallEmit
*
* Configura la emisión de evento hacia javascript
*
* @return   {napi_string}       Estado de la solicitud
*/
napi_value VcSetResultFunction(napi_env env, napi_callback_info info)
{
    napi_status status;
    napi_value result;
    
    // Obtenemos la función que re resultado
    size_t argc = 1;
    napi_value args[1];
    status = napi_get_cb_info(env, info, &argc, args, nullptr, nullptr);
    assert(status == napi_ok);
    
    if (argc < 1) {
        napi_throw_type_error(env, nullptr, "You must provide one argument");
        return nullptr;
    }

    // Creamos la función persistente
    FuncResult.env = env;
    
    status = napi_get_global(env, &FuncResult.global);
    assert(status == napi_ok);
    
    napi_create_reference(env, args[0], 1, &FuncResult.func);   
   
    return nullptr;
}

/*
* @function Listen
*
* Pone a la escucha el reconocedor de voz
*
* @return   {napi_string}       Estado de la solicitud
*/
napi_value VcListen(napi_env env, napi_callback_info info) 
{
    CppRecognizer::Listen();

    return nullptr;
};

/*
* @function Listen
*
* Pone a la escucha el reconocedor de voz
*
* @return   {napi_string}       Estado de la solicitud
*/
napi_value VcAddGrammarXML(napi_env env, napi_callback_info info)
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

    string path = get_javascript_string(env, args[0]);
    string name = get_javascript_string(env, args[1]);

    CppRecognizer::AddGrammarXML(path,name);

    return nullptr;
};

#define DECLARE_NAPI_METHOD(name, func) { name, 0, func, 0, 0, 0, napi_default, 0 }

/*
* @function InitNode
*
* Función que crea el addon
*
* @return   {napi_string}       Estado de la solicitud
*/
napi_value InitNode(napi_env env, napi_value exports) 
{
    napi_status status;

    // Incializams el componente al cargar el modulo nativo

    VcInitialize();

    // Creamos a la función de escucha

    napi_property_descriptor call_emit = DECLARE_NAPI_METHOD("_call_emit", VcSetResultFunction);
    status = napi_define_properties(env, exports, 1, &call_emit);
    assert(status == napi_ok);

    napi_property_descriptor listen = DECLARE_NAPI_METHOD("listen", VcListen);
    status = napi_define_properties(env, exports, 1, &listen);
    assert(status == napi_ok);
    
    napi_property_descriptor add_grammar_xml = DECLARE_NAPI_METHOD("add_grammar_XML", VcAddGrammarXML);
    status = napi_define_properties(env, exports, 1, &add_grammar_xml);
    assert(status == napi_ok);    

    return exports;
}

NAPI_MODULE(NODE_GYP_MODULE_NAME, InitNode)




