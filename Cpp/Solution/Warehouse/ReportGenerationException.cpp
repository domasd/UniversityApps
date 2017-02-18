#include "stdafx.h"
#include "ReportGenerationException.h"


ReportGenerationException::ReportGenerationException() : ReportException(Constants::REPORT_GENERATION_ERROR)
{
}

ReportGenerationException::ReportGenerationException(string message) : ReportException(message)
{
}

ReportGenerationException::~ReportGenerationException()
{
}
