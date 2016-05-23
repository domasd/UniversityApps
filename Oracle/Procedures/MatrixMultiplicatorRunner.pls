create or replace PROCEDURE MatrixMultiplicatorRunner (matrixSize NUMBER)
AS
  firstMatrix matrix.n_matrix;
  secondMatrix matrix.n_matrix;
  multipliedMatrix matrix.n_matrix;
BEGIN
  firstMatrix := matrix.generateMatrix(matrixSize); 
  secondMatrix := matrix.generateMatrix(matrixSize); 
  
  dbms_output.put_line('First matrix:');
  matrix.printMatrix(firstMatrix);
  dbms_output.put_line('Second matrix:');
  matrix.printMatrix(secondMatrix);
  
  multipliedMatrix := matrix.multiplicateMatrixes(firstMatrix, secondMatrix);

  dbms_output.put_line('Multiplied matrix:');
  matrix.printMatrix(multipliedMatrix);
  EXCEPTION
    WHEN VALUE_ERROR THEN
      COMMON.WRITE_LOG('Provide matrix size input that is an integer', 2);
END;