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

        scene = new THREE.Scene();
        renderer = new THREE.WebGLRenderer();
        renderer.setSize(window.innerWidth, window.innerHeight - 56);
        cameraPosition = 100;
        camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);

        document.body.append(renderer.domElement);

        renderScene();
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

    const renderScene = () => {
        requestAnimationFrame(renderScene);
        camera.position.z = cameraPosition;
        renderer.render(scene, camera);
    };

    return {
        init,
        updateToastMessage,
        disableElement
    };
})();
