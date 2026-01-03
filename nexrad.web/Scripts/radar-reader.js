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
    // Awful, but good enough for now!
    // Colour scale from -32.0 through to 94.5dbZ as array of mixed colours
    const reflectivityColourSet = [0x000000, 0x9C9C9C, 0x767676, 0xFFAAAA, 0xEE8C8C, 0xC97070, 0x00FB90, 0x00BB00, 0xFFFF70, 0xD0D060, 0xFF6060, 0xDA0000, 0xAE0000, 0x0000FF, 0xFFFFFF, 0xE700FF];

    const init = () => {
        renderer = new THREE.WebGLRenderer();
        renderer.setSize(window.innerWidth, window.innerHeight);

        document.body.append(renderer.domElement);

        renderScene();
    };

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
        if (reflectivityData === null || azimuthData === null) {
            return;
        }

        let renderer;
        const cameraPosition = 100;
        const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
        const scene = new THREE.Scene();

        const scans = azimuthData.length;
        const reflectivityColourScale = d3.scaleQuantize().domain([-32.0, 94.5]).range(reflectivityColourSet);

        for (let i = 0; i < scans; i++) {
            setTimeout(() => {
                const reflectivity = reflectivityData[i];
                const azimuth = math.utilities.degreesToRadians(azimuthData[i]);

                // Calculate the SIN multiplier from this azimuth (which is now in radians), so that we can later generate the x co-ordinates for this range of the radar signature
                const sinMultiplier = Math.sin(azimuth);

                // Calculate the COS multiplier from this azimuth (which is now in radians), so that we can later generate the y co-ordinates for this range of the radar signature
                const cosMultiplier = Math.cos(azimuth);

                // Generate an array of values in increments of 1 from 0 to the Reflectivity Gate Count (usually 2880).
                let initialRange = math.utilities.generateRange(0, reflectivity.GateCount, 1);

                // Multiply all values in the array by the Gate Size to determine the gaps between data points.
                initialRange = math.utilities.multiplyRange(initialRange, reflectivity.GateSize);

                // Add the First Gate value onto all values so that each value in the array starts at the right location.
                initialRange = math.utilities.addRange(initialRange, reflectivity.FirstGate);

                // SIN multiplier for x co-ordinates of this arc
                const x = math.utilities.multiplyRange(initialRange, sinMultiplier);

                // COS multiplier for y co-ordinates of this arc
                const y = math.utilities.multiplyRange(initialRange, cosMultiplier);

                // The dataset here should have three arrays representing this arc of the radar circle.
                // Array of x co-ordinate values
                // Array of y co-ordinate values
                // Array of reflectivityValues representing rainfall intensity, in the range -32 to 94.5 dbZ
                const dataSet = {
                    x: x,
                    y: y,
                    reflectivity: reflectivity
                };

                const threePointsMaterial = new THREE.PointsMaterial({
                    size: 2,
                    vertexColors: true,
                    sizeAttentuation: false
                });

                const geometry = new THREE.BufferGeometry();
                const pointsGraph = [];
                const coloursGraph = [];

                dataSet.x.forEach(function (index, value) {
                    if (reflectivity.MomentDataValues[i] > -33) {
                        const dataPointColour = new THREE.Color(reflectivityColourScale(reflectivity.MomentDataValues[i]));

                        pointGraph.push(dataset.x[i], dataset.y[i], 0);
                        colourGraph.push(dataPointColour.r, dataPointColour.g, dataPointColour.b);
                    }
                });

                const pointsGraphArray = new Float32Array(pointsGraph);
                const coloursGraphArray = new Float32Array(coloursGraph);

                geometry.setAttribute('position', new THREE.BufferAttribute(pointsGraphArray, 3));
                geometry.setAttribute('color', new THREE.BufferAttribute(coloursGraphArray, 3));

                const pointsMap = new THREE.Points(geometry, threePointsMaterial);
                scene.add(pointsMap);
                
            }, 10 * i);
        }
    };

    const renderScene = () => {
        requestAnimationFrame(renderScene);
        camera.position.z = cameraPosition;
        renderer.render(scene, camera);
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
