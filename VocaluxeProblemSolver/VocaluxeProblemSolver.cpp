// VocaluxeProblemSolver.cpp : Definiert den Einstiegspunkt für die Konsolenanwendung.
//

#include "stdafx.h"
#include <windows.h>
#include "DetectFx.h"
#include <string>
#include <iostream>
#include <fstream>
#include "resource.h"

std::string getTempFolder(){

	std::string uglyPath;
	char bufferForUglyPath[MAX_PATH];

	if (GetTempPathA(MAX_PATH, bufferForUglyPath))
	{
		uglyPath = bufferForUglyPath;
		WCHAR bufferForNicePath[MAX_PATH];

		std::wstring uglyPathWString = std::wstring(uglyPath.begin(), uglyPath.end());

		if (GetLongPathName(uglyPathWString.c_str(), bufferForNicePath, MAX_PATH)){
			std::wstring wtemp = std::wstring(bufferForNicePath);
			return std::string(wtemp.begin(), wtemp.end());
		}
	}


	return uglyPath;
}

//http://www.codeproject.com/Articles/4221/Adding-and-extracting-binary-resources
void ExtractBinResource(LPCTSTR strCustomResName, LPCTSTR nResourceType, std::string strOutputName)
{
	HGLOBAL hResourceLoaded;		// handle to loaded resource 
	HRSRC hRes;						// handle/ptr. to res. info. 
	char *lpResLock;				// pointer to resource data 
	DWORD dwSizeRes;
	std::string strOutputLocation;
	std::string strAppLocation;

	// lets get the app location
	strAppLocation = getTempFolder();
	strOutputLocation = strAppLocation += "\\";
	strOutputLocation += strOutputName;

	// find location of the resource and get handle to it
	hRes = FindResource(NULL, strCustomResName, nResourceType);

	// loads the specified resource into global memory. 
	hResourceLoaded = LoadResource(NULL, hRes);

	// get a pointer to the loaded resource!
	lpResLock = (char*)LockResource(hResourceLoaded);

	// determine the size of the resource, so we know how much to write out to file!  
	dwSizeRes = SizeofResource(NULL, hRes);

	std::ofstream* outputFile = new std::ofstream(strOutputLocation.c_str(), std::ios::binary);

	outputFile->write((const char*)lpResLock, dwSizeRes);
	outputFile->close();
}

void runApp(std::string exe, std::string param)
{
	STARTUPINFO si;
	PROCESS_INFORMATION pi;

	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));

	std::string command = "" + exe + " " + param;
	std::wstring commandWString = std::wstring(command.begin(), command.end());


	// Start the child process. 
	if (!CreateProcess(NULL,   // No module name (use command line)
		(LPWSTR)commandWString.c_str(),        // Command line
		NULL,           // Process handle not inheritable
		NULL,           // Thread handle not inheritable
		FALSE,          // Set handle inheritance to FALSE
		0,              // No creation flags
		NULL,           // Use parent's environment block
		NULL,           // Use parent's starting directory 
		&si,            // Pointer to STARTUPINFO structure
		&pi)           // Pointer to PROCESS_INFORMATION structure
		)
	{
		printf("CreateProcess failed (%d).\n", GetLastError());
		return;
	}

	// Wait until child process exits.
	WaitForSingleObject(pi.hProcess, INFINITE);

	// Close process and thread handles. 
	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);
}



void installNetFx(){
	ExtractBinResource(MAKEINTRESOURCE(IDR_BINARY1), _T("binary"), "dotNetFx45_Full_setup.exe");
	runApp(getTempFolder() + "\\dotNetFx45_Full_setup.exe", "/passive /promptrestart");
	std::cout << "Reboot your computer and restart the VocaluxeProblemSolver again.\n";
	std::cin.get();
}

void runFixApp()
{
	ExtractBinResource(MAKEINTRESOURCE(IDR_BINARY2), _T("binary"), "VocaluxeProblemFixer.exe");
	runApp(getTempFolder() + "\\VocaluxeProblemFixer.exe", "");
}

int _tmain(int argc, _TCHAR* argv[])
{
	DetectFx* detector = new DetectFx();
	if (detector->IsNetfx40FullInstalled()){
		runFixApp();
	}
	else{
		installNetFx();
	}
	return 0;
}