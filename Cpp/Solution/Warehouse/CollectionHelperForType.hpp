#pragma once
#pragma warning(suppress: 4244)
#include "stdafx.h"
#include <algorithm>
#include <functional>
#include <chrono>
#include <random>

template<typename T>
class CollectionHelperForType
{
public:
	T static Find(vector<T> collection, function<bool(const T&)> predicate)
	{
		auto retrievedItem = find_if(collection.begin(), collection.end(), predicate);
		return retrievedItem != collection.end() ? *retrievedItem : nullptr;
	}

	bool static Exists(vector<T> collection, T item)
	{
		return find(collection.begin(), collection.end(), item) != collection.end();
	}

	void static Sort(vector<T> &collection, function<bool(const T&, const T&)> comparisonFunc)
	{
		sort(collection.begin(), collection.end(), comparisonFunc);
	}

	vector<T> static SubVector(vector<T> &collection, function<bool(const T&)> predicate)
	{
		vector<T> subVector(collection.size());
		auto it = copy_if(collection.begin(), collection.end(), subVector.begin(), predicate);
		subVector.resize(distance(subVector.begin(), it));
		return subVector;
	}

	void static Shuffle(vector<T> &collection)
	{
		unsigned seed = chrono::system_clock::now().time_since_epoch().count();
		shuffle(collection.begin(), collection.end(), default_random_engine(seed));
	}

	void static Remove(vector<T> &collection, function<bool(const T&)> predicate)
	{
		collection.erase(
			remove_if(collection.begin(),
				collection.end(),
				predicate));
	}
};


