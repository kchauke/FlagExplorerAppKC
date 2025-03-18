import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import jest from 'jest';
import { fetchCountries, fetchCountryDetails } from '../../services/api';
import config from '../../config';

jest.describe('API Tests', () => {
    let mock;

    jest.beforeAll(() => {
        mock = new MockAdapter(axios);
    });

    jest.afterEach(() => {
        mock.reset();
    });

    jest.afterAll(() => {
        mock.restore();
    });

    jest.test('fetches all countries', async () => {
        const countries = [{ name: 'Country1', flag: 'flag1.png' }];
        mock.onGet(`${config.API_URL}/countries`).reply(200, countries);

        const response = await fetchCountries();
        jest.expect(response.data).toEqual(countries);
    });

    jest.test('fetches country details', async () => {
        const countryDetails = { name: 'Country1', population: 1000, capital: 'Capital1', flag: 'flag1.png' };
        mock.onGet(`${config.API_URL}/countries/Country1`).reply(200, countryDetails);

        const response = await fetchCountryDetails('Country1');
        jest.expect(response.data).toEqual(countryDetails);
    });
});