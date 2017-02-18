#pragma once
#include "stdafx.h"
#include "IReportsBuilder.h"

class ReportsBuilder : public IReportsBuilder
{
public:
	void SetWarehouses(const vector<Warehouse*> &warehouses) override;
	void SetWeeksBeforeToNoticeItemExpiration(const int &weeksBeforeToNotice) override;
	ResultType GenerateReports() override; /// GetResults() of builder pattern

private:
	int _weeksBeforeToNotice = 0;
	vector<Warehouse*> _warehouses;
};
