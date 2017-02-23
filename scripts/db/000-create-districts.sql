--tableName = districts
--user = palooza_app

create table if not exists districts (
    id uuid primary key,
    name text not null
);
grant select, insert, update, delete on table districts to palooza_app;
