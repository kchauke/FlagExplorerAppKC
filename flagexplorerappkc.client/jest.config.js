module.exports = {
    testEnvironment: 'node',
    moduleFileExtensions: ['js', 'jsx'],
    testMatch: ['**/tests/**/*.test.jsx'],
    transform: {
        '^.+\\.jsx?$': 'babel-jest',
    },
    transformIgnorePatterns: ['<rootDir>/node_modules/'],
};