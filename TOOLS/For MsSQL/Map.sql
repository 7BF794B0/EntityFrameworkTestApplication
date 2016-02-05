create table Map (
   Id 			int			not null,
   Login		varchar(50)	not null,
   Password		varchar(36)	not null,
   Server_Id	int			not null,
   constraint PK_Id primary key nonclustered (Id)
)
go