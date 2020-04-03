#include "loadAssemblies.h"

/*
* @function ListenLoadAssemblies
*
* Listens for assembly load events and if not found looks for it on specified paths
*
* @return   {void}
*/
void Assembler::ListenLoadAssemblies()
{
	System::AppDomain^ currentDomain = AppDomain::CurrentDomain;
	currentDomain->AssemblyResolve += gcnew ResolveEventHandler(Assembler::LoadFromBin);
}

/*
* @function GetAssemblyPath
*
* Get the load path of the assemblies
*
* @return   {String^}
*/
String^ Assembler::GetAssemblyPath()
{
	try
	{
		return Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location);
	}
	catch (Exception ^ e)
	{
		String^ codeBase = Assembly::GetExecutingAssembly()->CodeBase;
		if (codeBase->StartsWith("file://?/"))
		{
			Uri^ codeBaseUri = gcnew Uri("file://" + codeBase->Substring(8));
			return codeBaseUri->LocalPath;
		}
		else
		{
			throw;
		}
	}
}

/*
* @function LoadFromBin
*
* Loads assemblies from BIN directory of node package
*
* @return   {Assembly^ }
*/
Assembly^ Assembler::LoadFromBin(Object^ sender, ResolveEventArgs^ args)
{

	String^ folderPath = Path::GetDirectoryName(Assembler::GetAssemblyPath())->Replace("build\\Release", "bin");
	String^ assemblyPath = Path::Combine(folderPath, (gcnew AssemblyName(args->Name))->Name + ".dll");

	if (File::Exists(assemblyPath) == false)
	{
		Console::WriteLine("El ensamblado " + args->Name + " no existe en la carpeta BIN");
		return nullptr;
	}

	Assembly^ assembly = Assembly::LoadFrom(assemblyPath);
	return assembly;
}