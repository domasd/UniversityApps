#pragma once
#include "stdafx.h"
#include "Report.h"

typedef tuple<vector<Report*>, long double> ResultType;
class IReportsBuilder
{
public:
	virtual ~IReportsBuilder()
	{
	}

	virtual void SetWarehouses(const vector<Warehouse*> &warehouses) = 0;
	virtual void SetWeeksBeforeToNoticeItemExpiration(const int &weeksBeforeToNotice) = 0;

	/// GetResults() of builder pattern
	virtual ResultType GenerateReports() = 0;
};
