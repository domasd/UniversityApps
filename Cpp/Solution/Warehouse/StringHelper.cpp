#include "stdafx.h"
#include "StringHelper.h"
#include <sstream>

string StringHelper::GetDateString(tm date)
{
	stringstream ss;
	ss << date.tm_year << "-" << date.tm_mon << "-" << date.tm_mday;
	return ss.str();
}
