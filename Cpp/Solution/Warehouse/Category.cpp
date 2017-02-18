#include "stdafx.h"
#include "Category.h"
#include <sstream>
#include "CategoryImpl.hpp"

using namespace WarehouseEntities;

Category::Category(int id, string name, bool isHazardous)
	: pImpl(new Impl(name, id, isHazardous)) { 
}

Category::~Category()
{
}

string Category::GetName() const
{
	return pImpl->_name;
}
int Category::GetId() const
{
	return pImpl->_id;
}
bool Category::GetHazardouness() const
{
	return pImpl->_isHazardous;
}

string Category::ToString() const
{
	stringstream ss;

	ss << pImpl->_name << "/"
		<< pImpl->_isHazardous;
	return ss.str();
}

Category& Category::operator=(Category rhs)
{
	this->pImpl->_id = rhs.GetId();
	this->pImpl->_isHazardous = rhs.GetHazardouness();
	this->pImpl->_name = rhs.GetName();
	return *this;
}


bool WarehouseEntities::operator==(const Category& lhs, const Category& rhs)
{
	if (lhs.GetId() == rhs.GetId() 
		&& lhs.GetName() == rhs.GetName()
			&& lhs.GetHazardouness() == rhs.GetHazardouness())

	{
		return true;
	}
	return false;
}

bool WarehouseEntities::operator<(const Category& lhs, const Category& rhs)
{
	return lhs.GetId() < rhs.GetId();
}