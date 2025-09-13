document.getElementById('dataForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const apiKey = document.getElementById('apiKey').value;
    const role = document.getElementById('role').value;
    const dataId = document.getElementById('dataId').value;
    const resultDiv = document.getElementById('result');
    const loadingDiv = document.getElementById('loading');
    const fetchBtn = document.getElementById('fetchBtn');

    // Reset and show loading
    resultDiv.innerHTML = '';
    resultDiv.className = 'result';
    loadingDiv.style.display = 'block';
    fetchBtn.disabled = true;

    try {
        const response = await fetch(`http://localhost:5030/api/DataSharing/${dataId}?role=${role}`, {
            method: 'GET',
            headers: {
                'X-Api-Key': apiKey
            }
        });

        loadingDiv.style.display = 'none';
        fetchBtn.disabled = false;

        if (!response.ok) {
            const errorText = await response.text();
            if (response.status === 401) {
                throw new Error("API Key is missing or invalid.");
            } else if (response.status === 403) {
                throw new Error("Access denied or invalid API Key.");
            } else {
                throw new Error(`${response.status}: ${errorText}`);
            }
        }

        const data = await response.json();
        resultDiv.innerHTML = `
            <strong>ID:</strong> ${data.id}<br>
            <strong>Name:</strong> ${data.name}<br>
            <strong>Content:</strong> ${data.content}<br>
            <strong>Access Level:</strong> ${data.accessLevel}
        `;
        resultDiv.classList.add('success');
    } catch (error) {
        loadingDiv.style.display = 'none';
        fetchBtn.disabled = false;
        resultDiv.innerHTML = `Error: ${error.message}`;
        resultDiv.classList.add('error');
    }
});

document.getElementById('resetBtn').addEventListener('click', () => {
    const form = document.getElementById('dataForm');
    const resultDiv = document.getElementById('result');
    form.reset();
    resultDiv.innerHTML = '';
    resultDiv.className = 'result';
});