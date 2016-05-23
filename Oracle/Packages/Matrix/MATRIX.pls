create or replace PACKAGE matrix AS -- declaration
  TYPE vc_array IS TABLE OF NUMBER(38) INDEX BY PLS_INTEGER;
  TYPE n_matrix IS TABLE OF vc_array INDEX BY PLS_INTEGER;

  FUNCTION generateMatrix(matrixSize IN NUMBER) RETURN n_matrix;
  FUNCTION multiplicateMatrixes(firstMatrix IN n_matrix, secondMatrix IN n_matrix) RETURN n_matrix;
  PROCEDURE printMatrix(matrix IN n_matrix);

END matrix;