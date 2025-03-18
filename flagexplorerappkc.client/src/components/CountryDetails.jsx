import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from 'axios';
import config from '../config';

const CountryDetails = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const country = location.state?.country;
    const [countryDetails, setCountryDetails] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        if (country) {
            setIsLoading(true);
            axios.get(`${config.API_URL}/countries/${country.name}`)
                .then(response => {
                    setCountryDetails(response.data);
                    setIsLoading(false);
                })
                .catch(error => {
                    console.error('Error fetching country details:', error);
                    setIsLoading(false);
                });
        } else {
            navigate('/');
        }
    }, [country, navigate]);

    const formatPopulation = (population) => {
        if (typeof population === 'number') {
            return population.toLocaleString();
        }
        return 'N/A'; // Or handle the case where population is not a number
    };

    const handleBack = () => {
        navigate('/');
    };

    return (
        <div className="country-details-page">
            <button onClick={handleBack}>Back</button>
            {isLoading ? (
                <div>Loading...</div>
            ) : countryDetails ? (
                <div className="country-details">
                    <h1>{countryDetails.name}</h1>
                    <img src={country.flag} alt={`${countryDetails.name} flag`} />
                        <p>Population: {formatPopulation(countryDetails.population)}</p>
                    <p>Capital: {countryDetails.capital}</p>
                </div>
            ) : (
                <div>Country not found.</div>
            )}
        </div>
    );
};

export default CountryDetails;