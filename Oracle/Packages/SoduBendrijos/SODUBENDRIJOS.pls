create or replace PACKAGE SODUBENDRIJOS AS 

  bendrijaTableName varchar2(255) := 'bendrija';
  bendrijaTableCreateScript varchar2(4000) := 
  'CREATE TABLE BENDRIJA 
  ( 
    kodas number(5) PRIMARY KEY,
    pavadinimas varchar2(30),
    isteigimo_data DATE,
    pirmininko_ak  number(5),
    p_vardas  varchar2(255),
    p_pavarde varchar2(255),
    p_tel_nr varchar2(30),
    p_nuo DATE
  )';

  sklypasTableName varchar2(255) := 'sklypas';
  sklypasTableCreateScript varchar2(4000) := 
  'CREATE TABLE SKLYPAS 
  (
    nr number(5) PRIMARY KEY,
    bendrijos_kodas varchar2(255),
    adresas varchar2(255),
    plotas number(5)
  )';
  
  sodininkasTableName varchar2(255) := 'sodininkas';
  sodininkasTableCreateScript varchar2(4000) := 
  'CREATE TABLE SODININKAS 
  (
    ak number(5) PRIMARY KEY,
    vardas varchar2(255),
    pavarde varchar2(255),
    tel_nr varchar2(30),
    miestas varchar2(30),
    gimimo_data DATE
  )';
     
  paskyrimasTableName varchar2(255) := 'paskyrimas';
  paskyrimasTableCreateScript varchar2(4000) := 
  'CREATE TABLE PASKYRIMAS 
  (
    bendrijos_kodas number(5),
    sodininko_ak number(5),
    CONSTRAINT pk_bendrijosKodas_sodininkoAk PRIMARY KEY(bendrijos_kodas, sodininko_ak),
    CONSTRAINT fk_benrijos_kodas FOREIGN KEY(bendrijos_kodas) REFERENCES bendrija(kodas),
    CONSTRAINT fk_sodininko_ak FOREIGN KEY(sodininko_ak) REFERENCES sodininkas(ak) 
  )';
  
  prieziuraTableName varchar2(255) := 'prieziura';
  prieziuraTableCreateScript varchar2(4000) := 
  'CREATE TABLE PRIEZIURA 
  (
    sklypo_nr number(5),
    sodininko_ak number(5),
    CONSTRAINT pk_sklypoNr_sodininkoAk PRIMARY KEY(sklypo_nr, sodininko_ak),
    CONSTRAINT fk_prieziura_sklypoNr FOREIGN KEY(sklypo_nr) REFERENCES sklypas(nr),
    CONSTRAINT fk_prieziura_sodininkoAk FOREIGN KEY(sodininko_ak) REFERENCES sodininkas(ak) 
  )';
  
  invalidPrieziuraTableName varchar2(255) := 'invalid_prieziura';
  invalidPrieziuraCreateScript varchar2(4000) := 
  'CREATE TABLE INVALID_PRIEZIURA 
  (
    sklypo_nr number(5),
    sodininko_ak number(5),
    description varchar(255),
    CONSTRAINT fk_invalidPrieziura_prieziura FOREIGN KEY(sklypo_nr,sodininko_ak) REFERENCES prieziura(sklypo_nr,sodininko_ak)
  )';
  
  PROCEDURE INSERT_GENERATED_VALUES;
  PROCEDURE INSERT_VALID_PRIEZIURA(sodininko_ak IN NUMBER, sklypo_nr IN NUMBER);
  PROCEDURE VALIDATE_PRIEZIURA;
  PROCEDURE DELETE_INVALID_PRIEZIURA;

END SODUBENDRIJOS;