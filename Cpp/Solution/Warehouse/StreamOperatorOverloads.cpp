#include "stdafx.h"
#include "Category.h"
#include "Item.h"
#include "Rack.h"
#include "WarehouseImpl.hpp"

ostream& WarehouseEntities::operator<<(ostream& output, const Item& item)
{
	output << item.ToString() << endl;
	return output;
}

ostream& WarehouseEntities::operator<<(ostream &output, const Category& category)
{
	output << category.ToString() << endl;
	return output;
}

ostream& WarehouseEntities::operator<<(ostream &output, const Rack& rack) {
	output << rack.ToString() << endl;
	return output;
}

ostream& WarehouseEntities::operator<<(ostream &output, const Warehouse& warehouse) {
	output << warehouse.ToString() << endl;
	return output;
}