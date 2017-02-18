#pragma once
/*
	TODO: This class will be replaced as a real DB data provider
*/

#include "IWarehouseDao.h"

class TestDataWarehouseDao : public IWarehouseDao
{
	vector<Warehouse*> warehouses;
	vector <Rack*> racks;
	vector<Item*> items;
	Category* computers;

public:
	vector<Warehouse*> GetAllWarehouses(bool asc) override;
	vector<Item*> GetAllItems(bool asc) override;
	vector<Rack*> GetAllRacks(bool asc) override;

	void UpdateItem(Item* item) override;

	TestDataWarehouseDao();
	~TestDataWarehouseDao();

private:
	void CreateDummyRecords();
};

