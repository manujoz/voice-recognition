#pragma once

#include <assert.h>
#include <node_api.h>
#include <iostream>
#include <string>

using namespace std;
using namespace CsVoiceRecognition;

namespace Recognizer 
{
	class CppRecognizer {
		public:
			CppRecognizer();
			static void SetTriggerEvent();
			static void AddGrammarXML(string datos);
			static void Listen();
			static System::Boolean IsListen();

		private:
			static System::String^ prConvertString(string cadena);
	};

	System::String^ triggerEventFromCs(System::String^ data);
}
