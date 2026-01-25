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
        message = `File scan completed and ${data.length} Azimuth data records were found`;
        nexrad.ui.updateToastMessage(message);

        return data;
    };

    return {
        loadAzimuthData
    };
})();