create or replace PACKAGE BODY matrix AS
  FUNCTION generateMatrix (matrixSize IN NUMBER)
  RETURN n_matrix 
  IS 
    matrixToReturn n_matrix;
    innerLoopIndex NUMBER := 1;
    outerLoopIndex NUMBER := 1;
  BEGIN

  IF matrixSize < 0 THEN RAISE EXCEPTIONS_HANDLING.negative_parameter;
  END IF;
  IF matrixSize = 0 THEN RAISE EXCEPTIONS_HANDLING.zero_parameter;
  END IF;

	<<outerLoop>>
	WHILE outerLoopIndex <= matrixSize LOOP
	
		<<innerLoop>>
		WHILE innerLoopIndex <= matrixSize LOOP
			matrixToReturn(outerLoopIndex)(innerLoopIndex) := dbms_random.value(1,100);
			innerLoopIndex := innerLoopIndex + 1;
		END LOOP innerLoop;
		
		innerLoopIndex := 1;
		outerLoopIndex := outerLoopIndex + 1;
	END LOOP outerLoop;
  RETURN matrixToReturn;
    
  EXCEPTION
      WHEN EXCEPTIONS_HANDLING.zero_parameter THEN
      COMMON.WRITE_LOG('Provide positive integer as an input', 1);
    WHEN EXCEPTIONS_HANDLING.negative_parameter THEN
      COMMON.WRITE_LOG('Negative parameter error ocurred in generateMatrix',1);
  END generateMatrix;
  
  
  FUNCTION multiplicateMatrixes (firstMatrix IN n_matrix, secondMatrix IN n_matrix)
  RETURN n_matrix 
  IS
    matrixToReturn n_matrix;
    innerLoopIndex NUMBER := 1;
    outerLoopIndex NUMBER := 1;
    secondInnerLoopIndex NUMBER := 1;
    matrixSize NUMBER;
    currentCellValue NUMBER;
    v_i PLS_INTEGER;
    v_j PLS_INTEGER;
  begin
    matrixSize := firstMatrix.COUNT;
    
    -- copies firstMatrix to temp var just to init memory 
    matrixToReturn := firstMatrix; 
    
    v_i := matrixToReturn.first;

    <<rowsLoop>>
	  WHILE v_i IS NOT NULL LOOP
        v_j := matrixToReturn(v_i).first; 
        
        WHILE v_j IS NOT NULL LOOP
          currentCellValue := 0;
          
          WHILE secondInnerLoopIndex <= matrixSize LOOP
            currentCellValue := currentCellValue + firstMatrix(v_i)(secondInnerLoopIndex) * secondMatrix(secondInnerLoopIndex)(v_j);
            secondInnerLoopIndex := secondInnerLoopIndex + 1;
          END LOOP;
          secondInnerLoopIndex := 1;
          
          matrixToReturn(v_i)(v_j) := currentCellValue;
          v_j := matrixToReturn(v_i).NEXT(v_j);
        END LOOP;
        
        v_i := matrixToReturn.NEXT(v_i);
	  END LOOP rowsLoop;
	  
    RETURN matrixToReturn;
    
  END multiplicateMatrixes;
  
  
  
  PROCEDURE printMatrix(matrix IN n_matrix) 
  IS
	  v_i PLS_INTEGER;
	  v_j PLS_INTEGER;
    lineToPrint NVARCHAR2(50);
  BEGIN
	  v_i := matrix.FIRST; -- get first row index
	  WHILE v_i IS NOT NULL LOOP-- go through all rows
      v_j := matrix(v_i).FIRST; -- get first index in row
      
      WHILE v_j IS NOT NULL LOOP -- go through all elements in a row
        lineToPrint := lineToPrint || ' ' ||  TO_CHAR(matrix(v_i)(v_j));
        v_j := matrix(v_i).NEXT(v_j);
      END LOOP;
      
      dbms_output.put_line(lineToPrint); -- print matrix line
      lineToPrint := '';
      
      v_i := matrix.NEXT(v_i);
	  END LOOP;
  END printMatrix;
  
END matrix;