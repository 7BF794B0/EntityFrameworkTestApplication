create table Friends (
   User_Id          int         not null,
   Friend_Id        int         not null,
   constraint PK_Friends primary key (User_Id, Friend_Id)
)
go

create table User (
   Id             int         not null    auto_increment,
   First_Name     varchar(50) null,
   Last_Name      varchar(50) null,
   Age            int         null,
   Bio            varchar(1000) null,
   Country        varchar(50) null,
   City           varchar(50) null,
   constraint PK_Id primary key nonclustered (Id)
)
go