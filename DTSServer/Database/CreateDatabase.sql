use master
go

if exists(select * from sysdatabases where name='DTS')
begin
    drop database DTS
end

create database DTS