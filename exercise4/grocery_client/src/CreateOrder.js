import { useEffect, useState } from "react";
import axios from "axios";

function CreateOrder() {
  const [suppliers, setSuppliers] = useState([]); // שמירת רשימת הספקים
  const [selectedSupplierId, setSelectedSupplierId] = useState(""); // מזהה הספק שנבחר
  const [products, setProducts] = useState([]); // מוצרים של הספק שנבחר
  const [quantities, setQuantities] = useState({}); // כמויות להזמנה לפי productId
  const [success, setSuccess] = useState(""); // הודעת הצלחה
  const [error, setError] = useState(""); // הודעת שגיאה

  // שליפת כל הספקים ברגע שטוען הקומפוננט
  useEffect(() => {
    const fetchSuppliers = async () => {
      try {
        const res = await axios.get("http://localhost:5127/api/Suppliers");
        setSuppliers(res.data); // שמירת הספקים בסטייט
      } catch (err) {
        setError("שגיאה בשליפת הספקים");
      }
    };

    fetchSuppliers();
  }, []);

  // שליפת מוצרים רק אם נבחר ספק
  useEffect(() => {
    if (!selectedSupplierId) return;

    const fetchProducts = async () => {
      try {
        const res = await axios.get(
          `http://localhost:5127/api/Products/supplier/${selectedSupplierId}`
        );
        setProducts(res.data); // מוצרים של הספק הנבחר
        setQuantities({}); // איפוס כמויות כשמחליפים ספק
      } catch (err) {
        setError("שגיאה בשליפת המוצרים");
      }
    };

    fetchProducts();
  }, [selectedSupplierId]); // רץ רק כשמזהה הספק משתנה

  // שינוי כמויות לפי מוצר
  const handleQuantityChange = (productId, value) => {
    setQuantities({ ...quantities, [productId]: parseInt(value) || 0 });
  };

  // שליחת טופס ההזמנה
  const handleSubmit = async (e) => {
    e.preventDefault();

    const items = Object.entries(quantities)
      .filter(([_, quantity]) => quantity > 0) // מסנן פריטים עם כמות > 0
      .map(([productId, quantity]) => ({
        productId: parseInt(productId),
        quantity
      }));

    // בדיקת תקינות לפני שליחה
    if (items.length === 0 || !selectedSupplierId) {
      setError("בחר ספק והכנס כמויות תקינות");
      return;
    }

    try {
      await axios.post("http://localhost:5127/api/Orders", {
        supplierId: selectedSupplierId,
        items
      });

      // איפוס שדות והודעת הצלחה
      setSuccess("ההזמנה נשלחה בהצלחה!");
      setError("");
      setQuantities({});
      setProducts([]);
      setSelectedSupplierId("");
    } catch (err) {
      setError("שגיאה בשליחת ההזמנה");
    }
  };

  return (
    <div className="glass-box">
      <h2>הזמנת סחורה</h2>

      {error && <p style={{ color: "red" }}>{error}</p>}
      {success && <p style={{ color: "green" }}>{success}</p>}

      <form onSubmit={handleSubmit}>
        <label>בחר ספק:</label><br />
        <select
          value={selectedSupplierId}
          onChange={(e) => setSelectedSupplierId(e.target.value)} // בחירת ספק
        >
          <option value="">-- בחר --</option>
          {suppliers.map((s) => (
            <option key={s.id} value={s.id}>
              {s.companyName} ({s.phoneNumber})
            </option>
          ))}
        </select>

        {products.length > 0 && (
          <>
            <h3>בחר מוצרים להזמנה:</h3>
            {products.map((p) => (
              <div key={p.id}>
                {/* הצגת שם, מחיר וכמות מינימום של כל מוצר */}
                <label>{p.name} (₪{p.price}, מינימום: {p.minOrderQuantity})</label>
                <input
                  type="number"
                  min="0"
                  value={quantities[p.id] || ""}
                  onChange={(e) => handleQuantityChange(p.id, e.target.value)} // שינוי כמות
                />
              </div>
            ))}
          </>
        )}

        <br />
        <button type="submit" className="action-btn">שלח הזמנה</button>
      </form>
    </div>
  );
}

export default CreateOrder;
