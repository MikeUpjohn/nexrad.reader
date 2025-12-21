window.addEventListener('DOMContentLoaded', function () {
    nexrad.reader.init();
});

const nexrad = {
};

nexrad.reader = (function () {
    let message;

    const selectors = {
        radarFile: '#radar-file',
        runRadarLoop: '#run-radar-loop'
    };

    const init = () => {
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
        nexrad.ui.updateToastMessage(message);

        const query = {
            'RadarFile': 'KTLX20130520_200356_V06',
            'ElevationNumber': 1
        };

        const response = await fetch('/Home/GetData', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(query)
        });

        console.log(await response.json());
    };

    return {
        init
    };
})();

nexrad.ui = (function () {
    const selectors = {
        message: '#message'
    };

    const updateToastMessage = message => {
        const toastMessage = document.getElementById(selectors.message);
        if (!toastMessage) {
            return;
        }

        toastMessage.querySelector(selectors.toastBody).innerHtml = message;
        toastMessage.style.display = 'block';
    };

    return {
        updateToastMessage
    };
})();