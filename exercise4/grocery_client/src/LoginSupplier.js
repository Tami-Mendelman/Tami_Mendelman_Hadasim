import { useState } from "react";
import axios from "axios";

function LoginSupplier({ onLogin, setShowRegister }) {
    const [phoneNumber, setPhoneNumber] = useState(""); // שומר את מספר הטלפון שהמשתמש מקליד
    const [error, setError] = useState(""); // שומר שגיאות להצגה למשתמש

    const handleLogin = async (e) => {
        e.preventDefault(); // מונע רענון של הדף כששולחים את הטופס

        const cleanedPhone = phoneNumber.trim(); // מסיר רווחים מיותרים מהטלפון
        console.log("שולחת לשרת:", { phoneNumber: cleanedPhone });

        try {
            const res = await axios.post(
                "http://localhost:5127/api/Suppliers/login", // שליחת בקשה לשרת לבדוק התחברות לפי טלפון
                { phoneNumber: cleanedPhone },
                { headers: { "Content-Type": "application/json" } }
            );

            onLogin(res.data); // מעדכן את הספק המחובר באפליקציה הראשית
        } catch (err) {
            setError("מספר טלפון לא נמצא במערכת"); // הצגת שגיאה אם ההתחברות נכשלה
        }
    };

    return (
        <div className="supplier-register-container">
            <h2>התחברות ספק</h2>

            <form onSubmit={handleLogin}>
                <input
                    type="text"
                    placeholder="מספר טלפון"
                    value={phoneNumber}
                    onChange={(e) => setPhoneNumber(e.target.value)} // עדכון סטייט בכל שינוי בטלפון
                />
                <br /><br />
                <button type="submit">התחבר</button>
            </form>
        
            {error && <p style={{ color: "red" }}>{error}</p>} 

            <p style={{ marginTop: "10px" }}>
                עדיין לא רשום?{" "}
                <button
                    type="button"
                    className="link-button"
                    onClick={() => setShowRegister(true)} // מעביר למסך הרשמה
                >
                    להרשמה
                </button>
            </p>
        </div>
    );
}

export default LoginSupplier;
