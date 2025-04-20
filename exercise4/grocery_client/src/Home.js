import React from "react";
import backgroundImg from "./assets/shop.png.png"; // הנתיב לפי מיקום הקובץ

function Home({ setRole }) {
  return (
    <div style={styles.container}>
      <img src={backgroundImg} alt="store" style={styles.background} />
      <button onClick={() => setRole("supplier")} style={{ ...styles.button, ...styles.left }}>
         ספק
      </button>
      <button onClick={() => setRole("owner")} style={{ ...styles.button, ...styles.right }}>
         בעל המכולת
      </button>
    </div>
  );
}

const styles = {
  container: {
    position: "relative",
    width: "100vw",
    height: "100vh",
    overflow: "hidden",
  },
  background: {
    width: "100%",
    height: "100%",
    objectFit: "cover",
    position: "absolute",
    top: 0,
    left: 0,
    zIndex: 0,
  },
  button: {
    position: "absolute",
    zIndex: 2,
    padding: "12px 24px",
    fontSize: "18px",
    backgroundColor: "rgba(255, 255, 255, 0.8)",
    border: "2px solid #333",
    borderRadius: "12px",
    cursor: "pointer",
    color: "black" 
  },
    left: {
    top: "50%",     
    left: "25%",
    transform: "translate(-50%, -50%)",
  },
  right: {
    top: "50%",    
    left: "75%",
    transform: "translate(-50%, -50%)",
  },
};

export default Home;
