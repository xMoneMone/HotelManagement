import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token, hotelId) => {
    const [currency, setCurrency] = useState([]);

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        const {data} = await axios_base.get(`hotels/${hotelId}/format`, {headers: {authorization: token}})
        setCurrency(data)
    }

    return {currency, refresh}
}
