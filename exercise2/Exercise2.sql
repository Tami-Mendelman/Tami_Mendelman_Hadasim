create database FamilyDB
use FamilyDB

-- יצירת טבלת האנשים
CREATE TABLE Persons (
    Person_Id INT PRIMARY KEY,
    Personal_Name VARCHAR(50),
    Family_Name VARCHAR(50),
    Gender VARCHAR(10) CHECK (Gender IN ('Male', 'Female')),
    Father_Id INT NULL,
    Mother_Id INT NULL,
    Spouse_Id INT NULL,
    FOREIGN KEY (Father_Id) REFERENCES Persons(Person_Id),
    FOREIGN KEY (Mother_Id) REFERENCES Persons(Person_Id),
    FOREIGN KEY (Spouse_Id) REFERENCES Persons(Person_Id)
);

-- הכנסת נתוני בדיקה לטבלת Persons
INSERT INTO Persons (Person_Id, Personal_Name, Family_Name, Gender, Father_Id, Mother_Id, Spouse_Id) VALUES
(1, 'Yossi', 'Choen', 'Male', NULL, NULL, 2),
(2, 'Dana', 'Choen', 'Female', NULL, NULL, NULL),
(3, 'Avi', 'Choen', 'Male', 1, 2, NULL),
(4, 'Michal', 'Choen', 'Female', 1, 2, NULL),
(5, 'David', 'Levi', 'Male', NULL, NULL, 6),
(6, 'Ronit', 'Levi', 'Female', NULL, NULL, NULL);
SELECT * FROM Persons;

-- יצירת טבלת קרובי משפחה (Relatives)
CREATE TABLE RelativesFamily (
    Person_Id INT,--מזהה אדם
    Relative_Id INT,--מזהה קרוב
   Connection_Type VARCHAR(10) CHECK (Connection_Type IN ('Father', 'Mother', 'Brother', 'Sister', 'Son', 'Daughter', 'Husband', 'Wife')), 
    PRIMARY KEY (Person_Id, Relative_Id),
    FOREIGN KEY (Person_Id) REFERENCES Persons(Person_Id),
    FOREIGN KEY (Relative_Id) REFERENCES Persons(Person_Id)
);

-- קשרי אב וילד (שתי שורות - אב -> בן ובן -> אב)
-- אב -> ילד
INSERT INTO RelativesFamily (Person_Id, Relative_Id, Connection_Type)
SELECT p1.Person_Id, p2.Person_Id, 'Father'
FROM Persons p1
JOIN Persons p2 ON p1.Person_Id = p2.Father_Id
WHERE NOT EXISTS (
    SELECT 1
    FROM RelativesFamily ft
    WHERE ft.Person_Id = p1.Person_Id AND ft.Relative_Id = p2.Person_Id AND ft.Connection_Type = 'Father'
);

-- ילד -> אב (אם מדובר בבן, אז "Son", אם מדובר בבת אז "Daughter")
INSERT INTO RelativesFamily (Person_Id, Relative_Id, Connection_Type)
SELECT p2.Person_Id, p1.Person_Id, 
       CASE WHEN p2.Gender = 'Male' THEN 'Son' ELSE 'Daughter' END
FROM Persons p1
JOIN Persons p2 ON p1.Person_Id = p2.Father_Id
WHERE NOT EXISTS (
    SELECT 1
    FROM RelativesFamily ft
    WHERE ft.Person_Id = p2.Person_Id AND ft.Relative_Id = p1.Person_Id AND 
    ft.Connection_Type = CASE WHEN p2.Gender = 'Male' THEN 'Son' ELSE 'Daughter' END
);

-- קשרי אם וילד (שתי שורות - אם -> בת ובן -> אם)
-- אם -> ילד
INSERT INTO RelativesFamily (Person_Id, Relative_Id, Connection_Type)
SELECT p1.Person_Id, p2.Person_Id, 'Mother'
FROM Persons p1
JOIN Persons p2 ON p1.Person_Id = p2.Mother_Id
WHERE NOT EXISTS (
    SELECT 1
    FROM RelativesFamily ft
    WHERE ft.Person_Id = p1.Person_Id AND ft.Relative_Id = p2.Person_Id AND ft.Connection_Type = 'Mother'
);

