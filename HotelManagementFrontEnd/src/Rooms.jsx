import { useParams } from "react-router-dom"
import useRooms from "./hooks/useRooms"
import useHotelCurrency from "./hooks/useHotelCurrency"
import { useContext } from "react"
import { UserContext } from "./App"
import { Link } from 'react-router-dom';
import Room from "./Room"
import './css/room.css'

export default function Rooms() {
    const {pk} = useParams()
    const [token, setToken] = useContext(UserContext)
    const {rooms} = useRooms(token, pk)
    const hotelCurrency = useHotelCurrency(token, pk)
    return <>
            <div className="rooms">
                <div className="rooms-container">

                {rooms.map((room) => {
                    return <Link to="" key={room.id}><Room number={room.roomNumber} capacity={room.capacity} currency={hotelCurrency.currency} price={room.pricePerNight} available={room.available}></Room></Link>
                })}

                </div>
            </div>
            </>
}