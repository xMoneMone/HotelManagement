import Button from "./Button"
import { useContext, useEffect } from "react"
import { UserContext } from "./App"
import useHotels from "./hooks/useHotels"
import { Link } from 'react-router-dom';
import './css/home.css'

function Home () {
    const [token, setToken] = useContext(UserContext)
    const {hotels} = useHotels(token)

    useEffect(() => {
        document.title = 'Home | HMT';
      }, []);
      
    return  <>
                {!token && <div className="home-not-authorized">
                       
                    <Link to=""><Button>SIGN UP</Button></Link>
                    <Link to=""><Button>LOG IN</Button> </Link>
                            
                </div>}
                {token && <div className="home-authorized">
                
                    {hotels.map((hotel) => {

                        return <Link to="" key={hotel.id}>
                                    <div className="home-hotel">
                                        <div className="home-hotel-name">{hotel.name}</div>
                                        <Link to="">
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

                {token && <div className="home-not-authorized">
                       
                       <Link to="/signup"><Button>SIGN UP</Button></Link>
                       <Link to="/login"><Button>LOG IN</Button> </Link>
                               
                </div>}
            </>
}

export default Home