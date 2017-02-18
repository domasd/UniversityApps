#pragma once

#include "stdafx.h"
#include "pugixml-1.7/src/pugixml.hpp"

class ConfigurationObject 
{
private:
	static ConfigurationObject *instance;
	pugi::xml_document doc;
	pugi::xml_node addNodes;

	ConfigurationObject();
	~ConfigurationObject();

	string GetValueByKey(string key) const;

public:
	static ConfigurationObject* GetInstance();

	/// Configurations:
	int GetDefaultDaysBetweenMaintenance() const;
	int GetNoticeBeforeItemExpirationDateInWeeks() const;
	string GetLogPath() const;
};
