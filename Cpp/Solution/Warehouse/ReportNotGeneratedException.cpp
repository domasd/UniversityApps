#include "stdafx.h"
#include "ReportNotGeneratedException.h"


ReportNotGeneratedException::ReportNotGeneratedException() : ReportException(Constants::REPORT_NOT_GENERATED)
{
}


ReportNotGeneratedException::~ReportNotGeneratedException()
{
}
