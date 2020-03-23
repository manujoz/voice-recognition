#pragma once

using namespace System;
using namespace System::IO;
using namespace System::Reflection;

class Assembler
{
public:
	static void ListenLoadAssemblies();
	static String^ GetAssemblyPath();
	static Assembly^ LoadFromBin(Object^ sender, ResolveEventArgs^ args);
};
