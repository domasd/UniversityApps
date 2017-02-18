#pragma once
/// @Author ddziaugys
/// @date   October, 2016

#include "stdafx.h"
#include "Category.h"

namespace WarehouseEntities
{
	///
	/// Item class represents a physical item in Rack
	///
	class Item
	{
	private:
		class Impl;
		Impl* pImpl;
	public:
		/// General constructor. 
		/** Quantity is optional. */
		Item(string name,
			tm bestBeforeDate,
			int quantity = 1);

		/// Specific constructor which calls generic one. 
		/** All fields are mandatory. */
		Item(string name,
			tm bestBeforeDate,
			int quantity,
			double unitMarketPrice,
			Category* category);

		/// Copy constructor overload
		Item(const Item & rhs);

		virtual ~Item();

		string GetName() const;
		tm GetBestBeforeDate() const;
		int GetId() const;
		double GetUnitMarketPrice() const;
		int GetQuantity() const;
		Category* GetCategory() const;

		void SetUnitMarketPrice(double price);
		void SetQuantity(int quantity);
		void SetCategory(Category* category);

		/// Gets the current number of alive Item objects
		static int GetLiveObjectCount();

		/// Calculated worth based on quantity and market price
		virtual double CalculateWorth() const;
		/// Calculated worth based on quantity, market price and given currency rate
		/// @param[in] rate The currency rate.
		double CalculateWorth(float rate) const;

		virtual string ToString() const;

		/// Overloads = operator for item objects
		Item& Item::operator=(const Item &rhs);

		/// Overloads == operator for item objects
		friend bool operator==(const Item& lhs, const Item& rhs);

		friend ostream& operator<<(ostream &output, const Item& item);
	};
}
