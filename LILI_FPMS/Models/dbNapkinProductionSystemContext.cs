using System;
using LILI_FPMS.Temp_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
namespace LILI_FPMS.Models
{
    public partial class dbFormulationProductionSystemContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public dbFormulationProductionSystemContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public virtual DbSet<TblEmployee> TblEmployee { get; set; }
        public virtual DbSet<TblEmployeeEducationalDetail> TblEmployeeEducationalDetail { get; set; }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<MenuMaster> MenuMaster { get; set; }
        public virtual DbSet<TblMenu> TblMenu { get; set; }

        public virtual DbSet<TblProcessOrder> TblProcessOrder { get; set; }
        public virtual DbSet<TblProcessOrderDetail> TblProcessOrderDetail { get; set; }
        public virtual DbSet<TblRequisitionLine> TblRequisitionLine { get; set; }
        public virtual DbSet<TblRequisitionMachineSetup> TblRequisitionMachineSetup { get; set; }
        public virtual DbSet<TblEmpGrade> TblEmpGrade { get; set; }
        public virtual DbSet<TblEmployeeExpert> TblEmployeeExpert { get; set; }
        public virtual DbSet<TblExpertArea> TblExpertArea { get; set; }

        public virtual DbSet<TblCountry> TblCountry { get; set; }

        public virtual DbSet<ReportSubmission> ReportSubmission { get; set; }
        public virtual DbSet<ReportSupplierSubmission> ReportSupplierSubmission { get; set; }
        public virtual DbSet<ReportApprovalFormForImport> ReportApprovalFormForImport { get; set; }

        public virtual DbSet<TblLineSetup> TblLineSetup { get; set; }
        public virtual DbSet<TblMachineSetup> TblMachineSetup { get; set; }
        public virtual DbSet<TblShiftSetup> TblShiftSetup { get; set; }
        public virtual DbSet<TblMonthlyPlanning> TblMonthlyPlanning { get; set; }
        public virtual DbSet<TblMonthlyPlanningDetail> TblMonthlyPlanningDetail { get; set; }
        public virtual DbSet<TblProductionProcess> TblProductionProcess { get; set; }
        public virtual DbSet<TblProductionProcessDetail> TblProductionProcessDetail { get; set; }
        public virtual DbSet<GetTblProductionProcessDetail> GetTblProductionProcessDetail { get; set; }
        public virtual DbSet<TblProductionApprovalStatus> TblProductionApprovalStatus { get; set; }

        /*SP VMs*/
        public virtual DbSet<VMAllCurrentSTINo> VMAllCurrentSTINo { get; set; }
        public virtual DbSet<VMRequisitionSerachReturn> VMRequisitionSerachReturn { get; set; }  
        public virtual DbSet<VMGetAllSFGTransfer> VMGetAllSFGTransfer { get; set; }   
        public virtual DbSet<VMRebSearchProcessOrder> VMRebSearchProcessOrder { get; set; }
        public virtual DbSet<GetRebSearchProduct> GetRebSearchProduct { get; set; }
        public virtual DbSet<GetRebRequisitionSearch> GetRebRequisitionSearch { get; set; }
        public virtual DbSet<GetSearchRequisitionList> GetSearchRequisitionList { get; set; }
        public virtual DbSet<GetSearchProcessOrderList> GetSearchProcessOrderList { get; set; }
        public virtual DbSet<GetGRNList> GetGRNList { get; set; }
        public virtual DbSet<GetRequisitionWiseProcessList> GetRequisitionWiseProcessList { get; set; }
        public virtual DbSet<GetRebReturnIndex> GetRebReturnIndex { get; set; }
        public virtual DbSet<GetRequisitionIssueDetail> GetRequisitionIssueDetail { get; set; }

        public virtual DbSet<GetProductWiseBOMDetail> GetProductWiseBOMDetail { get; set; }
        public virtual DbSet<GetSTIFGDetialsVM> GetSTIFGDetialsVM { get; set; }

        public virtual DbSet<GetFGTNPendingList> GetFGTNPendingList { get; set; }

        public virtual DbSet<VMSetProductInfo> VMSetProductInfo { get; set; }
        
        public virtual DbSet <VMAllRebQCIndex> VMAllRebQCIndex { get; set; }
        /*SP VMs*/
        public virtual DbSet<TblQcparameter> TblQcparameter { get; set; }

        public virtual DbSet<TblQc> TblQc { get; set; }
        public virtual DbSet<TblQcdetails> TblQcdetails { get; set; }
        public virtual DbSet<TblQcparameterType> TblQcparameterType { get; set; }

        public virtual DbSet<TblRequisition> TblRequisition { get; set; }
        public virtual DbSet<TblRequisitionDetail> TblRequisitionDetail { get; set; }
        public virtual DbSet<TblReturn> TblReturn { get; set; }
        public virtual DbSet<TblReturnDetails> TblReturnDetails { get; set; }

        public virtual DbSet<TblRebSfgtransfer> TblRebSfgtransfer { get; set; }
        public virtual DbSet<TblRebSfgtransferDetail> TblRebSfgtransferDetail { get; set; }

        public virtual DbSet<View_BOM> View_BOM { get; set; }
        public virtual DbSet<View_BOMDetail> View_BOMDetail { get; set; }

        public virtual DbSet<View_Material> View_Material { get; set; }

        public virtual DbSet<View_IssueQuantity> View_IssueQuantity { get; set; }

        public virtual DbSet<View_Product> View_Product { get; set; }


        public virtual DbSet<tblRMRate> tblRMRate { get; set; }
        public virtual DbSet<TblRequisitionApprovalStatus> TblRequisitionApprovalStatus { get; set; }
        public virtual DbSet<TblFloorStock> TblFloorStock { get; set; }

       

        public virtual DbSet<TblManufacturingBreakDownCause> TblManufacturingBreakDownCause { get; set; }
        public virtual DbSet<TblManufacturingLine> TblManufacturingLine { get; set; }
        public virtual DbSet<TblManufacturingMachine> TblManufacturingMachine { get; set; }
        public virtual DbSet<TblManufacturingShift> TblManufacturingShift { get; set; }
        public virtual DbSet<TblPackingDetailLine> TblPackingDetailLine { get; set; }
        public virtual DbSet<TblPackingDetailMachine> TblPackingDetailMachine { get; set; }
        public virtual DbSet<TblPackingDetailShift> TblPackingDetailShift { get; set; }
        public virtual DbSet<SetProductInfoWithPlanning> SetProductInfoWithPlanning { get; set; }
        public virtual DbSet<SetProductInfo> SetProductInfo { get; set; }
        public virtual DbSet<UpdateFloorScockFromForductionQC> UpdateFloorScockFromForductionQC { get; set; } 
        public virtual DbSet<UpdateFloorStockRebQC> UpdateFloorStockRebQC { get; set; }
        public virtual DbSet<TblFgtn> TblFgtn { get; set; }
        public virtual DbSet<TblFgtndetails> TblFgtndetails { get; set; }

      
        public virtual DbSet<TblFgtransferLocation> TblFgtransferLocation { get; set; }
        public virtual DbSet<TblStandardManHourSetup> TblStandardManHourSetup { get; set; }
        public virtual DbSet<TblMachineName> TblMachineName { get; set; }
        public virtual DbSet<TblMaintenanceInformation> TblMaintenanceInformation { get; set; }


