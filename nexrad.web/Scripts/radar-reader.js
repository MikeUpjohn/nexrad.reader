window.addEventListener('DOMContentLoaded', function () {
    nexrad.reader.init();
    nexrad.ui.init();
});

const nexrad = {
};

nexrad.reader = {};
nexrad.ui = {};
nexrad.renderer = {};

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

        const selectedMenuItem = document.querySelector(selectors.radarFile).value;
        if (!selectedMenuItem) {
            return;
        }

        message = `Retrieving and loading file data for ${selectedMenuItem}`;
        nexrad.ui.updateToastMessage(message);

        const request = {
            'RadarFile': 'KTLX20130520_200356_V06',
            'ElevationNumber': 1
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

nexrad.renderer.reflectivity = (function () {
    const loadReflectivityData = async (request) => {
        const response = await fetch('/Nexrad/GetReflectivityData', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        const data = await response.json();
        message = `File scan completed and ${data.length} reflectivity records were found`;
        nexrad.ui.updateToastMessage(message);

        return data;
    };

    const drawReflectivity = (reflectivityData, azimuthData) => {
        
    };

    return {
        loadReflectivityData,
        drawReflectivity
    }
})();

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

nexrad.ui = (function () {
    let bootstrapToast;
    let toast;

    const selectors = {
        toastBody: '.toast-body',
        toastMessage: '#toast-message'
    };

    const init = () => {
        toast = document.querySelector(selectors.toastMessage);
        bootstrapToast = new bootstrap.Toast(toast);
        message = '';
    };

    const updateToastMessage = message => {
        if (!bootstrapToast) {
            return;
        }

        bootstrapToast._element.querySelector(selectors.toastBody).innerText = message;
        bootstrapToast.show();
    };

    const disableElement = element => {
        if (!element) {
            return;
        }

        element.setAttribute('disabled', 'disabled');
    }

    return {
        init,
        updateToastMessage,
        disableElement
    };
})();
