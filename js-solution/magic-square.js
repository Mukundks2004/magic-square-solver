function magicSquare(inputArray, size) {
    let result = []
    for (let i = 0; i < size; i++) {
        result.push([]);
        for (let j = 0; j < size; j++) {
            result[i].push(j);
        }
    }

    console.log(result);
    console.log("y");

    return result;
}