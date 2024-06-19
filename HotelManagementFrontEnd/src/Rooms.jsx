import { useParams } from "react-router-dom"
import useRooms from "./hooks/useRooms"
import useHotelCurrency from "./hooks/useHotelCurrency"
import { useContext, useState } from "react"
import { UserContext } from "./App"
import { Link } from 'react-router-dom';
import Room from "./Room"
import './css/room.css'
import Button from "./Button"

export default function Rooms() {
    const {pk} = useParams()
    const [token, setToken] = useContext(UserContext)
    const [startDate, setStartDate] = useState(new Date().toISOString().slice(0, 10));
    const [finishDate, setFinishDate] = useState(new Date().toISOString().slice(0, 10));
    const {rooms, refresh} = useRooms(token, pk, "")
    const hotelCurrency = useHotelCurrency(token, pk)

    function filterClick() {
        refresh(`/${startDate}/${finishDate}`)
    }

    return <>
            <div className="rooms">
                <div className="date-selector">
                    <input className="startDate"
                           type="date" data-date-inline-picker="true"
                           value={startDate}
                           onChange={(e) => setStartDate(e.target.value)}/>
                    <input className="finishDate" 
                           type="date" data-date-inline-picker="true"
                           value={finishDate}
                           onChange={(e) => setFinishDate(e.target.value)}/>
                    <Button onClick={filterClick}>FILTER</Button>
                </div>
                <div className="rooms-container">
                {rooms.map((room) => {
                    return <Link to={room.id.toString()} key={room.id}><Room number={room.roomNumber} capacity={room.capacity} currency={hotelCurrency.currency} price={room.pricePerNight} available={room.available}></Room></Link>
                })}
                </div>
            </div>
            </>
}