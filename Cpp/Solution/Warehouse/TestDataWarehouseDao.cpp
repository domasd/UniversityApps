#include "stdafx.h"
#include "TestDataWarehouseDao.h"
#include "Category.h"
#include "DateHelper.hpp"
#include "CollectionHelperForType.hpp"
#include "Comparers.hpp"

TestDataWarehouseDao::TestDataWarehouseDao()
{
	CreateDummyRecords();
}


TestDataWarehouseDao::~TestDataWarehouseDao()
{
	delete computers;
	while (!items.empty()) delete items.back(), items.pop_back();
	while (!warehouses.empty()) delete warehouses.back(), warehouses.pop_back();
	while (!racks.empty()) delete racks.back(), racks.pop_back();
}

vector<Warehouse*> TestDataWarehouseDao::GetAllWarehouses(bool asc)
{
	auto comparer = Comparers<Warehouse*>::GetIdComparer(asc);
	CollectionHelperForType<Warehouse*>::Sort(warehouses, comparer);
	return warehouses;
}

vector<Item*> TestDataWarehouseDao::GetAllItems(bool asc)
{
	auto comparer = Comparers<Item*>::GetIdComparer(asc);
	CollectionHelperForType<Item*>::Sort(items, comparer);
	return items;
}

vector<Rack*> TestDataWarehouseDao::GetAllRacks(bool asc)
{
	auto comparer = Comparers<Rack*>::GetIdComparer(asc);
	CollectionHelperForType<Rack*>::Sort(racks, comparer);
	return racks;
}

void TestDataWarehouseDao::UpdateItem(Item* itemToUpdate)
{
	auto id = itemToUpdate->GetId();
	auto foundItem = CollectionHelperForType<Item*>::Find(items, [id](const Item* item) -> bool { return item->GetId() == id; });
	// update logic on DB goes here
	foundItem = itemToUpdate;
}

void TestDataWarehouseDao::CreateDummyRecords()
{
	//TODO change to safe pointers to free the resources
	warehouses = {
		new Warehouse(1,"Home","Namu g. 1, Vilnius, Lithuania"),
		new Warehouse(0,"Garage","Garazo g. 1, Vilnius, Lithuania"),
		new Warehouse(2,"Larder","Namu g. 1 basement, Vilnius, Lithuania")
	};

	Rack *rack = new Rack(0, "Shelf", "On the left corner");
	rack->SetFullnessPercentage(10);
	racks = {
		rack,
		new Rack(1, "Big wardrobe", "In front of doors")
	};

	computers = new Category(0, "Computer", false);

	items = {
		new Item("Kinescope monitor", DateHelper::CreateTm(2018,1,1)),
		new Item("PC - 1 gHz, 512 RAM, no GPU",
		DateHelper::CreateTm(2016,1,1),
		3,
		50.0,
		computers),
		new Item("Laptop - 4 gHz i7, 8096 mb RAM, GPU 2gb",
		DateHelper::CreateTm(2020,1,1),
		3,
		1000.0,
		computers)
	};

	// change data little bit
	warehouses[0]->SetLastMaintenanceDate(DateHelper::CreateTm(2014, 1, 1));

	// link entities
	warehouses[0]->AddRack(racks[0]);
	racks[0]->SetItems(items);
}
