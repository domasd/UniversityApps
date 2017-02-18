#pragma once

/// @Author ddziaugys
/// @date   October, 2016

#include "stdafx.h"
#include "Item.h"

namespace WarehouseEntities {
	///
	/// Rack class represents a location for Item objects
	///
	class Rack
	{
		class iterator;

	private:
		class Impl;
		Impl *pImpl;

	public:
		/// Constructor with required fields for Rack
		Rack(int id, string name, string location);
		~Rack();

		string GetLocation() const;
		string GetName() const;
		double GetFullnessPercentage() const;
		int GetId() const;
		vector<Item*> GetItems() const;

		/// Sets the whole new collection as containing items.
		/// Vipes out current collection.
		void SetItems(const vector<Item*> &items);

		void AddItem(Item* item);
		/// @param [in] item Item pointer to remove from list
		void RemoveItem(Item* item);
		/// @param [in] id Item id to remove from list
		void RemoveItem(int id);
		/// Removes all items from a Rack
		void RemoveAllItems();
		/// Sets how much space is left in a rack
		void SetFullnessPercentage(double fullnessPercentage);

		string ToString() const;

		iterator Begin();
		iterator End();
		void Sort(bool asc);
		Item* Find(Item* itemToFind) const;

		friend ostream& operator<<(ostream &output, const Rack& rack);
		friend istream& operator >> (istream &input, Rack& rack);
	};
}

