USE PicDB;


DROP TABLE IF EXISTS PictureModel;
DROP TABLE IF EXISTS PhotographerModel;
DROP TABLE IF EXISTS CameraModel;
DROP TABLE IF EXISTS EXIFModel;
DROP TABLE IF EXISTS IPTCModel;


create table [CameraModel]
(
	ID INT NOT NULL IDENTITY(1,1) Primary Key,
	Producer text not null,
	Make text not null,
	BoughtOn date,
	Notes text,
	ISOLimitGood decimal,
	ISOLimitAcceptable decimal
)

create table [EXIFModel]
(
	ID INT not null identity(1,1) Primary Key,
	Make text not null,
	FNumber decimal, 
	ExposureTime decimal,
	ISOValue decimal,
	Flash bit,
	ExposureProgram int
)

create table [IPTCModel]
(
	ID int not null identity(1,1) Primary Key,
	Keywords text,
	ByLine text,
	CopyrightNotice text,
	Headline text,
	Caption text
)

create table [PhotographerModel]
(
	ID Int not null identity(1,1) Primary Key,
	FirstName varchar(100),
	LastName varchar(50) not null,
	Birthday date,
	Notes text
)

create Table [PictureModel]
(
	ID int not null identity(1,1) Primary Key,
	[FileName] text not null,
	fk_IPTC int not null,
	fk_EXIF int not null,
	fk_Camera int,
	fk_Photographer int,
)

Alter table PictureModel
	add constraint fk_IPTC foreign key (fk_IPTC) references IPTCModel(ID);
Alter table PictureModel
	add constraint fk_EXIF foreign key (fk_EXIF) references EXIFModel(ID);
Alter table PictureModel
	add constraint fk_Camera foreign key(fk_Camera) references CameraModel(ID);
Alter table PictureModel
	add constraint fk_Photographer foreign key (fk_Photographer) references PhotographerModel(ID);


-- Insert Mockdata into PhotographerModel
INSERT INTO dbo.PhotographerModel (FirstName, LastName, Birthday, Notes)
VALUES ('John', 'Whick', '1983-3-22', 'A Killer Photographer');

INSERT INTO dbo.PhotographerModel (FirstName, LastName, Birthday, Notes)
VALUES ('Max', 'Mustermann', '1986-4-23', 'Just Your Average Photographer');

INSERT INTO dbo.PhotographerModel (FirstName, LastName, Birthday, Notes)
VALUES ('Susi', 'Sommer', '1999-6-3', 'Photographer for over 18 Years');

INSERT INTO dbo.PhotographerModel (FirstName, LastName, Birthday, Notes)
VALUES ('Chuck', 'Norris', '1913-5-2', 'Roundhouse Photographer');

INSERT INTO dbo.PhotographerModel (FirstName, LastName, Birthday, Notes)
VALUES ('Arnold', 'McArnoldson', '1973-3-12', 'The Legend');


-- Insert MockData into Cameramodel
INSERT INTO dbo.CameraModel (Producer, Make, BoughtOn, Notes, ISOLimitGood, ISOLimitAcceptable)
VALUES ('Canon', 'EOS 123', '2000-3-22', 'A good Canon Camera', 300, 400);

INSERT INTO dbo.CameraModel (Producer, Make, BoughtOn, Notes, ISOLimitGood, ISOLimitAcceptable)
VALUES ('Canon', 'EOS 234', '2001-3-22', 'A better Canon Camera', 300, 400);

INSERT INTO dbo.CameraModel (Producer, Make, BoughtOn, Notes, ISOLimitGood, ISOLimitAcceptable)
VALUES ('Canon', 'EOS 345', '2002-3-22', 'The best Canon Camera', 450, 550);

INSERT INTO dbo.CameraModel (Producer, Make, BoughtOn, Notes, ISOLimitGood, ISOLimitAcceptable)
VALUES ('Canon', 'EOS 456', '2003-3-22', 'A not so Good Canon Camera', 600, 800);

