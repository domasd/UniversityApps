#pragma once
#include <ctime>

namespace DateHelper
{
	inline tm GetCurrentTime()
	{
		time_t time = std::time(0);
		tm now;
		localtime_s(&now, &time);
		return now;
	}

	inline tm DatePlusDays(struct tm* dateInput, int days)
	{
		const time_t ONE_DAY = 24 * 60 * 60;

		time_t date_seconds = mktime(dateInput) + days * ONE_DAY;

		tm dateSubtractedWeeks;
		localtime_s(&dateSubtractedWeeks, &date_seconds);
		return dateSubtractedWeeks;
	}

	inline tm DatePlusWeeks(struct tm* dateInput, int weeks)
	{
		return DatePlusDays(dateInput, weeks * 7);
	}

	inline tm CreateTm(int year, int month, int day)
	{
		tm date;
		date.tm_year = year-1900; // years since 1900
		date.tm_mon = month-1; // 0-indexed
		date.tm_mday = day; 
		date.tm_hour = 0;
		date.tm_min = 0;
		date.tm_sec = 0;
		return date;
	}

	/// Gets time difference in seconds
	inline double GetTimeDifference(const tm & first_tm, const tm & second_tm)
	{
		tm first = first_tm;
		tm second = second_tm;
		return difftime(mktime(&first), mktime(&second));
	}
}
