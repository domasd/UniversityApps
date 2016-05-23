create or replace PACKAGE EXCEPTIONS_HANDLING AS 

  negative_parameter EXCEPTION;
  zero_parameter EXCEPTION;
  validation_exception EXCEPTION;
  
  PRAGMA EXCEPTION_INIT (negative_parameter, -20001 );
END EXCEPTIONS_HANDLING;