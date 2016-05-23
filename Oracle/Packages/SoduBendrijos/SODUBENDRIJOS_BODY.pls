create or replace PACKAGE BODY sodubendrijos AS   
  PROCEDURE INSERT_GENERATED_VALUES
  IS
    bendrijaKodas number(5);
    bendrijaKodas2 number(5);
    sklypasNr number(5);
    sklypasNr2 number(5);
    sodininkasAk number(5);
  BEGIN
    bendrijaKodas := dbms_random.value(1,99999);
    bendrijaKodas2 := dbms_random.value(1,99999);

    INSERT INTO BENDRIJA VALUES(bendrijaKodas,
    'Bendrija' || bendrijaKodas,
    CURRENT_TIMESTAMP,
    dbms_random.value(1,99999),
    'PirmininkoVardas' || bendrijaKodas,
    'PirmininkoPavarde' || bendrijaKodas,
    'PirmininkoTelnr' || bendrijaKodas,
    CURRENT_TIMESTAMP);
    
    INSERT INTO BENDRIJA VALUES(bendrijaKodas2,
    'Bendrija' || bendrijaKodas,
    CURRENT_TIMESTAMP,
    dbms_random.value(1,99999),
    'PirmininkoVardas' || bendrijaKodas,
    'PirmininkoPavarde' || bendrijaKodas,
    'PirmininkoTelnr' || bendrijaKodas,
    CURRENT_TIMESTAMP);
    
    sklypasNr := dbms_random.value(1,99999);
    sklypasNr2 := dbms_random.value(1,99999);

    INSERT INTO SKLYPAS VALUES(sklypasNr,
    bendrijaKodas,
    'SklypasAdresas' || sklypasNr,
    dbms_random.value(1,200));
    
    INSERT INTO SKLYPAS VALUES(sklypasNr2,
    bendrijaKodas2,
    'SklypasAdresas' || sklypasNr,
    dbms_random.value(1,200));
    
    sodininkasAk := dbms_random.value(1,99999);
    INSERT INTO SODININKAS VALUES(sodininkasAk,
        'SodininkasVardas' || sodininkasAk,
        'SodininkasPavarde' || sodininkasAk,
        'SodidinkasTelNr' || sodininkasAk,
        'SodininkasMiestas' || sodininkasAk,
        CURRENT_TIMESTAMP);
    
    IF round(dbms_random.value(1,2)) = 1 THEN
      EXECUTE IMMEDIATE 'INSERT INTO Paskyrimas VALUES(:bendrijaKodas, :sodininkasAk)'
      USING bendrijaKodas, sodininkasAk;
    END IF;
    
    IF round(dbms_random.value(1,2)) = 1 THEN
      EXECUTE IMMEDIATE 'INSERT INTO Prieziura VALUES(:sklypasNr, :sodininkasAk)'
      USING sklypasNr, sodininkasAk;
    END IF;
    
   IF round(dbms_random.value(1,2)) = 1 THEN
      EXECUTE IMMEDIATE 'INSERT INTO Prieziura VALUES(:sklypasNr2, :sodininkasAk)'
      USING sklypasNr2, sodininkasAk;
    END IF;
    
  END INSERT_GENERATED_VALUES;
  
  PROCEDURE INSERT_VALID_PRIEZIURA(sodininko_ak IN NUMBER, sklypo_nr IN NUMBER)
  IS 
  BEGIN
    INSERT INTO Prieziura
    SELECT sklypo_nr,sodininko_ak
    FROM dual 
    WHERE EXISTS (SELECT *
                  FROM Sklypas s
                  JOIN Paskyrimas p ON p.sodininko_ak = sodininko_ak
                  JOIN Bendrija b ON b.kodas = p.bendrijos_kodas
                  WHERE s.nr = sklypo_nr AND b.kodas = s.bendrijos_kodas
                  );
                  
    IF SQL%ROWCOUNT = 0 THEN RAISE exceptions_handling.validation_exception; END IF;
  END INSERT_VALID_PRIEZIURA;
  
  PROCEDURE VALIDATE_PRIEZIURA
  IS
   CURSOR sodininkas_cursor IS
    SELECT *
    FROM Sodininkas sod;
    TYPE cursor_type IS REF CURSOR;
    dynamic_cursor cursor_type;
    
    sodininkas_row sodininkas%rowtype;
    fullPlotas number(10);
    
    sodininko_ak SODININKAS.AK%type;
    sklypo_nr SKLYPAS.NR%type;
    plotas SKLYPAS.PLOTAS%type;
  BEGIN
    OPEN sodininkas_cursor;
    LOOP
      FETCH sodininkas_cursor INTO sodininkas_row;
      
      fullPlotas := 0;
      
      OPEN dynamic_cursor FOR
        SELECT priez.sklypo_nr, priez.sodininko_ak
        FROM Prieziura priez
        JOIN Sklypas sk ON sk.NR = priez.SKLYPO_NR
        WHERE priez.SODININKO_AK = sodininkas_row.ak;
        
        FETCH dynamic_cursor INTO sklypo_nr, sodininko_ak, plotas;
        LOOP
          IF fullPlotas < 100 THEN
            fullPlotas := fullPlotas + plotas;
          ELSE 
            INSERT INTO INVALID_PRIEZIURA VALUES(sklypo_nr,sodininko_ak,plotas);
          END IF;
        END LOOP;
      CLOSE dynamic_cursor;
    END LOOP;
  END VALIDATE_PRIEZIURA;
  
    
  PROCEDURE DELETE_INVALID_PRIEZIURA
  IS
    CURSOR invalid_prieziura_cursor IS
    SELECT *
    FROM INVALID_PRIEZIURA;
    
    invalid_prieziura_row invalid_prieziura%rowtype;
  BEGIN
  
  OPEN invalid_prieziura_cursor ;
  LOOP
    FETCH invalid_prieziura_cursor INTO invalid_prieziura_row;
    
    DELETE FROM Prieziura p
    WHERE p.SKLYPO_NR = invalid_prieziura_row.SKLYPO_NR 
      AND p.SODININKO_AK = invalid_prieziura_row.SODININKO_AK;
  END LOOP;
  
  DELETE FROM INVALID_PRIEZIURA;
  
  DBMS_OUTPUT.PUT_LINE ('Deleted '|| SQL%ROWCOUNT || ' rows from Prieziura');

  END DELETE_INVALID_PRIEZIURA;
  
end sodubendrijos;