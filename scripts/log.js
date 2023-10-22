// @ts-check

export const RED = '[31m';
export const YELLOW = '[33;1m';

/**
 * @param {string} message
 */
export const info = (message) => {
    console.log(message);
};

/**
 * @param {string} message
 */
export const error = (message) => {
    log(RED, message);
};

/**
 * @param {string} message
 */
export const fatal = (message) => {
    log(RED, message);
    process.exitCode = 1;
};

/**
 * @param {string} color
 * @param {string} message
 */
export const log = (color, message) => {
    console.log('\x1b%s%s\x1b[0m', color, message);
};
