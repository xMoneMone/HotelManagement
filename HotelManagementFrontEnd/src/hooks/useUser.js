import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token) => {
    const [user, setUser] = useState();

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        try {
            const {data} = await axios_base.get("users", {headers: {authorization: token}})
            setUser(data)
        } catch (error) {
            const err = error
            if (err.response) {
               console.log(err.response.status)
               console.log(err.response.data)
            }
            this.handleAxiosError(error)
         }
    }

    return {user, refresh}
}
