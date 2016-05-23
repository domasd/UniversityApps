create or replace PACKAGE BODY common AS
   PROCEDURE write_log(message IN NVARCHAR2, severity INT)
   IS 
    PRAGMA AUTONOMOUS_TRANSACTION;
    
    CURSOR sessionInfoCursor IS 
    SELECT machine, username
    FROM V$SESSION
    WHERE audsid = USERENV('SESSIONID');
    
    rec  sessionInfoCursor%ROWTYPE;
    nextId INT;
    errorCode varchar2(4000);
    errorMsg varchar2(4000);
   BEGIN
    
    OPEN sessionInfoCursor;
    FETCH sessionInfoCursor INTO rec;
    CLOSE sessionInfoCursor;  
    
    SELECT MAX(Id)+1 INTO nextId FROM LOG;
    IF nextId IS NULL THEN 
      nextId := 1;
    END IF;
    
    errorMsg := SQLERRM;
    errorCode := SQLCODE;
    
    INSERT INTO LOG VALUES(nextId,message,severity,SYSTIMESTAMP,
    rec.machine,errorCode, errorCode, rec.USERNAME, dbms_utility.format_error_stack,
    SYS.DBMS_UTILITY.FORMAT_ERROR_BACKTRACE);
    
    COMMIT;
    EXCEPTION 
      WHEN NO_DATA_FOUND THEN 
        COMMON.WRITE_LOG('Can not get machine!',1);
        RAISE;
       WHEN TOO_MANY_ROWS THEN
        COMMON.WRITE_LOG('select returned too many rows. Can not write to log',1);
        RAISE;
      WHEN OTHERS THEN ROLLBACK;
   END;
   
   PROCEDURE create_or_replace_table(tableName IN VARCHAR2, sqlScript IN VARCHAR2)
   IS 
     rowCount PLS_INTEGER;
   BEGIN
    SELECT COUNT(*) INTO rowCount FROM user_tables 
    WHERE table_name = upper(tableName);
         
     IF rowCount = 1 THEN
        EXECUTE IMMEDIATE 'drop table ' || upper(tableName || ' CASCADE CONSTRAINTS');
     END IF;
     
     EXECUTE IMMEDIATE sqlScript;
   END;
   
   PROCEDURE insert_to_table(tableName IN VARCHAR2, tableValues IN VARCHAR2)
   IS
   BEGIN
     EXECUTE IMMEDIATE 'INSERT INTO ' || tableName || ' VALUES(' || tableValues || ')';
   END;


end common;