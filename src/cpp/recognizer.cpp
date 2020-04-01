#include "utils.h"
#include "recognizer.h"

namespace Recognizer {
	/*
	* @method	Constructor
	*
	* Constructor de la clase CppRecognizer
	*/
	CppRecognizer::CppRecognizer() {}

	/*
	* @method	AddGrammarXML
	*
	* A�ade una gram�tica XML al motor de reconocimiento.
	*
	* @param	{srting}	Ruta del archivo XML que queremos a�adir a la gram�tica
	* @returns	{void}		Cadena legible por C#
	*/
	void CppRecognizer::AddGrammarXML(string path, string name, string culture) 
	{
		System::String^ csCulture = Utils::convert_to_cs_string(culture);
		System::String^ p = Utils::convert_to_cs_string(path);
		System::String^ n = Utils::convert_to_cs_string(name);

		CsRecognizer::Instance(csCulture)->AddGrammarXML(p,n);
	}

	/*
	* @method	Listen
	*
	* Pone a escuchar el motor de reconocimiento
	*
	* @returns	{void}		Cadena legible por C#
	*/
	void CppRecognizer::Listen(string culture)
	{
		System::String^ csCulture = Utils::convert_to_cs_string(culture);
		CsRecognizer::Instance(csCulture)->Listen();
	}

	/*
	* @method	IsListen
	*
	* Devuelve si el motor de reconocimiento est� escuchando o no.
	*
	* @returns	{System::Boolean}		TRUE si est� escuchando, FALSE si no lo est� haciendo.
	*/
	System::Boolean CppRecognizer::IsListen(string culture)
	{
		System::String^ csCulture = Utils::convert_to_cs_string(culture);
		return CsRecognizer::Instance(csCulture)->IsListen;
	}
}
