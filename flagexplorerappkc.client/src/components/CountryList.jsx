import React, { useEffect, useState } from 'react';
import axios from 'axios';
import config from '../config';
import { useNavigate } from 'react-router-dom';

const CountryList = () => {
    const [countries, setCountries] = useState([]);
    const [visibleCountries, setVisibleCountries] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [page, setPage] = useState(1);
    const [isLoading, setIsLoading] = useState(true);

    const countriesPerPage = 15;
    const navigate = useNavigate();

    useEffect(() => {
        setIsLoading(true);
        axios.get(`${config.API_URL}/countries`)
            .then(response => {
                const sortedCountries = response.data.sort((a, b) => a.name.localeCompare(b.name));
                setCountries(sortedCountries);
                setVisibleCountries(sortedCountries.slice(0, countriesPerPage));
                setIsLoading(false);
            })
            .catch(error => {
                console.error('Error fetching countries:', error);
                setIsLoading(false);
            });
    }, []);

    const handleSearch = (event) => {
        const term = event.target.value.toLowerCase();
        setSearchTerm(term);
        const filteredCountries = countries.filter(country =>
            country.name.toLowerCase().includes(term)
        );
        setVisibleCountries(filteredCountries.slice(0, countriesPerPage));
        setPage(1);
    };

    const handleNextPage = () => {
        const nextPage = page + 1;
        const newVisibleCountries = countries.slice((nextPage - 1) * countriesPerPage, nextPage * countriesPerPage);
        setVisibleCountries(newVisibleCountries);
        setPage(nextPage);
    };

    const handlePrevPage = () => {
        const prevPage = page - 1;
        const newVisibleCountries = countries.slice((prevPage - 1) * countriesPerPage, prevPage * countriesPerPage);
        setVisibleCountries(newVisibleCountries);
        setPage(prevPage);
    };

    const handleCountryClick = (country) => {
        navigate(`/country/${country.name}`, { state: { country } });
    };

    const PaginationControls = () => (
        <div className="pagination">
            <button onClick={handlePrevPage} disabled={page === 1}>Previous</button>
            <button onClick={handleNextPage} disabled={visibleCountries.length < countriesPerPage}>Next</button>
        </div>
    );

    const renderTableRows = () => {
        const rows = [];
        for (let i = 0; i < visibleCountries.length; i += 5) {
            const row = visibleCountries.slice(i, i + 5);
            rows.push(
                <tr key={i}>
                    {row.map(country => (
                        <td key={country.name} onClick={() => handleCountryClick(country)}>
                            <img src={country.flag} alt={`${country.name} flag`} />
                            <p>{country.name}</p>
                        </td>
                    ))}
                    {row.length < 5 && Array.from({ length: 5 - row.length }).map((_, index) => <td key={`empty-${index}`}></td>)}
                </tr>
            );
        }
        return rows;
    };

    return (
        <div className="country-list">
            <input
                type="text"
                placeholder="Search countries..."
                value={searchTerm}
                onChange={handleSearch}
                className="search-input"
            />
            <PaginationControls />
            {isLoading ? (
                <div>Loading countries...</div>
            ) : (
                <table>
                    <tbody>
                        {renderTableRows()}
                    </tbody>
                </table>
            )}
            <PaginationControls />
        </div>
    );
};

export default CountryList;