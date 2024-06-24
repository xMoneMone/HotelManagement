import { useContext, useEffect } from "react"
import useRoomDetails from "./hooks/useRoomDetails"
import { UserContext } from "./App"
import { Link, useParams } from "react-router-dom"
import useHotelCurrency from "./hooks/useHotelCurrency"
import useBookings from "./hooks/useBookings"
import BottomOfPageButton from "./BottomOfPageButton"
import './css/roomdetails.css'
import Booking from "./Booking"

export default function RoomDetails(){
    const [token, setToken] = useContext(UserContext)   
    const {hotelId, roomId} = useParams()
    const {room} = useRoomDetails(token, hotelId, roomId)
    const beds = room.beds;
    const {currency} = useHotelCurrency(token, hotelId)
    const {bookings} = useBookings(token, hotelId, roomId)
    let key = 1;
    
    useEffect(() => {
        document.title = `Room ${room.roomNumber} | HMT`;
    }, []);

    return <>
            <BottomOfPageButton buttonText="+"></BottomOfPageButton>
            <div className="room-details-page">
                <div className="room-details">
                    <div className="room-details-room-number">
                        <div>{room.roomNumber}</div>
                    </div>
                    <div className="room-details-room-info">
                        <div className="room-details-beds">
                            {beds && beds.map((bed) => {
                                key += 1
                                return <div key={key} title={bed.bedType} className="room-details-bed>">
                                        {!bed.bedType.includes("couch") ? 
                                        <svg className="icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 512"><path d="M32 32c17.7 0 32 14.3 32 32V320H288V160c0-17.7 14.3-32 32-32H544c53 0 96 43 96 96V448c0 17.7-14.3 32-32 32s-32-14.3-32-32V416H352 320 64v32c0 17.7-14.3 32-32 32s-32-14.3-32-32V64C0 46.3 14.3 32 32 32zm144 96a80 80 0 1 1 0 160 80 80 0 1 1 0-160z"/></svg>
                                        : <svg className="icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 512"><path d="M64 160C64 89.3 121.3 32 192 32H448c70.7 0 128 57.3 128 128v33.6c-36.5 7.4-64 39.7-64 78.4v48H128V272c0-38.7-27.5-71-64-78.4V160zM544 272c0-20.9 13.4-38.7 32-45.3c5-1.8 10.4-2.7 16-2.7c26.5 0 48 21.5 48 48V448c0 17.7-14.3 32-32 32H576c-17.7 0-32-14.3-32-32H96c0 17.7-14.3 32-32 32H32c-17.7 0-32-14.3-32-32V272c0-26.5 21.5-48 48-48c5.6 0 11 1 16 2.7c18.6 6.6 32 24.4 32 45.3v48 32h32H512h32V320 272z"/></svg>}
                                        {bed.capacity}
                                        </div>
                            })
                            }
                        </div>
                        <div className="room-details-notes">
                            {room.notes}
                        </div>
                        <div className="room-details-price">
                            {currency && room.pricePerNight && currency.replace("*", room.pricePerNight.toString())}
                        </div>
                    </div>
                    <div className="bookings-title home-authorized-title">
                        <div className="home-authorized-title-line line"></div>
                        <h1>Bookings</h1>
                        <div className="home-authorized-title-line line"></div>
                    </div>
                    <div className="room-details-bookings">
                        {bookings && bookings.map((booking) => {
                            return <Link key={booking.id} to={`bookings/${booking.id}`}><Booking name={`${booking.firstName} ${booking.lastName}`} date={`${booking.startDate.substring(0, 10)} - ${booking.endDate.substring(0, 10)}`} downPaid={booking.downPaymentPaid} fullPaid={booking.fullPaymentPaid}></Booking></Link>
                        })}
                        {bookings.length == 0 && 
                        <div className="no-bookings">No bookings</div>
                        }
                    </div>
                </div>
            </div>
           </>
}