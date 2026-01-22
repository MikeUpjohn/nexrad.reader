const math = {
};

math.utilities = {};

math.utilities = (function () {
    const constants = {
        ONE_EIGHTY_DEGREES: 180
    };

    const addRange = (range, valueToAdd) => {
        const newRange = new Array(range.length);

        for (let i = 0; i < range.length; i++) {
            newRange[i] = range[i] + valueToAdd;
        }

        return newRange;
    };

    const degreesToRadians = degrees => {
        return degrees * Math.PI / constants.ONE_EIGHTY_DEGREES;
    }

    const generateRange = (start, finish, step) => {
        const range = [];

        for (let i = start; i < finish; i += step) {
            const value = start + (i * step);

            range.push(value);
        }

        return range;
    };

    const multiplyRange = (range, multiplyFactor) => {
        const newRange = new Array(range.length);

        for (let i = 0; i < range.length; i++) {
            newRange[i] = range[i] * multiplyFactor;
        }

        return newRange;
    }

    return {
        addRange,
        degreesToRadians,
        generateRange,
        multiplyRange
    };
})();