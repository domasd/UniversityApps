create or replace PROCEDURE SODUBENDRIJOS_RUNNER AS   
  TYPE sodininko_ak_type IS TABLE OF SODININKAS.AK%type;
  TYPE sklypo_nr_type IS TABLE OF SKLYPAS.NR%type;
  TYPE sklypo_plotas_type IS TABLE OF SKLYPAS.PLOTAS%type;
  
  sklypo_plotas_table sklypo_plotas_type;
  sklypo_nr_table sklypo_nr_type;
  sodininko_ak_table sodininko_ak_type;
  
  TYPE prieziura_type IS TABLE OF prieziura%rowtype INDEX BY PLS_INTEGER;
  prieziura_neTaBendrija_tab prieziura_type;

  limit_bulk_collect PLS_INTEGER := 100;
BEGIN
  SODUBENDRIJOS_CREATE();
  SODUBENDRIJOS.VALIDATE_PRIEZIURA();
 
END SODUBENDRIJOS_RUNNER;