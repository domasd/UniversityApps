#pragma once

#include "stdafx.h"
#include "Rack.h"

using namespace WarehouseEntities;

class Rack::Impl
{
	int _id = 0;
	string _name;
	string _location;
	/// sq. meters
	double _fullnessPercentage = 0; 

	vector<Item*> _items;

	friend class Rack;
	friend ostream& operator<<(ostream &output, const Rack& rack);
	friend istream& operator >> (istream &input, Rack& rack);
};