        public virtual DbSet<TblCodingDetailLine> TblCodingDetailLine { get; set; }
        public virtual DbSet<TblCodingDetailMachine> TblCodingDetailMachine { get; set; }
        public virtual DbSet<TblCodingDetailShift> TblCodingDetailShift { get; set; }
        public virtual DbSet<TblVisibility> TblVisibility { get; set; }
        public virtual DbSet<TblSubBusiness> TblSubBusiness { get; set; }

        public virtual DbSet<TblProductionManPower> TblProductionManPower { get; set; }
        public virtual DbSet<TblManufacturingManPower> TblManufacturingManPower { get; set; }
        public virtual DbSet<TblPackingManPower> TblPackingManPower { get; set; }

        public virtual DbSet<TblRebRequisitionType> TblRebRequisitionType { get; set; }
        public virtual DbSet<TblRebManufacturingBreakDownCause> TblRebManufacturingBreakDownCause { get; set; }
        public virtual DbSet<TblRebManufacturingLine> TblRebManufacturingLine { get; set; }
        public virtual DbSet<TblRebManufacturingMachine> TblRebManufacturingMachine { get; set; }
        public virtual DbSet<TblRebManufacturingManPower> TblRebManufacturingManPower { get; set; }
        public virtual DbSet<TblRebManufacturingShift> TblRebManufacturingShift { get; set; }
        public virtual DbSet<TblRebPackingDetailLine> TblRebPackingDetailLine { get; set; }
        public virtual DbSet<TblRebPackingDetailMachine> TblRebPackingDetailMachine { get; set; }
        public virtual DbSet<TblRebPackingDetailShift> TblRebPackingDetailShift { get; set; }
        public virtual DbSet<TblRebPackingManPower> TblRebPackingManPower { get; set; }
        public virtual DbSet<TblRebProcessOrder> TblRebProcessOrder { get; set; }
        public virtual DbSet<TblRebProcessOrderDetail> TblRebProcessOrderDetail { get; set; }
        public virtual DbSet<TblRebProcessOrderLine> TblRebProcessOrderLine { get; set; }
        public virtual DbSet<TblRebProcessOrderSti> TblRebProcessOrderSti { get; set; }
        public virtual DbSet<TblRebProductionProcess> TblRebProductionProcess { get; set; }
        public virtual DbSet<TblRebProductionProcessDetail> TblRebProductionProcessDetail { get; set; }
        public virtual DbSet<TblRebRequisition> TblRebRequisition { get; set; }
        public virtual DbSet<TblRebRequisitionApprovalStatus> TblRebRequisitionApprovalStatus { get; set; }
        public virtual DbSet<TblRebRequisitionDetail> TblRebRequisitionDetail { get; set; }
        public virtual DbSet<TblRebRmpmsfgreceive> TblRebRmpmsfgreceive { get; set; }
        public virtual DbSet<TblRebRmpmsfgreceiveDetail> TblRebRmpmsfgreceiveDetail { get; set; }
        public virtual DbSet<TblRebStifgreceive> TblRebStifgreceive { get; set; }
        public virtual DbSet<TblRebStifgreceiveDetail> TblRebStifgreceiveDetail { get; set; }
        public virtual DbSet<TblRebReturn> TblRebReturn { get; set; }
        public virtual DbSet<TblRebReturnDetails> TblRebReturnDetails { get; set; }
        public virtual DbSet<TblRebQc> TblRebQc { get; set; }
        public virtual DbSet<TblRebQcdetails> TblRebQcdetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
               // optionsBuilder.UseSqlServer(@"Server=192.168.100.60;Database=dbFormulationProduction;Persist Security Info=True;User ID=sa;Password=dataport;");
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblPackingManPower>(entity =>
            {
                entity.ToTable("tblPackingManPower");
                 
                entity.Property(e => e.StaffId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.HasOne(d => d.Production)
                //    .WithMany(p => p.TblPackingManPower)
                //    .HasForeignKey(d => d.ProductionId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblPackingManPower_tblProductionProcess");
            });
            modelBuilder.Entity<TblManufacturingManPower>(entity =>
            {
                entity.ToTable("tblManufacturingManPower");

                entity.Property(e => e.StaffId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.HasOne(d => d.Production)
                //    .WithMany(p => p.TblManufacturingManPower)
                //    .HasForeignKey(d => d.ProductionId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblManufacturingManPower_tblProductionProcess");
            });
            modelBuilder.Entity<TblProductionManPower>(entity =>
            {
                entity.ToTable("tblProductionManPower");

                entity.Property(e => e.CostUnit)
                    .HasColumnName("Cost Unit")
                    .HasMaxLength(255);

                entity.Property(e => e.Designation).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.StaffId)
                    .HasColumnName("Staff_Id")
                    .HasMaxLength(255);
            });
            modelBuilder.Entity<TblCodingDetailLine>(entity =>
            {
                entity.ToTable("tblCodingDetailLine");

                entity.Property(e => e.LineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblCodingDetailLine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCodingDetailLine_tblProductionProcess");
            });

            modelBuilder.Entity<TblCodingDetailMachine>(entity =>
            {
                entity.ToTable("tblCodingDetailMachine");

                entity.Property(e => e.MachineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CodeMachineManHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CodeMachineNoOfWorker)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblCodingDetailMachine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCodingDetailMachine_tblProductionProcess");
            });

            modelBuilder.Entity<TblCodingDetailShift>(entity =>
            {
                entity.ToTable("tblCodingDetailShift");

                entity.Property(e => e.BreakeDownTime)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MachineCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BreakeDownCause)
                    .WithMany(p => p.TblCodingDetailShift)
                    .HasForeignKey(d => d.BreakeDownCauseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCodingDetailShift_tblCodingDetailBreakDownCause");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblCodingDetailShift)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCodingDetailShift_tblProductionProcess");
            });

            modelBuilder.Entity<TblEmployee>(entity =>
            {
                entity.ToTable("tblEmployee");

                entity.HasIndex(e => e.EmpId)
                    .HasName("IX_tblEmployee")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Department)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Designation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpGrade)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.InverseIdNavigation)
                    .HasForeignKey<TblEmployee>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployee_tblEmployee");
            });

            modelBuilder.Entity<TblEmployeeEducationalDetail>(entity =>
            {
                entity.ToTable("tblEmployeeEducationalDetail");

                entity.Property(e => e.Comment)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Result)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.tblEmpEducationalDetail)
                    .HasPrincipalKey(p => p.EmpId)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployeeEducationalDetails_tblEmployee");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<MenuMaster>(entity =>
            {
                entity.HasKey(e => new { e.MenuIdentity, e.MenuId, e.MenuName });

                entity.Property(e => e.MenuIdentity).ValueGeneratedOnAdd();

                entity.Property(e => e.MenuId)
                    .HasColumnName("MenuID")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MenuOrder);
                   

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MenuFileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MenuUrl)
                    .IsRequired()
                    .HasColumnName("MenuURL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParentMenuId)
                    .IsRequired()
                    .HasColumnName("Parent_MenuID")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UseYn)
                    .HasColumnName("USE_YN")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.UserRoll)
                    .IsRequired()
                    .HasColumnName("User_Roll")
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.ToTable("tblMenu");

                entity.Property(e => e.ContentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Href)
                    .HasColumnName("href")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IconClass)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEmpGrade>(entity =>
            {
                entity.ToTable("tblEmpGrade");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.GradeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEmployeeExpert>(entity =>
            {
                entity.ToTable("tblEmployeeExpert");

                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EmpId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblEmployeeExpert)
                    .HasPrincipalKey(p => p.EmpId)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployeeExpert_tblEmployee");
            });

            modelBuilder.Entity<TblExpertArea>(entity =>
            {
                entity.ToTable("tblExpertArea");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpertArea)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblCountry>(entity =>
            {
                entity.ToTable("tblCountry");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblLineSetup>(entity =>
            {
                entity.ToTable("tblLineSetup");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LineCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LineName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMachineSetup>(entity =>
            {
                entity.ToTable("tblMachineSetup");

                entity.Property(e => e.Capacity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                //entity.Property(e => e.MachineName)
                //    .IsRequired()
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                entity.Property(e => e.Speed)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CapacityPacking)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpeedPacking)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblShiftSetup>(entity =>
            {
                entity.ToTable("tblShiftSetup");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftStartTime)
                    .HasColumnName("ShiftStartTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.ShiftEndTime)
                 .HasColumnName("ShiftEndTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlannedDownChangeTime)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductiveShiftHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ShiftCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandardShiftHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblMonthlyPlanning>(entity =>
            {
                entity.ToTable("tblMonthlyPlanning");

                entity.HasIndex(e => e.PlanningNo)
                    .HasName("IX_tblMonthlyPlanning")
                    .IsUnique();

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Month)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PlanningNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMonthlyPlanningDetail>(entity =>
            {
                entity.ToTable("tblMonthlyPlanningDetail");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Fgcode)
                    .IsRequired()
                    .HasColumnName("FGCode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PlanQty)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PlanningNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RevisedPlanQty)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");


                entity.Property(e => e.ActualForecast)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReviewedForecast)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OpeningStock)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductionRequirement)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Closing)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");



                entity.HasOne(d => d.PlanningNoNavigation)
                    .WithMany(p => p.TblMonthlyPlanningDetail)
                    .HasPrincipalKey(p => p.PlanningNo)
                    .HasForeignKey(d => d.PlanningNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMonthlyPlanningDetail_tblMonthlyPlanning");
            });

            //modelBuilder.Entity<TblProductionProcess>(entity =>
            //{
            //    entity.ToTable("tblProductionProcess");

            //    entity.HasIndex(e => e.ProcessNo)
            //        .HasName("IX_tblProductionProcess")
            //        .IsUnique();

            //    entity.Property(e => e.BusinessCode)
            //        .HasMaxLength(5)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Comments)
            //        .HasMaxLength(250)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Edate)
            //        .HasColumnName("EDate")
            //        .HasColumnType("datetime");

            //    entity.Property(e => e.Euser)
            //        .HasColumnName("EUser")
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Idate)
            //        .HasColumnName("IDate")
            //        .HasColumnType("datetime");

            //    entity.Property(e => e.IssueNo)
            //        .IsRequired()
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Iuser)
            //        .IsRequired()
            //        .HasColumnName("IUser")
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ManufacLineCode)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ManufacMachineCode)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ManufacMachineHour).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.ManufacManHour).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.ManufacNoOfWorker).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.ManufacShiftBreakDownChangeTime).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.ManufacShiftCode)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PackLineCode)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PackMachineCode)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PackMachineHour).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.PackManHour).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.PackNoOfWorker).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.PackShiftBreakDownChangeTime).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.PackShiftCode)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ProcessDate).HasColumnType("datetime");

            //    entity.Property(e => e.ManufacBatchStartTime).HasColumnType("datetime");
            //    entity.Property(e => e.ManufacBatchEndTime).HasColumnType("datetime");
            //    entity.Property(e => e.PackBatchStartTime).HasColumnType("datetime");
            //    entity.Property(e => e.PackBatchEndTime).HasColumnType("datetime");

            //    entity.Property(e => e.ProcessNo)
            //        .IsRequired()
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ProductionQty).HasColumnType("numeric(18, 2)");

            //    entity.Property(e => e.RequisitionNo)
            //        .IsRequired()
            //        .HasMaxLength(15)
            //        .IsUnicode(false);

            //    entity.HasOne(d => d.RequisitionNoNavigation)
            //        .WithMany(p => p.TblProductionProcess)
            //        .HasPrincipalKey(p => p.RequisitionNo)
            //        .HasForeignKey(d => d.RequisitionNo)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_tblProductionProcess_tblRequisition");
            //});

            //modelBuilder.Entity<TblProductionProcessDetail>(entity =>
            //{
            //    entity.ToTable("tblProductionProcessDetail");

            //    entity.Property(e => e.CurrentUseQty).HasColumnType("numeric(18, 4)");

            //    entity.Property(e => e.IssuedQty).HasColumnType("numeric(18, 4)");

            //    entity.Property(e => e.MaterialCode)
            //        .IsRequired()
            //        .HasMaxLength(20)
            //        .IsUnicode(false);

            //    entity.Property(e => e.StdConsumptionQty).HasColumnType("numeric(18, 4)");

            //    entity.Property(e => e.PreviousUsedQty).HasColumnType("numeric(18, 4)");

            //    entity.Property(e => e.ProcessLoss).HasColumnType("numeric(18, 4)");

            //    entity.Property(e => e.ProcessNo)
            //        .IsRequired()
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ReqQuantity).HasColumnType("numeric(18, 4)");

            //    entity.Property(e => e.TotalConsumption).HasColumnType("numeric(18, 4)");

            //    entity.Property(e => e.Wastage).HasColumnType("numeric(18, 4)");

            //    entity.HasOne(d => d.ProcessNoNavigation)
            //        .WithMany(p => p.TblProductionProcessDetail)
            //        .HasPrincipalKey(p => p.ProcessNo)
            //        .HasForeignKey(d => d.ProcessNo)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_tblProductionProcessDetail_tblProductionProcess");
            //});

            modelBuilder.Entity<TblQcparameter>(entity =>
            {
                entity.ToTable("tblQCParameter");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.QcparameterCode)
                    .IsRequired()
                    .HasColumnName("QCParameterCode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.QcparameterName)
                    .IsRequired()
                    .HasColumnName("QCParameterName")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.QcparameterStandardValue)
                    .IsRequired()
                    .HasColumnName("QCParameterStandardValue")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeCode)
                   //.IsRequired()
                   .HasColumnName("TypeCode")
                   .HasMaxLength(10)
                   .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblQc>(entity =>
            {
                entity.ToTable("tblQC");

                entity.HasIndex(e => e.Qcno)
                    .HasName("IX_tblQC")
                    .IsUnique();

                entity.Property(e => e.LinkedProcessNo)
                .HasMaxLength(15)
                .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Qcdate)
                    .HasColumnName("QCDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Qcno)
                    .IsRequired()
                    .HasColumnName("QCNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.QcpassQty)
                    .HasColumnName("QCPassQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Qcqty)
                    .HasColumnName("QCQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.QcrejectQty)
                    .HasColumnName("QCRejectQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.QcholdQty)
                    .HasColumnName("QcholdQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.SFGQcpassQty)
                    .HasColumnName("SFGQcpassQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.SFGQcqty)
                    .HasColumnName("SFGQcqty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.SFGQcrejectQty)
                    .HasColumnName("SFGQCRejectQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.QCQuarantineQty)
                    .HasColumnName("QCQuarantineQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.FGQCQtyBeforeConversion).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.FGQCQtyConversionFactor).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.QCReferenceSampleQty).HasColumnType("numeric(18, 2)");


                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.IsSendToFloorStockFG).HasColumnName("IsSendToFloorStockFG");

                entity.Property(e => e.IsSendToFloorStockSFG).HasColumnName("IsSendToFloorStockSFG");
            });

            modelBuilder.Entity<TblQcdetails>(entity =>
            {
                entity.ToTable("tblQCDetails");

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Qcno)
                    .IsRequired()
                    .HasColumnName("QCNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.QcparameterActualValue)
                    .IsRequired()
                    .HasColumnName("QCParameterActualValue")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.QcparameterCode)
                    .IsRequired()
                    .HasColumnName("QCParameterCode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.QcnoNavigation)
                    .WithMany(p => p.TblQcdetails)
                    .HasPrincipalKey(p => p.Qcno)
                    .HasForeignKey(d => d.Qcno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblQCDetails_tblQC");
            });

            modelBuilder.Entity<TblQcparameterType>(entity =>
            {
                entity.ToTable("tblQCParameterType");

                entity.Property(e => e.TypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRequisition>(entity =>
            {
                entity.ToTable("tblRequisition");

                entity.HasIndex(e => e.RequisitionNo)
                    .HasName("IX_tblRequisition")
                    .IsUnique();

                entity.Property(e => e.ManPower)
                 .HasColumnType("numeric(18, 2)")
                 .HasDefaultValueSql("((0))");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumberOfBatch).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionDate).HasColumnType("datetime");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ExtOfRequisitionNo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IssueStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<TblRequisitionDetail>(entity =>
            {
                entity.ToTable("tblRequisitionDetail");

                entity.Property(e => e.AvailableStock)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EstimatedQty)
               .HasColumnType("numeric(18, 4)")
               .HasDefaultValueSql("((0))");

                entity.Property(e => e.StandardRecipeQty)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FloorStock)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RequiredQty)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.RequisitionNoNavigation)
                    .WithMany(p => p.TblRequisitionDetail)
                    .HasPrincipalKey(p => p.RequisitionNo)
                    .HasForeignKey(d => d.RequisitionNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisitionDetail_tblRequisition");
            });

            modelBuilder.Entity<TblReturn>(entity =>
            {
                entity.ToTable("tblReturn");

                entity.HasIndex(e => e.ReturnNo)
                    .HasName("IX_tblReturn")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IssueNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);


                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.RequisitionNoNavigation)
                    .WithMany(p => p.TblReturn)
                    .HasPrincipalKey(p => p.RequisitionNo)
                    .HasForeignKey(d => d.RequisitionNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReturn_tblRequisition");
            });

            modelBuilder.Entity<TblReturnDetails>(entity =>
            {
                entity.ToTable("tblReturnDetails");

                entity.Property(e => e.IssuedQty).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GRNNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                   .HasMaxLength(10)
                   .IsUnicode(false);

                entity.Property(e => e.ReturnQty).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.ReturnNoNavigation)
                    .WithMany(p => p.TblReturnDetails)
                    .HasPrincipalKey(p => p.ReturnNo)
                    .HasForeignKey(d => d.ReturnNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReturnDetails_tblReturn");
            });

            modelBuilder.Entity<View_BOM>(entity =>
            {
                entity.ToTable("View_BOM");

                entity.Property(e => e.Bomtext)
                    .HasColumnName("BOMText")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Bomtype)
                    .HasColumnName("BOMType")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<View_BOMDetail>(entity =>
            {
                entity.ToTable("View_BOMDetail");

                entity.HasIndex(e => new { e.Bomid, e.MaterialCode })
                    .HasName("IX_tblBOMDetail")
                    .IsUnique();

                entity.Property(e => e.Bomid).HasColumnName("BOMId");

                entity.Property(e => e.CostPerProductUnit).HasColumnType("decimal(24, 8)");

                entity.Property(e => e.LossQuantity).HasColumnType("decimal(24, 8)");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PerBatchQuantityExcludingLoss).HasColumnType("decimal(24, 8)");

                entity.Property(e => e.QuantityPerBatch).HasColumnType("decimal(24, 8)");
            });

            modelBuilder.Entity<View_Material>(entity =>
            {
                entity.ToTable("View_Material");

                entity.Property(e => e.Id).ValueGeneratedNever();

                //entity.Property(e => e.AlternativeUoM)
                //    .IsRequired()
                //    .HasMaxLength(20)
                //    .IsUnicode(false);

                //entity.Property(e => e.AuoMconversionValue)
                //    .HasColumnName("AUoMConversionValue")
                //    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.BaseUnit)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                //entity.Property(e => e.ConversionValue).HasColumnType("numeric(18, 2)");

                //entity.Property(e => e.Discontinue)
                //    .IsRequired()
                //    .HasMaxLength(1)
                //    .IsUnicode(false);

                //entity.Property(e => e.Edate)
                //    .HasColumnName("EDate")
                //    .HasColumnType("datetime");

                //entity.Property(e => e.EditIpaddress)
                //    .HasColumnName("EditIPAddress")
                //    .HasMaxLength(30)
                //    .IsUnicode(false);

                //entity.Property(e => e.Euser)
                //    .HasColumnName("EUser")
                //    .HasMaxLength(20)
                //    .IsUnicode(false);

                //entity.Property(e => e.Idate)
                //    .HasColumnName("IDate")
                //    .HasColumnType("datetime");

                //entity.Property(e => e.InsertIpaddress)
                //    .HasColumnName("InsertIPAddress")
                //    .HasMaxLength(30)
                //    .IsUnicode(false);

                //entity.Property(e => e.Iuser)
                //    .HasColumnName("IUser")
                //    .HasMaxLength(20)
                //    .IsUnicode(false);

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                //entity.Property(e => e.SkuoM)
                //    .IsRequired()
                //    .HasColumnName("SKUoM")
                //    .HasMaxLength(20)
                //    .IsUnicode(false);

                //entity.Property(e => e.SubBusinessCode)
                //    .HasMaxLength(10)
                //    .IsUnicode(false);
            });

            modelBuilder.Entity<View_Product>(entity =>
            {
                entity.HasKey(e => e.ProductCode);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.BrandCode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.BusiSumGroupCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Business)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Carton).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.DiscountStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DistDiscount).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.EffectedDate).HasColumnType("datetime");

                entity.Property(e => e.GroupCode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Mrp)
                    .HasColumnName("MRP")
                    .HasColumnType("numeric(18, 4)");

                entity.Property(e => e.PackSize)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Pccc)
                    .IsRequired()
                    .HasColumnName("PCCC")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PrincipalCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCategory)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName1)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.RatePerCarton).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ReportGroupCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Show)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Smscode)
                    .IsRequired()
                    .HasColumnName("SMSCODE")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Smsorder)
                    .IsRequired()
                    .HasColumnName("SMSOrder")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.StorageType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SubBusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Vat)
                    .HasColumnName("VAT")
                    .HasColumnType("numeric(18, 4)");
            });



            modelBuilder.Entity<View_IssueQuantity>(entity =>
            {
                entity.ToTable("View_IssueQuantity");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Issue_Quantity_DC).HasColumnType("numeric(18, 2)");

            });

            modelBuilder.Entity<TblRequisitionApprovalStatus>(entity =>
            {
                entity.ToTable("tblRequisitionApprovalStatus");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Approver)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovalStatus)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionStatusDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblFloorStock>(entity =>
            {
                entity.ToTable("tblFloorStock");

                entity.Property(e => e.AvailableStock)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ConsumeStock)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Grnno)
                    .IsRequired()
                    .HasColumnName("GRNNo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IssueStock)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.StockDate).HasColumnType("datetime");
            });


            modelBuilder.Entity<TblManufacturingBreakDownCause>(entity =>
            {
                entity.HasKey(e => e.BreakeDownCauseId);

                entity.ToTable("tblManufacturingBreakDownCause");

                entity.Property(e => e.BreakeDownCause)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblManufacturingLine>(entity =>
            {
                entity.ToTable("tblManufacturingLine");

                entity.Property(e => e.LineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblManufacturingLine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblManufacturingLine_tblProductionProcess");
            });

            modelBuilder.Entity<TblManufacturingMachine>(entity =>
            {
                entity.ToTable("tblManufacturingMachine");

                entity.Property(e => e.MachineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineHour).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.ManufacMachineNoOfWorker).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.ManufacMachineManHour).HasColumnType("numeric(18, 2)");


                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblManufacturingMachine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblManufacturingMachine_tblProductionProcess");
            });

            modelBuilder.Entity<TblManufacturingShift>(entity =>
            {
                entity.ToTable("tblManufacturingShift");

                entity.Property(e => e.BreakeDownTime).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ShiftCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BreakeDownCause)
                    .WithMany(p => p.TblManufacturingShift)
                    .HasForeignKey(d => d.BreakeDownCauseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblManufacturingShift_tblManufacturingBreakDownCause");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblManufacturingShift)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblManufacturingShift_tblProductionProcess");
            });

            modelBuilder.Entity<TblPackingDetailLine>(entity =>
            {
                entity.ToTable("tblPackingDetailLine");

                entity.Property(e => e.LineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblPackingDetailLine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPackingDetailLine_tblProductionProcess");
            });

            modelBuilder.Entity<TblPackingDetailMachine>(entity =>
            {
                entity.ToTable("tblPackingDetailMachine");

                entity.Property(e => e.MachineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineHour).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.PackMachineNoOfWorker).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.PackMachineManHour).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblPackingDetailMachine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPackingDetailMachine_tblProductionProcess");
            });

            modelBuilder.Entity<TblPackingDetailShift>(entity =>
            {
                entity.ToTable("tblPackingDetailShift");

                entity.Property(e => e.BreakeDownTime).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ShiftCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BreakeDownCause)
                 .WithMany(p => p.TblPackingDetailShift)
                 .HasForeignKey(d => d.BreakeDownCauseId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_tblPackingDetailShift_tblManufacturingBreakDownCause");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblPackingDetailShift)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPackingDetailShift_tblProductionProcess");
            });


            modelBuilder.Entity<TblProductionProcess>(entity =>
            {

                entity.ToTable("tblProductionProcess");

                entity.Property(e => e.LinkedProcessNo)
                 .HasMaxLength(15)
                 .IsUnicode(false);

                entity.HasIndex(e => e.ProcessNo)
                    .HasName("IX_tblProductionProcess")
                    .IsUnique();

                //entity.Property(e => e.ManufacManPowerStaffId).HasMaxLength(50);
                //entity.Property(e => e.PackManPowerStaffId).HasMaxLength(50);

                entity.Property(e => e.CodingQty).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.PackingQty).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IssueNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ManufacBatchEndTime).HasColumnType("datetime");

                entity.Property(e => e.ManufacBatchStartTime).HasColumnType("datetime");

                entity.Property(e => e.ManufacLineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ManufacMachineCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ManufacMachineHour).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.ManufacMachineNoOfWorker).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.ManufacMachineManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacCommonManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacCommonNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacShiftBreakDownChangeTime).HasColumnType("numeric(18, 2)");


                entity.Property(e => e.ManufacShiftCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NumberOfBatch).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackBatchEndTime).HasColumnType("datetime");

                entity.Property(e => e.PackBatchStartTime).HasColumnType("datetime");

                entity.Property(e => e.PackLineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PackMachineCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PackMachineHour).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.PackMachineNoOfWorker).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.PackMachineManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackCommonManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackCommonNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackShiftBreakDownChangeTime).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackShiftCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ProductionQty).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.ProductionQtyBeforeConversion).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.ProductionQtyConversionFactor).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SFGCode)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SFGProductionQty).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.QCReferenceSampleQty).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.LumpQty).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.RequisitionNoNavigation)
                   .WithMany(p => p.TblProductionProcess)
                   .HasPrincipalKey(p => p.RequisitionNo)
                   .HasForeignKey(d => d.RequisitionNo)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_tblProductionProcess_tblRequisition");

            });

            modelBuilder.Entity<TblProductionProcessDetail>(entity =>
            {
                entity.ToTable("tblProductionProcessDetail");

                entity.Property(e => e.CurrentUseQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.IssuedQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PreviousUsedQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.ProcessLoss).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.ProcessNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ReqQuantity).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.StdConsumptionQty)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalConsumption).HasColumnType("numeric(18, 4)");
                entity.Property(e => e.FloorStock).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Wastage).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Grnno)
                    .HasColumnName("GRNNO")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RejectQty)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Wip)
                    .HasColumnName("WIP")
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ProcessNoNavigation)
                    .WithMany(p => p.TblProductionProcessDetail)
                    .HasPrincipalKey(p => p.ProcessNo)
                    .HasForeignKey(d => d.ProcessNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProductionProcessDetail_tblProductionProcess");
            });

            modelBuilder.Entity<TblFgtn>(entity =>
            {
                entity.ToTable("tblFGTN");

                entity.HasIndex(e => e.Fgtnno)
                    .HasName("IX_tblFGTN")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fgtndate)
                    .HasColumnName("FGTNDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fgtnno)
                    .IsRequired()
                    .HasColumnName("FGTNNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblFgtndetails>(entity =>
            {
                entity.ToTable("tblFGTNDetails");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.LinkedProcessNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.Fgtnno)
                    .IsRequired()
                    .HasColumnName("FGTNNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Fgtnqty)
                    .HasColumnName("FGTNQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ProcessNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Qcno)
                    .IsRequired()
                    .HasColumnName("QCNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.QcpassQty)
                    .HasColumnName("QCPassQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.FgtnnoNavigation)
                    .WithMany(p => p.TblFgtndetails)
                    .HasPrincipalKey(p => p.Fgtnno)
                    .HasForeignKey(d => d.Fgtnno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFGTNDetails_tblFGTNDetails");
            });

            modelBuilder.Entity<TblFgtransferLocation>(entity =>
            {
                entity.ToTable("tblFGTransferLocation");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblStandardManHourSetup>(entity =>
            {
                entity.ToTable("tblStandardManHourSetup");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StdHrPerBatchRM).HasDefaultValueSql("((0))");

                entity.Property(e => e.StdManPowerPerBatchRM).HasDefaultValueSql("((0))");

                entity.Property(e => e.StdHrPerBatchPM).HasDefaultValueSql("((0))");

                entity.Property(e => e.StdManPowerPerBatchPM).HasDefaultValueSql("((0))");


            });

            modelBuilder.Entity<TblMachineName>(entity =>
            {
                entity.ToTable("tblMachineName");

                entity.Property(e => e.MachineCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMaintenanceInformation>(entity =>
            {
                entity.ToTable("tblMaintenanceInformation");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Edate).HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Idate).HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MachineCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceDate).HasColumnType("datetime");

                entity.Property(e => e.MaintenanceHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.MaintenanceName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TblRequisitionLine>(entity =>
            {
                entity.ToTable("tblRequisitionLine");

                entity.Property(e => e.LineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessOrderNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.HasOne(d => d.ProcessOrderNoNavigation)
                 .WithMany(p => p.TblRequisitionLine)
                 .HasPrincipalKey(p => p.ProcessOrderNo)
                 .HasForeignKey(d => d.ProcessOrderNo)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_tblRequisitionLine_tblProcessOrder");
            });

            //modelBuilder.Entity<TblRequisitionMachineSetup>(entity =>
            //{
            //    entity.ToTable("tblRequisitionMachineSetup");

            //    entity.Property(e => e.MachineCode)
            //        .IsRequired()
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ProcessOrderNo)
            //      .HasMaxLength(15)
            //      .IsUnicode(false);

            //    entity.Property(e => e.RequisitionNo)
            //        .IsRequired()
            //        .HasMaxLength(15)
            //        .IsUnicode(false);

            //    entity.HasOne(d => d.RequisitionNoNavigation)
            //        .WithMany(p => p.TblRequisitionMachineSetup)
            //        .HasPrincipalKey(p => p.RequisitionNo)
            //        .HasForeignKey(d => d.RequisitionNo)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_tblRequisitionMachineSetup_tblRequisition");
            //});





            modelBuilder.Entity<TblVisibility>(entity =>
            {
                entity.ToTable("tblVisibility");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Isvisible)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<TblProductionApprovalStatus>(entity =>
            {
                entity.ToTable("tblProductionApprovalStatus");

                entity.Property(e => e.ApprovalStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Approver)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessStatusDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblSubBusiness>(entity =>
            {
                entity.ToTable("tblSubBusiness");

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SubBusinessCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.SubBusinessName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TblProcessOrder>(entity =>
            {
                entity.ToTable("tblProcessOrder");

                entity.HasIndex(e => e.ProcessOrderNo)
                    .HasName("IX_tblProcessOrder")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Bomid)
                    .HasColumnName("BOMId")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExtOfProcessOrderNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IssueStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ManPower)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NumberOfBatch).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.ProcessOrderDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessOrderNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.InverseIdNavigation)
                    .HasForeignKey<TblProcessOrder>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProcessOrder_tblProcessOrder");
            });

            modelBuilder.Entity<TblProcessOrderDetail>(entity =>
            {
                entity.ToTable("tblProcessOrderDetail");

                entity.Property(e => e.AvailableStock)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EstimatedQty)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FloorStock)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessOrderNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RequiredQty)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StandardRecipeQty)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ProcessOrderNoNavigation)
                    .WithMany(p => p.TblProcessOrderDetail)
                    .HasPrincipalKey(p => p.ProcessOrderNo)
                    .HasForeignKey(d => d.ProcessOrderNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProcessOrderDetail_tblProcessOrder");

                 
               

            });

            modelBuilder.Entity<TblRebManufacturingBreakDownCause>(entity =>
            {
                entity.HasKey(e => e.BreakeDownCauseId);

                entity.ToTable("tblRebManufacturingBreakDownCause");

                entity.Property(e => e.BreakeDownCause)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRebManufacturingLine>(entity =>
            {
                entity.ToTable("tblRebManufacturingLine");

                entity.Property(e => e.LineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblRebManufacturingLine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebManufacturingLine_tblRebProductionProcess");
            });

            modelBuilder.Entity<TblRebManufacturingMachine>(entity =>
            {
                entity.ToTable("tblRebManufacturingMachine");

                entity.Property(e => e.MachineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ManufacMachineManHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ManufacMachineNoOfWorker)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblRebManufacturingMachine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebManufacturingMachine_tblRebProductionProcess");
            });

            modelBuilder.Entity<TblRebManufacturingManPower>(entity =>
            {
                entity.ToTable("tblRebManufacturingManPower");

                entity.Property(e => e.StaffId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblRebManufacturingManPower)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebManufacturingManPower_tblRebProductionProcess");
            });

            modelBuilder.Entity<TblRebManufacturingShift>(entity =>
            {
                entity.ToTable("tblRebManufacturingShift");

                entity.Property(e => e.BreakeDownTime)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MachineCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BreakeDownCause)
                    .WithMany(p => p.TblRebManufacturingShift)
                    .HasForeignKey(d => d.BreakeDownCauseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebManufacturingShift_tblRebManufacturingBreakDownCause");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblRebManufacturingShift)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebManufacturingShift_tblRebProductionProcess");
            });

            modelBuilder.Entity<TblRebPackingDetailLine>(entity =>
            {
                entity.ToTable("tblRebPackingDetailLine");

                entity.Property(e => e.LineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblRebPackingDetailLine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebPackingDetailLine_tblRebProductionProcess");
            });

            modelBuilder.Entity<TblRebPackingDetailMachine>(entity =>
            {
                entity.ToTable("tblRebPackingDetailMachine");

                entity.Property(e => e.MachineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackMachineManHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackMachineNoOfWorker)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblRebPackingDetailMachine)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebPackingDetailMachine_tblRebProductionProcess");
            });

            modelBuilder.Entity<TblRebPackingDetailShift>(entity =>
            {
                entity.ToTable("tblRebPackingDetailShift");

                entity.Property(e => e.BreakeDownTime)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MachineCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BreakeDownCause)
                    .WithMany(p => p.TblRebPackingDetailShift)
                    .HasForeignKey(d => d.BreakeDownCauseId)
                    .HasConstraintName("FK_tblRebPackingDetailShift_tblRebManufacturingBreakDownCause");

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblRebPackingDetailShift)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebPackingDetailShift_tblRebProductionProcess");
            });

            modelBuilder.Entity<TblRebPackingManPower>(entity =>
            {
                entity.ToTable("tblRebPackingManPower");

                entity.Property(e => e.StaffId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Production)
                    .WithMany(p => p.TblRebPackingManPower)
                    .HasForeignKey(d => d.ProductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebPackingManPower_tblRebProductionProcess");
            });


            modelBuilder.Entity<TblRebProcessOrder>(entity =>
            {
                entity.ToTable("tblRebProcessOrder");

                entity.HasIndex(e => e.ProcessOrderNo)
                    .HasName("IX_tblRebProcessOrder")
                    .IsUnique();

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Bomid).HasColumnName("BOMId");

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExtOfProcessOrderNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IssueStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ManPower).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.NumberOfBatch).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ProcessOrderDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessOrderNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRebProcessOrderDetail>(entity =>
            {
                entity.ToTable("tblRebProcessOrderDetail");

                entity.Property(e => e.AvailableStock).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.EstimatedQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.FloorStock).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessOrderNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RequiredQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.StandardRecipeQty).HasColumnType("numeric(18, 4)");

                entity.HasOne(d => d.ProcessOrderNoNavigation)
                    .WithMany(p => p.TblRebProcessOrderDetail)
                    .HasPrincipalKey(p => p.ProcessOrderNo)
                    .HasForeignKey(d => d.ProcessOrderNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebProcessOrderDetail_tblRebProcessOrder");
            });

            modelBuilder.Entity<TblRebProcessOrderLine>(entity =>
            {
                entity.ToTable("tblRebProcessOrderLine");

                entity.Property(e => e.LineCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessOrderNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcessOrderNoNavigation)
                    .WithMany(p => p.TblRebProcessOrderLine)
                    .HasPrincipalKey(p => p.ProcessOrderNo)
                    .HasForeignKey(d => d.ProcessOrderNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebProcessOrderLine_tblRebProcessOrder");
            });

            modelBuilder.Entity<TblRebProcessOrderSti>(entity =>
            {
                entity.ToTable("tblRebProcessOrderSTI");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ProcessOrderNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Stino)
                    .IsRequired()
                    .HasColumnName("STINo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcessOrderNoNavigation)
                    .WithMany(p => p.TblRebProcessOrderSti)
                    .HasPrincipalKey(p => p.ProcessOrderNo)
                    .HasForeignKey(d => d.ProcessOrderNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebProcessOrderSTI_tblRebProcessOrder");
            });

            modelBuilder.Entity<TblRebProductionProcess>(entity =>
            {
                entity.ToTable("tblRebProductionProcess");

                entity.HasIndex(e => e.ProcessNo)
                    .HasName("IX_tblRebProductionProcess")
                    .IsUnique();

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CodeBatchEndTime).HasColumnType("datetime");

                entity.Property(e => e.CodeBatchStartTime).HasColumnType("datetime");

                entity.Property(e => e.CodeCommonManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CodeCommonNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CodeLineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodeMachineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodeMachineHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CodeMachineManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CodeMachineNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CodeManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CodeNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CodeShiftBreakDownChangeTime).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CodeShiftCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodingQty)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IssueNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LinkedProcessNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.LumpQty)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ManufacBatchEndTime).HasColumnType("datetime");

                entity.Property(e => e.ManufacBatchStartTime).HasColumnType("datetime");

                entity.Property(e => e.ManufacCommonManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacCommonNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacLineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ManufacMachineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ManufacMachineHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacMachineManHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ManufacMachineNoOfWorker)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ManufacManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacShiftBreakDownChangeTime).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ManufacShiftCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NumberOfBatch)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackBatchEndTime).HasColumnType("datetime");

                entity.Property(e => e.PackBatchStartTime).HasColumnType("datetime");

                entity.Property(e => e.PackCommonManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackCommonNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackLineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PackMachineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PackMachineHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackMachineManHour)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackMachineNoOfWorker)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackManHour).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackNoOfWorker).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackShiftBreakDownChangeTime).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PackShiftCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PackingQty)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProcessDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ProductionQty).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ProductionQtyBeforeConversion).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ProductionQtyConversionFactor).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.QcreferenceSampleQty)
                    .HasColumnName("QCReferenceSampleQty")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RequisitionNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Sfgcode)
                    .HasColumnName("SFGCode")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SfgproductionQty)
                    .HasColumnName("SFGProductionQty")
                    .HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.LinkedProcessNoNavigation)
                    .WithMany(p => p.TblRebProductionProcess)
                    .HasPrincipalKey(p => p.ProcessOrderNo)
                    .HasForeignKey(d => d.LinkedProcessNo)
                    .HasConstraintName("FK_tblRebProductionProcess_tblRebProductionProcess");
            });

            modelBuilder.Entity<TblRebProductionProcessDetail>(entity =>
            {
                entity.ToTable("tblRebProductionProcessDetail");

                entity.Property(e => e.CurrentUseQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.FloorStock).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Grnno)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IssuedQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PreviousUsedQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.ProcessLoss).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.ProcessNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RejectQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.ReqQuantity).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.RequisitionNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.StdConsumptionQty)
                    .HasColumnType("numeric(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalConsumption).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Wastage).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Wip).HasColumnType("numeric(18, 4)");

                entity.HasOne(d => d.ProcessNoNavigation)
                    .WithMany(p => p.TblRebProductionProcessDetail)
                    .HasPrincipalKey(p => p.ProcessNo)
                    .HasForeignKey(d => d.ProcessNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebProductionProcessDetail_tblRebProductionProcessDetail");
            });

            modelBuilder.Entity<TblRebRequisition>(entity =>
            {
                entity.ToTable("tblRebRequisition");

                entity.HasIndex(e => e.RequisitionNo)
                    .HasName("IX_tblRebRequisition")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExtOfRequisitionNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IssueStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LinkedProcessNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NumberOfBatch).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionDate).HasColumnType("datetime");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.TypeCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.InverseIdNavigation)
                    .HasForeignKey<TblRebRequisition>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebRequisition_tblRebRequisition");
            });

                       modelBuilder.Entity<TblRebRequisitionApprovalStatus>(entity =>
            {
                entity.ToTable("tblRebRequisitionApprovalStatus");

                entity.Property(e => e.ApprovalStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Approver)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionStatusDate).HasColumnType("datetime");

                entity.HasOne(d => d.RequisitionNoNavigation)
                    .WithMany(p => p.TblRebRequisitionApprovalStatus)
                    .HasPrincipalKey(p => p.RequisitionNo)
                    .HasForeignKey(d => d.RequisitionNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebRequisitionApprovalStatus_tblRequisition");
            });

            modelBuilder.Entity<TblRebRequisitionDetail>(entity =>
            {
                entity.ToTable("tblRebRequisitionDetail");

                entity.Property(e => e.AvailableStock).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.EstimatedQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.FloorStock).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequiredQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.StandardRecipeQty).HasColumnType("numeric(18, 4)");

                entity.HasOne(d => d.RequisitionNoNavigation)
                    .WithMany(p => p.TblRebRequisitionDetail)
                    .HasPrincipalKey(p => p.RequisitionNo)
                    .HasForeignKey(d => d.RequisitionNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebRequisitionDetail_tblRebRequisition");
            });

            modelBuilder.Entity<TblRebRmpmsfgreceive>(entity =>
            {
                entity.ToTable("tblRebRMPMSFGReceive");

                entity.HasIndex(e => e.ReceiveNo)
                    .HasName("IX_tblRebRMPMSFGReceive")
                    .IsUnique();

                entity.Property(e => e.BatchNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments).HasColumnType("nchar(10)");

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiveNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Stino)
                    .IsRequired()
                    .HasColumnName("STINo")
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.Edate)
                 .HasColumnName("EDate")
                 .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRebRmpmsfgreceiveDetail>(entity =>
            {
                entity.ToTable("tblRebRMPMSFGReceiveDetail");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasMaxLength(500);

             

                entity.Property(e => e.ReceiveNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiveQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Rmpmsfgcode)
                    .IsRequired()
                    .HasColumnName("RMPMSFGCode")
                    .HasMaxLength(15);

                entity.Property(e => e.Rmpmsfgname)
                    .IsRequired()
                    .HasColumnName("RMPMSFGName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.ReceiveNoNavigation)
                    .WithMany(p => p.TblRebRmpmsfgreceiveDetail)
                    .HasPrincipalKey(p => p.ReceiveNo)
                    .HasForeignKey(d => d.ReceiveNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebRMPMSFGReceiveDetail_tblRebRMPMSFGReceive");
            });


            modelBuilder.Entity<TblRebStifgreceive>(entity =>
            {
                entity.ToTable("tblRebSTIFGReceive");

                entity.HasIndex(e => e.StifgreceiveNo)
                    .HasName("IX_tblRebSTIFGReceive")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiveComment)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StifgreceiveDate)
                    .HasColumnName("STIFGReceiveDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.StifgreceiveNo)
                    .IsRequired()
                    .HasColumnName("STIFGReceiveNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Stino)
                    .IsRequired()
                    .HasColumnName("STINo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Stistock)
                    .HasColumnName("STIStock")
                    .HasColumnType("numeric(18, 4)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.InverseIdNavigation)
                    .HasForeignKey<TblRebStifgreceive>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebSTIFGReceive_tblRebSTIFGReceive");
            });
            modelBuilder.Entity<TblRebRequisitionType>(entity =>
            {
                entity.HasIndex(e => e.TypeCode)
                    .HasName("IX_TblRebRequisitionType")
                    .IsUnique();

                entity.Property(e => e.TypeCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TblRebReturn>(entity =>
            {
                entity.ToTable("tblRebReturn");

                entity.HasIndex(e => e.ReturnNo)
                    .HasName("IX_tblRebReturn")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IssueNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.RequisitionNoNavigation)
                    .WithMany(p => p.TblRebReturn)
                    .HasPrincipalKey(p => p.RequisitionNo)
                    .HasForeignKey(d => d.RequisitionNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebReturn_tblRebRequisition");
            });

            modelBuilder.Entity<TblRebReturnDetails>(entity =>
            {
                entity.ToTable("tblRebReturnDetails");

                entity.Property(e => e.Grnno)
                    .IsRequired()
                    .HasColumnName("GRNNo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IssuedQty).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnQty).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.ReturnNoNavigation)
                    .WithMany(p => p.TblRebReturnDetails)
                    .HasPrincipalKey(p => p.ReturnNo)
                    .HasForeignKey(d => d.ReturnNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebReturnDetails_tblRebReturn");
            });
            modelBuilder.Entity<TblRebStifgreceiveDetail>(entity =>
            {
                entity.ToTable("tblRebSTIFGReceiveDetail");

                entity.Property(e => e.ActualReceiveQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Comments)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Fgcode)
                    .IsRequired()
                    .HasColumnName("FGCode")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Fgname)
                    .IsRequired()
                    .HasColumnName("FGName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiveQuantity).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.StifgreceiveNo)
                    .IsRequired()
                    .HasColumnName("STIFGReceiveNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Stiquantity)
                    .HasColumnName("STIQuantity")
                    .HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.HasOne(e => e.StifgreceiveNoNavigation)
                  .WithMany(p => p.TblRebStifgreceiveDetail)
                  .HasPrincipalKey(p => p.StifgreceiveNo)
                  .HasForeignKey(d => d.StifgreceiveNo)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_tblRebSTIFGReceiveDetail_tblRebSTIFGReceive");

            });
            modelBuilder.Entity<TblRebQc>(entity =>
            {
                entity.ToTable("tblRebQC");

                entity.HasIndex(e => e.Qcno)
                    .HasName("IX_tblRebQC")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FgqcqtyBeforeConversion)
                    .HasColumnName("FGQCQtyBeforeConversion")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FgqcqtyConversionFactor)
                    .HasColumnName("FGQCQtyConversionFactor")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsSendToFloorStockFg).HasColumnName("IsSendToFloorStockFG");

                entity.Property(e => e.IsSendToFloorStockSfg).HasColumnName("IsSendToFloorStockSFG");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LinkedProcessNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Qcdate)
                    .HasColumnName("QCDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.QcholdQty)
                    .HasColumnName("QCHoldQty")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Qcno)
                    .IsRequired()
                    .HasColumnName("QCNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.QcpassQty)
                    .HasColumnName("QCPassQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Qcqty)
                    .HasColumnName("QCQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.QcquarantineQty)
                    .HasColumnName("QCQuarantineQty")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.QcreferenceSampleQty)
                    .HasColumnName("QCReferenceSampleQty")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.QcrejectQty)
                    .HasColumnName("QCRejectQty")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.RequisitionNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SfgqcpassQty)
                    .HasColumnName("SFGQCPassQty")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Sfgqcqty)
                    .HasColumnName("SFGQCQty")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SfgqcrejectQty)
                    .HasColumnName("SFGQCRejectQty")
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblRebQcdetails>(entity =>
            {
                entity.ToTable("tblRebQCDetails");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Qcno)
                    .IsRequired()
                    .HasColumnName("QCNo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.QcparameterActualValue)
                    .IsRequired()
                    .HasColumnName("QCParameterActualValue")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.QcparameterCode)
                    .IsRequired()
                    .HasColumnName("QCParameterCode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.QcnoNavigation)
                    .WithMany(p => p.TblRebQcdetails)
                    .HasPrincipalKey(p => p.Qcno)
                    .HasForeignKey(d => d.Qcno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebQCDetails_tblRebQC");
            });


            modelBuilder.Entity<TblRebSfgtransfer>(entity =>
            {
                entity.ToTable("tblRebSFGTransfer");

                entity.HasIndex(e => e.RequisitionNo)
                    .HasName("IX_tblRebSFGTrasnfer_1");

                entity.HasIndex(e => e.TransferNo)
                    .HasName("IX_tblRebSFGTrasnfer")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExtOfRequisitionNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IssueStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LinkedProcessNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NumberOfBatch).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TransferDate).HasColumnType("datetime");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TransferNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TypeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.InverseIdNavigation)
                    .HasForeignKey<TblRebSfgtransfer>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebSFGTrasnfer_tblRebSFGTrasnfer");

                entity.HasOne(d => d.RequisitionNoNavigation)
                    .WithMany(p => p.TblRebSfgtransfer)
                    .HasPrincipalKey(p => p.RequisitionNo)
                    .HasForeignKey(d => d.RequisitionNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebSFGTrasnfer_tblRebRequisition");
            });

            modelBuilder.Entity<TblRebSfgtransferDetail>(entity =>
            {
                entity.ToTable("tblRebSFGTransferDetail");

                entity.Property(e => e.AvailableStock).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.EstimatedQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.FloorStock).HasColumnType("numeric(18, 4)");
                entity.Property(e => e.IssueQty).HasColumnType("numeric(18, 4)");
                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequiredQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.RequisitionNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.StandardRecipeQty).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.TransferNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.TransferNoNavigation)
                    .WithMany(p => p.TblRebSfgtransferDetail)
                    .HasPrincipalKey(p => p.TransferNo)
                    .HasForeignKey(d => d.TransferNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRebSFGTrasnferDetail_tblRebSFGTrasnfer");
            });
        }
    }
}
