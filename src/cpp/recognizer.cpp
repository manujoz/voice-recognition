#include "recognizer.h";

namespace Recognizer {
	/*
	* @method	Constructor
	*
	* Constructor de la clase CppRecognizer
	*/
	CppRecognizer::CppRecognizer() {}

	/*
	* @method	SetTriggerEvent
	*
	* Asigna la funci�n a la clase de Cpp.
	*
	* @param	{srting}	Ruta del archivo XML que queremos a�adir a la gram�tica
	* @returns	{void}		Cadena legible por C#
	*/
	void CppRecognizer::SetTriggerEvent() {
		// Se asigna al emisor de eventos la funci�n que los recibe en CPP.
		CsRecognizer::Instance()->emitEventToCpp = gcnew System::Func<System::String^, System::String^>(triggerEventFromCs);
	}

	/*
	* @method	AddGrammarXML
	*
	* A�ade una gram�tica XML al motor de reconocimiento.
	*
	* @param	{srting}	Ruta del archivo XML que queremos a�adir a la gram�tica
	* @returns	{void}		Cadena legible por C#
	*/
	void CppRecognizer::AddGrammarXML(string datos) {
		CsRecognizer::Instance()->AddGrammarXML(CppRecognizer::prConvertString(datos));
	}

	/*
	* @method	Listen
	*
	* Pone a escuchar el motor de reconocimiento
	*
	* @returns	{void}		Cadena legible por C#
	*/
	void CppRecognizer::Listen() {
		CsRecognizer::Instance()->Listen();
	}

	/*
	* @method	IsListen
	*
	* Devuelve si el motor de reconocimiento est� escuchando o no.
	*
	* @returns	{System::Boolean}		TRUE si est� escuchando, FALSE si no lo est� haciendo.
	*/
	System::Boolean CppRecognizer::IsListen() {
		return CsRecognizer::Instance()->IsListen;
	}

	/*
	* @method	prConvertString
	*
	* Convierte una cadena nativa de C++ en una cadena legible por C#
	*
	* @param	{srting}			Cadena nativa que queremos convertir
	* @returns	{System::String^}	Cadena legible por C#
	*/
	System::String^ CppRecognizer::prConvertString(string cadena)
	{
		System::String^ cadenaConvert = gcnew System::String(cadena.c_str());
		return cadenaConvert;
	}

	/*
	* @function	triggerEventFromCs
	*
	* Funci�n que recibe los eventos del motor de reconocimiento en CS. Esta funci�n es asignada
	* a la biblioteca CS en el constructor de la clase CppRecognizer.
	*
	* @returns	{System::String}	Devuelve un string porque es necesaria la devoluci�n para el funcionamiento
	*/
	System::String^ triggerEventFromCs(System::String^ data) {
		// Esta función es llamada cuando el motro de reconocimiento emite
		// un evento. Desde aquí se tiene que emitir un evento hacia Node.js

		string str = "OK";
		return gcnew System::String(str.c_str());
	}
}
