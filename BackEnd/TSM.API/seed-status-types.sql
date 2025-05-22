IF NOT EXISTS (SELECT TOP 1 * FROM StatusTypes)
BEGIN
    INSERT INTO StatusTypes (Name) VALUES 
        ('Available'),
        ('In-Use'),
        ('Deprecated'),
        ('Planned');
END 