#include "stdafx.h"
#include "FileWriter.h"
#include <fstream>
#include <ctime>
#include "DateHelper.hpp"
#include "ConfigurationObject.h"

void FileWriter::WriteFile(string text, string path)
{
	ofstream file;
	file.open(path, ios::app);
	file << text;
	file.close();
}

/// Writes a log file with current date
void FileWriter::WriteLog(string text)
{
	tm now = DateHelper::GetCurrentTime();
	char formattedDate[100];
	strftime(formattedDate, 100, "%d-%m-%Y %T", &now);

	auto bufferSize = text.length() + 100;
	char *message = new char[bufferSize];
	sprintf_s(message, bufferSize, "[%s]:%s\n", formattedDate, text.c_str());

	WriteFile(message, ConfigurationObject::GetInstance()->GetLogPath());
}
