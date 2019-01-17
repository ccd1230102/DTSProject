use DTS
go

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
