import axios from 'axios';
import jest from 'jest';
import config from '../../src/config';

jest.describe('Real API Tests', () => {
    jest.test('fetches all countries from the real API', async () => {
        const response = await axios.get(`${config.API_URL}/countries`);
        jest.expect(response.status).toBe(200);
        jest.expect(Array.isArray(response.data)).toBe(true);
        jest.expect(response.data.length).toBeGreaterThan(0);
    });

    jest.test('fetches country details from the real API', async () => {
        const countryName = 'Country1'; // Replace with a valid country name
        const response = await axios.get(`${config.API_URL}/countries/${countryName}`);
        jest.expect(response.status).toBe(200);
        jest.expect(response.data).toHaveProperty('name', countryName);
        jest.expect(response.data).toHaveProperty('population');
        jest.expect(response.data).toHaveProperty('capital');
        jest.expect(response.data).toHaveProperty('flag');
    });
});
