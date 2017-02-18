#pragma once
#include "IWarehouseDao.h"

class DaoFactory
{
public:
	DaoFactory();
	~DaoFactory();

	static IWarehouseDao* CreateDao();
};

