#include "stdafx.h"
#include "ElectronicItem.h"
#include "DateHelper.hpp"
#include <sstream>

using namespace WarehouseEntities;

ElectronicItem::ElectronicItem(string name, tm bestBeforeDate, int quantity, int watts)
: Item(name,bestBeforeDate, quantity), _watts(watts)
{
}


ElectronicItem::~ElectronicItem()
{
}

int ElectronicItem::GetDailyEnergyConsumption() const
{
	return _watts * 24;
}

string ElectronicItem::ToString() const
{
	auto baseToString = Item::ToString();
	stringstream ss;
	ss << Constants::ELECTRONIC_ITEM << "/"
		<< GetDailyEnergyConsumption() << "/"
		<< baseToString;

	return ss.str();
}

