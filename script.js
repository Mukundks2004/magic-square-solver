let n = 3;

const inputNumber = document.getElementById('inputNumber');
const decrementBtn = document.getElementById('decrementBtn');
const incrementBtn = document.getElementById('incrementBtn');
const dynamicTable = document.getElementById('dynamicTable');
const goBtn = document.getElementById('goBtn');
const clrBtn = document.getElementById('clrBtn');
const modal = document.getElementById("helpModal");


function generateTable(size) {
    dynamicTable.innerHTML = '';
    
    for (let i = 0; i < size; i++) {
        const row = dynamicTable.insertRow();

        for (let j = 0; j < size; j++) {
            const cell = row.insertCell();            
            const input = document.createElement('input');
            
            input.id = `cell-${i + 1}-${j + 1}`;
            input.type = 'text';
            input.classList.add('cell-input');
            
            cell.appendChild(input);
        }
    }
}

goBtn.addEventListener('click', () => {
    let input = [];
    let size = parseInt(inputNumber.value);
    for (let i = 0; i < size; i++) {
        input.push([]);
        for (let j = 0; j < size; j++) {
            input[i][j] = document.getElementById(`cell-${i + 1}-${j + 1}`).value;
            // document.getElementById(`cell-${i + 1}-${j + 1}`).value = 6;
        }
    }

    let newArray = magicSquare(input, size);
    if (newArray == 0) {
        return;
    }
    for (let i = 0; i < size; i++) {
        for (let j = 0; j < size; j++) {
            document.getElementById(`cell-${i + 1}-${j + 1}`).value = newArray[i][j];
        }
    }
})

clrBtn.addEventListener('click', () => {
    let size = parseInt(inputNumber.value);
    for (let i = 0; i < size; i++) {
        for (let j = 0; j < size; j++) {
            document.getElementById(`cell-${i + 1}-${j + 1}`).value = "";
        }
    }
});

incrementBtn.addEventListener('click', () => {
    let currentValue = parseInt(inputNumber.value);
    if (currentValue < 10) {
        inputNumber.value = currentValue + 1;
        generateTable(parseInt(inputNumber.value));
    }
});

decrementBtn.addEventListener('click', () => {
    let currentValue = parseInt(inputNumber.value);
    if (currentValue > 1) {
        inputNumber.value = currentValue - 1;
        generateTable(parseInt(inputNumber.value));
    }
});

inputNumber.addEventListener('input', () => {
    if (parseInt(inputNumber.value) < 11) {
        generateTable(parseInt(inputNumber.value));
    } 
});

generateTable(n);

function openModal() {
    modal.style.display = "block";
}

function closeModal() {
    modal.style.display = "none";
}

window.onclick = function(event) {
    if (event.target === modal) {
        closeModal();
    }
}