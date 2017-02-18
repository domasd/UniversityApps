#pragma once

#include "stdafx.h"
#include "Item.h"

using namespace WarehouseEntities;

class Item::Impl {
	int _id = 0;
	string _name;
	tm _bestBeforeDate;
	Category* _category = nullptr;
	double _unitMarketPrice = 0;
	int _quantity = 0;

	static int _counter; ///< static field, for generating unique Id
	static int _liveObjectCount; ///< static field, for counting alive objects

	friend class Item;
	friend istream& WarehouseEntities::operator >> (istream &input, Category& category);
	friend ostream& WarehouseEntities::operator<<(ostream &output, const Category& category);


	void static IncrementCounters()
	{
		_counter++;
		_liveObjectCount++;
	}

	static Impl* CopyImpl(const Impl& rhs)
	{
		auto pImpl = new Impl(rhs);
		pImpl->_id = _counter;
		IncrementCounters();
		return pImpl;
	}
};

int Item::Impl::_counter = 1;
int Item::Impl::_liveObjectCount = 0;

