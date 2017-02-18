#include "stdafx.h"
#include "ConsoleOutputHelper.h"
#include <iostream>
#include "RackIterator.h"

void ConsoleOutputHelper::PrintAllWarehouses(IWarehouseDao* dao)
{
	auto warehouses = dao->GetAllWarehouses(true);

	cout << "Warehouse list: " << endl;
	vector<Warehouse*>::const_iterator const_iterator;
	for (const_iterator = warehouses.begin(); const_iterator != warehouses.end(); ++const_iterator)
	{
		auto index = const_iterator - warehouses.begin() + 1;
		cout << index << ". " << (*const_iterator)->ToString() << endl;
	}
}

void ConsoleOutputHelper::PrintAllItems(IWarehouseDao* dao)
{
	auto items = dao->GetAllItems(true);

	cout << "Items list: " << endl;
	vector<Item*>::const_iterator const_iterator;
	for (const_iterator = items.begin(); const_iterator != items.end(); ++const_iterator)
	{
		auto index = const_iterator - items.begin() + 1;
		cout << index << ". " << (*const_iterator)->ToString() << endl;
	}
}

void ConsoleOutputHelper::PrintAllRackItems(Rack &rack)
{
	for(auto it = rack.Begin(); it != rack.End(); ++it)
	{
		cout << (*it)->ToString() << endl;
	}
}