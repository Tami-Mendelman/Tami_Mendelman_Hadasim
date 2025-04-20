import { useState } from "react";
import axios from "axios";

function RegisterSupplier() {
    const [form, setForm] = useState({
        companyName: "",
        phoneNumber: "",
        representativeName: "",
        products: [{ name: "", price: "", minOrderQuantity: "" }]
    }); // יצירת state של טופס עם שדות הספק וסחורות ראשונית אחת

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value }); // עדכון שדות הספק (לפי שם השדה)
    };

    const handleProductChange = (index, e) => {
        const newProducts = [...form.products]; // שכפול מערך הסחורות
        newProducts[index][e.target.name] = e.target.value; // עדכון שדה של מוצר מסוים
        setForm({ ...form, products: newProducts }); // שמירה ל־state
    };

    const addProduct = () => {
        setForm({
            ...form,
            products: [...form.products, { name: "", price: "", minOrderQuantity: "" }]
        }); // הוספת מוצר ריק חדש למערך הסחורות
    };

    const handleSubmit = async (e) => {
        e.preventDefault(); // מניעת רענון הדף

        try {
            await axios.post("http://localhost:5127/api/Suppliers/register", form); // שליחת הטופס לשרת
            alert("הספק נרשם בהצלחה!");
        } catch (err) {
            console.error(err);
            alert("שגיאה בהרשמה");
        }
    };

    return (
        <div className="supplier-register-container">
            <h2>רישום ספק חדש</h2>
            <form onSubmit={handleSubmit}>
                <input
                    name="companyName"
                    placeholder="שם חברה"
                    value={form.companyName}
                    onChange={handleChange} // כל שינוי יעדכן את השדה המתאים ב־form
                />
                <input
                    name="phoneNumber"
                    placeholder="טלפון"
                    value={form.phoneNumber}
                    onChange={handleChange}
                />
                <input
                    name="representativeName"
                    placeholder="שם נציג"
                    value={form.representativeName}
                    onChange={handleChange}
                />

                <h3>סחורות</h3>
                {form.products.map((product, index) => (
                    <div key={index} style={{ display: "flex", flexDirection: "column", alignItems: "flex-end" }}>
                        <input
                            name="name"
                            placeholder="שם מוצר"
                            value={product.name}
                            onChange={(e) => handleProductChange(index, e)} // עדכון שם המוצר
                            type="text"
                        />
                        <input
                            name="price"
                            placeholder="מחיר"
                            type="number"
                            value={product.price}
                            onChange={(e) => handleProductChange(index, e)} // עדכון מחיר
                        />
                        <input
                            name="minOrderQuantity"
                            placeholder="כמות מינימלית"
                            type="number"
                            value={product.minOrderQuantity}
                            onChange={(e) => handleProductChange(index, e)} // עדכון כמות מינימלית
                        />
                        <button
                            type="button"
                            className="add-product-btn"
                            onClick={addProduct} // הוספת שורה חדשה של מוצר
                        >
                            ➕ הוסף מוצר
                        </button>
                    </div>
                ))}
                <button
                    type="submit"
                    className="submit-btn"
                >
                    רשום ספק
                </button>
            </form>
        </div>
    );
}

export default RegisterSupplier;
