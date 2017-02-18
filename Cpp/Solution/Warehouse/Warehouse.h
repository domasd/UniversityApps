#pragma once
/// @Author ddziaugys
/// @date   October, 2016

#include "stdafx.h"
#include "Rack.h"

namespace WarehouseEntities
{
	///
	/// Warehouse class is a central object representing a physical warehouse
	/// It contains Racks with items
	///
	class Warehouse
	{
	private:
		class Impl;
		Impl * pImpl;
	public:
		Warehouse(int id, string name, string address);
		~Warehouse();

		int GetId() const;
		string GetName() const;
		string GetAddress() const;
		vector<Rack*> GetRacks() const;
		tm GetLastMaintenanceDate() const;
		int GetDayCountBetweenMaintenances() const;

		void SetDayCountBetweenMaintenances(int daycount);
		void SetLastMaintenanceDate(const tm &date);
		/// Adds a rack to warehouse
		void AddRack(Rack* rack);

		string ToString() const;

		friend ostream& operator<<(ostream &output, const Warehouse& warehouse);
		friend istream& operator >> (istream &input, Warehouse& warehouse);
	};
}