-- ילד -> אם (אם מדובר בבן, אז "Son", אם מדובר בבת אז "Daughter")
INSERT INTO RelativesFamily (Person_Id, Relative_Id, Connection_Type)
SELECT p2.Person_Id, p1.Person_Id, 
       CASE WHEN p2.Gender = 'Male' THEN 'Son' ELSE 'Daughter' END
FROM Persons p1
JOIN Persons p2 ON p1.Person_Id = p2.Mother_Id
WHERE NOT EXISTS (
    SELECT 1
    FROM RelativesFamily ft
    WHERE ft.Person_Id = p2.Person_Id AND ft.Relative_Id = p1.Person_Id AND 
    ft.Connection_Type = CASE WHEN p2.Gender = 'Male' THEN 'Son' ELSE 'Daughter' END
);

-- קשרי אחים ואחיות (שתי שורות - אח -> אחות ואחות -> אח)
-- אח -> אחות (בהתאם למגדר של האדם)
INSERT INTO RelativesFamily (Person_Id, Relative_Id, Connection_Type)
SELECT p1.Person_Id, p2.Person_Id, 
       CASE WHEN p1.Gender = 'Male' THEN 'Brother' ELSE 'Sister' END
FROM Persons p1
JOIN Persons p2 ON p1.Father_Id = p2.Father_Id AND p1.Mother_Id = p2.Mother_Id AND p1.Person_Id != p2.Person_Id
WHERE NOT EXISTS (
    SELECT 1
    FROM RelativesFamily ft
    WHERE ft.Person_Id = p1.Person_Id AND ft.Relative_Id = p2.Person_Id AND 
    ft.Connection_Type = CASE WHEN p1.Gender = 'Male' THEN 'Brother' ELSE 'Sister' END
);

-- אחות -> אח (בהתאם למגדר של האדם)
INSERT INTO RelativesFamily (Person_Id, Relative_Id, Connection_Type)
SELECT p2.Person_Id, p1.Person_Id, 
       CASE WHEN p2.Gender = 'Male' THEN 'Brother' ELSE 'Sister' END
FROM Persons p1
JOIN Persons p2 ON p1.Father_Id = p2.Father_Id AND p1.Mother_Id = p2.Mother_Id AND p1.Person_Id != p2.Person_Id
WHERE NOT EXISTS (
    SELECT 1
    FROM RelativesFamily ft
    WHERE ft.Person_Id = p2.Person_Id AND ft.Relative_Id = p1.Person_Id AND 
    ft.Connection_Type = CASE WHEN p2.Gender = 'Male' THEN 'Brother' ELSE 'Sister' END
);

-- קשרי בני זוג (שתי שורות - Husband -> Wife ו-Wife -> Husband)
-- Husband -> Wife
INSERT INTO RelativesFamily (Person_Id, Relative_Id, Connection_Type)
SELECT p1.Person_Id, p2.Person_Id, 'Husband'
FROM Persons p1
JOIN Persons p2 ON p1.Spouse_Id = p2.Person_Id
WHERE p1.Spouse_Id IS NOT NULL
AND p1.Gender = 'Male' -- husband
AND NOT EXISTS (
    SELECT 1
    FROM RelativesFamily ft
    WHERE ft.Person_Id = p1.Person_Id AND ft.Relative_Id = p2.Person_Id AND ft.Connection_Type = 'Husband'
);

-- Wife -> Husband
INSERT INTO RelativesFamily (Person_Id, Relative_Id, Connection_Type)
SELECT p2.Person_Id, p1.Person_Id, 'Wife'
FROM Persons p1
JOIN Persons p2 ON p1.Person_Id = p2.Spouse_Id
WHERE p1.Spouse_Id IS NOT NULL
AND p2.Gender = 'Female' -- wife
AND NOT EXISTS (
    SELECT 1
    FROM RelativesFamily ft
    WHERE ft.Person_Id = p2.Person_Id AND ft.Relative_Id = p1.Person_Id AND ft.Connection_Type = 'Wife'
);






--עדכון בן/בת זוג
UPDATE P2
SET P2.Spouse_Id = P1.Person_Id
FROM Persons P1
JOIN Persons P2 ON P1.Spouse_Id = P2.Person_Id
WHERE P2.Spouse_Id IS NULL;

select* from RelativesFamily
select* from Persons





