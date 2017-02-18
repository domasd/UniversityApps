#include "stdafx.h"
#include "Report.h"
#include "Timer.hpp"
#include "ReportsBuilder.h"
#include <boost/thread.hpp>

void ReportsBuilder::SetWarehouses(const vector<Warehouse*> &warehouses)
{
	_warehouses = warehouses;
}

void ReportsBuilder::SetWeeksBeforeToNoticeItemExpiration(const int &weeksBeforeToNotice)
{
	_weeksBeforeToNotice = weeksBeforeToNotice;
}


void CreateReportWithPromise(boost::promise<Report*> &promise, Warehouse* warehouse, int &weeksBeforeToNotice)
{
	promise.set_value(new Report(*warehouse, weeksBeforeToNotice));
}

/// GetResults() of builder pattern
ResultType ReportsBuilder::GenerateReports()
{
	Timer timer;
	timer.Start();

	if (_weeksBeforeToNotice > 0 && !_warehouses.empty())
	{
		vector<Report*> reports;
		vector<boost::promise<Report*>*> reportsPromises;


		for (int i = 0; i<_warehouses.size(); i++)
		{
			boost::promise<Report*> *reportPromise = new boost::promise<Report*>();
			reportsPromises.push_back(reportPromise);
			boost::thread *thread = new boost::thread{
				CreateReportWithPromise,
				boost::ref(*reportPromise),
				boost::ref(_warehouses[i]),
				boost::ref(_weeksBeforeToNotice)
			};
		}

		for(auto reportPromise : reportsPromises)
		{
			// wait on threads
			auto future = reportPromise->get_future();
			auto value = future.get();
			reports.push_back(value);
		}

		return make_tuple(reports, timer.Elapsed());
	}
	throw logic_error(Constants::NOT_SUFFICIENT_ARGUMENTS);

}


