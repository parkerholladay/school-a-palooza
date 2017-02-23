--tableName = schools
--user = palooza_app

create table if not exists schools (
    id uuid primary key,
    district_id uuid not null,
    name text not null,
    level text not null
);
grant select, insert, update, delete on table schools to palooza_app;
create index if not exists schools_district_id_index on schools (district_id);
