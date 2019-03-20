use DTS
go

if exists(select * from sysobjects where name='WarningData')
begin
    drop table WarningData
end

if exists(select * from sysobjects where name='ConsumableData')
begin
    drop table ConsumableData
end

if exists(select * from sysobjects where name='ConsumableReplaceData')
begin
    drop table ConsumableReplaceData
end

if exists(select * from sysobjects where name='WarningConfig')
begin
    drop table WarningConfig
end

if exists(select * from sysobjects where name='ConsumableConfig')
begin
    drop table ConsumableConfig
end

if exists(select * from sysobjects where name='DeviceShiftData')
begin
    drop table DeviceShiftData
end

if exists(select * from sysobjects where name='DeviceData')
begin
    drop table DeviceData
end

if exists(select * from sysobjects where name='AutomaticConfig')
begin
    drop table AutomaticConfig
end

create table WarningConfig
(
    ID int identity(1,1) primary key,
    Name nvarchar(18) not null,
    Level int not null,
	Popup bit not null,
    Treatment nvarchar(50)
)

create table ConsumableConfig
(
    ID int identity(1,1) primary key,
    Name nvarchar(18) not null,
	Information nvarchar(50),
	Type int not null,
    Limit int not null
)

create table DeviceData
(
	ID int primary key,
	ZeroFaultTime bigint not null,
)

create table DeviceShiftData
(
    ID int identity(1,1) primary key,
	DeviceID int not null foreign key references DeviceData(ID),
    Shift nvarchar(18) not null,
    Count int not null,
	StartTime DateTime not null,
	StopTime DateTime
)

create table WarningData
(
    ID int identity(1,1) primary key,
	DeviceID int not null foreign key references DeviceData(ID),
	WarningID int not null foreign key references WarningConfig(ID),
	OccurTime DateTime not null,
	FixTime DateTime,
	Treatment nvarchar(50),
	Result nvarchar(50),
	FixDuration int
)

create table ConsumableData
(
    ID int identity(1,1) primary key,
    DeviceID int not null foreign key references DeviceData(ID),
	ConsumableID int not null foreign key references ConsumableConfig(ID),
	Residual int not null
)

create table ConsumableReplaceData
(
    ID int identity(1,1) primary key,
    DeviceID int not null foreign key references DeviceData(ID),
	ConsumableID int not null foreign key references ConsumableConfig(ID),
	ReplacedTime DateTime not null,
	ReplacedPeople nvarchar(18)
)

create table AutomaticConfig
(
	LeftDays int not null,
	Enable bit not null
)