CREATE TABLE users (
	id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	login varchar(64) UNIQUE NOT NULL,
	password varchar(256) NOT NULL
);

CREATE TABLE room_types (
	id int PRIMARY KEY NOT NULL,
	title varchar(32) UNIQUE NOT NULL
);

CREATE TABLE rooms (
	id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	title nvarchar(64) NULL,
	room_type_id int NOT NULL,
	status tinyint DEFAULT(1) NOT NULL,

	CONSTRAINT FK_rooms_room_type FOREIGN KEY(room_type_id) REFERENCES room_types(id)
);

CREATE TABLE rooms_users (
	room_id int NOT NULL,
	user_id int NOT NULL,

	CONSTRAINT PK_rooms_users PRIMARY KEY(room_id, user_id),

	CONSTRAINT FK_rooms_users_room FOREIGN KEY(room_id) REFERENCES rooms(id),
	CONSTRAINT FK_rooms_users_user FOREIGN KEY(user_id) REFERENCES users(id)
);

CREATE TABLE messages (
	id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	content nvarchar(2048) NULL,
	user_id int NOT NULL,
	room_id int NOT NULL,
	created_at smalldatetime NULL,
	updated_at smalldatetime NULL,
	deleted_at smalldatetime NULL,

	CONSTRAINT FK_messages_user FOREIGN KEY(user_id) REFERENCES users(id),
	CONSTRAINT FK_messages_room FOREIGN KEY(room_id) REFERENCES rooms(id)
);