create table users(
    id int PRIMARY key IDENTITY(1,1),
    name varchar(255),
    email varchar(255) UNIQUE,
    password varchar(255)
);

select * from users;