#pragma once
#include "ReportException.h"

class ReportGenerationException : public ReportException
{
public:
	ReportGenerationException();
	ReportGenerationException(string message);
	~ReportGenerationException();
};

