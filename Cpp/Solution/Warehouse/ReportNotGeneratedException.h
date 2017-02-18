#pragma once
#include "ReportException.h"

class ReportNotGeneratedException : public ReportException
{
public:
	ReportNotGeneratedException();
	~ReportNotGeneratedException();
};

