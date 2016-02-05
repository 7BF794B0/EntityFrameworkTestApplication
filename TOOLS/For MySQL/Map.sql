use Map;
create table Map
(
   Id                   int not null,
   Login                varchar(50) not null,
   Password             varchar(36) not null,
   Server_Id            int not null,
   primary key (Id)
);