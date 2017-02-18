#pragma once
#include "Warehouse.h"
#include "Item.h"
#include <set>

using namespace WarehouseEntities;
///
/// Report class represents and generates an informational report for Warehouse
///

	class Report
	{
		int _numberOfItems = 0;
		int _numberOfCategories = 0;
		tm* _dateCreated;
		double _totalWorth = 0;
		double _fullnessPercentage = 0;
		bool _isMaintenanceNeeded = false;
		bool _isGenerated = false;

		Warehouse _warehouse;
		vector<Item*> _soonestExpiringItems;

	public:
		/// Constructor takes a warehouse 
		/** From corresponding warehouse information immidiately generates a report */
		Report(const Warehouse &warehouse, int noticeWeeksTillExpiration);
		/// Copy restriction constructor
		Report(const Report& that) = delete;
		~Report();

		tm* GetDateCreated() const;
		int GetNumberOfItems() const;
		int GetNumberOfCategories() const;
		double GetTotalWorth() const;
		/// Returns how much free space is left
		/// @return float >=0 and <=100 
		double GetFullnessPercentage() const;
		/// Returns if maintenance to a warehouse needed
		bool IsMaintenanceNeeded() const;
		/// @return Items which has the soonest ending expiration date
		vector<Item*> GetSoonestEndingBestBeforeItems() const;
		Warehouse GetWarehouse() const;

		string ToString();
	private:
		bool CheckIfReportIsGenerated() const;
		void LoopThroughRacksAndItems(const vector<Rack*> &racks, 
			set<Category> &uniqueCategories, 
			double &fullnessSum, 
			int noticeWeeksTillExpiration);
		void ValidateWarehouse(const Warehouse &warehouse);
	};


