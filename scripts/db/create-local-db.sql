--dbName = school_a_palooza
--userName = palooza_app

create role palooza_app login password 'testing1' valid until 'infinity';
create database school_a_palooza;
create database school_a_palooza_integration;
