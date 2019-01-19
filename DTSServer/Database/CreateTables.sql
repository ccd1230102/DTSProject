use DTS
go

if exists(select * from sysobjects where name='WarningList')
begin
    drop table WarningList
end

if exists(select * from sysobjects where name='ConsumableList')
begin
    drop table ConsumableList
end

if exists(select * from sysobjects where name='Warning')
begin
    drop table Warning
end

if exists(select * from sysobjects where name='Consumable')
begin
    drop table Consumable
end

if exists(select * from sysobjects where name='Device')
begin
    drop table Device
end

create table Warning
(
    ID int identity(1,1) primary key,
    Name nvarchar(18) not null,
    Level int not null,
    Treatment nvarchar(50)
)

create table Consumable
(
    ID int identity(1,1) primary key,
    Name nvarchar(18) not null,
	Information nvarchar(50),
    Limit int not null
)

create table Device
(
    ID int primary key,
	Running bit not null,
    Shift nvarchar(18) not null,
    Count int not null,
	LastOperationTime DateTime not null,
	LastWarningTime DateTime not null
)

create table WarningList
(
    ID int identity(1,1) primary key,
	DeviceID int not null foreign key references Device(ID),
	WarningID int not null foreign key references Warning(ID),
	OccurTime DateTime not null,
	FixTime DateTime,
	Treatment nvarchar(50),
	Result nvarchar(50),
	FixDuration int
)

create table ConsumableList
(
    ID int identity(1,1) primary key,
    DeviceID int not null foreign key references Device(ID),
	ConsumableID int not null foreign key references Consumable(ID),
	UsedTime int not null,
	ReplacedTime DateTime,
	ReplacedPeople nvarchar(18)
)