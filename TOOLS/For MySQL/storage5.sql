create database Storage5 character set utf8 COLLATE utf8_general_ci;
use Storage5;
create table Friends
(
   User_Id              int not null,
   Friend_Id            int not null,
   primary key (User_Id, Friend_Id)
);

create table User
(
   Id                   int not null AUTO_INCREMENT,
   First_Name           varchar(50),
   Last_Name            varchar(50),
   Age                  int,
   Bio                  varchar(1000),
   Country              varchar(50),
   City                 varchar(50),
   primary key (Id)
);