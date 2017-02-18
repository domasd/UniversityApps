#include "stdafx.h"
#include "DaoFactory.h"
#include "TestDataWarehouseDao.h"


DaoFactory::DaoFactory()
{
}


DaoFactory::~DaoFactory()
{
}

IWarehouseDao* DaoFactory::CreateDao()
{
	return new TestDataWarehouseDao();
}
