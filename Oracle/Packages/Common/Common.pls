create or replace PACKAGE common AS -- declaration
  PROCEDURE write_log(message IN NVARCHAR2, severity INT);
  PROCEDURE create_or_replace_table(tableName IN VARCHAR2, sqlScript IN VARCHAR2);
  PROCEDURE insert_to_table(tableName IN VARCHAR2, tableValues IN VARCHAR2);
END common;