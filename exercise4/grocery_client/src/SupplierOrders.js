import { useEffect, useState } from "react";
import axios from "axios";

function SupplierOrders({ supplier }) {
    const [orders, setOrders] = useState([]); // state לאחסון ההזמנות של הספק
    const [error, setError] = useState("");   // state להודעות שגיאה

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const res = await axios.get(
                    `http://localhost:5127/api/Orders/supplier/${supplier.id}`
                ); // שליפת ההזמנות של הספק לפי ה־id
                setOrders(res.data); // שמירת ההזמנות ב־state
            } catch (err) {
                console.error(err);
                setError("שגיאה בשליפת ההזמנות"); // במקרה של שגיאה
            }
        };

        fetchOrders(); // שליפת הזמנות ברגע שהקומפוננטה נטענת
    }, [supplier]); // כל פעם ש־supplier משתנה (למשל בהתחברות), תתבצע שליפה מחדש

    const approveOrder = async (orderId) => {
        try {
            await axios.post(`http://localhost:5127/api/Orders/${orderId}/approve`);
            alert("ההזמנה אושרה!"); // הודעה על הצלחה

            // שליפה מחדש של ההזמנות לאחר האישור
            const res = await axios.get(
                `http://localhost:5127/api/Orders/supplier/${supplier.id}`
            );
            setOrders(res.data); // עדכון ההזמנות לאחר אישור
        } catch (err) {
            alert("אירעה שגיאה באישור ההזמנה"); // במקרה של שגיאה
        }
    };

    return (
        <div style={{ padding: 20 }}>
            <h2>ההזמנות שלי</h2>
            {error && <p style={{ color: "red" }}>{error}</p>} {/* הצגת שגיאה אם יש */}
            {orders.length === 0 ? (
                <p>אין הזמנות כרגע</p> // אין הזמנות להצגה
            ) : (
                orders.map((order) => (
                    <div
                        key={order.id}
                        className="order-card"
                        style={{ border: "1px solid #ccc", margin: 10, padding: 10 }}
                    >
                        <p><strong>מזהה הזמנה:</strong> {order.id}</p>
                        <p><strong>סטטוס:</strong> {order.status}</p>
                        {/* <p><strong>תאריך:</strong> {new Date(order.createdAt).toLocaleString()}</p> */}
                        {/* תאריך - ניתן להפעיל אם רוצים להציג תאריך */}
                        
                        <p><strong>פריטים:</strong></p>
                        <ul>
                            {order.items.map((item) => (
                                <li key={item.id}>
                                    {item.product.name} - {item.quantity} יחידות
                                </li>
                            ))}
                        </ul>

                        {/* כפתור אישור יוצג רק אם הסטטוס של ההזמנה הוא "חדש" */}
                        {order.status === "חדש" && (
                            <button onClick={() => approveOrder(order.id)}>אשר הזמנה</button>
                        )}
                    </div>
                ))
            )}
        </div>
    );
}

export default SupplierOrders;
