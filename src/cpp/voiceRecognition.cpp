#include "recognizer.h";
#include "loadAssemblies.h";

using namespace Recognizer;

#define DECLARE_NAPI_METHOD(name, func) { name, 0, func, 0, 0, 0, napi_default, 0 }

/*
* @function Initialize
*
* Función que realiza todas las tareas necesarias al cargar el módulo
*
* @return   {void}
*/
void VcInitialize()
{
    // Ponemos a la eschca la carga dez los ensamblados para que se busquen en la carpeta BIN

    Assembler::ListenLoadAssemblies();

    // Asignamos el TriggerEvent para escuchar los eventos enviados por el motor de reconocimiento

    CppRecognizer::SetTriggerEvent();
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
};

/*
* @function Listen
*
* Pone a la escucha el reconocedor de voz
*
* @return   {napi_string}       Estado de la solicitud
*/
void VcAddGrammarXML(napi_env env, napi_callback_info info)
{
    CppRecognizer::AddGrammarXML("Aquí la ruta del archivo XML");
};

/*
* @function Listen
*
* Pone a la escucha el reconocedor de voz
*
* @return   {napi_string}       Estado de la solicitud
*/
/*
napi_value VcAddGrammarXML(napi_env env, napi_callback_info info)
{
    napi_status status;
    napi_value world;

    status = napi_create_string_utf8(env, "Hola mundo que tal estás...bién.\nEstoy bien", NAPI_AUTO_LENGTH, &world);

    assert(status == napi_ok);
    return world;
};
*/

napi_value InitNode(napi_env env, napi_value exports) 
{
    napi_status status;

    // Incializams el componente al cargar el modulo nativo

    VcInitialize();

    // Creamos a la función de escucha

    napi_property_descriptor listen = DECLARE_NAPI_METHOD("listen", VcListen);
    status = napi_define_properties(env, exports, 1, &listen);
    assert(status == napi_ok);

    
    napi_property_descriptor add_grammar_xml = DECLARE_NAPI_METHOD("add_grammar_XML", VcAddGrammarXML);
    status = napi_define_properties(env, exports, 1, &add_grammar_xml);
    assert(status == napi_ok);
    

    return exports;
}

NAPI_MODULE(NODE_GYP_MODULE_NAME, InitNode)




