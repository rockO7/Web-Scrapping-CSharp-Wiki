window.extractTableData = function () {
    let table = document.querySelector('table');
    let rows = table.querySelectorAll('tr');
    let data = [];

    for (let i = 1; i < rows.length; i++) { // Skip the header row
        let cells = rows[i].querySelectorAll('td');
        let rowData = {
            Company: cells[0].innerText,
            Country: cells[1].innerText
        };
        data.push(rowData);
    }

    return JSON.stringify(data);
};
