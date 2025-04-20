import { useState } from "react";
import Home from "./Home";
import RegisterSupplier from "./RegisterSupplier";
import LoginSupplier from "./LoginSupplier";
import SupplierOrders from "./SupplierOrders";
import CreateOrder from "./CreateOrder";
import AllOrders from "./AllOrders";
import makoletImage from "./assets/makolet.jpg"; // תמונה לרקע של הממשק

function App() {
  const [role, setRole] = useState(null); // קובע האם המשתמש הוא "owner" או "supplier"
  const [loggedSupplier, setLoggedSupplier] = useState(null); // שמירת הספק המחובר
  const [showRegister, setShowRegister] = useState(false); // האם להציג טופס הרשמה
  const [view, setView] = useState("orders"); // תצוגה נוכחית (בין אם "create", "orders" או "admin")

  if (!role) return <Home setRole={setRole} />; // אם המשתמש לא בחר תפקיד - הצג את דף הבית

  // ספק
  if (role === "supplier") {
    if (!loggedSupplier) {
      return (
        <div style={{ ...styles.pageWrapper, ...styles.ownerBackground }}>
          <button onClick={() => setRole(null)} className="logout-btn">התנתק</button>
          <div style={styles.ownerContent}>
            {!showRegister ? (
              <>
                {/* הצגת קומפוננטת התחברות עם יכולת לעבור להרשמה */}
                <LoginSupplier onLogin={setLoggedSupplier} setShowRegister={setShowRegister} />
              </>
            ) : (
              <>
                {/* הצגת קומפוננטת הרשמה */}
                <RegisterSupplier />
                <div style={{ marginTop: "20px" }}>
                  <span>כבר יש לך משתמש? </span>
                  <button onClick={() => setShowRegister(false)} style={styles.link}>
                    חזור להתחברות
                  </button>
                </div>
              </>
            )}
          </div>
        </div>
      );
    }
  
    return (
      <div  style={{ ...styles.pageWrapper, ...styles.ownerBackground }}>
        <button onClick={() => setLoggedSupplier(null)} className="logout-btn">התנתק</button>

        <div style={styles.ownerContent}>
          <h2 style={{ fontSize: "28px" }}>שלום, {loggedSupplier.representativeName}!</h2>

          <div style={styles.ownerButtons}>
            <button onClick={() => setView("orders")}>ההזמנות שלי</button>
          </div>

          {/* הצגת ההזמנות של הספק */}
          {view === "orders" && <SupplierOrders supplier={loggedSupplier} />}
        </div>
      </div>
    );
  }

  // בעל המכולת
  if (role === "owner") {
    return (
      <div style={{ ...styles.pageWrapper, ...styles.ownerBackground }}>
        <button onClick={() => setRole(null)} className="logout-btn">התנתק</button>

        <div style={styles.ownerContent}>
          <h1 style={styles.bigHeader}>שלום, בעל המכולת!</h1>

          <div style={styles.ownerButtons}>
            {/* מעבר בין תצוגת שליחת הזמנה לכל ההזמנות */}
            <button onClick={() => setView("create")}>שלח הזמנה</button>
            <button onClick={() => setView("admin")}>כל ההזמנות</button>
          </div>

          {/* תצוגה דינמית לפי הבחירה */}
          {view === "create" && <CreateOrder />}
          {view === "admin" && <AllOrders />}
        </div>
      </div>
    );
  }

  return null; // fallback - לא אמור לקרות אלא אם יש תקלה
}

const styles = {
  pageWrapper: {
    position: "relative",
    minHeight: "100vh",
    paddingTop: "20px",
    direction: "rtl", // התאמת הכיוון לעברית
  },
  logoutBtn: {
    // כפתור התנתקות שממוקם בפינה
    position: "absolute",
    top: "10px",
    right: "10px",
    padding: "8px 16px",
    fontSize: "14px",
    backgroundColor: "#ffecec",
    border: "1px solid #cc0000",
    borderRadius: "8px",
    cursor: "pointer",
  },
  link: {
    color: "blue",
    background: "none",
    border: "none",
    cursor: "pointer",
    textDecoration: "underline", // נראה כמו קישור
  },
  ownerBackground: {
    backgroundImage: `url(${makoletImage})`, // רקע עם תמונה
    backgroundSize: "100% auto",
    backgroundRepeat: "no-repeat",
    backgroundPosition: "top center",
    backgroundAttachment: "fixed",
    minHeight: "100vh",
    paddingTop: "80px",
    textAlign: "center",
  },
  ownerContent: {
    paddingTop: "100px",
    color: "black",
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    gap: "20px",
  },
  ownerButtons: {
    display: "flex",
    flexDirection: "row",
    gap: "20px",
    justifyContent: "center",
    marginTop: "20px",
  },
};

export default App;
