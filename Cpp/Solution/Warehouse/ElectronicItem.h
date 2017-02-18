#pragma once
#include "stdafx.h"
#include "Item.h"

namespace  WarehouseEntities
{
	class ElectronicItem : public Item
	{
	private:
		int _watts;

		using Item::GetBestBeforeDate;

	public:
		ElectronicItem(string name, tm bestBeforeDate, int quantity, int watts);
		~ElectronicItem();

		int GetDailyEnergyConsumption() const;

		string ToString() const override;
	};
}