#pragma once

#include "stdafx.h"
#include <sstream>

typedef vector<string> stringVector;

namespace StringHelpers
{
	class StringHelper
	{
	public:
		static string GetDateString(tm date)
		{
			stringstream ss;
			ss << date.tm_year << "-" << date.tm_mon << "-" << date.tm_mday;
			return ss.str();
		}

		static  stringVector split(const std::string &s, char delim) {
			stringVector elems;
			splitToVector(s, delim, elems);
			return elems;
		}

		static int ToInt(string input)
		{
			return stoi(input);
		}

		static char* ToChar(string input)
		{
			return _strdup(input.c_str());
		}

	private:
		static void splitToVector(const string &s, char delim, stringVector &elems) {
			stringstream ss;
			ss.str(s);
			string item;
			while (std::getline(ss, item, delim)) {
				elems.push_back(item);
			}
		}
	};
};

