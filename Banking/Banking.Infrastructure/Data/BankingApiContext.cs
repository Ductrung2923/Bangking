using System;
using System.Collections.Generic;
using Banking.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Data;

public partial class BankingApiContext : DbContext
{
    public BankingApiContext()
    {
    }

    public BankingApiContext(DbContextOptions<BankingApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ExternalAccount> ExternalAccounts { get; set; }

    public virtual DbSet<ExternalBank> ExternalBanks { get; set; }

    public virtual DbSet<ExternalTransfer> ExternalTransfers { get; set; }

    public virtual DbSet<Saving> Savings { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-UBB3LO2;Database=BankingAPI;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5866DF20314");

            entity.HasIndex(e => e.AccountNumber, "UQ__Accounts__BE2ACD6FF6270DF8").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountNumber).HasMaxLength(20);
            entity.Property(e => e.Balance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Accounts__Custom__5441852A");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E86780EF10");

            entity.HasIndex(e => e.UserId, "UQ__Admins__1788CCAD427B4B15").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.PhotoUrl).HasMaxLength(255);
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.UserId)
                .HasConstraintName("FK__Admins__UserID__05D8E0BE");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8E8EB1492");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D105349A956DC9").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CardNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IdentityNumber).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.PhotoUrl).HasMaxLength(255);
        });

        modelBuilder.Entity<ExternalAccount>(entity =>
        {
            entity.HasKey(e => e.ExternalAccountId).HasName("PK__External__7E67D7CB851ED186");

            entity.HasIndex(e => new { e.AccountNumber, e.ExternalBankId }, "UQ_Account_Bank").IsUnique();

            entity.Property(e => e.ExternalAccountId).HasColumnName("ExternalAccountID");
            entity.Property(e => e.AccountHolderName).HasMaxLength(100);
            entity.Property(e => e.AccountNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExternalBankId).HasColumnName("ExternalBankID");

            entity.HasOne(d => d.ExternalBank).WithMany(p => p.ExternalAccounts)
                .HasForeignKey(d => d.ExternalBankId)
                .HasConstraintName("FK__ExternalA__Exter__3E1D39E1");
        });

        modelBuilder.Entity<ExternalBank>(entity =>
        {
            entity.HasKey(e => e.ExternalBankId).HasName("PK__External__EB3D6D2B49E983F1");

            entity.Property(e => e.ExternalBankId).HasColumnName("ExternalBankID");
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.SwiftCode).HasMaxLength(20);
        });

        modelBuilder.Entity<ExternalTransfer>(entity =>
        {
            entity.HasKey(e => e.ExternalTransferId).HasName("PK__External__3CA3DF051BC1AF1F");

            entity.Property(e => e.ExternalTransferId).HasColumnName("ExternalTransferID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExternalAccountId).HasColumnName("ExternalAccountID");
            entity.Property(e => e.FromAccountId).HasColumnName("FromAccountID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ExternalAccount).WithMany(p => p.ExternalTransfers)
                .HasForeignKey(d => d.ExternalAccountId)
                .HasConstraintName("FK__ExternalT__Exter__42E1EEFE");

            entity.HasOne(d => d.FromAccount).WithMany(p => p.ExternalTransfers)
                .HasForeignKey(d => d.FromAccountId)
                .HasConstraintName("FK__ExternalT__FromA__41EDCAC5");

            entity.HasOne(d => d.Staff).WithMany(p => p.ExternalTransfers)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__ExternalT__Staff__45BE5BA9");
        });

        modelBuilder.Entity<Saving>(entity =>
        {
            entity.HasKey(e => e.SavingId).HasName("PK__Savings__E3D384D991969A3F");

            entity.Property(e => e.SavingId).HasColumnName("SavingID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EndDate)
                .HasComputedColumnSql("(dateadd(month,[TermMonths],[StartDate]))", false)
                .HasColumnType("datetime");
            entity.Property(e => e.LastInterestCalculatedAt).HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.ReceiveInterestMethod)
                .HasMaxLength(20)
                .HasDefaultValue("ToAccount");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");
            entity.Property(e => e.TotalInterestEarned)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.Savings)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Savings__Account__2A164134");

            entity.HasOne(d => d.Customer).WithMany(p => p.Savings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Savings__Custome__29221CFB");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF7A0D01A3F");

            entity.HasIndex(e => e.EmployeeCode, "UQ__Staff__1F64254836D782D1").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Department).HasMaxLength(255);
            entity.Property(e => e.EmployeeCode).HasMaxLength(20);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.PhotoUrl).HasMaxLength(255);
            entity.Property(e => e.Position).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Staff)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Staff__UserID__5070F446");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B025CF089");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FromAccountId).HasColumnName("FromAccountID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.ToAccountId).HasColumnName("ToAccountID");
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.FromAccount).WithMany(p => p.TransactionFromAccounts)
                .HasForeignKey(d => d.FromAccountId)
                .HasConstraintName("FK__Transacti__FromA__59063A47");

            entity.HasOne(d => d.Staff).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Transacti__Staff__5BE2A6F2");

            entity.HasOne(d => d.ToAccount).WithMany(p => p.TransactionToAccounts)
                .HasForeignKey(d => d.ToAccountId)
                .HasConstraintName("FK__Transacti__ToAcc__59FA5E80");
        });

        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Transact__4D7B4ADD52570B3E");

            entity.ToTable("TransactionHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ExternalAccountNumber).HasMaxLength(20);
            entity.Property(e => e.ExternalBankId).HasColumnName("ExternalBankID");
            entity.Property(e => e.FromAccountId).HasColumnName("FromAccountID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.SourceTransactionId).HasColumnName("SourceTransactionID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.ToAccountId).HasColumnName("ToAccountID");
            entity.Property(e => e.TransactionType).HasMaxLength(20);

            entity.HasOne(d => d.ExternalBank).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.ExternalBankId)
                .HasConstraintName("FK__Transacti__Exter__17F790F9");

            entity.HasOne(d => d.FromAccount).WithMany(p => p.TransactionHistoryFromAccounts)
                .HasForeignKey(d => d.FromAccountId)
                .HasConstraintName("FK__Transacti__FromA__160F4887");

            entity.HasOne(d => d.Staff).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Transacti__Staff__18EBB532");

            entity.HasOne(d => d.ToAccount).WithMany(p => p.TransactionHistoryToAccounts)
                .HasForeignKey(d => d.ToAccountId)
                .HasConstraintName("FK__Transacti__ToAcc__17036CC0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC8197EC03");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4D87704AB").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
