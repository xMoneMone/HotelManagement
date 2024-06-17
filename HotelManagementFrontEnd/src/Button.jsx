import './css/button.css'

export default function Button({children, onClick = () => {}}) {
    return <button className="dark-button" onClick={onClick}>{children}</button>
}