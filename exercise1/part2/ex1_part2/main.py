import pandas as pd  # ספרייה לעבודה עם טבלאות נתונים
import os  # ספרייה לעבודה עם קבצים ותיקיות


def read_data(file_path):
    """
    קורא קובץ נתונים בהתאם לסיומת שלו (CSV, Excel או Parquet)
    """
    if file_path.endswith('.csv'):
        return pd.read_csv(file_path)
    elif file_path.endswith('.xlsx'):
        return pd.read_excel(file_path)
    elif file_path.endswith('.parquet'):
        return pd.read_parquet(file_path)
    else:
        raise ValueError("פורמט קובץ לא נתמך. יש להשתמש ב-CSV, XLSX או PARQUET.")



def clean_and_split_data(file_path, output_dir):
    """
    פונקציה זו קוראת את הקובץ, מנקה את הנתונים, ומפצלת לקבצים לפי תאריך.
    """
    df = read_data(file_path)  # קריאת הנתונים בהתאם לסוג הקובץ

    # המרת עמודת חותמת הזמן לפורמט datetime (תאריך-שעה)
    df['timestamp'] = pd.to_datetime(df['timestamp'], errors='coerce')

    # המרת הערכים המספריים לעשרוניים (float), ערכים לא תקינים יהפכו ל-NaN
    df['value'] = pd.to_numeric(df['value'], errors='coerce')

    # הסרת שורות עם ערכים חסרים וכפולים
    df = df.dropna().drop_duplicates()

    # יצירת עמודת תאריך (ללא שעה) לצורך חלוקה יומית
    df['date'] = df['timestamp'].dt.date

    # יצירת תיקיית פלט אם היא לא קיימת
    os.makedirs(output_dir, exist_ok=True)

    # פיצול הנתונים לפי תאריך ושמירתם כקבצי CSV
    for date, group in df.groupby('date'):
        group.to_csv(f"{output_dir}/{date}.csv", index=False)


def compute_hourly_averages(input_dir):
    """
    פונקציה זו מחשבת ממוצעים שעתיים לכל קובץ יומי ומאחדת את כל הממצעים לקובץ אחד.
    """
    all_hourly_averages = []  # רשימה לאיסוף הממוצעים מכל יום

    # מעבר על כל הקבצים בתיקייה
    for file in os.listdir(input_dir):
        if file.endswith(".csv"):  # רק קבצי CSV
            path = os.path.join(input_dir, file)  # הנתיב המלא לקובץ
            daily_df = pd.read_csv(path)  # קריאת הנתונים

            # המרת חותמת הזמן ל-datetime והערכים ל-float
            daily_df['timestamp'] = pd.to_datetime(daily_df['timestamp'], errors='coerce')
            daily_df['value'] = pd.to_numeric(daily_df['value'], errors='coerce')

            # הסרת שורות עם ערכים חסרים
            daily_df = daily_df.dropna()

            # יצירת עמודת שעה – עיגול כל זמן לשעה הקרובה למטה (לדוגמה 08:45 -> 08:00)
            daily_df['hour'] = daily_df['timestamp'].dt.floor('H')

            # חישוב ממוצע לכל שעה
            hourly_avg = daily_df.groupby('hour')['value'].mean().reset_index()

            # שינוי שם העמודות לפורמט אחיד
            hourly_avg.columns = ['timestamp', 'average_value']

            # הוספת הממוצעים לרשימה הכללית
            all_hourly_averages.append(hourly_avg)

    # איחוד כל הממוצעים מכל הימים
    final_df = pd.concat(all_hourly_averages)

    # אם יש כפילויות של שעות (משני קבצים) – חישוב ממוצע כולל לכל שעה
    final_result = final_df.groupby('timestamp')['average_value'].mean().reset_index()

    return final_result  # מחזיר טבלה עם ממוצעים שעתיים


def main():
    """
    פונקציית הראשית – מריצה את כל השלבים: קריאה, ניקוי, פיצול, חישוב ממוצעים ושמירה.
    """
    # בחרי כאן את הקובץ שאת רוצה להשתמש בו: Excel, CSV או Parquet
    # דוגמה ל-Parquet:
    input_file = "time_series.parquet"

    # דוגמה ל-CSV:
    # input_file = "time_series.csv"

    # תיקיית פלט לקבצים יומיים
    output_dir = "daily_chunks"

    # קובץ הפלט הסופי – טבלת ממוצעים שעתיים
    output_file = "final_hourly_averages.csv"

    print(" מנקה ומחלקת את הנתונים לפי ימים...")
    clean_and_split_data(input_file, output_dir)

    print(" מחשב ממוצעים שעתיים...")
    final_result = compute_hourly_averages(output_dir)

    # שמירת התוצאה לקובץ CSV
    final_result.to_csv(output_file, index=False)
    print(f" הקובץ '{output_file}' נוצר בהצלחה.")


# נקודת התחלה של התוכנית
if __name__ == "__main__":
    main()
