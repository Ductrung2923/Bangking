
-- =========================
-- DATABASE: BANK SYSTEM
-- =========================

-- =========================
-- 1. Users
-- =========================
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100),
    Role INT NOT NULL, -- 1 = Admin, 0 = Staff
    IsActive BIT DEFAULT 1
);


-- =========================
-- 2. Customers (Khách hàng)
-- =========================
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    Phone NVARCHAR(15)NOT NULL,
    Address NVARCHAR(255)NOT NULL,
    IdentityNumber NVARCHAR(20) NOT NULL
);

CREATE TABLE Staff (
    StaffID INT PRIMARY KEY IDENTITY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Position NVARCHAR(255) NOT NULL,  -- 'Teller', 'Manager', 'Support'
    Department NVARCHAR(255) NOT NULL  -- 'Finance', 'Operations', 'Support',
	EmployeeCode NVARCHAR(20) UNIQUE NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender BIT NOT NULL,
	PhotoUrl NVARCHAR(255),
    HireDate DATE NOT NULL,
    Notes NVARCHAR(255);
	CreatedAt DATETIME DEFAULT GETDATE() 
);

-- =========================
-- 3. Accounts (Tài khoản ngân hàng)
-- =========================
CREATE TABLE Accounts (
    AccountID INT PRIMARY KEY IDENTITY,
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    AccountNumber NVARCHAR(20) UNIQUE,
    Balance DECIMAL(18,2) DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
);



-- =========================
-- 4. Transactions (Nạp / Rút / Chuyển nội bộ)
-- =========================
CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY IDENTITY,
    FromAccountID INT NULL FOREIGN KEY REFERENCES Accounts(AccountID),
    ToAccountID INT NULL FOREIGN KEY REFERENCES Accounts(AccountID),
    Amount DECIMAL(18,2) NOT NULL,
    Type NVARCHAR(20) NOT NULL, -- 'Deposit', 'Withdraw', 'Transfer'
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20), -- 'Pending', 'Success', 'Failed'
    StaffID INT FOREIGN KEY REFERENCES Users(UserID),
);



-- =========================
-- 5. ExternalBanks (Ngân hàng ngoài)
-- =========================
CREATE TABLE ExternalBanks (
    ExternalBankID INT PRIMARY KEY IDENTITY,
    BankName NVARCHAR(100) NOT NULL,
    SwiftCode NVARCHAR(20)
);




-- =========================
-- 6. ExternalTransfers (Chuyển khoản ra ngoài)
-- =========================
CREATE TABLE ExternalTransfers (
    ExternalTransferID INT PRIMARY KEY IDENTITY,

    -- Tài khoản chuyển đi trong hệ thống (nội bộ)
    FromAccountID INT FOREIGN KEY REFERENCES Accounts(AccountID),

    -- Tài khoản nhận thuộc ngân hàng ngoài (tham chiếu bảng ExternalAccounts)
    ExternalAccountID INT FOREIGN KEY REFERENCES ExternalAccounts(ExternalAccountID),

    -- Số tiền giao dịch
    Amount DECIMAL(18, 2) NOT NULL,

    -- Ngày thực hiện giao dịch
    TransactionDate DATETIME DEFAULT GETDATE(),

    -- Trạng thái giao dịch
    Status NVARCHAR(20), -- 'Pending', 'Success', 'Failed'

    -- Ngày tạo bản ghi
    CreatedAt DATETIME DEFAULT GETDATE(),

    -- Nhân viên xử lý giao dịch (tham chiếu Users)
    StaffID INT FOREIGN KEY REFERENCES Users(UserID)
);



CREATE TABLE ExternalAccounts (
    ExternalAccountID INT PRIMARY KEY IDENTITY,

    AccountNumber NVARCHAR(20) NOT NULL,
    AccountHolderName NVARCHAR(100) NOT NULL,
    ExternalBankID INT FOREIGN KEY REFERENCES ExternalBanks(ExternalBankID),
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT UQ_Account_Bank UNIQUE(AccountNumber, ExternalBankID)
);


ALTER TABLE ExternalTransfers
ADD Amount DECIMAL(18, 2) NOT NULL DEFAULT 0;


