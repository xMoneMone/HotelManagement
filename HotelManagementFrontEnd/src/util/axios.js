import axios from "axios"

export const axios_base = axios.create({baseURL: import.meta.env.VITE_BASEURL})