export default function Room(props) {
    return <>
            <div className="room">
                <div className={props.available ? "room-number room-available" : "room-number room-taken"}>
                    {props.number}
                </div>
                <div className="room-info">
                    <div className="room-info-capacity">
                    <svg className="icon bed-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 512"><path d="M32 32c17.7 0 32 14.3 32 32V320H288V160c0-17.7 14.3-32 32-32H544c53 0 96 43 96 96V448c0 17.7-14.3 32-32 32s-32-14.3-32-32V416H352 320 64v32c0 17.7-14.3 32-32 32s-32-14.3-32-32V64C0 46.3 14.3 32 32 32zm144 96a80 80 0 1 1 0 160 80 80 0 1 1 0-160z"/></svg>
                        {props.capacity}
                    </div>
                    <div className="room-info-price">
                        {props.currency.replace("*", props.price.toString())}
                    </div>
                </div>
            </div>
            </>
}