ALTER TABLE Customers
ADD
	DateOfBirth DATE NOT NULL,
    Gender BIT NOT NULL,
	PhotoUrl NVARCHAR(255),
	CardNumber NVARCHAR(20) NOT NULL,
	CreatedAt DATETIME DEFAULT GETDATE() 
	


	CREATE TABLE Admins (
    AdminID INT PRIMARY KEY IDENTITY,
    UserID INT UNIQUE FOREIGN KEY REFERENCES Users(UserID),
	PhotoUrl NVARCHAR(255),
	Phone	NVARCHAR(15) NOT NULL,
	Gender BIT NOT NULL,
    Position NVARCHAR(100)NOT NULL,-- Vai trò cụ thể của admin
    Notes NVARCHAR(255),            -- Ghi chú nội bộ nếu có
    CreatedAt DATETIME DEFAULT GETDATE() -- Ngày tạo bản ghi
);

CREATE TABLE TransactionHistory (
    HistoryID INT PRIMARY KEY IDENTITY,

    -- Kiểu giao dịch: 'Deposit', 'Withdraw', 'Transfer', 'ExternalTransfer'
    TransactionType NVARCHAR(20) NOT NULL,

    -- Tài khoản thực hiện (nạp/rút), hoặc tài khoản gửi (chuyển khoản)
    FromAccountID INT NULL FOREIGN KEY REFERENCES Accounts(AccountID),

    -- Tài khoản nhận (nếu là chuyển khoản nội bộ hoặc ngoài)
    ToAccountID INT NULL FOREIGN KEY REFERENCES Accounts(AccountID),

    -- Số tài khoản nhận ngoài hệ thống nếu có
    ExternalAccountNumber NVARCHAR(20) NULL,

    -- Tham chiếu đến ngân hàng ngoài nếu có
    ExternalBankID INT NULL FOREIGN KEY REFERENCES ExternalBanks(ExternalBankID),

    -- Số tiền giao dịch
    Amount DECIMAL(18,2) NOT NULL,

    -- Mô tả chi tiết (nội dung chuyển khoản, lý do nạp/rút,...)
    Description NVARCHAR(255),

    -- Người thực hiện (nhân viên), dùng UserID trong bảng Users
    StaffID INT NULL FOREIGN KEY REFERENCES Users(UserID),

    -- Trạng thái giao dịch
    Status NVARCHAR(20) NOT NULL, -- 'Pending', 'Success', 'Failed'

    -- Ngày thực hiện giao dịch
    CreatedAt DATETIME DEFAULT GETDATE(),

    -- ID của bản ghi gốc (nếu muốn tham chiếu ngược)
    SourceTransactionID INT NULL,

    -- Ghi chú bổ sung nếu cần
    Notes NVARCHAR(255)
);



CREATE TABLE Savings (
    SavingID INT PRIMARY KEY IDENTITY,

    -- Khách hàng sở hữu sổ tiết kiệm
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),

    -- Tài khoản nguồn (chuyển tiền gửi tiết kiệm)
    AccountID INT FOREIGN KEY REFERENCES Accounts(AccountID),

    -- Số tiền gửi
    Amount DECIMAL(18,2) NOT NULL,

    -- Lãi suất phần trăm trên năm (VD: 6% → nhập 6.0)
    InterestRate FLOAT NOT NULL,

    -- Kỳ hạn tính bằng tháng (VD: 3, 6, 12...)
    TermMonths INT NOT NULL,

    -- Ngày bắt đầu gửi tiết kiệm
    StartDate DATETIME DEFAULT GETDATE(),

    -- Ngày kết thúc tự động tính từ kỳ hạn
    EndDate AS DATEADD(MONTH, TermMonths, StartDate),

    -- Cách nhận lãi: 'ToAccount' = trả về tài khoản, 'AddToPrincipal' = gộp vào gốc
    ReceiveInterestMethod NVARCHAR(20) DEFAULT 'ToAccount', 

    -- Ghi nhận tổng lãi đã sinh ra
    TotalInterestEarned DECIMAL(18,2) DEFAULT 0,

    -- Ghi nhớ lần cuối tính lãi
    LastInterestCalculatedAt DATETIME NULL,

    -- Trạng thái khoản gửi: 'Active', 'Matured', 'Withdrawn'
    Status NVARCHAR(20) DEFAULT 'Active',

    -- Ghi chú tuỳ ý
    Notes NVARCHAR(255),

    -- Ngày tạo bản ghi
    CreatedAt DATETIME DEFAULT GETDATE()
);
