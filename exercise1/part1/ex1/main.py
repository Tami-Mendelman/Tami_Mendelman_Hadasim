import os
import pandas as pd
import re
from collections import Counter


# שלב 1: חילוץ קודי השגיאה מקובץ Excel ושמירתם ב-logs.txt
def extract_error_codes_from_excel(excel_path, sheet_name='גיליון1', output_txt_path='logs.txt'):
    df = pd.read_excel(excel_path, sheet_name=sheet_name, header=None)

    # הנחה: כל שורה מכילה טקסט שבו מופיע קוד שגיאה בסגנון: Error: CODE1234
    error_pattern = re.compile(r'Error:\s*(\S+)')
    error_codes = []

    for row in df[0]:  # רק העמודה הראשונה
        match = error_pattern.search(str(row))
        if match:
            error_codes.append(match.group(1))

    # שמירה לקובץ טקסט: כל שורה קוד שגיאה
    with open(output_txt_path, 'w') as f:
        for code in error_codes:
            f.write(code + '\n')

    print(f"הקובץ נוצר בהצלחה: {output_txt_path}")


# שלב 2: פיצול קובץ logs.txt לחלקים קטנים
def split_log_file(filename, lines_per_chunk=1_000_000):
    chunk_files = []
    with open(filename, 'r') as infile:
        count = 0
        chunk_index = 0
        outfile = open(f'chunk_{chunk_index}.txt', 'w')
        chunk_files.append(outfile.name)

        for line in infile:
            if count >= lines_per_chunk:
                outfile.close()
                chunk_index += 1
                outfile = open(f'chunk_{chunk_index}.txt', 'w')
                chunk_files.append(outfile.name)
                count = 0
            outfile.write(line)
            count += 1

        outfile.close()
    return chunk_files


# שלב 3: ספירת שכיחות לכל חלק
def count_errors_in_chunk(file_path):
    counter = Counter()
    with open(file_path, 'r') as f:
        for line in f:
            error_code = line.strip()
            counter[error_code] += 1
    return counter


# שלב 4: מיזוג הספירות מכל החלקים
def merge_counters(counters):
    total = Counter()
    for c in counters:
        total.update(c)
    return total


# שלב 5: מציאת N קודי השגיאה השכיחים ביותר
def get_top_n_errors(counter, n):
    return counter.most_common(n)


# שלב 6: תהליך מלא
def process_log_file(excel_path, top_n, lines_per_chunk=1_000_000):
    # חילוץ קודי השגיאה מקובץ ה-Excel
    extract_error_codes_from_excel(excel_path, output_txt_path='logs.txt')

    # פיצול קובץ logs.txt לחלקים קטנים
    chunk_files = split_log_file('logs.txt', lines_per_chunk)
    counters = [count_errors_in_chunk(chunk) for chunk in chunk_files]

    # מיזוג הספירות
    total_counter = merge_counters(counters)

    # מציאת N השגיאות השכיחות ביותר
    top_errors = get_top_n_errors(total_counter, top_n)

    # ניקוי קבצי chunk זמניים
    for chunk in chunk_files:
        os.remove(chunk)

    return top_errors


# הרצת הקוד
if __name__ == "__main__":
    excel_file_path = "logs.xlsx"
    n = int(input("הכנס את מספר קודי השגיאה השכיחים להצגה (N): "))
    result = process_log_file(excel_file_path, n)
    print("\nקודי השגיאה השכיחים ביותר:")
    for code, count in result:
        print(f"{code}: {count}")
