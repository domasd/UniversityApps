#include "stdafx.h"
#include "Item.h"
#include "StringHelper.hpp"
#include "DateHelper.hpp"
#include "ItemImpl.hpp"

using namespace WarehouseEntities;


Item::Item(string name, tm bestBeforeDate, int quantity)
	: pImpl(new Impl())
{
	if (quantity <= 0) {
		throw std::invalid_argument(Constants::NEGATIVE_ZERO_ARGUMENT);
	}

	pImpl->_id = Impl::_counter;
	pImpl->_name = name;
	pImpl->_bestBeforeDate = bestBeforeDate;
	pImpl->_quantity = quantity;

	Impl::IncrementCounters();
}

Item::Item(
	string name,
	tm bestBeforeDate,
	int quantity,
	double unitMarketPrice,
	Category* category)
	: Item(name, bestBeforeDate, quantity)
{
	pImpl->_category = category;
	pImpl->_unitMarketPrice = unitMarketPrice;
}


Item::Item(const Item& rhs)
{
	this->pImpl = Impl::CopyImpl(*rhs.pImpl);
}

Item::~Item()
{
	//TODO move this to Impl class destructor after smart pointers refactoring
	pImpl->_liveObjectCount--;
	delete pImpl;
}

string Item::GetName() const
{
	return pImpl->_name;
}

tm Item::GetBestBeforeDate() const
{
	return pImpl->_bestBeforeDate;
}

int Item::GetId() const
{
	return pImpl->_id;
}

double  Item::GetUnitMarketPrice() const
{
	return pImpl->_unitMarketPrice;
}

int  Item::GetQuantity() const
{
	return pImpl->_quantity;
}

 Category*  Item::GetCategory() const
{
	return pImpl->_category;
}

void  Item::SetCategory(Category* category)
{
	if(category == nullptr)
		throw std::invalid_argument(Constants::NULLPTR_ARGUMENT);

	pImpl->_category = category;
}

void  Item::SetUnitMarketPrice(double price)
{
	if (price < 0)
		throw std::invalid_argument(Constants::NEGATIVE_ARGUMENT);
	pImpl->_unitMarketPrice = price;
}

void  Item::SetQuantity(int quantity)
{
	if (quantity <= 0)
		throw std::invalid_argument(Constants::NEGATIVE_ZERO_ARGUMENT);
	pImpl->_quantity = quantity;
}

int  Item::GetLiveObjectCount() {
	return  Impl::_liveObjectCount;
}

double  Item::CalculateWorth() const
{
	return pImpl->_unitMarketPrice * pImpl->_quantity;
}

double  Item::CalculateWorth(float rate) const
{
	return rate * CalculateWorth();
}

string  Item::ToString() const
{
	stringstream ss;

	string categoryName;

	if (pImpl->_category == nullptr) {
		categoryName = "-";
	}
	else {
		categoryName = pImpl->_category->GetName();
	}

	ss << pImpl->_id << "/"
		<< pImpl->_name << "/"
		<< StringHelpers::StringHelper::GetDateString(pImpl->_bestBeforeDate) << "/"
		<< pImpl->_quantity << "/"
		<< categoryName << "/"
		<< pImpl->_unitMarketPrice << "/"
		<< CalculateWorth();
	return ss.str();
}

bool WarehouseEntities::operator==(const Item& lhs, const  Item& rhs)
{
	auto rhsNestBeforeDate = rhs.GetBestBeforeDate();
	auto lhsNestBeforeDate = lhs.GetBestBeforeDate();

	if (lhs.GetName() == rhs.GetName()
		&& lhs.CalculateWorth() == rhs.CalculateWorth()
		&& DateHelper::GetTimeDifference(lhsNestBeforeDate, rhsNestBeforeDate) == 0
		&& lhs.GetCategory()->GetId() == rhs.GetCategory()->GetId()
		&& lhs.GetQuantity() == rhs.GetQuantity()
		&& lhs.GetUnitMarketPrice() == rhs.GetUnitMarketPrice()
		&& lhs.GetId() == rhs.GetId())
	{
		return true;
	}
	return false;
}

Item& Item::operator=(const Item &rhs) 
{
	if (this != &rhs)
	{
		delete this->pImpl;
		this->pImpl = Impl::CopyImpl(*rhs.pImpl);
	}
	return *this;
}

