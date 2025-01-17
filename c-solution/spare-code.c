#include <stdio.h>

// TODO: delete
void Print2dFloatArray(float* arr, int rows, int cols)
{
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            printf("%05.1f ", *(arr + i * cols + j));
        }

        printf("\n");
    }
}

void Print2dArray(int* arr, int rows, int cols)
{
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            printf("%03d ", *(arr + i * cols + j));
        }
        printf("\n");
    }
}

void PrintArray(int arr[], int length)
{
    for (int i = 0; i < length; i++) {
        printf("%03d ", *(arr + i));
    }
    printf("\n");
}

int main() {

    // Get magic square size
    int size;
    printf("Enter size of magic square: ");
    scanf("%d", &size);

    // Create array that stores indices of knowns
    int cellsWhoseValuesAreKnown[size * size];
    int knownCellCount = 0;
    for (int i = 0; i < size * size; i++) {
        cellsWhoseValuesAreKnown[i] = 0;
    }

    // Populate magic square
    float inputMagicSquare[size][size];
    printf("Enter the elements of the %dx%d magic square (Enter 'x' to indicate unknown):\n", size, size);

    for (int i = 0; i < size; i++) {
        for (int j = 0; j < size; j++) {
            printf("Element [%d][%d]: ", i, j);
            int scanResult = scanf("%f", &inputMagicSquare[i][j]);
            if (scanResult == 1) {
                cellsWhoseValuesAreKnown[i * size + j] = 1;
                knownCellCount++;
            }
            else {
                inputMagicSquare[i][j] = 0;
                scanf("%*s");
            }
        }
    }
    printf("\n");

    //Make coefficient table
    int coefficientTable[size * 2 + 2][size * size];
    for (int i = 0; i < size * 2 + 2; i++) {
        for (int j = 0; j < size * size; j++) {
            coefficientTable[i][j] = 0;
        }
    }

    for (int i = 0; i < size; i++) {
        for (int j = 0; j < size; j++) {
            coefficientTable[i][i * size + j] = 1;
        }
    }

    for (int i = 0; i < size; i++) {
        for (int j = 0; j < size; j++) {
            coefficientTable[size + i][i + size * j] = 1;
        }
    }

    for (int i = 0; i < size; i++) {
        coefficientTable[2 * size][(size + 1) * i] = 1;
    }

    for (int i = 0; i < size; i++) {
        coefficientTable[2 * size + 1][(size - 1) * (i + 1)] = 1;
    }

    // Make adjoint matrix
    int adjointMatrixCols = size * size - knownCellCount + 2;
    float adjointMatrix[size * 2 + 2][adjointMatrixCols];
    for (int row = 0; row < 2 * size + 2; row++) {
        int columnBeingWrittenTo = 0;
        float columnTotal = 0;
        for (int col = 0; col < size * size; col++) {
            if (coefficientTable[row][col] == 1 && cellsWhoseValuesAreKnown[col] != 0) {
                columnTotal -= inputMagicSquare[col / size][col % size];
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

    // Perform Gauss-Jordan Elimination

    int lead = 0;
    int rowCount = 2 * size + 2;

    for (int i = 0; i < rowCount; i++) {
        if (adjointMatrixCols <= lead) {
            break;
        }

        int pivot = i;
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

        for (int j = 0; j < adjointMatrixCols; j++) {
            float temp = adjointMatrix[pivot][j];
            adjointMatrix[pivot][j] = adjointMatrix[i][j];
            adjointMatrix[i][j] = temp;
        }

        float div = adjointMatrix[i][lead];
        if (div != 0) {
            for (int j = 0; j < adjointMatrixCols; j++) {
                adjointMatrix[i][j] /= div;
            }
            for (int j = 0; j < rowCount; j++) {
                if (j != i) {
                    float sub = adjointMatrix[j][lead];

                    for (int k = 0; k < adjointMatrixCols; k++) {
                        adjointMatrix[j][k] -= sub * adjointMatrix[i][k];
                    }
                }
            }
        }

        lead++;
    }

    int variableCount = 0;
    for (int i = 0; i < size; i++) {
        for (int j = 0; j < size; j++) {
            if (cellsWhoseValuesAreKnown[size * i + j] == 1) {
                printf("%05.1f ", inputMagicSquare[i][j]);
            }
            else {
                printf("%05.1f ", adjointMatrix[variableCount][adjointMatrixCols - 1]);
                variableCount++;
            }
        }

        printf("\n");
    }

    printf("The magic constant is: %d", adjointMatrix[variableCount][adjointMatrixCols - 1]);



    // Output
    /*Print2dFloatArray(&inputMagicSquare[0][0], size, size);
    printf("\n");
    PrintArray(cellsWhoseValuesAreKnown, size * size);
    printf("\n");
    Print2dArray(&coefficientTable[0][0], size * 2 + 2, size * size);
    printf("\n");
    Print2dFloatArray(&adjointMatrix[0][0], size * 2 + 2, adjointMatrixCols);
    printf("\n");*/
    return 0;
}