#pragma once
#include "ReportGenerationException.h"

class WarehouseNotSufficientForReportException : public ReportGenerationException
{
private:
	string missingPropertyName;
public:
	WarehouseNotSufficientForReportException(string missingPropertyName);
	~WarehouseNotSufficientForReportException();

	virtual const char* what() const noexcept;
};

