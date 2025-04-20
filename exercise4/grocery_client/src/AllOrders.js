import { useEffect, useState } from "react";
import axios from "axios";

function AllOrders() {
    const [orders, setOrders] = useState([]);
    const [error, setError] = useState("");

    const fetchOrders = async () => {
        try {
            const res = await axios.get("http://localhost:5127/api/Orders");
            setOrders(res.data); // שמירת ההזמנות שהגיעו מהשרת
        } catch (err) {
            setError("שגיאה בשליפת ההזמנות"); // שמירת הודעת שגיאה במידה והבקשה נכשלה
        }
    };

    useEffect(() => {
        fetchOrders(); // קריאה לפונקציית שליפת ההזמנות רק כשהקומפוננטה נטענת
    }, []);

    const completeOrder = async (orderId) => {
        try {
            await axios.post(`http://localhost:5127/api/Orders/${orderId}/complete`); // קריאה לשרת לעדכון סטטוס הזמנה
            alert("הזמנה סומנה כהושלמה!");
            fetchOrders(); // רענון רשימת ההזמנות מהשרת לאחר העדכון
        } catch (err) {
            alert("שגיאה בסימון ההזמנה כהושלמה");
        }
    };

    return (
        <div style={{ padding: 20 }}>
            <h2>כל ההזמנות במערכת</h2>
            {error && <p style={{ color: "red" }}>{error}</p>} {/* הצגת שגיאה אם יש */}
            {orders.map((order) => (
                <div key={order.id} className="order-card" style={{ border: "1px solid #ccc", margin: 10, padding: 10 }}>
                    <p><strong>מספר הזמנה:</strong> {order.id}</p>
                    <p><strong>סטטוס:</strong> {order.status}</p>
                    <p><strong>ספק:</strong> {order.supplier?.companyName || "לא ידוע"}</p> {/* בדיקה אם הספק קיים */}
                    <p><strong>פריטים:</strong></p>
                    <ul>
                        {order.items.map((item) => (
                            <li key={item.id}>
                                {item.product?.name || "מוצר לא ידוע"} – {item.quantity} יחידות
                            </li>
                        ))}
                    </ul>
                    {order.status === "בתהליך" && ( // כפתור פעיל רק אם ההזמנה עדיין בתהליך
                        <button onClick={() => completeOrder(order.id)}>אשר קבלת הזמנה</button>
                    )}
                </div>
            ))}
        </div>
    );
}

export default AllOrders;
