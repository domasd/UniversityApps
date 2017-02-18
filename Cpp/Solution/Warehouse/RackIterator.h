#pragma once
#include "stdafx.h"
#include "Rack.h"

using namespace WarehouseEntities;

class Rack::iterator
{
public:
	iterator(vector<Item*>::iterator pos, Rack &owner);
	~iterator();
	bool operator==(const iterator& rhs);
	bool operator!=(const iterator& rhs);
	void operator++();
	Item* operator*();
private:
	vector<Item*>::iterator _pos;
	Rack &_owner;
};