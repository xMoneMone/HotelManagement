import Button from "./Button"
import { useContext, useEffect } from "react"
import { UserContext } from "./App"
import useHotels from "./hooks/useHotels"
import { Link } from 'react-router-dom';
import './css/home.css'
import BottomOfPageButton from "./BottomOfPageButton";

export default function Home () {
    const [token, setToken] = useContext(UserContext)
    const {hotels} = useHotels(token)

    useEffect(() => {
        document.title = 'Home | HMT';
      }, []);
      
    return  <>
                {token && <BottomOfPageButton title="Add hotel" link="" buttonText="+"></BottomOfPageButton>}
                {token && hotels && <div className="home-authorized">
                    <div className="home-authorized-title">
                        <div className="home-authorized-title-line line"></div>
                        <h1>Hotels</h1>
                        <div className="home-authorized-title-line line"></div>
                    </div>
                    {hotels.map((hotel) => {
                        return <Link to={`/hotels/${hotel.id}/rooms`} key={hotel.id}>
                                    <div className="home-hotel">
                                        <div className="home-hotel-name">
                                        <svg  className="icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path d="M0 32C0 14.3 14.3 0 32 0H480c17.7 0 32 14.3 32 32s-14.3 32-32 32V448c17.7 0 32 14.3 32 32s-14.3 32-32 32H304V464c0-26.5-21.5-48-48-48s-48 21.5-48 48v48H32c-17.7 0-32-14.3-32-32s14.3-32 32-32V64C14.3 64 0 49.7 0 32zm96 80v32c0 8.8 7.2 16 16 16h32c8.8 0 16-7.2 16-16V112c0-8.8-7.2-16-16-16H112c-8.8 0-16 7.2-16 16zM240 96c-8.8 0-16 7.2-16 16v32c0 8.8 7.2 16 16 16h32c8.8 0 16-7.2 16-16V112c0-8.8-7.2-16-16-16H240zm112 16v32c0 8.8 7.2 16 16 16h32c8.8 0 16-7.2 16-16V112c0-8.8-7.2-16-16-16H368c-8.8 0-16 7.2-16 16zM112 192c-8.8 0-16 7.2-16 16v32c0 8.8 7.2 16 16 16h32c8.8 0 16-7.2 16-16V208c0-8.8-7.2-16-16-16H112zm112 16v32c0 8.8 7.2 16 16 16h32c8.8 0 16-7.2 16-16V208c0-8.8-7.2-16-16-16H240c-8.8 0-16 7.2-16 16zm144-16c-8.8 0-16 7.2-16 16v32c0 8.8 7.2 16 16 16h32c8.8 0 16-7.2 16-16V208c0-8.8-7.2-16-16-16H368zM328 384c13.3 0 24.3-10.9 21-23.8c-10.6-41.5-48.2-72.2-93-72.2s-82.5 30.7-93 72.2c-3.3 12.8 7.8 23.8 21 23.8H328z"/></svg>
                                            {hotel.name}
                                            </div>
                                        <Link title="Hotel settings" to="">
                                            <div className="home-hotel-edit">
                                                {hotel.userIsOwner &&
                                                <svg className="icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512"><path d="M0 96C0 78.3 14.3 64 32 64H416c17.7 0 32 14.3 32 32s-14.3 32-32 32H32C14.3 128 0 113.7 0 96zM0 256c0-17.7 14.3-32 32-32H416c17.7 0 32 14.3 32 32s-14.3 32-32 32H32c-17.7 0-32-14.3-32-32zM448 416c0 17.7-14.3 32-32 32H32c-17.7 0-32-14.3-32-32s14.3-32 32-32H416c17.7 0 32 14.3 32 32z"/></svg>
                                                }
                                            </div>
                                        </Link>
                                    </div>
                                </Link>

                    })}
                </div>} 

                {!token && <div className="home-not-authorized">
                       
                       <Link className="signup-button" to="/signup"><Button>SIGN UP</Button></Link>
                       <Link className="login-button" to="/login"><Button>LOG IN</Button> </Link>
                               
                </div>}
            </>
}
