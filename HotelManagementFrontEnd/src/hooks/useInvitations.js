import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token) => {
    const [invitations, setInvitations] = useState([]);

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        const {data} = await axios_base.get("user/invitations", {headers: {authorization: token}})
        setInvitations(data)
    }

    return {invitations, refresh}
}
