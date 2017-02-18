#pragma once
#include "stdafx.h"
#include <functional>

template<typename T>
class Comparers
{
public:
	// where T type must have GetId() method and be a pointer type
	function<bool(const T&, const T&)> static GetIdComparer(bool asc)
	{
		return asc ? IdComparer(less<int>()) : IdComparer(greater<int>());
	}

	function<bool(const T&)> static GetEqualityComparer(T itemToFind)
	{
		return EqualityComparer(itemToFind);
	}

	class EqualityComparer : function<bool(const T&)>
	{
	public:
		EqualityComparer(T itemToFind) : _itemToFind(itemToFind)
		{

		}

		bool operator()(const T &itemInput) {
			return _itemToFind == itemInput;
		}

	private:
		T _itemToFind;
	};

	class IdComparer : function<bool(const T&, const T&)>
	{
		typedef function<bool(const int&, const int&)> intComparerType;

	public:
		IdComparer(intComparerType intComparer) : _intComparer(intComparer)
		{

		}

		bool operator()(const T &first, const T &second) {
			return _intComparer(first->GetId(), second->GetId());
		}

	private:
		intComparerType _intComparer;
	};
};




