#pragma once

#include "stdafx.h"
#include "Warehouse.h"

using namespace WarehouseEntities;

class Warehouse::Impl
{
private:
	int _id = 0;
	string _name, _address;

	vector<Rack*> _racks;
	/// Last time of maintenance
	/// clearing, cleaning, revisioning, etc.
	tm _lastMaintenanceDate; 
	int DayCountBetweenMaintenances = 0;

	friend class Warehouse;
	friend ostream& operator<<(ostream &output, const Warehouse& warehouse);
	friend istream& operator >> (istream &input, Warehouse& warehouse);
};