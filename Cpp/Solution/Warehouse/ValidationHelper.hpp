#pragma once
#include "stdafx.h"
#include "Constants.h"

namespace ValidationHelpers
{
	class NumberValidationHelpers
	{
	public:
		/// Validates double >0
		static void ValidatePositiveNumber(double numberToValidate)
		{
			int numberToValidateInt = static_cast<int>(numberToValidate);
			ValidatePositiveNumber(numberToValidateInt);
		};

		/// Validates int >0
		static void ValidatePositiveNumber(int numberToValidate)
		{
			if (numberToValidate <= 0)
				throw std::invalid_argument(Constants::NEGATIVE_ZERO_ARGUMENT);
		};
	};

}
