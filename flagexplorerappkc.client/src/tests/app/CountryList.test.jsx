import { render, screen } from '@testing-library/react';
import CountryList from '../pages/CountryList';
import axios from 'axios';
import jest from 'jest';

jest.mock('axios');

jest.test('fetches and displays countries', async () => {
    const countries = [{ name: 'Country1', flag: 'flag1.png' }];
    axios.get.mockResolvedValue({ data: countries });

    render(<CountryList />);

    const countryElements = await screen.findAllByText(/Country1/i);
    jest.expect(countryElements).toHaveLength(1);
});
