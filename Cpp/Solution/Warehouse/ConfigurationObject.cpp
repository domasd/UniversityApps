#include "stdafx.h"
#include "StringHelper.hpp"
#include "pugixml-1.7/src/pugixml.hpp"
#include "ConfigurationObject.h"

ConfigurationObject * ConfigurationObject::instance = nullptr;

ConfigurationObject::ConfigurationObject()
{
	auto result = doc.load_file("app.config");
}

ConfigurationObject::~ConfigurationObject()
{
	delete instance;
}

ConfigurationObject* ConfigurationObject::GetInstance()
{
	if (!instance)
		instance = new ConfigurationObject();
	return instance;
}

int ConfigurationObject::GetDefaultDaysBetweenMaintenance() const
{
	auto value = GetValueByKey("DefaultDayCountBetweenWarehouseMaintenance");
	return StringHelpers::StringHelper::ToInt(value);
}

int ConfigurationObject::GetNoticeBeforeItemExpirationDateInWeeks() const
{
	auto value = GetValueByKey("NoticeBeforeItemExpirationDateInWeeks");
	return StringHelpers::StringHelper::ToInt(value);
}

string ConfigurationObject::GetLogPath() const
{
	return GetValueByKey("LogPath");
}

string ConfigurationObject::GetValueByKey(string key) const
{
	auto xpath = "//add[@key=\"" + key + "\"]";
	return doc.select_node(xpath.c_str())
		.node().attribute("value").value();
}
