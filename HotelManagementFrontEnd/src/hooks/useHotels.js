import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token) => {
    const [hotels, setHotels] = useState([]);

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        const {data} = await axios_base.get("user/hotels", {headers: token})
        setHotels(data)
    }

    return {hotels, refresh}
}
