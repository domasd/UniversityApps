#include "stdafx.h"
#include "Rack.h"
#include "Item.h"
#include <algorithm>
#include <sstream>
#include "RackImpl.hpp"
#include "CollectionHelperForType.hpp"
#include "DateHelper.hpp"
#include "RackIterator.h"
#include "Comparers.hpp"
using namespace WarehouseEntities;

Rack::Rack(int id, string name, string location)
{
	pImpl = new Impl();
	pImpl->_id = id;
	pImpl->_name = name;
	pImpl->_location = location;
	pImpl->_fullnessPercentage = 0;
}

Rack::~Rack()
{
	RemoveAllItems();
}

string Rack::GetLocation() const
{
	return pImpl->_location;
}

string Rack::GetName() const
{
	return pImpl->_name;
}

double Rack::GetFullnessPercentage() const
{
	return pImpl->_fullnessPercentage;
}

int Rack::GetId() const
{
	return pImpl->_id;
}

vector<Item*> Rack::GetItems() const
{
	return pImpl->_items;
}


void Rack::SetItems(const vector<Item*>& items)
{
	pImpl->_items = items;
}

void Rack::AddItem(Item* item)
{
	pImpl->_items.push_back(item);
}


void Rack::SetFullnessPercentage(double fullnessPercentage)
{
	if (fullnessPercentage < 0)
		throw invalid_argument(Constants::NEGATIVE_ARGUMENT);
	if (fullnessPercentage > 100)
		throw invalid_argument(Constants::BAD_INPUT_ARGUMENT);

	pImpl->_fullnessPercentage = fullnessPercentage;
}

void Rack::RemoveItem(Item* item)
{
	if (item == nullptr)
	{
		throw invalid_argument(Constants::NULLPTR_ARGUMENT);
	}

	auto it = find(pImpl->_items.begin(), pImpl->_items.end(), item);
	if (it != pImpl->_items.end())
		pImpl->_items.erase(it);
}

void Rack::RemoveItem(int id)
{
	try
	{
		CollectionHelperForType<Item*>::Remove(pImpl->_items, [=](const Item* item)-> bool { return item->GetId() == id; });
	}
	catch (...)
	{
		throw runtime_error("could not remove item. Id - " + id);
	}
}


void Rack::RemoveAllItems()
{
	pImpl->_items.clear();
}

string Rack::ToString() const
{
	stringstream ss;

	ss << pImpl->_name << "/"
		<< pImpl->_location << "/"
		<< pImpl->_fullnessPercentage;
	return ss.str();
}

Rack::iterator Rack::Begin()
{
	return iterator(pImpl->_items.begin(), *this);
}

Rack::iterator Rack::End()
{
	return iterator(pImpl->_items.end(), *this);
}

void Rack::Sort(bool asc)
{
	struct BestBeforeComparer
	{
		bool operator()(Item* first, Item* second) {
			auto diffValue = DateHelper::GetTimeDifference(first->GetBestBeforeDate(), second->GetBestBeforeDate());
			return diffValue < 0;
		}
	};

	BestBeforeComparer comparer;

	CollectionHelperForType<Item*>::Sort(pImpl->_items, comparer);
}

Item* Rack::Find(Item* itemToFind) const
{
	auto comparer = Comparers<Item*>::GetEqualityComparer(itemToFind);
	return CollectionHelperForType<Item*>::Find(pImpl->_items, comparer);
}
