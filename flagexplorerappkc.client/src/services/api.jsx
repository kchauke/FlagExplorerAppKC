import axios from 'axios';
import config from '../config';

export const fetchCountries = () => axios.get(`${config.API_URL}/countries`);
export const fetchCountryDetails = (name) => axios.get(`${config.API_URL}/countries/${name}`);
