let tooltipTriggerList;
let selectAllCheckbox;
let rowCheckboxes;
let blockButton;
let unblockButton;
let deleteButton;
let selectedIds

function updatePageSize(size) {
    const url = new URL(window.location.href);
    url.searchParams.set('size', size);
    url.searchParams.set('page', 1);
    window.location.href = url.toString();
}

async function performSearch(query) {
    const url = new URL(window.location.href);
    url.searchParams.set('search', query);
    url.searchParams.set('page', 1);

    history.replaceState(null, '', url.toString());

    const response = await fetch(url, { headers: { "X-Requested-With": "XMLHttpRequest" }})
        .catch(error => console.log(error));

    document.getElementById('usersTableContainer').innerHTML = await response.text();
}

document.addEventListener('DOMContentLoaded', function () {
    selectAllCheckbox = document.getElementById('selectAll');
    rowCheckboxes = document.querySelectorAll('.row-checkbox');

    blockButton = document.getElementById('blockSelected');
    unblockButton = document.getElementById('unblockSelected');
    deleteButton = document.getElementById('deleteSelected');
    
    tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
    
    selectAllCheckbox.addEventListener('change', function () {
        const isChecked = selectAllCheckbox.checked;

        rowCheckboxes.forEach(checkbox => {
            checkbox.checked = isChecked;
        });
    });
    
    rowCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            const allChecked = Array.from(rowCheckboxes).every(cb => cb.checked);
            const someChecked = Array.from(rowCheckboxes).some(cb => cb.checked);

            selectAllCheckbox.checked = allChecked;
            selectAllCheckbox.indeterminate = !allChecked && someChecked;
        });
    });

    blockButton.addEventListener('click', async function () {
        selectedIds = Array.from(rowCheckboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);

        if (selectedIds.length === 0) console.log('No users selected for block.');

        let response = await fetch('/Admin/BlockUser', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(selectedIds)
        })
            .catch(error => console.log(error));

        if (response.ok) window.location.reload();
    })

    unblockButton.addEventListener('click', async function () {
        selectedIds = Array.from(rowCheckboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);

        if (selectedIds.length === 0) console.log('No users selected for unblock.');

        let response = await fetch('/Admin/UnblockUser', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(selectedIds)
        })
            .catch(error => console.log(error));

        if (response.ok) window.location.reload();
    })

    deleteButton.addEventListener('click', async function () {
        const selectedIds = Array.from(rowCheckboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);

        if (selectedIds.length === 0) console.log('No users selected for deletion.');

        let response = await fetch('/Admin/DeleteUser', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(selectedIds)
        })
            .catch(error => console.log(error));

        if (response.ok) window.location.reload();
    });
});



