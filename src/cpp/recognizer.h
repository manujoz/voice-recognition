#pragma once

#include "includes.h"

using namespace std;
using namespace CsVoiceRecognition;

namespace Recognizer 
{
	class CppRecognizer {
		public:
			CppRecognizer();

			static void AddGrammarXML(string path, string name);
			static void Listen();
			static System::Boolean IsListen();
	};
}
