#pragma once

#include "includes.h"

using namespace std;
using namespace CsVoiceRecognition;

namespace Recognizer 
{
	class CppRecognizer {
		public:
			CppRecognizer();

			static void AddGrammarXML(string path, string name, string culture);
			static void Listen(string culture);
			static System::Boolean IsListen(string culture);
	};
}
