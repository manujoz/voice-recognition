#include "managedRecognizer.h"

namespace ManagedRecognizer
{
    void _constructor(string culture)
    {
        CS::recognizer->Init(Utils::convert_to_cs_string(culture));
    }

    bool _isInstalledCulture(string culture)
    {
        return CS::recognizer->IsInstalledCulture(Utils::convert_to_cs_string(culture));
    }

    string _getInstalledCultures()
    {
        System::String^ Cultures = CS::recognizer->InstalledCultures();
        return Utils::convert_from_cs_string(Cultures);
    }

    string _getEngineCulture()
    {
        System::String^ Culture = CS::recognizer->Culture;
        return Utils::convert_from_cs_string(Culture);
    }

    void _addGrammarXML(string path, string name)
    {
        CS::recognizer->AddGrammarXML(Utils::convert_to_cs_string(path), Utils::convert_to_cs_string(name));
    }

    void _listen()
    {
        CS::recognizer->Listen();
    }
}