nexrad.reader = (function () {
    let loadFileButton;
    let azmiuthData;
    let request;

    const selectors = {
        radarFile: '#radar-file',
        runRadarLoop: '#run-radar-loop'
    };

    const init = () => {
        loadFileButton = document.querySelector(selectors.runRadarLoop);

        setupLoadFileHandler();
    };

    const setupLoadFileHandler = () => {
        const radarFileDropdown = document.querySelector(selectors.radarFile);
        const loadFileButton = document.querySelector(selectors.runRadarLoop);
        if (!radarFileDropdown || !loadFileButton) {
            return;
        }

        radarFileDropdown.removeEventListener('click', handleRadarFileChange);
        radarFileDropdown.addEventListener('click', handleRadarFileChange);

        loadFileButton.removeEventListener('click', handleLoadFile);
        loadFileButton.addEventListener('click', handleLoadFile);
    };

    const handleRadarFileChange = async () => {
        const radarFileMenu = document.querySelector(selectors.radarFile);
        const selectedMenuItem = radarFileMenu.value;

        if (!selectedMenuItem) {
            radarFileMenu.classList.add('invalid');
            nexrad.ui.enableElement(loadFileButton);
            return;
        }

        message = `Retrieving and loading file data for ${selectedMenuItem}`;
        nexrad.ui.updateToastMessage(message);

        request = {
            'RadarFile': selectedMenuItem,
        };

        // Always required to run
        azimuthData = await nexrad.renderer.azimuth.loadAzimuthData(request);
    };

    const handleLoadFile = async () => {
        nexrad.ui.disableElement(loadFileButton);

        const elevationScan = document.querySelector('#elevation-scan');

        request.ElevationNumber = elevationScan.value;

        // this will change later to render something based off the dropdown list of options (reflectivity, velocity etc.)
        const reflectivityData = await nexrad.renderer.reflectivity.loadReflectivityData(request);

        nexrad.renderer.reflectivity.drawReflectivity(reflectivityData, azimuthData.AzimuthData[request.ElevationNumber]);

        nexrad.ui.disableElement(loadFileButton);
    };

    return {
        init,
        loadFileButton
    };
})();