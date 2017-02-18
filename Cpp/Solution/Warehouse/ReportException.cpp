
#include "stdafx.h"
#include "ReportException.h"


ReportException::ReportException(string message) : exception(message.c_str())
{
}

ReportException::~ReportException()
{
}
