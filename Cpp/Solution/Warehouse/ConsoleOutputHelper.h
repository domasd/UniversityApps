#pragma once

#include "TestDataWarehouseDao.h"

namespace ConsoleOutputHelper
{
	void PrintAllWarehouses(IWarehouseDao* dao);
	void PrintAllItems(IWarehouseDao* dao);
	void PrintAllRackItems(Rack &rack);
}