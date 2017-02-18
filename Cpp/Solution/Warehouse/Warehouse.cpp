#include "stdafx.h"
#include "Warehouse.h"
#include <sstream>
#include "DateHelper.hpp"
#include "WarehouseImpl.hpp"
#include "ConfigurationObject.h"

using namespace WarehouseEntities;

Warehouse::Warehouse(int id, string name, string address)
{
	pImpl = new Impl();

	pImpl->_name = name;
	pImpl->_address = address;
	pImpl->_id = id;
	pImpl->DayCountBetweenMaintenances = ConfigurationObject::GetInstance()->GetDefaultDaysBetweenMaintenance();
	pImpl->_lastMaintenanceDate = DateHelper::GetCurrentTime();
}

Warehouse::~Warehouse()
{
}

int Warehouse::GetId() const
{
	return pImpl->_id;
}

string Warehouse::GetName() const
{
	return pImpl->_name;
}

string Warehouse::GetAddress() const
{
	return pImpl->_address;
}

vector<Rack*> Warehouse::GetRacks() const
{
	return pImpl->_racks;
}

tm Warehouse::GetLastMaintenanceDate() const
{
	return pImpl->_lastMaintenanceDate;
}

int Warehouse::GetDayCountBetweenMaintenances() const
{
	return pImpl->DayCountBetweenMaintenances;
}

void Warehouse::SetDayCountBetweenMaintenances(int daycount)
{
	if (daycount < 0)
	{
		throw std::invalid_argument(Constants::NEGATIVE_ARGUMENT);
	}

	pImpl->DayCountBetweenMaintenances = daycount;
}

void Warehouse::SetLastMaintenanceDate(const tm& date)
{
	pImpl->_lastMaintenanceDate = date;
}

void Warehouse::AddRack(Rack * rack)
{
	if (rack == nullptr)
		throw std::invalid_argument(Constants::NULLPTR_ARGUMENT);

	pImpl->_racks.push_back(rack);
}

string Warehouse::ToString() const
{
	stringstream ss;
	ss << pImpl->_name << "/"
		<< pImpl->_address << "/"
		<< pImpl->_racks.size();

	return ss.str();
}