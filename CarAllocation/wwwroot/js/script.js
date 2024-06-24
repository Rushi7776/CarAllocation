let currentPage = 1;

function formatDateTime(dateTimeString) {
    const date = new Date(dateTimeString);
    return date.toLocaleString();
}

async function allocateParking() {
    const carPlateNumber = document.getElementById('carPlateNumber').value;
    const response = await fetch('/api/parking/allocate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ carPlateNumber: carPlateNumber })
    });

    if (response.ok) {
        const result = await response.json();
        document.getElementById('result').innerHTML = `
            <p>Car Plate Number: ${result.carPlateNumber}</p>
            <p>Start Time: ${formatDateTime(result.startTime)}</p>
            <p>End Time: ${formatDateTime(result.endTime)}</p>
            <p>Parking Number: ${result.parkingNumber}</p>
            <p>Total Amount: ${result.totalAmount}</p>
        `;
        loadParkingList(currentPage); // Refresh the parking list
    } else {
        const errorText = await response.text();
        document.getElementById('result').innerHTML = `Error: ${errorText}`;
    }
}

async function loadParkingList(pageNumber = 1) {
    const response = await fetch(`/api/parking?pageNumber=${pageNumber}&pageSize=10`);
    if (response.ok) {
        const parkingList = await response.json();
        const parkingListDiv = document.getElementById('parkingList');
        parkingListDiv.innerHTML = ''; // Clear previous list

        parkingList.forEach(parking => {
            parkingListDiv.innerHTML += `
                <p>Parking Number: ${parking.parkingNumber}, Car: ${parking.carPlateNumber}, Start: ${formatDateTime(parking.startTime)}, End: ${formatDateTime(parking.endTime)}, Amount: ${parking.totalAmount}</p>
            `;
        });

        currentPage = pageNumber;
        document.getElementById('prevPage').disabled = currentPage === 1;
        document.getElementById('nextPage').disabled = parkingList.length < 10;
    } else {
        const errorText = await response.text();
        document.getElementById('parkingList').innerHTML = `Error: ${errorText}`;
    }
}

window.onload = () => {
    loadParkingList();
};
