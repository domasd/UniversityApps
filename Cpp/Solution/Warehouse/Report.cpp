#include "stdafx.h"
#include "Report.h"
#include <sstream>
#include "DateHelper.hpp"
#include <set>
#include "ReportNotGeneratedException.h"
#include "WarehouseNotSufficientForReportException.h"

tm now = DateHelper::GetCurrentTime();

bool CheckIfMaintenanceNeeded(tm requiredMaintenanceDate)
{
	if (DateHelper::GetTimeDifference(requiredMaintenanceDate, now) <= 0)
	{
		return true;
	}
	return false;
}

Report::Report(const Warehouse& warehouse, int noticeWeeksTillExpiration)
	: _warehouse(warehouse)
{
	ValidateWarehouse(warehouse);

	auto racks = warehouse.GetRacks();
	set<Category> uniqueCategories;
	double fullnessSum = 0;

	LoopThroughRacksAndItems(racks, uniqueCategories, fullnessSum, noticeWeeksTillExpiration);

	auto daysBetweenMaintenance = warehouse.GetDayCountBetweenMaintenances();
	auto lastMaintenanceDate = warehouse.GetLastMaintenanceDate();
	auto requiredMaintenanceDate = DateHelper::DatePlusDays(&lastMaintenanceDate, daysBetweenMaintenance);
	this->_isMaintenanceNeeded = CheckIfMaintenanceNeeded(requiredMaintenanceDate);

	this->_numberOfCategories = static_cast<int>(uniqueCategories.size());
	this->_fullnessPercentage = racks.size() == 0 ? 0 : fullnessSum / racks.size();
	this->_dateCreated = &now;

	this->_isGenerated = true;
}

Report::~Report()
{
}

tm* Report::GetDateCreated() const
{
	CheckIfReportIsGenerated();
	return _dateCreated;
}

int Report::GetNumberOfItems() const
{
	CheckIfReportIsGenerated();
	return _numberOfItems;
}

int Report::GetNumberOfCategories() const
{
	CheckIfReportIsGenerated();
	return _numberOfCategories;
}

double Report::GetTotalWorth() const
{
	CheckIfReportIsGenerated();
	return _totalWorth;
}

double Report::GetFullnessPercentage() const
{
	CheckIfReportIsGenerated();
	return _fullnessPercentage;
}

bool Report::IsMaintenanceNeeded() const
{
	CheckIfReportIsGenerated();
	return _isMaintenanceNeeded;
}

vector<Item*> Report::GetSoonestEndingBestBeforeItems() const
{
	CheckIfReportIsGenerated();
	return _soonestExpiringItems;
}

Warehouse Report::GetWarehouse() const
{
	return _warehouse;
}

string Report::ToString()
{
	stringstream ss;

	ss << _numberOfItems << "/"
		<< _numberOfCategories << "/"
		<< _totalWorth << "/"
		<< _fullnessPercentage << "/"
		<< _isMaintenanceNeeded;
	return ss.str();
}

bool Report::CheckIfReportIsGenerated() const
{
	if (_isGenerated)
	{
		return true;
	}
	throw ReportNotGeneratedException();
}

bool CheckIfItemExpiresSoon(tm expirationDate, int weeksTillExpiration)
{
	auto soonestEndingProductsDate = DateHelper::DatePlusWeeks(&now, weeksTillExpiration);
	return DateHelper::GetTimeDifference(expirationDate, soonestEndingProductsDate) <= 0;
}

void Report::LoopThroughRacksAndItems(const vector<Rack*>& racks, set<Category>& uniqueCategories, double& fullnessSum, int noticeWeeksTillExpiration)
{
	for (auto const& rack : racks)
	{
		fullnessSum += rack->GetFullnessPercentage();

		auto items = rack->GetItems();
		_numberOfItems += static_cast<int>(items.size());
		for (auto const& item : items)
		{
			auto category = item->GetCategory();
			if (category != nullptr)
			{
				uniqueCategories.insert(*category);
			}

			if (CheckIfItemExpiresSoon(item->GetBestBeforeDate(), noticeWeeksTillExpiration))
			{
				_soonestExpiringItems.push_back(item);
			}

			this->_totalWorth += item->GetUnitMarketPrice();
		}
	}
}

void Report::ValidateWarehouse(const Warehouse& warehouse)
{
	if (warehouse.GetName().empty())
		throw WarehouseNotSufficientForReportException("Name");

	if (warehouse.GetDayCountBetweenMaintenances() == 0)
		throw WarehouseNotSufficientForReportException("DayCountBetweenMaintenances");
}
