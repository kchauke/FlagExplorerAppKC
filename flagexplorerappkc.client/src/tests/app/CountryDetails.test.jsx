import { render, screen } from '@testing-library/react';
import CountryDetails from '../pages/CountryDetails';
import axios from 'axios';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import jest from 'jest';

jest.mock('axios');

jest.test('fetches and displays country details', async () => {
    const country = { name: 'Country1', population: 1000, capital: 'Capital1', flag: 'flag1.png' };
    axios.get.mockResolvedValue({ data: country });

    render(
        <MemoryRouter initialEntries={['/country/Country1']}>
            <Routes>
                <Route path="/country/:name" element={<CountryDetails />} />
            </Routes>
        </MemoryRouter>
    );

    const nameElement = await screen.findByText(/Country1/i);
    jest.expect(nameElement).toBeInTheDocument();
});
