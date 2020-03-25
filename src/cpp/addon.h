#pragma once

#include <node_api.h>
#include <msclr/marshal_cppstd.h>

#include "includes.h"
#include "loadAssemblies.h"
#include "utils.h"
#include "recognizer.h"

using namespace Recognizer;
using namespace Utils;

struct _func_result
{
    napi_env env;
    napi_value global;
    napi_ref func;
    napi_threadsafe_function funcTh;
} FuncResult;
