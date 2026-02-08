nexrad.reader = (function () {
    let loadFileButton;

    const selectors = {
        radarFile: '#radar-file',
        runRadarLoop: '#run-radar-loop'
    };

    const init = () => {
        loadFileButton = document.querySelector(selectors.runRadarLoop);

        setupLoadFileHandler();
    };

    const setupLoadFileHandler = () => {
        const loadFileButton = document.querySelector(selectors.runRadarLoop);
        if (!loadFileButton) {
            return;
        }

        loadFileButton.removeEventListener('click', handleLoadFile);
        loadFileButton.addEventListener('click', handleLoadFile);
    };

    const handleLoadFile = async () => {
        loadFileButton.setAttribute('disabled', 'disabled');

        const radarFileMenu = document.querySelector(selectors.radarFile);
        const selectedMenuItem = radarFileMenu.value;

        if (!selectedMenuItem) {
            radarFileMenu.classList.add('invalid');
            nexrad.ui.enableElement(loadFileButton);
            return;
        }

        message = `Retrieving and loading file data for ${selectedMenuItem}`;
        nexrad.ui.updateToastMessage(message);

        const request = {
            'RadarFile': selectedMenuItem,
            'ElevationNumber': 3
        };

        // Always required to run
        const azimuthData = await nexrad.renderer.azimuth.loadAzimuthData(request);

        // this will change later to render something based off the dropdown list of options (reflectivity, velocity etc.)
        const reflectivityData = await nexrad.renderer.reflectivity.loadReflectivityData(request);

        nexrad.renderer.reflectivity.drawReflectivity(reflectivityData, azimuthData);

        nexrad.ui.disableElement(loadFileButton);
    };

    return {
        init,
        loadFileButton
    };
})();