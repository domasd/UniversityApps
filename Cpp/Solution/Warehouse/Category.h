#pragma once
/// @Author ddziaugys
/// @date   October, 2016

#include "stdafx.h"

namespace WarehouseEntities
{
	///
	/// Category class is a group divider for items
	///
	class Category
	{
	private:
		class Impl;
		Impl* pImpl;
	public:
		Category(int id, string name, bool isHazardous);
		~Category();

		string GetName() const;
		int GetId() const;
		bool GetHazardouness() const;

		string ToString() const;

		Category& Category::operator=(Category rhs);

		friend bool operator==(const Category& lhs, const Category& rhs);
		friend bool operator<(const Category& lhs, const Category& rhs);
		friend ostream& operator<<(ostream &output, const Category& category);
		friend istream& operator >> (istream &input, Category& category);
	};
}
