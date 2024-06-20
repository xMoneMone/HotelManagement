import { Link } from 'react-router-dom';
import './css/bottomofpagebutton.css'

export default function BottomOfPageButton(props) {
    return <Link title={props.title} to={props.linkTo}><div className="bottom-of-page-button">{props.buttonText}</div></Link>
}