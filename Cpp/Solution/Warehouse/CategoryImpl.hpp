#pragma once

#include "stdafx.h"
#include "Category.h"

using namespace WarehouseEntities;

class Category::Impl {
public:
	Category::Impl(const string& cs, int id, bool is_hazardous)
		: _name(cs),
		_id(id),
		_isHazardous(is_hazardous)
	{
	}

private:
	std::string _name;
	int _id;
	bool _isHazardous;
	friend class Category;
	friend bool operator==(const Category& lhs, const Category& rhs); 
	friend istream& WarehouseEntities::operator >> (istream &input, Category& category);
	friend ostream& WarehouseEntities::operator<<(ostream &output, const Category& category);
};
