function create2DArray(n, m) {
    let array = [];
    for (let i = 0; i < n; i++) {
        array[i] = new Array(m).fill(0);  // Fill each row with zeros
    }
    return array;
}

function magicSquare(inputArray, size) {
    let result = []
    console.log(inputArray);

    for (let i = 0; i < size; i++) {
        result.push([]);
        for (let j = 0; j < size; j++) {
            result[i].push(j);
        }
    }


    let cellsWhoseValuesAreKnown = new Array(size * size).fill(0);
    let knownCellCount = inputArray.flat().filter(element => element !== '').length;
    console.log(inputArray);

    for (let i = 0; i < size; i++) {
        for (let j = 0; j < size; j++) {
            if (inputArray[i][j] != 0) {
                cellsWhoseValuesAreKnown[i * size + j] = 1;
            }
        }
    }

    let coefficientTable = [];
    for (let i = 0; i < size * 2 + 2; i++) {
        coefficientTable[i] = new Array(size * size).fill(0);
    }

    for (let i = 0; i < size; i++) {
        for (let j = 0; j < size; j++) {
            coefficientTable[i][i * size + j] = 1;
        }
    }

    for (let i = 0; i < size; i++) {
        for (let j = 0; j < size; j++) {
            coefficientTable[size + i][i + size * j] = 1;
        }
    }

    for (let i = 0; i < size; i++) {
        coefficientTable[2 * size][(size + 1) * i] = 1;
    }

    for (let i = 0; i < size; i++) {
        coefficientTable[2 * size + 1][(size - 1) * (i + 1)] = 1;
    }



    let adjointMatrixCols = size * size - knownCellCount + 2;
    let adjointMatrix = create2DArray(size * 2 + 2, adjointMatrixCols);
    
    for (let row = 0; row < 2 * size + 2; row++) {
        let columnBeingWrittenTo = 0;
        let columnTotal = 0;
        for (let col = 0; col < size * size; col++) {
            if (coefficientTable[row][col] == 1 && cellsWhoseValuesAreKnown[col] != 0) {
                columnTotal -= inputArray[Math.floor(col / size)][col % size];
            }
            else {
                adjointMatrix[row][columnBeingWrittenTo] = coefficientTable[row][col];
            }
            if (cellsWhoseValuesAreKnown[col] == 0) {
                columnBeingWrittenTo++;
            }
        }


        adjointMatrix[row][adjointMatrixCols - 2] = -1;
        adjointMatrix[row][adjointMatrixCols - 1] = columnTotal;
    }


    

    let lead = 0;
    let rowCount = 2 * size + 2;

    for (let i = 0; i < rowCount; i++) {
        if (adjointMatrixCols <= lead) {
            break;
        }

        let pivot = i;
        while (adjointMatrix[pivot][lead] == 0) {
            pivot++;
            if (pivot == rowCount) {
                pivot = i;
                lead++;

                if (adjointMatrixCols == lead) {
                    lead--;
                    break;
                }
            }
        }

        for (let j = 0; j < adjointMatrixCols; j++) {
            let temp = adjointMatrix[pivot][j];
            adjointMatrix[pivot][j] = adjointMatrix[i][j];
            adjointMatrix[i][j] = temp;
        }

        let div = adjointMatrix[i][lead];
        if (div != 0) {
            for (let j = 0; j < adjointMatrixCols; j++) {
                adjointMatrix[i][j] /= div;
            }
            for (let j = 0; j < rowCount; j++) {
                if (j != i) {
                    let sub = adjointMatrix[j][lead];

                    for (let k = 0; k < adjointMatrixCols; k++) {
                        adjointMatrix[j][k] -= sub * adjointMatrix[i][k];
                    }
                }
            }
        }

        lead++;
    }



    let variableCount = 0;
    for (let i = 0; i < size * size; i++) {
        if (inputArray[Math.floor((i / 3))][i % 3] == '') {
            inputArray[Math.floor((i / 3))][i % 3] = adjointMatrix[variableCount][adjointMatrixCols - 1];
            variableCount++;
        }
    }

    return inputArray;
}