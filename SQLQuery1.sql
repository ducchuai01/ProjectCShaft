Create database ProjectCShaft
go
use ProjectCShaft
go
create table Table_Bida(
	idTable nvarchar(10) primary key,
	nameTable nvarchar(50) not null,
	typeTable nvarchar(100) not null,
	priceTable float not null,
	statusTable bit,
	description nvarchar
)
create table Account(
	id int identity primary key,
	username varchar(50) not null,
	password varchar(255) not null,
	status bit
)

create table Menu(
	idMenu int identity primary key,
	nameMenu nvarchar(100) not null,
	unitMenu nvarchar(20),
	priceMenu float,
	descriptionMenu nvarchar(255),
	status bit
	
)

insert into Menu values(N'String Vàng','chai',10000,N'Nước ngọt',1)

insert into Account values('nvmduc','123',1)

select * from Menu