#pragma once

#include "includes.h"
#include "globals.h"
#include "utils.h"


namespace ManagedRecognizer
{
    void    _constructor(string culture);
    bool    _isInstalledCulture(string culture);
    string  _getInstalledCultures();
    string  _getEngineCulture();
    void    _addGrammarXML(string path, string name);
    void    _listen();
}