INSERT INTO dbo.CameraModel (Producer, Make, BoughtOn, Notes, ISOLimitGood, ISOLimitAcceptable)
VALUES ('Canon', 'EOS 330', '2004-3-22', 'The worst Canon Camera', 800, 1000);


-- Insert MockData into EXIFModel
INSERT INTO dbo.EXIFModel (Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram)
VALUES ('Canon EOS 123', 2.0, 2.5, 200, 0, 0);

INSERT INTO dbo.EXIFModel (Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram)
VALUES ('Canon EOS 234', 1.0, 5.3, 300, 1, 1);

INSERT INTO dbo.EXIFModel (Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram)
VALUES ('Canon EOS 345', 4.0, 7.8, 400, 0, 2);

INSERT INTO dbo.EXIFModel (Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram)
VALUES ('Canon EOS 345', 1.4, 9.7, 500, 1, 4);

INSERT INTO dbo.EXIFModel (Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram)
VALUES ('Canon EOS 234', 1.4, 1.2, 700, 0, 5);

INSERT INTO dbo.EXIFModel (Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram)
VALUES ('Canon EOS 123', 5.6, 2.1, 600, 1, 7);


-- Insert Mockdata into IPTCModel
INSERT INTO dbo.IPTCModel (Keywords, ByLine, CopyrightNotice, Headline, Caption)
VALUES ('Squirrel;Nature;Tree;Animal', 'Max Mustermann', 'No Copyright', 'Squirrel', 'A Squirrel on a Tree');

INSERT INTO dbo.IPTCModel (Keywords, ByLine, CopyrightNotice, Headline, Caption)
VALUES ('Cheetah;Nature;Animal', 'John Whick', 'No Copyright', 'Cheetah', 'A Cheetah on a Tree');

INSERT INTO dbo.IPTCModel (Keywords, ByLine, CopyrightNotice, Headline, Caption)
VALUES ('Nature', 'Max Mustermann', 'No Copyright', 'Something Natury', 'What even is this');

INSERT INTO dbo.IPTCModel (Keywords, ByLine, CopyrightNotice, Headline, Caption)
VALUES ('Panda;Nature;Tree;Animal', 'Chuck Norris', 'No Copyright', 'Fake Panda', 'What a platant imposter');

INSERT INTO dbo.IPTCModel (Keywords, ByLine, CopyrightNotice, Headline, Caption)
VALUES ('Parrot;Nature;Tree;Bird;Animal', 'Susi Sommer', 'No Copyright', 'Parrot', 'A Parrot on a Tree');

INSERT INTO dbo.IPTCModel (Keywords, ByLine, CopyrightNotice, Headline, Caption)
VALUES ('New;Fun', 'Max Mustermann', 'No Copyright', 'New Picture', 'What a funny new Picture haha');


-- Insert Mockdata into the PictureModel with correct foreign Keys
INSERT INTO dbo.PictureModel (FileName, fk_IPTC, fk_EXIF, fk_Camera, fk_Photographer)
VALUES ('Img1.jpg', 1, 1, 1, 1);

INSERT INTO dbo.PictureModel (FileName, fk_IPTC, fk_EXIF, fk_Camera, fk_Photographer)
VALUES ('Img2.jpg', 2, 2, 2, 2);

INSERT INTO dbo.PictureModel (FileName, fk_IPTC, fk_EXIF, fk_Camera, fk_Photographer)
VALUES ('Img3.jpg', 3, 3, 3, 3);

INSERT INTO dbo.PictureModel (FileName, fk_IPTC, fk_EXIF, fk_Camera, fk_Photographer)
VALUES ('Img4.jpg', 4, 4, 4, 4);

INSERT INTO dbo.PictureModel (FileName, fk_IPTC, fk_EXIF, fk_Camera, fk_Photographer)
VALUES ('Img5.jpg', 5, 5, 5, 5);