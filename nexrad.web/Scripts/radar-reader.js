window.addEventListener('DOMContentLoaded', function () {
    nexrad.reader.init();
});

const nexrad = {
};

nexrad.reader = (function () {
    let bootstrapToast;
    let message;

    const selectors = {
        radarFile: '#radar-file',
        runRadarLoop: '#run-radar-loop'
    };

    const init = () => {
        const toast = document.querySelector('#toast-message')
        bootstrapToast = new bootstrap.Toast(toast);

        message = '';

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
        const loadFileButton = document.querySelector(selectors.runRadarLoop);
        loadFileButton.setAttribute('disabled', 'disabled');

        const selectedMenuItem = document.querySelector(selectors.radarFile).value;
        if (!selectedMenuItem) {
            return;
        }

        message = `Retrieving and loading file data for ${selectedMenuItem}`;
        nexrad.ui.updateToastMessage(bootstrapToast, message);

        const query = {
            'RadarFile': 'KTLX20130520_200356_V06',
            'ElevationNumber': 1
        };

        const response = await fetch('/Nexrad/LoadRadarFile', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(query)
        });
    };

    return {
        init
    };
})();

nexrad.ui = (function () {
    const selectors = {
        toastBody: '.toast-body',
        toastMessage: '#toast-message'
    };

    const updateToastMessage = (bootstrapToast, message) => {
        if (!bootstrapToast) {
            return;
        }

        bootstrapToast._element.querySelector(selectors.toastBody).innerText = message;
        bootstrapToast.show();
    };

    return {
        updateToastMessage
    };
})();