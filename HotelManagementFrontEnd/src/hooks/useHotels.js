import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default () => {
    const [hotels, setHotels] = useState([]);

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        const {data} = await axios_base.get("user/hotels", {headers: {authorization: "bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJPd25lciIsImV4cCI6MTcyMDcxMTA1M30.aecVAbSUzzes7zq3UVuQhxdKH8yzGryeEneo5l0fXcc"}})
        setHotels(data)
    }

    return {hotels, refresh}
}
