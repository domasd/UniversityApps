#include "stdafx.h"
#include "DatabaseWarehouseDao.h"

// TODO implement MySQL Connector

DatabaseWarehouseDao::DatabaseWarehouseDao()
{
}

DatabaseWarehouseDao::~DatabaseWarehouseDao()
{
}

vector<Warehouse*> DatabaseWarehouseDao::GetAllWarehouses(bool asc)
{
	vector<Warehouse*> warehouses = { new Warehouse(1, "Test warehouse from DB", "Test address") }; // TODO delete and replace with real DB provider
	return warehouses;
}

vector<Item*> DatabaseWarehouseDao::GetAllItems(bool asc)
{
	throw exception("not implemented yet");
}

vector<Rack*> DatabaseWarehouseDao::GetAllRacks(bool asc)
{
	throw exception("not implemented yet");
}

void DatabaseWarehouseDao::UpdateItem(Item* item)
{
	throw exception("not implemented yet");
}
