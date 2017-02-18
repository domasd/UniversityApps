// iterator implementation
#include "stdafx.h"
#include "RackIterator.h"

using namespace WarehouseEntities;

Rack::iterator::iterator(vector<Item*>::iterator pos, Rack& owner)
	: _pos(pos), _owner(owner)
{

}

Rack::iterator::~iterator()
{
}

bool Rack::iterator::operator==(const iterator& rhs)
{
	return _pos == rhs._pos;
}

bool Rack::iterator::operator!=(const iterator& rhs)
{
	return _pos != rhs._pos;
}

void Rack::iterator::operator++()
{
	++_pos;
}

Item* Rack::iterator::operator*()
{
	return *_pos;
}