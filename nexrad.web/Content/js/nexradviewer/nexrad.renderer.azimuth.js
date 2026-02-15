nexrad.renderer.azimuth = (function () {
    const loadAzimuthData = async (request) => {
        const response = await fetch('/Nexrad/GetAzimuthData', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const data = await response.json();
        message = `File scan completed and ${data.AzimuthData.length} Azimuth data records were found`;
        nexrad.ui.updateToastMessage(message);

        const createElevationDropdownResponse = nexrad.ui.createElevationDropdown(data.AvailableElevationScans);
        const placeholder = document.querySelector('#elevation-scans-placeholder');
        placeholder.innerHTML = '';
        placeholder.append(createElevationDropdownResponse.label);
        placeholder.appendChild(createElevationDropdownResponse.select);

        return data;
    };

    return {
        loadAzimuthData
    };
})();