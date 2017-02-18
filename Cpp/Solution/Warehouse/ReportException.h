#pragma once
class ReportException : public exception
{
public:
	ReportException(string message);
	~ReportException();
};

