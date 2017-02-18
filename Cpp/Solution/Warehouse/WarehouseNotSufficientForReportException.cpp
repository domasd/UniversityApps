#include "stdafx.h"
#include "WarehouseNotSufficientForReportException.h"
#include <sstream>
#include <boost/format.hpp>
#include "StringHelper.hpp"


WarehouseNotSufficientForReportException::WarehouseNotSufficientForReportException(string missingPropertyName)
	: ReportGenerationException(Constants::WAREHOUSE_NOT_SUFFICIENT_FOR_REPORT)
{
	this->missingPropertyName = missingPropertyName;
}

WarehouseNotSufficientForReportException::~WarehouseNotSufficientForReportException()
{
}

const char* WarehouseNotSufficientForReportException::what() const noexcept 
{
	stringstream ss;
	auto missingPropertyText = boost::format("The property of warehouse - %1% is required for report to generate") % missingPropertyName;

	ss << missingPropertyText.str() << endl;
	ss << ReportGenerationException::what();

	return StringHelpers::StringHelper::ToChar(ss.str());
}

