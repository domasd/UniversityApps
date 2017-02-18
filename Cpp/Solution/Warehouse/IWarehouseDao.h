#pragma once

#include "Item.h"
#include "Warehouse.h"

using namespace WarehouseEntities;

class IWarehouseDao
{
// READ
public:
	virtual ~IWarehouseDao()
	{
	}

	virtual vector<Warehouse*> GetAllWarehouses(bool asc = true) = 0;
	virtual vector<Item*> GetAllItems(bool asc = true) = 0;
	virtual vector<Rack*> GetAllRacks(bool asc = true) = 0;

// UPDATE
	virtual void UpdateItem(Item* item) = 0;
// CREATE
// DELETE
//private:
//	IWarehouseDao(const IWarehouseDao& input);
};

