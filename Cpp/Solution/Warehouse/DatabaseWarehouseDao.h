#pragma once
#include "IWarehouseDao.h"

class DatabaseWarehouseDao : public IWarehouseDao
{
public:
	DatabaseWarehouseDao();
	~DatabaseWarehouseDao();

	vector<Warehouse*> GetAllWarehouses(bool asc) override;
	vector<Item*> GetAllItems(bool asc) override;
	vector<Rack*> GetAllRacks(bool asc) override;

	void UpdateItem(Item* item) override;
};

