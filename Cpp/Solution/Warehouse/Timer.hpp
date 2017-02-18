#include "stdafx.h"
#include <boost/date_time.hpp>

using namespace boost::posix_time;

class Timer
{
private:
	bool _started = false;
	ptime _start;
public:
	void Start()
	{
		if (!_started)
		{
			_started = true;
			_start = microsec_clock::local_time();
		}
		else
		{
			throw logic_error(Constants::TIMER_ALREADY_STARTED);
		}
	}

	/// Returns duration in seconds
	long double Elapsed() const
	{
		if (_started)
		{
			time_duration diff = microsec_clock::local_time() - _start;
			return diff.total_milliseconds() / 1000.0;
		}
		throw logic_error(Constants::TIMER_NOT_STARTED);
	}
};
