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
