using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hospitaly.Modules.Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialClinicMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "clinics");

            migrationBuilder.CreateTable(
                name: "Appointments",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: true),
                    TimeSlot_DateTimeRange_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TimeSlot_DateTimeRange_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AppointmentType_Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AppointmentType_ExpectedDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Status_Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status_SetAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Cancellation_Reason = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Cancellation_InitiatedBy = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Cancellation_Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Cancellation_CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RescheduleInfo_OriginalTimeSlot_DateTimeRange_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RescheduleInfo_OriginalTimeSlot_DateTimeRange_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RescheduleInfo_Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RescheduleInfo_RescheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RescheduleInfo_RequestedBy = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Info_Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Info_TradingName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Info_Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Info_LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Address_Value_Street = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Address_Value_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address_Value_Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address_Value_PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Address_Value_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address_Coordinates_Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Address_Coordinates_Longitude = table.Column<double>(type: "double precision", nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ContactInfo_Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OperatingHours_Day = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    OperatingHours_Hours_Value_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OperatingHours_Hours_Value_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    hours_active = table.Column<bool>(type: "boolean", nullable: true),
                    OperatingHours_IsResting = table.Column<bool>(type: "boolean", nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSchedules",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identity_FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Identity_LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Identity_DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Identity_Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Identity_NationalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Contact_Address_Street = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Contact_Address_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Contact_Address_Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Contact_Address_PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Contact_Address_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Insurance_Value_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Insurance_Value_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Insurance_InsurerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Insurance_PolicyNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Insurance_GroupNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PatientType_Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PatientType_RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RoomType_Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialties_Specialties_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "clinics",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffMembers",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Role_Role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Role_Department = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Employment_HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Employment_Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Employment_ContractType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClinicOwnerships",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerShipType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SharePercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    OwnershipEffectivePeriod_Range_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OwnershipEffectivePeriod_Range_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicOwnerships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicOwnerships_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalSchema: "clinics",
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalSchema: "clinics",
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "clinics",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperatingLicenses",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LicenseNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IssuingAuthority = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LicenseType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ValidityPeriod_Value_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidityPeriod_Value_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AdministrativeStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatingLicenses_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalSchema: "clinics",
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicAffiliations",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TerminatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicAffiliations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicAffiliations_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "clinics",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorCredentials",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IssuingAuthority = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DocumentNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ValidityPeriod_Value_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidityPeriod_Value_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VerifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorCredentials_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "clinics",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleBlocks",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SpecificDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeRange_Value_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TimeRange_Value_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    BlockType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MaxAppointmentsAllowed = table.Column<int>(type: "integer", nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleBlocks_DoctorSchedules_DoctorScheduleId",
                        column: x => x.DoctorScheduleId,
                        principalSchema: "clinics",
                        principalTable: "DoctorSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceBlocks",
                schema: "clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaintenancePeriod_Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MaintenancePeriod_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Reason = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ScheduledBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Audit_CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Audit_CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Audit_UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Audit_UpdatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceBlocks_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "clinics",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomCapabilities",
                schema: "clinics",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomCapabilities", x => new { x.RoomId, x.Id });
                    table.ForeignKey(
                        name: "FK_RoomCapabilities_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "clinics",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicSpecialties",
                schema: "clinics",
                columns: table => new
                {
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecialtyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "numeric(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicSpecialties", x => new { x.ClinicId, x.SpecialtyId });
                    table.ForeignKey(
                        name: "FK_ClinicSpecialties_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalSchema: "clinics",
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "clinics",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSpecialties",
                schema: "clinics",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecialtyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    CertificationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CertifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSpecialties", x => new { x.DoctorId, x.SpecialtyId });
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "clinics",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "clinics",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicAffiliationPrivileges",
                schema: "clinics",
                columns: table => new
                {
                    ClinicAffiliationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrantedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicAffiliationPrivileges", x => new { x.ClinicAffiliationId, x.Id });
                    table.ForeignKey(
                        name: "FK_ClinicAffiliationPrivileges_ClinicAffiliations_ClinicAffili~",
                        column: x => x.ClinicAffiliationId,
                        principalSchema: "clinics",
                        principalTable: "ClinicAffiliations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicId",
                schema: "clinics",
                table: "Appointments",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                schema: "clinics",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_ClinicId",
                schema: "clinics",
                table: "Appointments",
                columns: new[] { "DoctorId", "ClinicId" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                schema: "clinics",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicAffiliations_ClinicId_DoctorId",
                schema: "clinics",
                table: "ClinicAffiliations",
                columns: new[] { "ClinicId", "DoctorId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicAffiliations_DoctorId",
                schema: "clinics",
                table: "ClinicAffiliations",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicOwnerships_ClinicId",
                schema: "clinics",
                table: "ClinicOwnerships",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicSpecialties_SpecialtyId",
                schema: "clinics",
                table: "ClinicSpecialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ClinicId",
                schema: "clinics",
                table: "Departments",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                schema: "clinics",
                table: "Departments",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentId",
                schema: "clinics",
                table: "Departments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCredentials_DoctorId",
                schema: "clinics",
                table: "DoctorCredentials",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCredentials_DocumentNumber",
                schema: "clinics",
                table: "DoctorCredentials",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_ClinicId",
                schema: "clinics",
                table: "DoctorSchedules",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_DoctorId",
                schema: "clinics",
                table: "DoctorSchedules",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialties_SpecialtyId",
                schema: "clinics",
                table: "DoctorSpecialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceBlocks_RoomId",
                schema: "clinics",
                table: "MaintenanceBlocks",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatingLicenses_ClinicId",
                schema: "clinics",
                table: "OperatingLicenses",
                column: "ClinicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Identity_NationalId",
                schema: "clinics",
                table: "Patients",
                column: "Identity_NationalId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleBlocks_DoctorScheduleId",
                schema: "clinics",
                table: "ScheduleBlocks",
                column: "DoctorScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Name",
                schema: "clinics",
                table: "Specialties",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_ParentId",
                schema: "clinics",
                table: "Specialties",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_IdentityId",
                schema: "clinics",
                table: "StaffMembers",
                column: "IdentityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "ClinicAffiliationPrivileges",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "ClinicOwnerships",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "ClinicSpecialties",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "DoctorCredentials",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "DoctorSpecialties",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "MaintenanceBlocks",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "OperatingLicenses",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "Patients",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "RoomCapabilities",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "ScheduleBlocks",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "StaffMembers",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "ClinicAffiliations",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "Specialties",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "Clinics",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "DoctorSchedules",
                schema: "clinics");

            migrationBuilder.DropTable(
                name: "Doctors",
                schema: "clinics");
        }
    }
}
