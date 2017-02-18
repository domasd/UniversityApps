#include "stdafx.h"
#include <iostream>
#include "Warehouse.h"
#include "ConsoleOutputHelper.h"
#include "FileWriter.h"
#include "Item.h"
#include "DaoFactory.h"
#include "Report.h"
#include "ConfigurationObject.h"
#include "CollectionHelperForType.hpp"
#include "Timer.hpp"
#include <boost/format.hpp>
#include "IReportsBuilder.h"
#include "ReportsBuilder.h"

/// \mainpage Warehouse application
///
/// \section intro_sec Introduction
///
/// A desktop application for managing items in different warehouse, generating reports, etc.
/// Developed by ddziaugys.

using namespace std;
using namespace WarehouseEntities;

void OutputToFileAndConsole(string text);
vector<Report*> GenerateReports();

void main()
{
	OutputToFileAndConsole("Warehouse runner initialized");

	try
	{
		vector<Report*> reports = GenerateReports();

		auto reportsWithWarehousesWhichNeedsMaintenance = CollectionHelperForType<Report*>::SubVector(reports,
			[](Report* const& input)
		{
			return input->IsMaintenanceNeeded();
		});

	}
	catch (exception e)
	{
		OutputToFileAndConsole("unknown error occurred");
		OutputToFileAndConsole(e.what());
	}
}

void OutputToFileAndConsole(string text)
{
	FileWriter::WriteLog(text);
	clog << text << endl;
}


vector<Report*> GenerateReports()
{
	IWarehouseDao* dao = DaoFactory::CreateDao();
	IReportsBuilder* builder = new ReportsBuilder();

	auto warehouses = dao->GetAllWarehouses(true);
	builder->SetWarehouses(warehouses);

	auto weeksBeforeToNotice = ConfigurationObject::GetInstance()->GetNoticeBeforeItemExpirationDateInWeeks();
	builder->SetWeeksBeforeToNoticeItemExpiration(weeksBeforeToNotice);

	auto result = builder->GenerateReports();

	auto reports = get<0>(result);
	auto secondsItTook = get<1>(result);

	string timeResults = (boost::format("It took %1% seconds to generate reports") % secondsItTook).str();
	OutputToFileAndConsole(timeResults);

	delete dao;
	return reports;

}




