Create database ProjectCShaft
go
use ProjectCShaft
go
drop table Table_Bida(
	idTable nvarchar(10) primary key,
	nameTable nvarchar(50) not null,
	typeTable nvarchar(100) not null,
	priceTable float not null,
	statusTable bit,
	description nvarchar(255)
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

create table Order_Table(
	idOrder int identity primary key,
	idTable nvarchar(10) foreign key(idTable) REFERENCES Table_Bida(idTable),
	idMenuOrder_Table int foreign key(idMenuOrder_Table) REFERENCES Menu(idMenu),
	timeStart datetime,
	sumPriceTable float,
	status bit
)

create table OrderMenu(
	idOrderMenu int identity primary key,
	idTable nvarchar(10) foreign key(idTable) REFERENCES Table_Bida(idTable) NOT NULL,
	nameMenuOrder nvarchar(100),
	unitMenuOrder nvarchar(20),
	priceMenuOrder float,
	quantity int,
	sumPrice float
)

insert into Menu values(N'String Vàng','chai',10000,N'Nước ngọt',1)

insert into Account values('nvmduc','123',1)

insert into Table_Bida values('B016',N'Bàn số 16','France',60000,0,N'Hết nơ')

insert into OrderMenu values('B02',N'Sting đỏ',N'Chai',15000,1,15000)

select * from OrderMenu